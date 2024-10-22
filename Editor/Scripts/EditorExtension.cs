using System;
using System.IO;
using UnityEngine;
using UnityEditor;
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
		private static List<Action> updateExecutionList = new();

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
		}

		protected virtual void OnDisable()
		{
			if (target == null)
				ButtonDrawer.DeleteParamsData(buttonParamsDataFilePath);

			updateExecutionList.Clear();
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
			//var staticFields = DrawStaticFields();
			var buttons = DrawButtons();

			root.Add(defaultInspector);
			//root.Add(staticFields);
			root.Add(buttons);

			RunUpdateLoop(root);

			return root;
		}

		protected virtual new VisualElement DrawDefaultInspector()
		{
			var root = new VisualElement();

			using (var property = serializedObject.GetIterator())
			{
				if (property.NextVisible(true))
				{
					IColorAttribute prevColor = null;

					do
					{
						var propertyField = new PropertyField(property);
		
						if (property.name == "m_Script")
							propertyField.SetEnabled(false);

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

						root.Add(propertyField);
					}
					while (property.NextVisible(false));
				}
			}

            return root;
		}

		internal static void AddToUpdateLoop(Action action) => updateExecutionList.Add(action);

		private void RunUpdateLoop(VisualElement root)
		{
			root.schedule.Execute(() => 
			{
				if (!PropertyDrawerBase.IsCollectionValid(updateExecutionList))
					return;

				foreach (var action in updateExecutionList)
					action.Invoke();
			}).Every(50);
		}

		/// <summary>
		/// Draws all the static and const field marked as public or with SerializeField
		/// </summary>
		/// <remarks> 
		/// THIS FUNCTION IS EXPERIMENTAL! To enable drawing of static variables uncomment the lines of code at 60 and 64 in the CreateInspectorGUI() function
		/// </remarks>
		/// <returns>A visual element containing all static and const fields</returns>
		protected VisualElement DrawStaticFields()
		{
			var root = new VisualElement();
			var staticFields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			foreach (var staticField in staticFields)
			{
				if (staticField.IsPrivate && staticField.GetCustomAttribute<SerializeField>() == null)
					continue;
				
				var propertyField = ButtonDrawer.DrawParameterField(staticField.FieldType, ObjectNames.NicifyVariableName(staticField.Name), staticField.GetValue(target));

				propertyField.AddToClassList("unity-base-field__aligned");
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
					AddToUpdateLoop(() =>
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
