using UnityEngine;
using EditorAttributes;

[CreateAssetMenu(fileName = "AttributeSO")]
public class AttributesExample : ScriptableObject
{
	[Space(20f), Header("EnableField Attribute")]
	[SerializeField] private bool enableFieldCondition;
	[SerializeField, EnableField(nameof(enableFieldCondition))] private int enabledField;
	[SerializeField, EnableField(nameof(enableFieldCondition))] private int[] enabledArray;

	[Space(20f), Header("DisableField Attribute")]
	[SerializeField] private bool disableFieldCondition;
	[SerializeField, DisableField(nameof(disableFieldCondition))] private int disabledField;
	[SerializeField, DisableField(nameof(disableFieldCondition))] private int[] disabledArray;

	[Space(20f), Header("DisableInPlaymode Attribute")]
	[SerializeField, DisableInPlayMode] private int thisWillBeDisabledInPlayMode;

	[Space(20f), Header("Readonly Attribute")]
	[SerializeField, ReadOnly] private int readonlyField;

	[Space(20f), Header("HideField Attribute")]
	[SerializeField] private bool hideFieldCondition;
	[SerializeField, HideField(nameof(hideFieldCondition))] private int hiddenField;

	[Space(20f), Header("ShowField Attribute")]
	[SerializeField] private bool showFieldCondition;
	[SerializeField, ShowField(nameof(showFieldCondition))] private int shownField;

	[Space(20f), Header("HideInPlaymode Attribute")]
	[SerializeField, HideInPlayMode] private int thisWillBeHiddenInPlayMode;

	[Space(20f), Header("ConditionalField Attribute")]
	[SerializeField] private bool firstCondition;
	[SerializeField] private bool secondCondition;
	[Space]
	[SerializeField, ConditionalField(ConditionType.AND, nameof(firstCondition), nameof(secondCondition))] private int conditionalFieldAND;
	[SerializeField, ConditionalField(ConditionType.OR, nameof(firstCondition), nameof(secondCondition))] private int conditionalFieldOR;
	[SerializeField, ConditionalField(ConditionType.NAND, nameof(firstCondition), nameof(secondCondition))] private int conditionalFieldNAND;
	[SerializeField, ConditionalField(ConditionType.NOR, nameof(firstCondition), nameof(secondCondition))] private int conditionalFieldNOR;
	[SerializeField, ConditionalField(ConditionType.AND, new bool[2] { true, false }, nameof(firstCondition), nameof(secondCondition))] private int conditionalFieldANDNegated;

	[Space(20f), Header("Dropdown Attribute")]
	[SerializeField, Dropdown(nameof(dropdownValues))] private string stringDropdown;
	[SerializeField] private string[] dropdownValues;

	[Space(20f), Header("HorizontalGroup Attribute")]
	[SerializeField, HorizontalGroup(20f, 50f, nameof(x), nameof(y), nameof(z))] private Void horizonalGroup;
	[SerializeField, HideInInspector] private string x;
	[SerializeField, HideInInspector] private string y;
	[SerializeField, HideInInspector] private string z;

	[Space(20f), Header("HelpBox Attribute")]
	[SerializeField, HelpBox("This is a help box", true, MessageMode.None)] private int helpBox;
	[SerializeField, HelpBox("This is a log help box", true, MessageMode.Log)] private int helpBoxLog;
	[SerializeField, HelpBox("This is a warning help box", true, MessageMode.Warning)] private int helpBoxWarning;
	[SerializeField, HelpBox("This is a error help box", true, MessageMode.Error)] private int helpBoxError;
	[SerializeField, HelpBox("This is a log help box with the field hidden", false, MessageMode.Log)] private Void helpBoxLogHidden;

	[Space(20f), Header("MessageBox Attribute")]
	[SerializeField] private bool showMessageBox;
	[SerializeField, MessageBox("This is a message box", nameof(showMessageBox), true, MessageMode.None)] private int messageBox;
	[SerializeField, MessageBox("This is a log message box", nameof(showMessageBox), true, MessageMode.Log)] private int messageBoxLog;
	[SerializeField, MessageBox("This is a warning message box", nameof(showMessageBox), true, MessageMode.Warning)] private int messageBoxWarning;
	[SerializeField, MessageBox("This is a error message box", nameof(showMessageBox), true, MessageMode.Error)] private int messageBoxError;
	[SerializeField, MessageBox("This is a log message box with the field hidden", nameof(showMessageBox), false, MessageMode.Log)] private Void messageBoxLogHidden;

	[Space(20f), Header("Button Attribute")]
    [SerializeField, Button(nameof(FunctionButton))] private Void functionButton;

    public void FunctionButton() => Debug.Log("Button Pressed");
}
