using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
    public class SceneDropdownDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = EditorGUI.Popup(position, label, property.intValue, GUIGetSceneNames());
                    break;

                case SerializedPropertyType.String:
					int selectedIndex = 0;
                    var sceneArray = GetSceneNames();

                    if (sceneArray == null || sceneArray.Length == 0) break;

					for (int i = 0; i < sceneArray.Length; i++)
					{
						if (sceneArray[i] == GetPropertyValueAsString(property)) selectedIndex = i;
					}

					selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, sceneArray);

                    SetProperyValueFromString(sceneArray[selectedIndex], ref property);
					break;

                default:
                    EditorGUILayout.HelpBox("The SceneDropdown attribute can only be attached to a string or int", MessageType.Error);
                    break;
            }
        }

        private string[] GetSceneNames()
        {
			var sceneList = new List<string>();
            var activeSceneList = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

            if (activeSceneList == null || activeSceneList.Length == 0)
            {
                EditorGUILayout.HelpBox("There are no scenes in the build settings", MessageType.Error);
                return sceneList.ToArray();
            }

			foreach (var scene in activeSceneList)
			{
			    var sceneName = scene.Split('/')[^1].Split('.')[0]; // Remove the asset paths and file extension from the name

			    sceneList.Add(sceneName);
			}

			return sceneList.ToArray();
		}

        private GUIContent[] GUIGetSceneNames()
        {
            var sceneNames = GetSceneNames();
			var sceneList = new List<GUIContent>();

			foreach (var sceneName in sceneNames) sceneList.Add(new GUIContent(sceneName));

            return sceneList.ToArray();
        }
    }
}
