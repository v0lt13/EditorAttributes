using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/validate.html")]
	public class ValidateSample : MonoBehaviour
	{
		[Header("Validate Attribute:")]
		[SerializeField, Validate("The field must be above zero", nameof(MustBeAboveZero))] private int intField;
		[SerializeField, Validate("String can't be empty", nameof(CantBeEmpty), MessageMode.Warning)] private string stringField;

		[SerializeField, Validate(nameof(AdvancedValidation))] private float floatField;

		private bool MustBeAboveZero() => intField <= 0;
		private bool CantBeEmpty => stringField == string.Empty;

		private ValidationCheck AdvancedValidation()
		{
			if (floatField <= 0)
			{
				return ValidationCheck.Fail("The value must be above zero", MessageMode.Error);
			}
			else if (floatField >= 100)
			{
				return ValidationCheck.Fail("The value must be less than 100", MessageMode.Warning);
			}

			return ValidationCheck.Pass();
		}
	}
}
