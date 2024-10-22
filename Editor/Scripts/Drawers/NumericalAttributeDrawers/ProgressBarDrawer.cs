using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.Float)
			{
				var progressBarAttribute = attribute as ProgressBarAttribute;

				var progressBar = new ProgressBar { highValue = progressBarAttribute.MaxValue };

				// Make the visual elements in the progress bar grow
				progressBar.ElementAt(0).style.flexGrow = 1f;
				progressBar.ElementAt(0).ElementAt(0).style.flexGrow = 1f;
				
				progressBar.style.height = progressBarAttribute.BarHeight;

				progressBar.schedule.Execute(() =>
				{
					if (CanApplyGlobalColor)
						progressBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
				}).ExecuteLater(1);

				UpdateVisualElement(root, () =>
				{
					var propertyValue = GetPropertyValue(property);

					progressBar.value = propertyValue;
					progressBar.title = $"{property.displayName}: {propertyValue}/{progressBarAttribute.MaxValue}";
				});

				root.Add(progressBar);
			}
			else
			{
				root.Add(new HelpBox("The ProgressBar Attribute can only be attached to an int or float", HelpBoxMessageType.Error));
			}

			return root;
		}

		private float GetPropertyValue(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => property.intValue,
				SerializedPropertyType.Float => property.floatValue,
				_ => 0f
			};
		}
	}
}
