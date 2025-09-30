using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using FilePath = UnityEditor.FilePathAttribute;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
	[@FilePath("ProjectSettings/EditorAttributes/EditorAttributesSettings.asset", FilePath.Location.ProjectFolder)]
	internal class EditorAttributesSettings : ScriptableSingleton<EditorAttributesSettings>
	{
		[Tooltip("Disables automatic validation when building the project")]
		[SerializeField] internal bool disableBuildValidation;

		[Tooltip("Time in milliseconds to wait for the asset preview to load, increase this value if the previews are not showing up")]
		[SerializeField, Suffix("ms")] internal int assetPreviewLoadTime = 20;

		[Space, Tooltip("Define custom units for use with UnitField Attribute")]
		[SerializeField, DataTable] internal UnitDefinition[] customUnitDefinitions;

		[MessageBox(nameof(messageBoxText), nameof(CheckValidUnitDefinitions), MessageMode.Warning, StringInputMode.Dynamic)]
		[SerializeField] private Void messageBoxHolder;

		private string messageBoxText;

		void OnValidate() => UnitConverter.UNIT_CONVERSION_MAP = UnitConverter.GenerateConversionMap();

		internal void AddCustomDefinitions()
		{
			var unitDefinitions = UnitConverter.UNIT_DEFINITIONS;

			unitDefinitions.RemoveWhere((unitDefinition) => unitDefinition.unit == Unit.Custom);

			if (customUnitDefinitions == null)
				return;

			foreach (var customUnitDefinition in customUnitDefinitions)
			{
				if (string.IsNullOrWhiteSpace(customUnitDefinition.unitName) || string.IsNullOrWhiteSpace(customUnitDefinition.unitLabel))
					continue;

				if (customUnitDefinition.category != UnitCategory.Custom)
				{
					customUnitDefinition.categoryName = customUnitDefinition.category.ToString();
				}
				else if (string.IsNullOrWhiteSpace(customUnitDefinition.unitName))
				{
					continue;
				}
			}

			unitDefinitions.UnionWith(customUnitDefinitions);
		}

		internal void SaveSettings() => Save(true);

		private bool CheckValidUnitDefinitions()
		{
			foreach (var customUnitDefinition in customUnitDefinitions)
			{
				if (string.IsNullOrWhiteSpace(customUnitDefinition.unitName))
				{
					messageBoxText = "Custom unit name cannot be empty";
					return true;
				}

				if (customUnitDefinition.category == UnitCategory.Custom && string.IsNullOrWhiteSpace(customUnitDefinition.categoryName))
				{
					messageBoxText = "Custom unit category name cannot be empty";
					return true;
				}

				if (string.IsNullOrWhiteSpace(customUnitDefinition.unitLabel))
				{
					messageBoxText = "Custom unit label cannot be empty";
					return true;
				}
			}

			messageBoxText = string.Empty;
			return false;
		}
	}

	internal class EditorAttributesSettingsProvider : SettingsProvider
	{
		internal EditorAttributesSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			var serializedObject = new SerializedObject(EditorAttributesSettings.instance);
			var settingsContainer = new VisualElement();

			var header = new Label("Editor Attributes")
			{
				style =
				{
					fontSize = 19f,
					unityFontStyleAndWeight = FontStyle.Bold,
					justifyContent = Justify.FlexEnd,
					height = 19f,
					marginTop = 1f,
					marginLeft = 9f,
					marginBottom = 12f
				}
			};

			var helpButton = new Button(() => Application.OpenURL("https://editorattributesdocs.readthedocs.io/en/latest/GettingStarted/editorattributessettings.html"))
			{
				tooltip = "Open reference for EditorAttributes Settings.",
				style =
				{
					borderTopLeftRadius = 0f, borderTopRightRadius = 0f, borderBottomLeftRadius = 0f, borderBottomRightRadius = 0f,
					borderTopWidth = 0f, borderBottomWidth = 0f, borderLeftWidth = 0f, borderRightWidth = 0f,
					paddingBottom = 0f, paddingTop = 0f, paddingLeft = 0f, paddingRight = 0f,
					marginTop = 0f, marginBottom = 0f, marginLeft = 0f, marginRight = 0f,
					width = 16.6f, height = 16.6f,
					alignSelf = Align.FlexEnd,
					backgroundColor = Color.clear,
					backgroundImage = (StyleBackground)EditorGUIUtility.IconContent("d__Help@2x").image
				}
			};

			helpButton.RegisterCallback<MouseOverEvent>((callack) => helpButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f));
			helpButton.RegisterCallback<MouseOutEvent>((callack) => helpButton.style.backgroundColor = Color.clear);

			var inspectorElement = new InspectorElement(serializedObject);

			inspectorElement.Q<ObjectField>("unity-input-m_Script").parent.RemoveFromHierarchy(); // Remove the auto-generated script field

			var clearParamsButton = new Button(() => ButtonDrawer.ClearAllParamsData())
			{
				text = "Delete Buttons Parameter Data",
				tooltip = "Deletes all buttons parameter data stored in ProjectSettings/EditorAttributes",
				style = { marginTop = 10f }
			};

			header.Add(helpButton);

			settingsContainer.Add(inspectorElement);
			settingsContainer.Add(clearParamsButton);

			rootElement.Add(header);
			rootElement.Add(settingsContainer);
		}

		public override void OnDeactivate() => EditorAttributesSettings.instance.SaveSettings();

		[SettingsProvider]
		internal static SettingsProvider CreateSettingsProvider() => new EditorAttributesSettingsProvider("Project/EditorAttributes", SettingsScope.Project)
		{
			keywords = new HashSet<string>(new[] { "EditorAttributes", "Editor", "Attributes" })
		};
	}
}
