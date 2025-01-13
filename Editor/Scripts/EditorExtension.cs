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

		private const string MENU_ITEM_PATH = "CONTEXT/Object/Show Static Fields";
		private static bool ENABLE_STATIC_FIELDS;

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

			ENABLE_STATIC_FIELDS = Menu.GetChecked(MENU_ITEM_PATH);
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
			var staticFields = DrawStaticFields();
			var buttons = DrawButtons();

			root.Add(defaultInspector);

			PropertyDrawerBase.UpdateVisualElement(root, () =>
			{
				if (ENABLE_STATIC_FIELDS)
				{
					PropertyDrawerBase.AddElement(root, staticFields);
				}
				else
				{
					PropertyDrawerBase.RemoveElement(root, staticFields);
				}
			});
			
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
						var propertyField = new PropertyField(property);

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

		[MenuItem(MENU_ITEM_PATH, priority = 0)]
		private static void ToggleStaticFields()
		{
			ENABLE_STATIC_FIELDS = !ENABLE_STATIC_FIELDS;

			Menu.SetChecked(MENU_ITEM_PATH, ENABLE_STATIC_FIELDS);
		}

		/// <summary>
		/// Draws all the static and const fields
		/// </summary>
		/// <returns>A visual element containing all static and const fields</returns>
		protected VisualElement DrawStaticFields()
		{
			var root = new VisualElement();
			var staticFields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			foreach (var staticField in staticFields)
			{
				if (staticField.GetCustomAttribute<HideInInspector>() != null)
					continue;
				
				var propertyField = ButtonDrawer.DrawParameterField(staticField.FieldType, ObjectNames.NicifyVariableName(staticField.Name), staticField.GetValue(target));

				propertyField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				propertyField.SetEnabled(false);

				root.Add(propertyField);
			}

			return root;
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
