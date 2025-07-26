using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display a numerical field as a specified unit and convert it to another unit
	/// </summary>
	public class UnitFieldAttribute : PropertyAttribute
	{
		public string DisplayUnit { get; private set; }
		public string ConversionUnit { get; private set; }

        /// <summary>
        /// Attribute to display a numerical field as a specified unit and convert it to another unit
        /// </summary>
        /// <param name="displayUnit">The unit to display in the inspector</param>
        /// <param name="conversionUnit">The unit to convert to</param>
        public UnitFieldAttribute(Unit displayUnit, Unit conversionUnit)
		{
			DisplayUnit = displayUnit.ToString();
			ConversionUnit = conversionUnit.ToString();
		}

		/// <summary>
		/// Attribute to display a numerical field as a specified unit and convert it to another unit
		/// </summary>
		/// <param name="customDisplayUnit">The custom unit to display in the inspector</param>
		/// <param name="conversionUnit">The unit to convert to</param>
		public UnitFieldAttribute(string customDisplayUnit, Unit conversionUnit)
		{
			DisplayUnit = customDisplayUnit;
			ConversionUnit = conversionUnit.ToString();
		}

		/// <summary>
		/// Attribute to display a numerical field as a specified unit and convert it to another unit
		/// </summary>
		/// <param name="displayUnit">The unit to display in the inspector</param>
		/// <param name="customConversionUnit">The custom unit to convert to</param>
		public UnitFieldAttribute(Unit displayUnit, string customConversionUnit)
		{
			DisplayUnit = displayUnit.ToString();
			ConversionUnit = customConversionUnit;
		}

		/// <summary>
		/// Attribute to display a numerical field as a specified unit and convert it to another unit
		/// </summary>
		/// <param name="customDisplayUnit">The custom unit to display in the inspector</param>
		/// <param name="customConversionUnit">The custom unit to convert to</param>
		public UnitFieldAttribute(string customDisplayUnit, string customConversionUnit)
		{
			DisplayUnit = customDisplayUnit;
			ConversionUnit = customConversionUnit;
		}
	}
}
