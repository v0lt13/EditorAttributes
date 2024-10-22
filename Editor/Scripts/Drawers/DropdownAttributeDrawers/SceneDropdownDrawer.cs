using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

				var dropdownField = IsCollectionValid(sceneNames) ? new DropdownField(property.displayName, sceneNames, GetDropdownDefaultValue(sceneNames, property)) : new DropdownField(property.displayName, new List<string>() { "NULL" }, 0);

				root.schedule.Execute(() => dropdownField.Q(className: "unity-base-popup-field__input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f).ExecuteLater(1);

				dropdownField.AddToClassList("unity-base-field__aligned");
				dropdownField.RegisterValueChangedCallback(callback => ApplyPropertyValue(property, dropdownField));

				if (dropdownField.value != "NULL")
				{
					dropdownField.showMixedValue = property.hasMultipleDifferentValues;
					ApplyPropertyValue(property, dropdownField);
				}

				UpdateVisualElement(root, () =>
				{
					var sceneNames = GetSceneNames(errorBox);

					if (IsCollectionValid(sceneNames))
						dropdownField.choices = sceneNames;
				});

				root.Add(dropdownField);
				DisplayErrorBox(root, errorBox);
				return root;
			}
			else
			{
				errorBox.text = "The SceneDropdown attribute can only be attached to a string or int";
				DisplayErrorBox(root, errorBox);

				return root;
			}
		}

        private List<string> GetSceneNames(HelpBox errorBox)
        {
			var sceneList = new List<string>();
            var activeSceneList = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (activeSceneList == null || activeSceneList.Length == 0)
            {
				errorBox.text = "There are no scenes in the build settings";
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
