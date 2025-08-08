using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
	public class SceneDropdownDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var errorBox = new HelpBox();

			if (property.propertyType == SerializedPropertyType.String || property.propertyType == SerializedPropertyType.Integer)
			{
				var sceneNames = GetSceneNames(errorBox);

				var dropdownField = IsCollectionValid(sceneNames) ? new DropdownField(property.displayName, sceneNames, GetDropdownDefaultValue(sceneNames, property))
					: new DropdownField(property.displayName, new List<string>() { "NULL" }, 0);

				dropdownField.tooltip = property.tooltip;
				dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				dropdownField.RegisterValueChangedCallback(callback => ApplyPropertyValue(property, dropdownField));

				AddPropertyContextMenu(dropdownField, property);

				if (dropdownField.value != "NULL")
				{
					dropdownField.showMixedValue = property.hasMultipleDifferentValues;
					ApplyPropertyValue(property, dropdownField);
				}

				root.Add(dropdownField);
				DisplayErrorBox(root, errorBox);

				dropdownField.TrackPropertyValue(property, (trackedProperty) =>
				{
					string sceneName = trackedProperty.propertyType == SerializedPropertyType.Integer ? SceneNameFromIndex(trackedProperty.intValue) : trackedProperty.stringValue;

					if (dropdownField.choices.Contains(sceneName))
					{
						dropdownField.SetValueWithoutNotify(sceneName);
					}
					else
					{
						Debug.LogWarning($"The value <b>{trackedProperty.boxedValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a valid scene identifier.", trackedProperty.serializedObject.targetObject);
					}
				});

				ExecuteLater(dropdownField, () => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

				UpdateVisualElement(dropdownField, () =>
				{
					var sceneNames = GetSceneNames(errorBox);

					if (IsCollectionValid(sceneNames))
						dropdownField.choices = sceneNames;
				});

				return root;
			}
			else
			{
				errorBox.text = "The SceneDropdown attribute can only be attached to a string or int";
				DisplayErrorBox(root, errorBox);

				return root;
			}
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var dropdown = element as DropdownField;

			string sceneName = int.TryParse(clipboardValue, out int sceneIndex) ? SceneNameFromIndex(sceneIndex) : clipboardValue;

			if (dropdown.choices.Contains(sceneName))
			{
				dropdown.value = sceneName;
			}
			else
			{
				Debug.LogWarning($"Could not paste value \"{clipboardValue}\" since is not availiable as an option in the dropdown");
			}
		}

		private List<string> GetSceneNames(HelpBox errorBox)
		{
			var sceneList = new List<string>();
			var activeSceneList = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

			if (activeSceneList == null || activeSceneList.Length == 0)
			{
				errorBox.text = "There are no scenes in the active build settings";
				return sceneList;
			}

			foreach (var scene in activeSceneList)
			{
				var sceneName = scene.Split('/')[^1].Split('.')[0]; // Remove the asset paths and file extension from the name

				sceneList.Add(sceneName);
			}

			return sceneList;
		}

		private string SceneNameFromIndex(int BuildIndex)
		{
			var path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);

			if (path == string.Empty)
				return "";

			var slashIndex = path.LastIndexOf('/');
			var name = path[(slashIndex + 1)..];
			var dotIndex = name.LastIndexOf('.');

			return name[..dotIndex];
		}

		private string GetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
		{
			var propertyStringValue = property.propertyType == SerializedPropertyType.String ? property.stringValue : SceneNameFromIndex(property.intValue);

			return collectionValues.Contains(propertyStringValue) ? propertyStringValue : collectionValues[0];
		}

		private void ApplyPropertyValue(SerializedProperty property, DropdownField dropdown)
		{
			if (property.hasMultipleDifferentValues)
				return;

			if (property.propertyType == SerializedPropertyType.String)
			{
				property.stringValue = dropdown.value;
			}
			else
			{
				property.intValue = dropdown.index;
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
