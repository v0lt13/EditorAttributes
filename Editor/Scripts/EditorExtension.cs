using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
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

		private const string STATIC_MENU_ITEM_PATH = "CONTEXT/Object/Show Static Members";
		private const string NON_SERIALZIED_MENU_ITEM_PATH = "CONTEXT/Object/Show Non Serialized Members";

		private static bool ENABLE_STATIC_MEMBERS;
		private static bool ENABLE_NON_SERIALIZED_MEMBERS;

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
				buttonParamsDataFilePath = Path.Combine(ButtonDrawer.PARAMS_DATA_LOCATION, $"{target}ParamsData.json");
			}
			catch (ArgumentException)
			{
				return;
			}

			ENABLE_STATIC_MEMBERS = Menu.GetChecked(STATIC_MENU_ITEM_PATH);
			ENABLE_NON_SERIALIZED_MEMBERS = Menu.GetChecked(NON_SERIALZIED_MENU_ITEM_PATH);
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

			var defaultInspector = DrawDefaultInspector();
			var buttons = DrawButtons();

			var staticMembers = DrawStaticMembers();
			var nonSerializedMembers = DrawNonSerilizedMembers();

			root.Add(defaultInspector);
			root.Add(buttons);

			PropertyDrawerBase.UpdateVisualElement(root, () =>
			{
				if (ENABLE_STATIC_MEMBERS)
				{
					PropertyDrawerBase.AddElement(root, staticMembers);
				}
				else
				{
					PropertyDrawerBase.RemoveElement(root, staticMembers);
				}

				if (ENABLE_NON_SERIALIZED_MEMBERS)
				{
					PropertyDrawerBase.AddElement(root, nonSerializedMembers);
				}
				else
				{
					PropertyDrawerBase.RemoveElement(root, nonSerializedMembers);
				}
			});

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
						var propertyField = PropertyDrawerBase.CreateProperty(property);

						if (property.name == "m_Script")
						{
							propertyField.SetEnabled(false);
							root.Add(propertyField);
						}

						var field = ReflectionUtility.FindField(property.name, target);

						if (field?.GetCustomAttribute<HidePropertyAttribute>() != null)
							continue;

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

						if (property.name != "m_Script")
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

		[MenuItem(STATIC_MENU_ITEM_PATH, priority = 0)]
		private static void ToggleStaticFields()
		{
			ENABLE_STATIC_MEMBERS = !ENABLE_STATIC_MEMBERS;

			Menu.SetChecked(STATIC_MENU_ITEM_PATH, ENABLE_STATIC_MEMBERS);
		}

		[MenuItem(NON_SERIALZIED_MENU_ITEM_PATH, priority = 1)]
		private static void ToggleNonSerializedFields()
		{
			ENABLE_NON_SERIALIZED_MEMBERS = !ENABLE_NON_SERIALIZED_MEMBERS;

			Menu.SetChecked(NON_SERIALZIED_MENU_ITEM_PATH, ENABLE_NON_SERIALIZED_MEMBERS);
		}

		/// <summary>
		/// Draws all the static members
		/// </summary>
		/// <returns>A visual element containing all static members</returns>
		protected VisualElement DrawStaticMembers()
		{
			var root = new VisualElement();
			var header = new Label("Static Members:")
			{
				style = {
					unityFontStyleAndWeight = FontStyle.Bold,
					unityTextAlign = TextAnchor.MiddleLeft,
					fontSize = 12,
					marginTop = 5
				}
			};

			var attributeFilter = new Type[] {
				typeof(HideInInspector),
				typeof(HidePropertyAttribute),
				typeof(ObsoleteAttribute)
			};

			var staticFields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

			foreach (var staticField in staticFields)
			{
				if (ReflectionUtility.HasAnyAttributes(staticField, attributeFilter))
					continue;

				if (staticField.Name == "OffsetOfInstanceIDInCPlusPlusObject")
					continue;

				var field = DrawField(staticField, staticField.FieldType, staticField.GetValue(target));

				if (field == null)
					continue;

				if (field.name.Contains("k__BackingField")) // Don't draw properties with backing fields since we already draw the properties ourself
					continue;

				root.Add(field);
			}

			var staticProperties = target.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			
			foreach (var staticProperty in staticProperties)
			{
				if (ReflectionUtility.HasAnyAttributes(staticProperty, attributeFilter))
					continue;

				var field = DrawField(staticProperty, staticProperty.PropertyType, staticProperty.GetValue(target));

				if (field == null)
					continue;

				root.Add(field);
			}

			var staticMethods = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

			foreach (var staticMethod in staticMethods)
			{
				if (ReflectionUtility.HasAnyAttributes(staticMethod, attributeFilter))
					continue;

				if (staticMethod.GetParameters().Length > 0 || staticMethod.ContainsGenericParameters)
					continue;

				var field = DrawField(staticMethod, staticMethod.ReturnType, staticMethod.Invoke(target, null));

				if (field == null)
					continue;

				if (field.name.StartsWith("get_")) // Don't draw the property backing functions since we already draw the properties ourself
					continue;

				root.Add(field);
			}

			if (root.childCount != 0)
			{
				root.Add(header);
				header.SendToBack();
			}

			return root;
		}

		/// <summary>
		/// Draws all the non serialized members
		/// </summary>
		/// <returns>A visual element containing all non serialized members</returns>
		protected VisualElement DrawNonSerilizedMembers()
		{
			var root = new VisualElement();
			var header = new Label("Non Serialized Members:")
			{
				style = {
					unityFontStyleAndWeight = FontStyle.Bold,
					unityTextAlign = TextAnchor.MiddleLeft,
					fontSize = 12,
					marginTop = 5
				}
			};

			var attributeFilter = new Type[] {
				typeof(HideInInspector),
				typeof(HidePropertyAttribute),
				typeof(SerializeField),
				typeof(ObsoleteAttribute)
			};

			var nonSerializedFields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			foreach (var nonSerializedField in nonSerializedFields)
			{
				if (ReflectionUtility.HasAnyAttributes(nonSerializedField, attributeFilter))
					continue;

                if (!HasValidDeclaringType(nonSerializedField))
                    continue;

                var field = DrawField(nonSerializedField, nonSerializedField.FieldType, nonSerializedField.GetValue(target));

				if (field == null)
					continue;

				if (field.name.Contains("k__BackingField")) // Don't draw properties with backing fields since we already draw the properties ourself
					continue;

				root.Add(field);
			}

			var nonSerializedProperties = target.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			foreach (var nonSerializedProperty in nonSerializedProperties)
			{
				if (ReflectionUtility.HasAnyAttributes(nonSerializedProperty, attributeFilter))
					continue;

				if (!HasValidDeclaringType(nonSerializedProperty))
					continue;

				var field = DrawField(nonSerializedProperty, nonSerializedProperty.PropertyType, nonSerializedProperty.GetValue(target));

				if (field == null)
					continue;

				root.Add(field);
			}

			var nonSerializedMethods = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			foreach (var nonSerializedMethod in nonSerializedMethods)
			{
				if (ReflectionUtility.HasAnyAttributes(nonSerializedMethod, attributeFilter))
					continue;

				if (nonSerializedMethod.GetParameters().Length > 0 || nonSerializedMethod.ContainsGenericParameters)
					continue;

                if (!HasValidDeclaringType(nonSerializedMethod))
                    continue;

                var field = DrawField(nonSerializedMethod, nonSerializedMethod.ReturnType, nonSerializedMethod.Invoke(target, null));

				if (field == null)
					continue;

				if (field.name.StartsWith("get_")) // Don't draw the property backing functions since we already draw the properties ourself
					continue;

				root.Add(field);
			}

			if (root.childCount != 0)
			{
				root.Add(header);
				header.SendToBack();
			}

			return root;
		}

		private VisualElement DrawField(MemberInfo memberInfo, Type memberType, object memberValue)
		{
			var field = PropertyDrawerBase.CreateFieldForType(memberType, memberInfo.Name, memberValue);
			
			if (field.GetType() == typeof(HelpBox)) // If it returns a help box it means it was not able to create a field for the type
				return null;

			field.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
			field.SetEnabled(false);

			PropertyDrawerBase.BindFieldToMember(memberType, field, memberInfo, target);

			return field;
		}

		private bool HasValidDeclaringType(MemberInfo memberInfo)
        {
            if (memberInfo.DeclaringType == null)
                return false;

            if (memberInfo.DeclaringType == typeof(Component) || memberInfo.DeclaringType == typeof(Object) || memberInfo.DeclaringType == typeof(Behaviour) || memberInfo.DeclaringType == typeof(MonoBehaviour))
                return false;

            return true;
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

				var button = ButtonDrawer.DrawButton(function, buttonAttribute, buttonFoldouts, buttonParameterValues, target);
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
