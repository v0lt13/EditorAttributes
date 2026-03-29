using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
    [HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/validate.html")]
    public class ValidateSample : MonoBehaviour
    {
        [Header("Validate Attribute:")]
        [Validate("The field must be above zero", nameof(MustBeAboveZero))]
        [SerializeField] private int intField;

        [Validate("String can't be empty", nameof(CantBeEmpty), MessageMode.Warning)]
        [SerializeField] private string stringField;

        [Validate(nameof(AdvancedValidation), applyToCollection: false)]
        [SerializeField] private float[] floatField;

        private bool MustBeAboveZero() => intField <= 0;
        private bool CantBeEmpty => stringField == string.Empty;

        private ValidationCheck AdvancedValidation(int index)
        {
            if (floatField[index] <= 0)
            {
                return ValidationCheck.Fail("The value must be above zero", MessageMode.Error);
            }
            else if (floatField[index] >= 100)
            {
                return ValidationCheck.Fail("The value must be less than 100", MessageMode.Warning);
            }

            return ValidationCheck.Pass();
        }
    }
}
