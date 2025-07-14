using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using EditorAttributes.Editor.Utility;

#if HAS_ADDRESSABLES_PACKAGE
using UnityEditor.AddressableAssets;
#endif

namespace EditorAttributes.Editor
{
	[InitializeOnLoad]
	public class EditorValidation : IPreprocessBuildWithReport
	{
		private static int BUILD_KILLERS;

		public int callbackOrder => 0;

		static EditorValidation() { }

		public void OnPreprocessBuild(BuildReport report)
		{
			BUILD_KILLERS = 0;

			if (!EditorAttributesSettings.instance.disableBuildValidation)
				ValidateAll();

			if (BUILD_KILLERS != 0)
				throw new BuildFailedException("Validation Failed");
		}

		/// <summary>
		/// Validates every asset and scene in the project
		/// </summary>
		[MenuItem("Tools/EditorValidation/Validate All", priority = 0)]
		public static void ValidateAll()
		{
			ValidateAllAssets();
			ValidateAllScenes();
		}

		/// <summary>
		/// Validates all scenes in the build
		/// </summary>
		[MenuItem("Tools/EditorValidation/Validate Scenes", priority = 2)]
		public static void ValidateAllScenes()
		{
			int failedValidations = 0;
			int successfulValidations = 0;

			var sceneGuids = AssetDatabase.FindAssets("t:Scene");

			var previouslyOpenedScenes = GetAllOpenedScenes();

			foreach (var sceneGuid in sceneGuids)
			{
				string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);

				if (IsPackageAsset(scenePath))
					continue;

				if (SceneUtility.GetBuildIndexByScenePath(scenePath) == -1 && !IsAddressable(sceneGuid))
					continue;

				var openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

				ValidateScene(openedScene, ref failedValidations, ref successfulValidations);

				if (previouslyOpenedScenes.All((scene) => scene.path != scenePath))
					EditorSceneManager.CloseScene(openedScene, true);
			}

			Debug.Log($"Scenes Validated: <b>(Failed: {failedValidations}, Succeeded: {successfulValidations}, Total: {failedValidations + successfulValidations})</b>");
		}

		private static bool IsAddressable(string guid)
		{
#if HAS_ADDRESSABLES_PACKAGE
			var settings = AddressableAssetSettingsDefaultObject.Settings;

			if (settings == null)
				return false;
			
			foreach (var group in settings.groups)
			{
				if (group == null || group.entries.Count == 0)
					continue;

				foreach (var entry in group.entries)
				{
					if (entry.guid == guid)
						return true;
				}
			}
#endif
			return false;
		}

		/// <summary>
		/// Validates all scenes currently open
		/// </summary>
		[MenuItem("Tools/EditorValidation/Validate Open Scenes", priority = 3)]
		public static void ValidateOpenScenes()
		{
			int failedValidations = 0;
			int successfulValidations = 0;

			foreach (var openedScene in GetAllOpenedScenes())
				ValidateScene(openedScene, ref failedValidations, ref successfulValidations);

			Debug.Log($"Scenes Validated: <b>(Failed: {failedValidations}, Succeeded: {successfulValidations}, Total: {failedValidations + successfulValidations})</b>");
		}

		/// <summary>
		/// Validates all assets in the project
		/// </summary>
		[MenuItem("Tools/EditorValidation/Validate Assets", priority = 1)]
		public static void ValidateAllAssets()
		{
			int failedValidations = 0;
			int successfulValidations = 0;

			var prefabGuids = AssetDatabase.FindAssets("t:Prefab");

			foreach (var prefabGuid in prefabGuids)
			{
				string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);

				if (IsPackageAsset(prefabPath))
					continue;

				var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

				ValidateComponents(prefab.GetComponentsInChildren<Component>(true), ref failedValidations, ref successfulValidations);
			}

			var scriptableObjectGuids = AssetDatabase.FindAssets("t:ScriptableObject");

			foreach (var scriptableObjectGuid in scriptableObjectGuids)
			{
				string scriptableObjectPath = AssetDatabase.GUIDToAssetPath(scriptableObjectGuid);

				if (IsPackageAsset(scriptableObjectPath))
					continue;

				var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptableObjectPath);

				Validate(scriptableObject, ref failedValidations, ref successfulValidations);
			}

			Debug.Log($"Assets Validated: <b>(Failed: {failedValidations}, Succeeded: {successfulValidations}, Total: {failedValidations + successfulValidations})</b>");
		}

		/// <summary>
		/// Validates all fields marked for validation with an attribute
		/// </summary>
		/// <param name="targetObject">The target object to validate</param>
		/// <param name="failedValidations">The amount of validations that failed</param>
		/// <param name="successfulValidations">The amount of validations that succeded</param>
		public static void Validate(Object targetObject, ref int failedValidations, ref int successfulValidations)
		{
			var type = targetObject.GetType();
			var fields = type.GetFields(ReflectionUtility.BINDING_FLAGS);

			foreach (var field in fields)
			{
				string validationMessage = $"Validation failed on <b>{type.Name}.{field.Name}</b> in <b>{targetObject.name}</b>: ";

				var requiredAttribute = field.GetCustomAttribute<RequiredAttribute>();

				if (requiredAttribute != null && requiredAttribute.ThrowValidationError)
				{
					var fieldValue = field.GetValue(targetObject);

					if (IsNotValid(fieldValue))
					{
						if (requiredAttribute.BuildKiller)
						{
							BUILD_KILLERS++;
							validationMessage = "<color=#FF0000><b>(Build Killer)</b></color> " + validationMessage;
						}

						Debug.LogError(validationMessage + "Field not assigned", targetObject);
						failedValidations++;
					}
					else
					{
						successfulValidations++;
					}
				}

				var validateAttribute = field.GetCustomAttribute<ValidateAttribute>();

				if (validateAttribute != null)
				{
					var conditionalMember = ReflectionUtility.GetValidMemberInfo(validateAttribute.ConditionName, targetObject);

					if (EvaluateCondition(conditionalMember, targetObject, out ValidationCheck customCheck))
					{
						string customMessage = customCheck == null ? validateAttribute.ValidationMessage : customCheck.ValidationMessage;
						bool isBuildKiller = customCheck == null ? validateAttribute.BuildKiller : customCheck.KillBuild;
						var severety = customCheck == null ? validateAttribute.Severety : customCheck.Severety;

						if (isBuildKiller)
						{
							BUILD_KILLERS++;
							validationMessage = "<color=#FF0000><b>(Build Killer)</b></color> " + validationMessage;
						}

						switch (severety)
						{
							case MessageMode.None:
							case MessageMode.Log:
								Debug.Log(validationMessage + customMessage, targetObject);
								break;

							case MessageMode.Warning:
								Debug.LogWarning(validationMessage + customMessage, targetObject);
								break;

							case MessageMode.Error:
								Debug.LogError(validationMessage + customMessage, targetObject);
								break;
						}

						failedValidations++;
					}
					else
					{
						successfulValidations++;
					}
				}
			}
		}

		/// <summary>
		/// Checks to see if an asset is inside the Packages folder
		/// </summary>
		/// <param name="assetPath">The path of the asset</param>
		/// <returns>True if the asset is inside the packages folder</returns>
		public static bool IsPackageAsset(string assetPath) => assetPath.StartsWith("Packages/");

		/// <summary>
		/// Returns an array of all the Scenes currently open in the hierarchy
		/// </summary>
		/// <returns>Array of Scenes in the Hierarchy</returns>
		public static Scene[] GetAllOpenedScenes()
		{
			var array = new Scene[SceneManager.sceneCount];

			for (int i = 0; i < SceneManager.sceneCount; i++)
				array[i] = SceneManager.GetSceneAt(i);

			return array;
		}

		private static void ValidateScene(Scene scene, ref int failedValidations, ref int successfulValidations)
		{
			var rootObjects = scene.GetRootGameObjects();

			foreach (var rootObject in rootObjects)
			{
				// Check all children recursively
				var childTransforms = rootObject.GetComponentsInChildren<Transform>(true);

				foreach (var childTransform in childTransforms)
					ValidateComponents(childTransform.gameObject.GetComponents<Component>(), ref failedValidations, ref successfulValidations);
			}
		}

		private static void ValidateComponents(Component[] components, ref int failedValidations, ref int successfulValidations)
		{
			foreach (var component in components)
			{
				if (component == null)
					continue;

				Validate(component, ref failedValidations, ref successfulValidations);
			}
		}

		private static bool EvaluateCondition(MemberInfo memberInfo, object targetObject, out ValidationCheck customValidationCheck)
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);
			string errorMessage = $"Couldn't validate condition, check for any error box messages on <b>{targetObject}</b>";

			customValidationCheck = null;

			if (memberInfoType == null)
			{
				Debug.LogError(errorMessage, (Object)targetObject);
				return true;
			}

			if (memberInfoType == typeof(bool))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject);

				if (memberInfoValue == null)
					return false;

				return (bool)memberInfoValue;
			}
			else if (memberInfoType == typeof(ValidationCheck))
			{
				if (ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject) is not ValidationCheck memberInfoValue)
					return false;

				customValidationCheck = memberInfoValue;
				return !memberInfoValue.PassedCheck;
			}

			Debug.LogError(errorMessage, (Object)targetObject);

			return true;
		}

		private static bool IsNotValid(object fieldValue) => fieldValue == null || fieldValue.Equals(null);
	}
}
