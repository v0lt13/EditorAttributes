using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEditor.Search;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
using Object = UnityEngine.Object;

namespace EditorAttributes.Editor
{
	public class PropertyDrawerBase : PropertyDrawer
	{
		internal const string GROUPED_PROPERTY_ID = "GroupedProperty";

		protected bool CanApplyGlobalColor => EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR;

		public override VisualElement CreatePropertyGUI(SerializedProperty property) => CreatePropertyField(property);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label, true);

			var helpBoxStyle = GUI.skin.GetStyle("HelpBox");
			helpBoxStyle.richText = true;

			EditorGUILayout.HelpBox("You cannot use <b>EditorAttributes</b> with <b>ImGUI</b> based editors. " +
				"Convert your editor to <b>UI Toolkit</b> for attributes to work, or remove the attributes from properties drawn by the editor script.", MessageType.Warning);
		}

		/// <summary>
		/// Override this function to customize the copied value from an element with using <see cref="AddPropertyContextMenu(VisualElement, SerializedProperty)"/>
		/// </summary>
		/// <param name="element">The element on which the context menu was added</param>
		/// <param name="property">The attached serialized property</param>
		/// <returns>The string that will be copied into the clipboard</returns>
		protected virtual string CopyValue(VisualElement element, SerializedProperty property) => GetCopyPropertyValue(property);

		/// <summary>
		/// Override this function to customize the paste behaivour for an element with using <see cref="AddPropertyContextMenu(VisualElement, SerializedProperty)"/>
		/// </summary>
		/// <param name="element">The element on which the context menu was added</param>
		/// <param name="property">The attached serialized property</param>
		/// <param name="clipboardValue">The current clipboard value</param>
		protected virtual void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue) => SetPropertyValueFromString(clipboardValue, property);

		/// <summary>
		/// Creates a properly binded property field from a serialized property
		/// </summary>
		/// <param name="property">The serialized property</param>
		/// <returns>The binded property field</returns>
		public static PropertyField CreatePropertyField(SerializedProperty property)
		{
			var propertyField = new PropertyField(property);
			propertyField.BindProperty(property.serializedObject);

			return propertyField;
		}

		/// <summary>
		/// Sets the value of a property from a string
		/// </summary>
		/// <param name="value">The string value to convert</param>
		/// <param name="property">The serialized property to assign the value to</param>
		protected void SetPropertyValueFromString(string value, SerializedProperty property)
		{
			try
			{
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						property.intValue = Convert.ToInt32(value);
						break;

					case SerializedPropertyType.Float:
						property.floatValue = Convert.ToSingle(value);
						break;

					case SerializedPropertyType.Boolean:
						property.boolValue = Convert.ToBoolean(value);
						break;

					case SerializedPropertyType.String:
						property.stringValue = value;
						break;

					case SerializedPropertyType.Character:
						property.intValue = Convert.ToChar(value);
						break;

					case SerializedPropertyType.Color:
						property.colorValue = ColorUtility.TryParseHtmlString(value, out Color color) ? color : Color.white;
						break;

					case SerializedPropertyType.Vector2:
						property.vector2Value = VectorUtils.ParseVector2(value);
						break;

					case SerializedPropertyType.Vector3:
						property.vector3Value = VectorUtils.ParseVector3(value);
						break;

					case SerializedPropertyType.Vector4:
						property.vector4Value = VectorUtils.ParseVector4(value);
						break;

					case SerializedPropertyType.Vector2Int:
						property.vector2IntValue = VectorUtils.ParseVector2Int(value);
						break;

					case SerializedPropertyType.Vector3Int:
						property.vector3IntValue = VectorUtils.ParseVector3Int(value);
						break;

					default:
						Debug.LogWarning($"The type <b>{property.propertyType}</b> is not supported", property.serializedObject.targetObject);
						break;
				}

				property.serializedObject.ApplyModifiedProperties();
			}
			catch (FormatException)
			{
				Debug.LogError($"Could not convert the value \"{value}\" to <b>{property.propertyType}</b>", property.serializedObject.targetObject);
			}
		}

		/// <summary>
		/// Gets the value of a serialzied property and returns it as a string
		/// </summary>
		/// <param name="property">The serialized property to get the value from</param>
		/// <returns>The serialized property value as a string</returns>
		protected string GetPropertyValueAsString(SerializedProperty property) => property.propertyType switch
		{
			SerializedPropertyType.String => property.stringValue,
			SerializedPropertyType.Integer or SerializedPropertyType.LayerMask or SerializedPropertyType.Character => property.intValue.ToString(),
			SerializedPropertyType.Enum => IsPropertyEnumFlag() ? property.enumValueFlag.ToString() : property.enumDisplayNames[property.enumValueIndex],
			SerializedPropertyType.Float => property.floatValue.ToString(),
			SerializedPropertyType.Boolean => property.boolValue.ToString(),
			SerializedPropertyType.Vector2 => property.vector2Value.ToString(),
			SerializedPropertyType.Vector3 => property.vector3Value.ToString(),
			SerializedPropertyType.Vector4 => property.vector4Value.ToString(),
			SerializedPropertyType.Rect => property.vector4Value.ToString(),
			SerializedPropertyType.Bounds => property.boundsValue.ToString(),
			SerializedPropertyType.Color => property.colorValue.ToString(),
			SerializedPropertyType.Gradient => property.gradientValue.ToString(),
			SerializedPropertyType.AnimationCurve => property.animationCurveValue.ToString(),
			SerializedPropertyType.Quaternion => property.quaternionValue.ToString(),
			SerializedPropertyType.Vector2Int => property.vector2IntValue.ToString(),
			SerializedPropertyType.Vector3Int => property.vector3IntValue.ToString(),
			SerializedPropertyType.RectInt => property.rectIntValue.ToString(),
			SerializedPropertyType.BoundsInt => property.boundsIntValue.ToString(),
			SerializedPropertyType.Hash128 => property.hash128Value.ToString(),
			SerializedPropertyType.ArraySize => property.arraySize.ToString(),
			SerializedPropertyType.FixedBufferSize => property.fixedBufferSize.ToString(),
			SerializedPropertyType.ObjectReference => property.objectReferenceValue.ToString(),
			SerializedPropertyType.ExposedReference => property.exposedReferenceValue.ToString(),
			SerializedPropertyType.ManagedReference => property.managedReferenceValue.ToString(),
			_ => string.Empty
		};

		/// <summary>
		/// Converts the values of a collection into strings
		/// </summary>
		/// <param name="collectionName">The name of the collection to convert</param>
		/// <param name="serializedProperty">The serialized property</param>
		/// <param name="memberInfo">The member info of the collection</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>The values of the collection in a list of strings</returns>
		protected static List<string> ConvertCollectionValuesToStrings(string collectionName, SerializedProperty serializedProperty, MemberInfo memberInfo, HelpBox errorBox)
		{
			var stringList = new List<string>();
			var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

			if (memberInfoValue is Array array)
			{
				foreach (var item in array)
					stringList.Add(item == null ? "NULL" : item.ToString());
			}
			else if (memberInfoValue is IList list)
			{
				foreach (var item in list)
					stringList.Add(item == null ? "NULL" : item.ToString());
			}
			else if (memberInfoValue is IDictionary dictionary)
			{
				foreach (DictionaryEntry item in dictionary)
					stringList.Add(item.Value == null ? "NULL" : item.Value.ToString());
			}
			else
			{
				errorBox.text = $"Could not find the collection <b>{collectionName}</b>";
			}

			return stringList;
		}

		/// <summary>
		/// Finds a nested serialized property
		/// </summary>
		/// <param name="property">The serialized property</param>
		/// <param name="propertyName">The name of the property to find</param>
		/// <returns>The nested serialized property</returns>
		protected static SerializedProperty FindNestedProperty(SerializedProperty property, string propertyName)
		{
			var propertyPath = property.propertyPath;
			var cutPathIndex = propertyPath.LastIndexOf('.');

			if (cutPathIndex == -1)
			{
				return property.serializedObject.FindProperty(propertyName);
			}
			else
			{
				propertyPath = propertyPath[..cutPathIndex];

				return property.serializedObject.FindProperty(propertyPath).FindPropertyRelative(propertyName);
			}
		}

		/// <summary>
		/// Gets the collection property from a collection item property
		/// </summary>
		/// <param name="property">The collection item property</param>
		/// <returns>The collection property</returns>
		public static SerializedProperty GetCollectionProperty(SerializedProperty property)
		{
			string path = property.propertyPath;

			int index = path.LastIndexOf(".Array.data[");

			if (index >= 0)
			{
				string collectionPath = path[..index];
				return property.serializedObject.FindProperty(collectionPath);
			}

			return null;
		}

		/// <summary>
		/// Gets the name of a serialized property accounting for C# properties
		/// </summary>
		/// <param name="propertyName">The name of the property to look for</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The name of the serialized property</returns>
		public static string GetSerializedPropertyName(string propertyName, SerializedProperty property)
		{
			var memberInfo = ReflectionUtility.GetValidMemberInfo(propertyName, property);

			return memberInfo is PropertyInfo ? $"<{propertyName}>k__BackingField" : propertyName;
		}

		/// <summary>
		/// Checks to see if the serialized property is a flagged enum
		/// </summary>
		/// <returns>True if the serialized property type is a flagged enum</returns>
		protected bool IsPropertyEnumFlag() => fieldInfo.FieldType.IsDefined(typeof(FlagsAttribute), false);

		/// <summary>
		/// Displays an error box in the inspector
		/// </summary>
		/// <param name="root">The root visual element</param>
		/// <param name="errorBox">The help box to displaying the errors</param>
		public static void DisplayErrorBox(VisualElement root, HelpBox errorBox)
		{
			errorBox.messageType = HelpBoxMessageType.Error;

			if (!string.IsNullOrEmpty(errorBox.text))
			{
				AddElement(root, errorBox);
			}
			else
			{
				RemoveElement(root, errorBox);
			}
		}

		/// <summary>
		/// Schedules an action to update
		/// </summary>
		/// <param name="visualElement">The visual element to schedule the update</param>
		/// <param name="logicToUpdate">The logic to execute on the specified element</param>
		/// <param name="intervalMs">The update interval in milliseconds</param>
		/// <returns>The scheduled visual element item</returns>
		public static IVisualElementScheduledItem UpdateVisualElement(VisualElement visualElement, Action logicToUpdate, long intervalMs = 60)
		{
			logicToUpdate.Invoke(); // Execute the logic once so we don't have to wait for the first execution of the scheduler

			return visualElement.schedule.Execute(logicToUpdate).Every(intervalMs);
		}

		/// <summary>
		/// Schedules an action to execute after a delay
		/// </summary>
		/// <param name="visualElement">The visual element to schedule the execution</param>
		/// <param name="logicToExecute">The logic to execute on the specified element</param>
		/// <param name="delayMs">The execution delay in milliseconds</param>
		/// <returns>The scheduled visual element item</returns>
		public static IVisualElementScheduledItem ExecuteLater(VisualElement visualElement, Action logicToExecute, long delayMs = 1) => visualElement.schedule.Execute(logicToExecute).StartingIn(delayMs);

		/// <summary>
		/// Add an element from another visual element if it doesn't exist
		/// </summary>
		/// <param name="root">The root to add the element on</param>
		/// <param name="element">The element to add</param>
		public static void AddElement(VisualElement root, VisualElement element)
		{
			if (!root.Contains(element))
				root.Add(element);
		}

		/// <summary>
		/// Removes an element from another visual element if it exists
		/// </summary>
		/// <param name="owner">The owner containing the element</param>
		/// <param name="element">The element to remove</param>
		public static void RemoveElement(VisualElement owner, VisualElement element)
		{
			if (owner.Contains(element))
				owner.Remove(element);
		}

		/// <summary>
		/// Gets the value of a condition for a conditional attribute
		/// </summary>
		/// <param name="memberInfo">The member info of the condition</param>
		/// <param name="conditionalAttribute">The conditional attribute</param>
		/// <param name="serializedProperty">The serialized property</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>True if the condition is satisfied</returns>
		public static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, SerializedProperty serializedProperty, HelpBox errorBox)
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found";
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue == null)
					return false;

				return (bool)memberInfoValue;
			}
			else if (memberInfoType.IsEnum)
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue == null)
					return false;

				return (int)memberInfoValue == conditionalAttribute.EnumValue;
			}

			errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum";

			return false;
		}

		internal static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, object targetObject, HelpBox errorBox) // Internal function used for the button drawer
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found";
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				return (bool)ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject);
			}
			else if (memberInfoType.IsEnum)
			{
				return (int)ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject) == conditionalAttribute.EnumValue;
			}

			errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum";
			return false;
		}

		/// <summary>
		/// Gets the string value from a member if the input mode is set to Dynamic
		/// </summary>
		/// <param name="inputText">The string input that may contain the member name</param>
		/// <param name="property">The serialized property</param>
		/// <param name="dynamicStringAttribute">The dynamic string attribute</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>If the input mode is Constant will return the base input string, if is Dynamic will return the string value of the member</returns>
		public static string GetDynamicString(string inputText, SerializedProperty property, IDynamicStringAttribute dynamicStringAttribute, HelpBox errorBox)
		{
			switch (dynamicStringAttribute.StringInputMode)
			{
				default:
				case StringInputMode.Constant:
					return inputText;

				case StringInputMode.Dynamic:
					var memberInfo = ReflectionUtility.GetValidMemberInfo(inputText, property);

					if (memberInfo == null)
					{
						errorBox.text = $"The member <b>{inputText}</b> could not be found";
						return inputText;
					}

					var memberValue = ReflectionUtility.GetMemberInfoValue(memberInfo, property);
					var memberType = ReflectionUtility.GetMemberInfoType(memberInfo);

					if (memberValue == null)
						return inputText;

					if (memberType == typeof(string))
						return memberValue.ToString();

					errorBox.text = $"The member <b>{inputText}</b> needs to return a string";
					return inputText;
			}
		}

		/// <summary>
		/// Adds the property context menu to a non property element
		/// </summary>
		/// <param name="element">The element to add the context menu to</param>
		/// <param name="property">The serialized property</param>
		protected void AddPropertyContextMenu(VisualElement element, SerializedProperty property)
		{
			if (element is PropertyField)
				Debug.LogError("Can't add the property context menu to a property field since it already has one by default.");

			element.AddManipulator(new ContextualMenuManipulator((@event) =>
			{
				string searchText = $"h:#{property.serializedObject.targetObject.GetType().Name}.{property.propertyPath}={GetPropertyValueAsString(property).Replace(" ", "")}";

				@event.menu.AppendAction("Copy Property Path", (action) => EditorGUIUtility.systemCopyBuffer = property.propertyPath);
				@event.menu.AppendAction("Search Same Property Value", (action) => SearchService.ShowWindow().SetSearchText(searchText));

				@event.menu.AppendSeparator();

				@event.menu.AppendAction("Copy", (action) => EditorGUIUtility.systemCopyBuffer = CopyValue(element, property));
				@event.menu.AppendAction("Paste", (action) => PasteValue(element, property, ParsePropertyClipboardValue(property, EditorGUIUtility.systemCopyBuffer)));

				@event.menu.AppendSeparator();
			}));
		}

		private string GetCopyPropertyValue(SerializedProperty property)
		{
			string propertyValue = GetPropertyValueAsString(property);

			return property.propertyType switch
			{
				SerializedPropertyType.Vector2 or SerializedPropertyType.Vector2Int => $"Vector2{propertyValue}",
				SerializedPropertyType.Vector3 or SerializedPropertyType.Vector3Int => $"Vector3{propertyValue}",
				SerializedPropertyType.Rect or SerializedPropertyType.RectInt => $"Rect{propertyValue}",
				SerializedPropertyType.Bounds or SerializedPropertyType.BoundsInt => $"Bounds{propertyValue}",
				SerializedPropertyType.Vector4 or SerializedPropertyType.Quaternion => property.type + propertyValue,
				SerializedPropertyType.LayerMask => $"LayerMask({propertyValue})",
				SerializedPropertyType.Enum => $"Enum:{(IsPropertyEnumFlag() ? Convert.ToString(property.enumValueFlag, 2) : propertyValue)}",
				_ => propertyValue
			};
		}

		private string ParsePropertyClipboardValue(SerializedProperty property, string clipboardValue) => property.propertyType switch
		{
			SerializedPropertyType.Vector2 or SerializedPropertyType.Vector2Int => clipboardValue.Replace("Vector2", ""),
			SerializedPropertyType.Vector3 or SerializedPropertyType.Vector3Int => clipboardValue.Replace("Vector3", ""),
			SerializedPropertyType.Rect or SerializedPropertyType.RectInt => clipboardValue.Replace("Rect", ""),
			SerializedPropertyType.Bounds or SerializedPropertyType.BoundsInt => clipboardValue.Replace("Bounds", ""),
			SerializedPropertyType.Vector4 or SerializedPropertyType.Quaternion => clipboardValue.Replace(property.type, ""),
			SerializedPropertyType.LayerMask => clipboardValue.Replace("LayerMask", ""),
			SerializedPropertyType.Enum => clipboardValue.Replace("Enum:", ""),
			_ => clipboardValue
		};

		private protected string CreatePropertySaveKey(SerializedProperty property, string key) => $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}_{key}";

		/// <summary>
		/// Invokes a function on all specified targets
		/// </summary>
		/// <param name="targets">The property to get the targets from</param>
		/// <param name="functionName">The name of the function to invoke</param>
		/// <param name="parameterValues">Parameter values for the function</param>
		public static void InvokeFunctionOnAllTargets(Object[] targets, string functionName, object[] parameterValues = null)
		{
			foreach (var target in targets)
			{
				var methodInfo = ReflectionUtility.FindFunction(functionName, target);

				Undo.RecordObject(target, $"Invoke {functionName}");

				methodInfo.Invoke(target, parameterValues);

				EditorUtility.SetDirty(target);
			}
		}

		/// <summary>
		/// Applies the help box style to a visual element
		/// </summary>
		/// <param name="visualElement">The element to apply the style to</param>
		public static void ApplyBoxStyle(VisualElement visualElement)
		{
			visualElement.style.borderTopLeftRadius = 3f;
			visualElement.style.borderTopRightRadius = 3f;
			visualElement.style.borderBottomLeftRadius = 3f;
			visualElement.style.borderBottomRightRadius = 3f;

			visualElement.style.borderBottomWidth = 1f;
			visualElement.style.borderTopWidth = 1f;
			visualElement.style.borderLeftWidth = 1f;
			visualElement.style.borderRightWidth = 1f;

			visualElement.style.borderTopColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderBottomColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderLeftColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderRightColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);

			visualElement.style.backgroundColor = EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR ? EditorExtension.GLOBAL_COLOR / 2f : new Color(63f / 255f, 63f / 255f, 63f / 255f);

			visualElement.style.paddingTop = 3f;
			visualElement.style.paddingBottom = 3f;
			visualElement.style.paddingLeft = 3f;
			visualElement.style.paddingRight = 3f;

			visualElement.style.marginTop = 1f;
			visualElement.style.marginBottom = 1f;
			visualElement.style.marginRight = 3f;
			visualElement.style.marginLeft = 3f;
		}

		/// <summary>
		/// Copies all of the style values from a <see cref="VisualElement"/> to another
		/// </summary>
		/// <param name="copyFrom">The element to copy the style from</param>
		/// <param name="copyTo">The element to copy the style to</param>
		public void CopyStyle(VisualElement copyFrom, VisualElement copyTo)
		{
			copyTo.style.position = copyFrom.style.position;
			copyTo.style.top = copyFrom.style.top;
			copyTo.style.bottom = copyFrom.style.bottom;
			copyTo.style.left = copyFrom.style.left;
			copyTo.style.right = copyFrom.style.right;
			copyTo.style.paddingTop = copyFrom.style.paddingTop;
			copyTo.style.paddingBottom = copyFrom.style.paddingBottom;
			copyTo.style.paddingLeft = copyFrom.style.paddingLeft;
			copyTo.style.paddingRight = copyFrom.style.paddingRight;
			copyTo.style.alignContent = copyFrom.style.alignContent;
			copyTo.style.alignItems = copyFrom.style.alignItems;
			copyTo.style.alignSelf = copyFrom.style.alignSelf;
			copyTo.style.flexBasis = copyFrom.style.flexBasis;
			copyTo.style.flexDirection = copyFrom.style.flexDirection;
			copyTo.style.flexWrap = copyFrom.style.flexWrap;
			copyTo.style.width = copyFrom.style.width;
			copyTo.style.height = copyFrom.style.height;
			copyTo.style.justifyContent = copyFrom.style.justifyContent;
			copyTo.style.marginTop = copyFrom.style.marginTop;
			copyTo.style.marginBottom = copyFrom.style.marginBottom;
			copyTo.style.marginLeft = copyFrom.style.marginLeft;
			copyTo.style.marginRight = copyFrom.style.marginRight;
			copyTo.style.transformOrigin = copyFrom.style.transformOrigin;
			copyTo.style.translate = copyFrom.style.translate;
			copyTo.style.rotate = copyFrom.style.rotate;
			copyTo.style.scale = copyFrom.style.scale;
			copyTo.style.transitionDelay = copyFrom.style.transitionDelay;
			copyTo.style.transitionDuration = copyFrom.style.transitionDuration;
			copyTo.style.transitionProperty = copyFrom.style.transitionProperty;
			copyTo.style.transitionTimingFunction = copyFrom.style.transitionTimingFunction;
			copyTo.style.color = copyFrom.style.color;
			copyTo.style.backgroundColor = copyFrom.style.backgroundColor;
			copyTo.style.unityBackgroundImageTintColor = copyFrom.style.unityBackgroundImageTintColor;
			copyTo.style.backgroundImage = copyFrom.style.backgroundImage;
			copyTo.style.backgroundPositionX = copyFrom.style.backgroundPositionX;
			copyTo.style.backgroundPositionY = copyFrom.style.backgroundPositionY;
			copyTo.style.backgroundRepeat = copyFrom.style.backgroundRepeat;
			copyTo.style.backgroundSize = copyFrom.style.backgroundSize;
			copyTo.style.opacity = copyFrom.style.opacity;
			copyTo.style.unityOverflowClipBox = copyFrom.style.unityOverflowClipBox;
			copyTo.style.minWidth = copyFrom.style.minWidth;
			copyTo.style.maxWidth = copyFrom.style.maxWidth;
			copyTo.style.minHeight = copyFrom.style.minHeight;
			copyTo.style.maxHeight = copyFrom.style.maxHeight;
			copyTo.style.borderTopColor = copyFrom.style.borderTopColor;
			copyTo.style.borderBottomColor = copyFrom.style.borderBottomColor;
			copyTo.style.borderLeftColor = copyFrom.style.borderLeftColor;
			copyTo.style.borderRightColor = copyFrom.style.borderRightColor;
			copyTo.style.fontSize = copyFrom.style.fontSize;
			copyTo.style.unityFont = copyFrom.style.unityFont;
			copyTo.style.unityFontStyleAndWeight = copyFrom.style.unityFontStyleAndWeight;
			copyTo.style.unityFontDefinition = copyFrom.style.unityFontDefinition;
			copyTo.style.unityTextAlign = copyFrom.style.unityTextAlign;
			copyTo.style.textShadow = copyFrom.style.textShadow;
			copyTo.style.unityTextOutlineColor = copyFrom.style.unityTextOutlineColor;
			copyTo.style.unityTextOverflowPosition = copyFrom.style.unityTextOverflowPosition;
			copyTo.style.textOverflow = copyFrom.style.textOverflow;
			copyTo.style.unityTextOutlineWidth = copyFrom.style.unityTextOutlineWidth;
			copyTo.style.wordSpacing = copyFrom.style.wordSpacing;
			copyTo.style.unityParagraphSpacing = copyFrom.style.unityParagraphSpacing;
			copyTo.style.whiteSpace = copyFrom.style.whiteSpace;
			copyTo.style.cursor = copyFrom.style.cursor;
			copyTo.style.overflow = copyFrom.style.overflow;

#if UNITY_6000_0_OR_NEWER
			copyTo.style.unityTextGenerator = copyFrom.style.unityTextGenerator;
			copyTo.style.unityEditorTextRenderingMode = copyFrom.style.unityEditorTextRenderingMode;
#endif
			foreach (var @class in copyFrom.GetClasses())
				copyTo.AddToClassList(@class);
		}

		/// <summary>
		/// Creates a field for a specific type
		/// </summary>
		/// <typeparam name="T"> The type of the field to create</typeparam>
		/// <param name="fieldName">The name of the field</param>
		/// <param name="fieldValue">The default value of the field</param>
		/// <param name="showMixedValue">Whether to show the mixed value state for the field</param>
		/// <returns>A visual element of the appropriate field</returns>
		public static VisualElement CreateFieldForType<T>(string fieldName, object fieldValue, bool showMixedValue = false) => CreateFieldForType(typeof(T), fieldName, fieldValue, showMixedValue);

		/// <summary>
		/// Creates a field for a specific type
		/// </summary>
		/// <param name="fieldType">The type of the field to create</param>
		/// <param name="fieldName">The name of the field</param>
		/// <param name="fieldValue">The default value of the field</param>
		/// <param name="showMixedValue">Whether to show the mixed value state for the field</param>
		/// <returns>A visual element of the appropriate field</returns>
		public static VisualElement CreateFieldForType(Type fieldType, string fieldName, object fieldValue, bool showMixedValue = false)
		{
			fieldName = ObjectNames.NicifyVariableName(fieldName);

			if (fieldType == typeof(string))
			{
				return new TextField(fieldName) { value = (string)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(char))
			{
				return new TextField(fieldName) { value = fieldValue.ToString(), showMixedValue = showMixedValue, maxLength = 1 };
			}
			else if (fieldType == typeof(int))
			{
				return new IntegerField(fieldName) { value = (int)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(uint))
			{
				return new UnsignedIntegerField(fieldName) { value = (uint)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(long))
			{
				return new LongField(fieldName) { value = (long)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(ulong))
			{
				return new UnsignedLongField(fieldName) { value = (ulong)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(float))
			{
				return new FloatField(fieldName) { value = (float)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(double))
			{
				return new DoubleField(fieldName) { value = (double)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(bool))
			{
				return new Toggle(fieldName) { value = (bool)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType.IsEnum)
			{
				return new EnumField(fieldName, (Enum)fieldValue) { showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Vector2))
			{
				return new Vector2Field(fieldName) { value = (Vector2)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Vector2Int))
			{
				return new Vector2IntField(fieldName) { value = (Vector2Int)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Vector3))
			{
				return new Vector3Field(fieldName) { value = (Vector3)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Vector3Int))
			{
				return new Vector3IntField(fieldName) { value = (Vector3Int)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Vector4))
			{
				return new Vector4Field(fieldName) { value = (Vector4)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Color))
			{
				return new ColorField(fieldName) { value = (Color)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Gradient))
			{
				return new GradientField(fieldName) { value = (Gradient)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				return new CurveField(fieldName) { value = (AnimationCurve)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(LayerMask))
			{
				return new LayerMaskField(fieldName, (LayerMask)fieldValue) { showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Rect))
			{
				return new RectField(fieldName) { value = (Rect)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(RectInt))
			{
				return new RectIntField(fieldName) { value = (RectInt)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(Bounds))
			{
				return new BoundsField(fieldName) { value = (Bounds)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType == typeof(BoundsInt))
			{
				return new BoundsIntField(fieldName) { value = (BoundsInt)fieldValue, showMixedValue = showMixedValue };
			}
			else if (fieldType.IsSerializable && !ReflectionUtility.IsTypeCollection(fieldType) && !fieldType.IsPrimitive)
			{
				var serializedObjectFoldout = new Foldout { text = fieldName };

				var nestedFields = fieldType.GetFields();

				foreach (var field in nestedFields)
				{
					var createdField = CreateFieldForType(field.FieldType, field.Name, field.GetValue(fieldValue));

					createdField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

					serializedObjectFoldout.Add(createdField);
				}

				return serializedObjectFoldout;
			}
			else
			{
				return new HelpBox($"The type <b>{fieldType}</b> is not supported", HelpBoxMessageType.Error);
			}
		}

		/// <summary>
		/// Registers a value changed callback for field of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="field">The visual element of the field</param>
		/// <param name="valueCallback">The callback action</param>
		/// <param name="objectValue">The value of the registered serialized object. This parameter is only required if you need to register value callbacks to serialized objects</param>
		public static void RegisterValueChangedCallbackByType<T>(VisualElement field, Action<object> valueCallback, object objectValue = null) => RegisterValueChangedCallbackByType(typeof(T), field, valueCallback, objectValue);

		/// <summary>
		/// Registers a value changed callback for field of a specific type.
		/// </summary>
		/// <param name="fieldType">The type of the value</param>
		/// <param name="field">The visual element of the field</param>
		/// <param name="valueCallback">The callback action</param>
		/// <param name="objectValue">The value of the registered serialized object. This parameter is only required if you need to register value callbacks to serialized objects</param>
		public static void RegisterValueChangedCallbackByType(Type fieldType, VisualElement field, Action<object> valueCallback, object objectValue = null)
		{
			if (fieldType == typeof(string) || fieldType == typeof(char))
			{
				field.RegisterCallback<ChangeEvent<string>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(int))
			{
				field.RegisterCallback<ChangeEvent<int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(uint))
			{
				field.RegisterCallback<ChangeEvent<uint>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(long))
			{
				field.RegisterCallback<ChangeEvent<long>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(ulong))
			{
				field.RegisterCallback<ChangeEvent<ulong>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(float))
			{
				field.RegisterCallback<ChangeEvent<float>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(double))
			{
				field.RegisterCallback<ChangeEvent<double>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(bool))
			{
				field.RegisterCallback<ChangeEvent<bool>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType.IsEnum)
			{
				field.RegisterCallback<ChangeEvent<Enum>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Vector2))
			{
				field.RegisterCallback<ChangeEvent<Vector2>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Vector2Int))
			{
				field.RegisterCallback<ChangeEvent<Vector2Int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Vector3))
			{
				field.RegisterCallback<ChangeEvent<Vector3>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Vector3Int))
			{
				field.RegisterCallback<ChangeEvent<Vector3Int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Vector4))
			{
				field.RegisterCallback<ChangeEvent<Vector4>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Color))
			{
				field.RegisterCallback<ChangeEvent<Color>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Gradient))
			{
				field.RegisterCallback<ChangeEvent<Gradient>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				field.RegisterCallback<ChangeEvent<AnimationCurve>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(LayerMask))
			{
				field.RegisterCallback<ChangeEvent<int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Rect))
			{
				field.RegisterCallback<ChangeEvent<Rect>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(RectInt))
			{
				field.RegisterCallback<ChangeEvent<RectInt>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(Bounds))
			{
				field.RegisterCallback<ChangeEvent<Bounds>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType == typeof(BoundsInt))
			{
				field.RegisterCallback<ChangeEvent<BoundsInt>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (fieldType.IsSerializable && !ReflectionUtility.IsTypeCollection(fieldType) && !fieldType.IsPrimitive)
			{
				if (objectValue == null)
				{
					Debug.LogError("You are attempting to register a value on a custom serialized object but the <b>objectValue</b> parameter is not assigned");
					return;
				}

				var nestedFields = fieldType.GetFields();

				foreach (var nestedField in nestedFields)
				{
					RegisterValueChangedCallbackByType(nestedField.FieldType, field, (value) =>
					{
						nestedField.SetValue(objectValue, value);

						valueCallback.Invoke(objectValue);
					});
				}
			}
		}

		/// <summary>
		/// Gets the label of the appropriate field
		/// </summary>
		/// <param name="field">The visual element of the field</param>
		/// <returns>The field label</returns>
		public static string GetFieldLabel(VisualElement field) => field switch
		{
			TextField textField => textField.label,
			IntegerField integerField => integerField.label,
			UnsignedIntegerField unsignedIntegerField => unsignedIntegerField.label,
			LongField longField => longField.label,
			UnsignedLongField unsignedLongField => unsignedLongField.label,
			FloatField floatField => floatField.label,
			DoubleField doubleField => doubleField.label,
			Toggle toggle => toggle.label,
			EnumField enumField => enumField.label,
			Vector2Field vector2Field => vector2Field.label,
			Vector2IntField vector2IntField => vector2IntField.label,
			Vector3Field vector3Field => vector3Field.label,
			Vector3IntField vector3IntField => vector3IntField.label,
			Vector4Field vector4Field => vector4Field.label,
			ColorField colorField => colorField.label,
			GradientField gradientField => gradientField.label,
			CurveField curveField => curveField.label,
			LayerMaskField layerMaskField => layerMaskField.label,
			RectField rectField => rectField.label,
			RectIntField rectIntField => rectIntField.label,
			BoundsField boundsField => boundsField.label,
			BoundsIntField boundsIntField => boundsIntField.label,
			_ => null,
		};

		/// <summary>
		/// Gets the value of the appropriate field
		/// </summary>
		/// <param name="field">The visual element of the field</param>
		/// <returns>The field value</returns>
		public static object GetFieldValue(VisualElement field) => field switch
		{
			TextField textField => textField.value,
			IntegerField integerField => integerField.value,
			UnsignedIntegerField unsignedIntegerField => unsignedIntegerField.value,
			LongField longField => longField.value,
			UnsignedLongField unsignedLongField => unsignedLongField.value,
			FloatField floatField => floatField.value,
			DoubleField doubleField => doubleField.value,
			Toggle toggle => toggle.value,
			EnumField enumField => enumField.value,
			Vector2Field vector2Field => vector2Field.value,
			Vector2IntField vector2IntField => vector2IntField.value,
			Vector3Field vector3Field => vector3Field.value,
			Vector3IntField vector3IntField => vector3IntField.value,
			Vector4Field vector4Field => vector4Field.value,
			ColorField colorField => colorField.value,
			GradientField gradientField => gradientField.value,
			CurveField curveField => curveField.value,
			LayerMaskField layerMaskField => layerMaskField.value,
			RectField rectField => rectField.value,
			RectIntField rectIntField => rectIntField.value,
			BoundsField boundsField => boundsField.value,
			BoundsIntField boundsIntField => boundsIntField.value,
			_ => null,
		};

		/// <summary>
		/// Sets the value of the appropriate field
		/// </summary>
		/// <param name="field">The visual element of the field</param>
		/// <param name="value">The value to set</param>
		/// <param name="notify">Whether to call the value change callback when setting the value</param>
		public static void SetFieldValue(VisualElement field, object value, bool notify = false)
		{
			if (field is TextField textField)
			{
				if (notify)
				{
					textField.value = value.ToString();
				}
				else
				{
					textField.SetValueWithoutNotify(value.ToString());
				}
			}
			else if (field is IntegerField integerField)
			{
				if (notify)
				{
					integerField.value = (int)value;
				}
				else
				{
					integerField.SetValueWithoutNotify((int)value);
				}
			}
			else if (field is UnsignedIntegerField unsignedIntegerField)
			{
				if (notify)
				{
					unsignedIntegerField.value = (uint)value;
				}
				else
				{
					unsignedIntegerField.SetValueWithoutNotify((uint)value);
				}
			}
			else if (field is LongField longField)
			{
				if (notify)
				{
					longField.value = (long)value;
				}
				else
				{
					longField.SetValueWithoutNotify((long)value);
				}
			}
			else if (field is UnsignedLongField unsignedLongField)
			{
				if (notify)
				{
					unsignedLongField.value = (ulong)value;
				}
				else
				{
					unsignedLongField.SetValueWithoutNotify((ulong)value);
				}
			}
			else if (field is FloatField floatField)
			{
				if (notify)
				{
					floatField.value = (float)value;
				}
				else
				{
					floatField.SetValueWithoutNotify((float)value);
				}
			}
			else if (field is DoubleField doubleField)
			{
				if (notify)
				{
					doubleField.value = (double)value;
				}
				else
				{
					doubleField.SetValueWithoutNotify((double)value);
				}
			}
			else if (field is Toggle toggle)
			{
				if (notify)
				{
					toggle.value = (bool)value;
				}
				else
				{
					toggle.SetValueWithoutNotify((bool)value);
				}
			}
			else if (field is EnumField enumField)
			{
				if (notify)
				{
					enumField.value = (Enum)value;
				}
				else
				{
					enumField.SetValueWithoutNotify((Enum)value);
				}
			}
			else if (field is Vector2Field vector2Field)
			{
				if (notify)
				{
					vector2Field.value = (Vector2)value;
				}
				else
				{
					vector2Field.SetValueWithoutNotify((Vector2)value);
				}
			}
			else if (field is Vector2IntField vector2IntField)
			{
				if (notify)
				{
					vector2IntField.value = (Vector2Int)value;
				}
				else
				{
					vector2IntField.SetValueWithoutNotify((Vector2Int)value);
				}
			}
			else if (field is Vector3Field vector3Field)
			{
				if (notify)
				{
					vector3Field.value = (Vector3)value;
				}
				else
				{
					vector3Field.SetValueWithoutNotify((Vector3)value);
				}
			}
			else if (field is Vector3IntField vector3IntField)
			{
				if (notify)
				{
					vector3IntField.value = (Vector3Int)value;
				}
				else
				{
					vector3IntField.SetValueWithoutNotify((Vector3Int)value);
				}
			}
			else if (field is Vector4Field vector4Field)
			{
				if (notify)
				{
					vector4Field.value = (Vector4)value;
				}
				else
				{
					vector4Field.SetValueWithoutNotify((Vector4)value);
				}
			}
			else if (field is ColorField colorField)
			{
				if (notify)
				{
					colorField.value = (Color)value;
				}
				else
				{
					colorField.SetValueWithoutNotify((Color)value);
				}
			}
			else if (field is GradientField gradientField)
			{
				if (notify)
				{
					gradientField.value = (Gradient)value;
				}
				else
				{
					gradientField.SetValueWithoutNotify((Gradient)value);
				}
			}
			else if (field is CurveField curveField)
			{
				if (notify)
				{
					curveField.value = (AnimationCurve)value;
				}
				else
				{
					curveField.SetValueWithoutNotify((AnimationCurve)value);
				}
			}
			else if (field is LayerMaskField layerMaskField)
			{
				if (notify)
				{
					layerMaskField.value = (LayerMask)value;
				}
				else
				{
					layerMaskField.SetValueWithoutNotify((LayerMask)value);
				}
			}
			else if (field is RectField rectField)
			{
				if (notify)
				{
					rectField.value = (Rect)value;
				}
				else
				{
					rectField.SetValueWithoutNotify((Rect)value);
				}
			}
			else if (field is RectIntField rectIntField)
			{
				if (notify)
				{
					rectIntField.value = (RectInt)value;
				}
				else
				{
					rectIntField.SetValueWithoutNotify((RectInt)value);
				}
			}
			else if (field is BoundsField boundsField)
			{
				if (notify)
				{
					boundsField.value = (Bounds)value;
				}
				else
				{
					boundsField.SetValueWithoutNotify((Bounds)value);
				}
			}
			else if (field is BoundsIntField boundsIntField)
			{
				if (notify)
				{
					boundsIntField.value = (BoundsInt)value;
				}
				else
				{
					boundsIntField.SetValueWithoutNotify((BoundsInt)value);
				}
			}
		}

		/// <summary>
		/// Bind a field to the target member value
		/// </summary>
		/// <typeparam name="T">The type of the field</typeparam>
		/// <param name="field">The field visual element</param>
		/// <param name="memberInfo">The member to bind</param>
		/// <param name="targetObject">The target object of the member</param>
		public static void BindFieldToMember<T>(VisualElement field, MemberInfo memberInfo, object targetObject) => BindFieldToMember(typeof(T), field, memberInfo, targetObject);

		/// <summary>
		/// Bind a field to the target member value
		/// </summary>
		/// <param name="fieldType">The type of the field</param>
		/// <param name="field">The field visual element</param>
		/// <param name="memberInfo">The member to bind</param>
		/// <param name="targetObject">The target object of the member</param>
		public static void BindFieldToMember(Type fieldType, VisualElement field, MemberInfo memberInfo, object targetObject)
		{
			UpdateVisualElement(field, () =>
			{
				var memberValue = ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject);

				if (IsTypeValid(fieldType))
				{
					SetFieldValue(field, memberValue);
				}
				else if (!fieldType.IsPrimitive && fieldType.IsSerializable && !ReflectionUtility.IsTypeCollection(fieldType))
				{
					var childFields = field.contentContainer.Children().ToArray();
					var nestedFields = fieldType.GetFields();

					for (int i = 0; i < nestedFields.Length; i++)
					{
						var nestedField = nestedFields[i];

						SetFieldValue(childFields[i], nestedField.GetValue(memberValue));
					}
				}
				else
				{
					Debug.LogError($"Cannot bind to the field to <b>{fieldType}</b>, this type is not supported", (Object)targetObject);
				}
			});

			static bool IsTypeValid(Type type) => type.IsEnum || type == typeof(string) || type == typeof(char)
			|| type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(float) || type == typeof(double) || type == typeof(bool)
			|| type == typeof(Vector2) || type == typeof(Vector2Int) || type == typeof(Vector3) || type == typeof(Vector3Int) || type == typeof(Vector4) || type == typeof(Color)
			|| type == typeof(LayerMask) || type == typeof(Rect) || type == typeof(RectInt) || type == typeof(Bounds) || type == typeof(BoundsInt) || type == typeof(Gradient) || type == typeof(AnimationCurve);
		}

		#region NON_GUI_RELATED_UTILITY_FUNCITONS

		/// <summary>
		/// A short handy version of Debug.Log
		/// </summary>
		/// <param name="message">The message to print</param>
		protected static void Print(object message) => Debug.Log(message);

		/// <summary>
		/// Checks if a collection is null or has no members
		/// </summary>
		/// <param name="collection">The collection to check</param>
		/// <returns>False is the collection is null or has no members, true otherwise</returns>
		public static bool IsCollectionValid(ICollection collection) => collection != null && collection.Count != 0;

		/// <summary>
		/// Gets the size of a 2D texture
		/// </summary>
		/// <param name="texture">The texture to get the size from</param>
		/// <returns>The width and height of the texture as a Vector2</returns>
		public static Vector2 GetTextureSize(Texture2D texture) => new(texture.width, texture.height);

		#endregion
	}
}
