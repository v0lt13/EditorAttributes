using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using EditorAttributes.Editor.Utility;

using Object = UnityEngine.Object;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(RequiredAttribute))]
	public class RequiredDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var requiredAttribute = attribute as RequiredAttribute;

			var propertyField = CreatePropertyField(property);

			root.Add(propertyField);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				var helpBox = new HelpBox($"The field <b>{property.displayName}</b> must be assigned", HelpBoxMessageType.Error);
				var fixButton = new Button(() => FixNullReference(property, requiredAttribute)) { text = "Fix" };

				if (CanApplyGlobalColor)
				{
					helpBox.style.color = EditorExtension.GLOBAL_COLOR;
					helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
				}

				if (requiredAttribute.FixMode != ReferenceFixMode.None)
					helpBox.Add(fixButton);

				DoRequiredCheck();

				propertyField.RegisterCallback<SerializedPropertyChangeEvent>((changeEvent) => DoRequiredCheck());

				void DoRequiredCheck()
				{
					if (property.objectReferenceValue == null)
					{
						AddElement(root, helpBox);
					}
					else
					{
						RemoveElement(root, helpBox);
					}
				}
			}
			else
			{
				root.Add(new HelpBox("The attached field must derive from <b>UnityEngine.Object</b>", HelpBoxMessageType.Error));
			}

			return root;
		}

		private void FixNullReference(SerializedProperty property, RequiredAttribute requiredAttribute)
		{
			var memberType = ReflectionUtility.GetMemberInfoType(ReflectionUtility.GetValidMemberInfo(property.name, property));

			foreach (var target in property.serializedObject.targetObjects)
			{
				if (!IsReferenceFixValid(property, target.GetType(), requiredAttribute))
					return;

				var component = target as Component;
				Object objectReference = null;

				switch (requiredAttribute.FixMode)
				{
					case ReferenceFixMode.Auto:
						for (int i = 0; i <= 3; i++)
						{
							if (objectReference != null)
								break;

							switch (i)
							{
								case 0:
									GetSelf();
									break;

								case 1:
									GetChild();
									break;

								case 2:
									GetParent();
									break;

								case 3:
									GetScene();
									break;
							}
						}
						break;

					case ReferenceFixMode.Self:
						GetSelf();
						break;

					case ReferenceFixMode.Children:
						GetChild();
						break;

					case ReferenceFixMode.Parents:
						GetParent();
						break;

					case ReferenceFixMode.Scene:
						GetScene();
						break;

					case ReferenceFixMode.Custom:
						objectReference = ReflectionUtility.FindFunction(requiredAttribute.CustomFixFunctionName, target).Invoke(target, null) as Object;
						break;
				}

				property.objectReferenceValue = objectReference;
				property.serializedObject.ApplyModifiedProperties();

				if (property.objectReferenceValue == null)
					Debug.LogWarning($"Could not find a valid reference of the type <b>{memberType}</b> with <b>ReferenceFixMode.{requiredAttribute.FixMode}</b>", target);

				void GetSelf()
				{
					if (typeof(Component).IsAssignableFrom(memberType))
					{
						objectReference = component.GetComponent(memberType);
					}
					else if (typeof(GameObject).IsAssignableFrom(memberType))
					{
						objectReference = component.gameObject;
					}
				}

				void GetChild()
				{
					if (typeof(Component).IsAssignableFrom(memberType))
					{
						objectReference = component.GetComponentInChildren(memberType, true);
					}
					else if (typeof(GameObject).IsAssignableFrom(memberType))
					{
						if (component.transform.childCount > 0)
							objectReference = component.transform.GetChild(0).gameObject;
					}
				}

				void GetParent()
				{
					if (typeof(Component).IsAssignableFrom(memberType))
					{
						objectReference = component.GetComponentInParent(memberType, true);
					}
					else if (typeof(GameObject).IsAssignableFrom(memberType))
					{
						if (component.transform.parent != null)
							objectReference = component.transform.parent.gameObject;
					}
				}

				void GetScene()
				{
					if (PrefabStageUtility.GetCurrentPrefabStage() == null)
						objectReference = Object.FindFirstObjectByType(memberType, FindObjectsInactive.Include);
				}
			}
		}

		private bool IsReferenceFixValid(SerializedProperty property, Type targetType, RequiredAttribute requiredAttribute)
		{
			bool isComponent = typeof(Component).IsAssignableFrom(targetType);
			bool isScriptableObject = typeof(ScriptableObject).IsAssignableFrom(targetType);

			var targetObject = property.serializedObject.targetObject;

			switch (requiredAttribute.FixMode)
			{
				case ReferenceFixMode.Auto:
				case ReferenceFixMode.Self:
				case ReferenceFixMode.Children:
				case ReferenceFixMode.Parents:
				case ReferenceFixMode.Scene:
					if (isScriptableObject)
					{
						Debug.LogError($"<b>{requiredAttribute.FixMode}</b> is not valid on <b>ScriptableObjects</b>", targetObject);
						return false;
					}
					else if (!isComponent)
					{
						Debug.LogError($"<b>{requiredAttribute.FixMode}</b> is not valid on the type <b>{targetType}</b>", targetObject);
						return false;
					}
					break;

				case ReferenceFixMode.Custom:
					string functionName = requiredAttribute.CustomFixFunctionName;

					if (string.IsNullOrEmpty(functionName))
					{
						Debug.LogError($"No custom fix function name was provided", targetObject);
						return false;
					}

					var functionInfo = ReflectionUtility.FindFunction(functionName, property);

					if (functionInfo == null)
					{
						Debug.LogError($"Could not find function <b>{functionName}</b>. If this function is inherited make sure is marked at protected", targetObject);
						return false;
					}
					else if (functionInfo.GetParameters().Length != 0)
					{
						Debug.LogError($"The function <b>{functionName}</b> cannot have parameters", targetObject);
						return false;
					}
					else if (!typeof(Object).IsAssignableFrom(functionInfo.ReturnType))
					{
						Debug.LogError($"The function <b>{functionName}</b> needs to return a <b>Unity.Object</b>", targetObject);
						return false;
					}
					break;
			}

			return true;
		}
	}
}
