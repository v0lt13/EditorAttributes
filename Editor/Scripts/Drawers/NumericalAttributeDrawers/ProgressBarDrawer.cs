using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType is not (SerializedPropertyType.Integer or SerializedPropertyType.Float))
                return new HelpBox("The ProgressBar Attribute can only be attached to an int or float", HelpBoxMessageType.Error);

            var progressBarAttribute = attribute as ProgressBarAttribute;

            ProgressBar progressBar = new() { highValue = progressBarAttribute.MaxValue, tooltip = property.tooltip };

            // Make the visual elements in the progress bar grow
            progressBar.ElementAt(0).style.flexGrow = 1f;
            progressBar.ElementAt(0).ElementAt(0).style.flexGrow = 1f;

            progressBar.style.height = progressBarAttribute.BarHeight;

            progressBar.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                if (CanApplyGlobalColor)
                    progressBar.Q(className: AbstractProgressBar.progressUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
            });

            UpdateVisualElement(progressBar, () =>
            {
                var propertyValue = GetPropertyValue(property);

                progressBar.value = propertyValue;
                progressBar.title = $"{property.displayName}: {propertyValue}/{progressBarAttribute.MaxValue}";
            }, 30L);

            return progressBar;
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
