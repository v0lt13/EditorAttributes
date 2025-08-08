using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor.Utility
{
	public static class ColorUtils
	{
		/// <summary>
		/// Applies a color to a visual element via the color attribute
		/// </summary>
		/// <param name="visualElement">The visual element to color</param>
		/// <param name="color">The color attribute</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		public static void ApplyColor(VisualElement visualElement, IColorAttribute color, HelpBox errorBox)
		{
			if (color.Color == GUIColor.Default && !color.UseRGB && string.IsNullOrEmpty(color.HexColor))
				return;

			visualElement.schedule.Execute(() =>
			{
				var labels = visualElement.Query<Label>().ToList();

				foreach (var label in labels)
					label.style.color = GetColorFromAttribute(color, errorBox);

				var textElements = visualElement.Query<TextElement>().ToList();

				foreach (var textElement in textElements)
					textElement.style.color = GetColorFromAttribute(color, errorBox);

				var scrollviews = visualElement.Query<ScrollView>(className: "unity-collection-view__scroll-view").ToList();

				foreach (var scrollview in scrollviews)
					scrollview.style.backgroundColor = GetColorFromAttribute(color, errorBox) / 3f;

				var inputFields = visualElement.Query(className: "unity-base-field__input").ToList();

				foreach (var inputField in inputFields)
					inputField.style.backgroundColor = GetColorFromAttribute(color, errorBox) / 3f;

				var checkMarks = visualElement.Query(className: "unity-toggle__checkmark").ToList();

				foreach (var checkMark in checkMarks)
				{
					checkMark.style.unityBackgroundImageTintColor = GetColorFromAttribute(color, errorBox);
					checkMark.parent.style.backgroundColor = StyleKeyword.Initial;
				}

			}).ExecuteLater(50);
		}

		/// <summary>
		/// Applies a color to a visual element
		/// </summary>
		/// <param name="visualElement">The visual element to color</param>
		/// <param name="color">The color to apply</param>
		/// <param name="delay">How many milliseconds to delay before applying the color</param>
		public static void ApplyColor(VisualElement visualElement, Color color, int delay = 50)
		{
			visualElement.schedule.Execute(() =>
			{
				var labels = visualElement.Query<Label>().ToList();

				foreach (var label in labels)
					label.style.color = color;

				var textElements = visualElement.Query<TextElement>().ToList();

				foreach (var textElement in textElements)
					textElement.style.color = color;

				var scrollviews = visualElement.Query<ScrollView>(className: "unity-collection-view__scroll-view").ToList();

				foreach (var scrollview in scrollviews)
					scrollview.style.backgroundColor = color / 3f;

				var inputFields = visualElement.Query(className: "unity-base-field__input").ToList();

				foreach (var inputField in inputFields)
					inputField.style.backgroundColor = color / 3f;

				var checkMarks = visualElement.Query(className: "unity-toggle__checkmark").ToList();

				foreach (var checkMark in checkMarks)
				{
					checkMark.style.unityBackgroundImageTintColor = color;
					checkMark.parent.style.backgroundColor = StyleKeyword.Initial;
				}

			}).ExecuteLater(delay);
		}

		internal static Color? GetPropertyColor(SerializedProperty property)
		{
			var propertyColor = GetColorFromProperty(property);

			if (propertyColor.HasValue)
			{
				return propertyColor.Value;
			}
			else if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
			{
				return EditorExtension.GLOBAL_COLOR;
			}

			return null;
		}

		internal static Color? GetPropertyColor(SerializedProperty property, float customAlpha)
		{
			var propertyColor = GetColorFromProperty(property);

			if (propertyColor.HasValue)
			{
				return new Color(propertyColor.Value.r, propertyColor.Value.g, propertyColor.Value.b, customAlpha);
			}
			else if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
			{
				return new Color(EditorExtension.GLOBAL_COLOR.r, EditorExtension.GLOBAL_COLOR.g, EditorExtension.GLOBAL_COLOR.b, customAlpha);
			}

			return null;
		}

		/// <summary>
		/// Gets the color from a serialzied property with a color attribute
		/// </summary>
		/// <param name="property">The property to get the color from</param>
		/// <returns>The color from the attribute, null if the attribute is not found</returns>
		public static Color? GetColorFromProperty(SerializedProperty property)
		{
			var field = ReflectionUtility.FindField(property.name, property);

			IColorAttribute colorAttribute = field?.GetCustomAttribute<GUIColorAttribute>();
#pragma warning disable CS0618
			colorAttribute ??= field?.GetCustomAttribute<ColorFieldAttribute>();

			return colorAttribute != null ? GetColorFromAttribute(colorAttribute, new HelpBox()) : null;
		}

		/// <summary>
		/// Gets the color value from a color attribute
		/// </summary>
		/// <param name="attribute">The color attribute</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>The color from the attribute</returns>
		public static Color GetColorFromAttribute(IColorAttribute attribute, HelpBox errorBox) => GetColorFromAttribute(attribute, 1f, errorBox);

		/// <summary>
		/// Gets the color value from a color attribute with custom alpha
		/// </summary>
		/// <param name="attribute">The color attribute</param>
		/// <param name="alpha">Custom transparency value</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>The color from the attribute</returns>
		public static Color GetColorFromAttribute(IColorAttribute attribute, float alpha, HelpBox errorBox)
		{
			if (ColorUtility.TryParseHtmlString(attribute.HexColor, out Color color))
			{
				color.a = alpha;
				return color;
			}
			else if (!string.IsNullOrEmpty(attribute.HexColor))
			{
				errorBox.text = $"The provided value <b>{attribute.HexColor}</b> is not a valid Hex color";
			}

			return ColorAttributeToColor(attribute, alpha);
		}

		/// <summary>
		/// Converts the color attribute values from the color attribute to a color
		/// </summary>
		/// <param name="colorAttribute">The color attribute</param>
		/// <returns>The color value</returns>
		public static Color ColorAttributeToColor(IColorAttribute colorAttribute) => ColorAttributeToColor(colorAttribute, 1f);

		/// <summary>
		/// Converts the color attribute values from the color attribute to a color
		/// </summary>
		/// <param name="colorAttribute">The color attribute</param>
		/// <param name="alpha">Custom transparency value</param>
		/// <returns>The color value</returns>
		public static Color ColorAttributeToColor(IColorAttribute colorAttribute, float alpha)
		{
			if (colorAttribute.UseRGB)
				return new(colorAttribute.R / 255f, colorAttribute.G / 255f, colorAttribute.B / 255f, alpha);

			return GUIColorToColor(colorAttribute.Color, alpha);
		}

		/// <summary>
		/// Converts the GUIColor value to a color
		/// </summary>
		/// <param name="color">The GUIColor</param>
		/// <returns>The color value</returns>
		public static Color GUIColorToColor(GUIColor color) => GUIColorToColor(color, 1f);

		/// <summary>
		/// Converts the GUIColor value to a color
		/// </summary>
		/// <param name="color">The GUIColor</param>
		/// <param name="alpha">Custom transparency value</param>
		/// <returns>The color value</returns>
		public static Color GUIColorToColor(GUIColor color, float alpha)
		{
			return color switch
			{
				GUIColor.White => new(Color.white.r, Color.white.g, Color.white.b, alpha),
				GUIColor.Black => new(Color.black.r, Color.black.g, Color.black.b, alpha),
				GUIColor.Gray => new(Color.gray.r, Color.gray.g, Color.gray.b, alpha),
				GUIColor.Red => new(Color.red.r, Color.red.g, Color.red.b, alpha),
				GUIColor.Green => new(Color.green.r, Color.green.g, Color.green.b, alpha),
				GUIColor.Blue => new(Color.blue.r, Color.blue.g, Color.blue.b, alpha),
				GUIColor.Cyan => new(Color.cyan.r, Color.cyan.g, Color.cyan.b, alpha),
				GUIColor.Magenta => new(Color.magenta.r, Color.magenta.g, Color.magenta.b, alpha),
				GUIColor.Yellow => new(Color.yellow.r, Color.yellow.g, Color.yellow.b, alpha),
				GUIColor.Orange => new(1f, 149f / 255f, 0f, alpha),
				GUIColor.Brown => new(161f / 255f, 62f / 255f, 0f, alpha),
				GUIColor.Purple => new(158f / 255f, 5f / 255f, 247f / 255f, alpha),
				GUIColor.Pink => new(247f / 255f, 5f / 255f, 171f / 255f, alpha),
				GUIColor.Lime => new(145f / 255f, 1f, 0f, alpha),
				_ => new(EditorExtension.DEFAULT_GLOBAL_COLOR.r, EditorExtension.DEFAULT_GLOBAL_COLOR.g, EditorExtension.DEFAULT_GLOBAL_COLOR.b, alpha)
			};
		}
	}
}
