using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[CreateAssetMenu(fileName = "ExampleScriptableObject", menuName = "ScriptableObjects/ExampleScriptableObject")]
	public class ExampleScriptableObject : ScriptableObject
	{
		[GUIColor(GUIColor.Yellow)]
		[Title("Currency Settings")]
		[Dropdown(nameof(currencies))] public string currency;
		[AssetPreview] public Sprite currencyIcon;

		[GUIColor(GUIColor.Lime)]
		[Title("Item Data")]
		public string itemName;
		[SelectionButtons] public ItemType itemType;
		[Required] public Sprite itemIcon;
		[TextArea] public string itemDescription;
		public bool isPurchasable;
		[ShowField(nameof(isPurchasable)), MinMaxSlider(0, 100)] public Vector2Int priceRange;

		[GUIColor(GUIColor.Magenta)]
		[Title("Specific Item Properties", 15, true, TextAnchor.MiddleCenter)]
		[Clamp(0, 64)] public int itemStack;
		[ShowField(nameof(itemType), ItemType.Weapon), Min(0)] public int damage;
		[ShowField(nameof(itemType), ItemType.Weapon), Min(0)] public float attackSpeed;
		[ShowField(nameof(itemType), ItemType.Weapon)] public bool isRanged;
		[ShowField(nameof(itemType), ItemType.Consumable), SelectionButtons] public EffectType consumableEffect;

		public enum ItemType
		{
			General,
			Weapon,
			Consumable
		}

		public enum EffectType
		{
			Health,
			Stamina
		}


		private string[] currencies = new string[] { "Gold", "Gems", "Pebbles" };
	}
}