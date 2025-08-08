using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
using Object = UnityEngine.Object;

namespace EditorAttributes.Editor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Object), true)]
	public class EditorExtension : UnityEditor.Editor
	{
		public static readonly Color DEFAULT_GLOBAL_COLOR = new(0.8f, 0.8f, 0.8f, 1.0f);
		public static Color GLOBAL_COLOR = DEFAULT_GLOBAL_COLOR;

		private string buttonParamsDataFilePath;

		private Dictionary<MethodInfo, bool> buttonFoldouts = new();
		private Dictionary<MethodInfo, object[]> buttonParameterValues = new();

		private MethodInfo[] functions;

		protected virtual void OnEnable()
		{
			functions = target.GetType().GetMethods(ReflectionUtility.BINDING_FLAGS);

			ButtonDrawer.LoadParamsData(functions, target, ref buttonFoldouts, ref buttonParameterValues);

			try
			{
				buttonParamsDataFilePath = Path.Combine(ButtonDrawer.PARAMS_DATA_LOCATION, ButtonDrawer.GetFileName(target));
			}
			catch (ArgumentException)
			{
				return;
			}
		}

		protected virtual void OnDisable()
		{
			if (target == null)
				ButtonDrawer.DeleteParamsData(buttonParamsDataFilePath);

			EditorHandles.handleProperties.Clear();
			EditorHandles.boundsHandleList.Clear();
		}

		void OnSceneGUI() => EditorHandles.DrawHandles();

		public override VisualElement CreateInspectorGUI()
		{
			// Reset the global color per component GUI so it doesnt leak from other components
			GLOBAL_COLOR = DEFAULT_GLOBAL_COLOR;

			var root = new VisualElement();

			var nonSerializedMembers = DrawNonSerilizedMembers();
			var defaultInspector = DrawDefaultInspector();
			var buttons = DrawButtons();

			root.Add(defaultInspector);
			root.Add(nonSerializedMembers);
			root.Add(buttons);

			return root;
		}

		protected virtual new VisualElement DrawDefaultInspector()
		{
			var root = new VisualElement();
			var propertyList = new Dictionary<string, PropertyField>();

			using (var property = serializedObject.GetIterator())
			{
				if (property.NextVisible(true))
				{
					IColorAttribute prevColor = null;

					do
					{
						var propertyField = PropertyDrawerBase.CreatePropertyField(property);

						if (property.name == "m_Script")
						{
							propertyField.SetEnabled(false);
							root.Add(propertyField);

							continue;
						}

						var field = ReflectionUtility.FindField(property.name, target);

						if (field?.GetCustomAttribute<HidePropertyAttribute>() != null)
						{
							propertyField.style.display = DisplayStyle.None;
						}

						var colorAttribute = field?.GetCustomAttribute<GUIColorAttribute>();

						if (colorAttribute != null)
						{
							GUIColorDrawer.ColorField(propertyField, colorAttribute);
							prevColor = colorAttribute;
						}
						else if (prevColor != null)
						{
							GUIColorDrawer.ColorField(propertyField, prevColor);
						}

						propertyList.Add(property.name, propertyField);
					}
					while (property.NextVisible(false));
				}
			}

			var orderedProperties = propertyList.OrderBy((property) =>
			{
				var field = ReflectionUtility.FindField(property.Key, target);

				var propertyOrderAttribute = field?.GetCustomAttribute<PropertyOrderAttribute>();

				if (propertyOrderAttribute != null)
					return propertyOrderAttribute.PropertyOrder;

				return 0;
			});

			foreach (var property in orderedProperties)
				root.Add(property.Value);

			return root;
		}

		/// <summary>
		/// Draws all the members marked with the ShowInInspector attribute
		/// </summary>
		/// <returns>A visual element containing all non serialized member fields</returns>
		protected VisualElement DrawNonSerilizedMembers()
		{
			var root = new VisualElement();

			var nonSerializedFields = target.GetType().GetFields(ReflectionUtility.BINDING_FLAGS).Where((field) => field.GetCustomAttribute<ShowInInspectorAttribute>() != null);

			foreach (var nonSerializedField in nonSerializedFields)
			{
				if (HasRestrictedAttributes(nonSerializedField, out string errorMessage))
				{
					root.Add(new HelpBox(errorMessage, HelpBoxMessageType.Error));
					continue;
				}

				var field = DrawNonSerializedField(nonSerializedField, nonSerializedField.FieldType, nonSerializedField.GetValue(target));

				root.Add(field);
			}

			var nonSerializedProperties = target.GetType().GetProperties(ReflectionUtility.BINDING_FLAGS).Where((field) => field.GetCustomAttribute<ShowInInspectorAttribute>() != null);

			foreach (var nonSerializedProperty in nonSerializedProperties)
			{
				if (HasRestrictedAttributes(nonSerializedProperty, out string errorMessage))
				{
					root.Add(new HelpBox(errorMessage, HelpBoxMessageType.Error));
					continue;
				}

				var field = DrawNonSerializedField(nonSerializedProperty, nonSerializedProperty.PropertyType, nonSerializedProperty.GetValue(target));

				root.Add(field);
			}

			var nonSerializedMethods = target.GetType().GetMethods(ReflectionUtility.BINDING_FLAGS).Where((field) => field.GetCustomAttribute<ShowInInspectorAttribute>() != null);

			foreach (var nonSerializedMethod in nonSerializedMethods)
			{
				if (HasRestrictedAttributes(nonSerializedMethod, out string errorMessage))
				{
					root.Add(new HelpBox(errorMessage, HelpBoxMessageType.Error));
					continue;
				}

				if (nonSerializedMethod.GetParameters().Length > 0 || nonSerializedMethod.ContainsGenericParameters)
				{
					root.Add(new HelpBox($"Method <b>{nonSerializedMethod.Name}</b> cannot be drawn because it has parameters or is generic", HelpBoxMessageType.Error));
					continue;
				}

				var field = DrawNonSerializedField(nonSerializedMethod, nonSerializedMethod.ReturnType, nonSerializedMethod.Invoke(target, null));

				root.Add(field);
			}

			return root;
		}

		private VisualElement DrawNonSerializedField(MemberInfo memberInfo, Type memberType, object memberValue)
		{
			var root = new VisualElement();

			var headerAttribute = memberInfo.GetCustomAttribute<HeaderAttribute>();

			var header = new Label()
			{
				style = {
					marginTop = 13,
					marginLeft = 3,
					marginRight = -2,
					unityFontStyleAndWeight = FontStyle.Bold,
					unityTextAlign = TextAnchor.LowerLeft
				}
			};

			header.AddToClassList("unity-header-drawer__label");

			var field = PropertyDrawerBase.CreateFieldForType(memberType, memberInfo.Name, memberValue, AreNonSerializedMemberValuesDifferent(memberInfo, targets));

			field.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

			if (field is Foldout)
			{
				field.contentContainer.SetEnabled(false);
				field.Q<Label>().SetEnabled(false);
			}
			else
			{
				field.SetEnabled(false);
			}

			PropertyDrawerBase.BindFieldToMember(memberType, field, memberInfo, target);

			foreach (var spaceAttribute in memberInfo.GetCustomAttributes<SpaceAttribute>())
			{
				var space = new VisualElement();

				space.style.height = spaceAttribute.height;

				space.AddToClassList("unity-space-drawer");
				root.Add(space);
			}

			if (headerAttribute != null)
			{
				header.text = headerAttribute.header;
				root.Add(header);
			}

			root.Add(field);

			return root;
		}

		private bool AreNonSerializedMemberValuesDifferent(MemberInfo memberInfo, Object[] targets)
		{
			if (targets == null || targets.Length <= 1)
				return false;

			object firstValue = ReflectionUtility.GetMemberInfoValue(memberInfo, targets[0]);

			for (int i = 1; i < targets.Length; i++)
			{
				object otherValue = ReflectionUtility.GetMemberInfoValue(memberInfo, targets[i]);

				if (!Equals(firstValue, otherValue))
					return true;
			}

			return false;
		}

		private bool HasRestrictedAttributes(MemberInfo memberInfo, out string errorMessage)
		{
			if (memberInfo.GetCustomAttribute<HideInInspector>() != null || memberInfo.GetCustomAttribute<HidePropertyAttribute>() != null)
			{
				errorMessage = $"You want to show the member <b>{memberInfo.Name}</b> but you mark it with the HideInInspector or HideProperty Attribute, make up your mind bro";
				return true;
			}

			if (memberInfo.GetCustomAttribute<SerializeField>() != null || memberInfo.GetCustomAttribute<SerializeReference>() != null)
			{
				errorMessage = $"The member <b>{memberInfo.Name}</b> is already serialized, there is no need to use the ShowInInspector Attribute";
				return true;
			}

			errorMessage = string.Empty;
			return false;
		}

		/// <summary>
		/// Draws all the buttons from functions using the Button Attribute
		/// </summary>
		/// <returns>A visual element containing all drawn buttons</returns>
		protected VisualElement DrawButtons()
		{
			var root = new VisualElement();
			var errorBox = new HelpBox();

			IColorAttribute prevColor = null;

			foreach (var function in functions)
			{
				var buttonAttribute = function.GetCustomAttribute<ButtonAttribute>();

				if (buttonAttribute == null)
					continue;

				var colorAttribute = function?.GetCustomAttribute<GUIColorAttribute>();

				if (colorAttribute != null)
				{
					GUIColorDrawer.ColorField(root, colorAttribute);
					prevColor = colorAttribute;
				}
				else if (prevColor != null)
				{
					GUIColorDrawer.ColorField(root, prevColor);
				}

				var button = ButtonDrawer.DrawButton(function, buttonAttribute, buttonFoldouts, buttonParameterValues, targets);
				var conditionalProperty = ReflectionUtility.GetValidMemberInfo(buttonAttribute.ConditionName, target);

				button.RegisterCallback<FocusOutEvent>((callback) => ButtonDrawer.SaveParamsData(functions, target, buttonFoldouts, buttonParameterValues));

				if (conditionalProperty != null)
				{
					PropertyDrawerBase.UpdateVisualElement(root, () =>
					{
						var conditionValue = PropertyDrawerBase.GetConditionValue(conditionalProperty, buttonAttribute, target, errorBox);

						if (buttonAttribute.Negate)
							conditionValue = !conditionValue;

						switch (buttonAttribute.ConditionResult)
						{
							case ConditionResult.ShowHide:
								if (conditionValue)
								{
									if (!root.Contains(button))
										root.Add(button);
								}
								else
								{
									PropertyDrawerBase.RemoveElement(root, button);
								}
								break;

							case ConditionResult.EnableDisable:
								button.SetEnabled(conditionValue);
								break;
						}

						PropertyDrawerBase.DisplayErrorBox(root, errorBox);
					});

					root.Add(button);
				}
				else
				{
					root.Add(button);
				}
			}

			return root;
		}
	}
}
