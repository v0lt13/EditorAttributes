using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace EditorAttributesSamples
{
	[CreateAssetMenu(fileName = "ExampleScriptableObject", menuName = "ScriptableObjects/ExampleScriptableObject")]
	public class ExampleScriptableObject : ScriptableObject
	{
		public enum AmmoType
		{
			Bullets,
			Arrows,
			EnergyCells
		}

		[Serializable]
		public class ItemData
		{
			public string itemName;
			[SelectionButtons] public ItemType itemType;
			[AssetPreview(64f, 64f)] public Sprite itemIcon;

			[Line(GUIColor.Blue, 0.5f)]
			[Clamp(0, 100), Prefix("Coins:", -100f)] public int itemPrice;
			[TextArea(3, 5)] public string itemDescription;

			[Line(GUIColor.Cyan, 0.5f)]
			public bool isPurchasable;

			[ShowField(nameof(isPurchasable)), IndentProperty, MinMaxSlider(0, 100)] 
			public Vector2Int priceRange;

			[Line(GUIColor.Magenta)]
			[ShowField(nameof(itemType), ItemType.Weapon), ColorField("#e3a6ff"), DataTable(true)] 
			public WeaponData weaponData;

			[ShowField(nameof(itemType), ItemType.Consumable), ColorField("#ffa6f0"), DataTable(true)] 
			public ConsumableData consumableData;

			public enum ItemType
			{
				Weapon,
				Consumable
			}
		}

		[Serializable]
		public class ItemCategory
		{
			public string categoryName;
			[ColorField("#a6ffe1")] public ItemData[] items;
		}

		[Serializable]
		public class WeaponData
		{
			public int damage;
			public float attackSpeed;
			public bool isRanged;
			[ShowField(nameof(isRanged))] public AmmoType ammoType;
		}

		[Serializable]
		public class ConsumableData
		{
			public EffectType effectType;
			[Wrap(0, 100)] public int effectValue;

			public enum EffectType
			{
				Health,
				Stamina
			}
		}

		[GUIColor("#fffea6"), Title("Item Config", 30, alignment: TextAnchor.MiddleCenter)]
		[FoldoutGroup("Currency Settings", true, nameof(currency), nameof(currencyIcon))]
		[SerializeField] private Void currencyFoldout;

		[Dropdown(nameof(currencyTypes))] 
		[HideInInspector] public string currency;

		[AssetPreview(64f, 64f)] 
		[HideInInspector] public Sprite currencyIcon;

		[GUIColor("#a6c8ff")] 
		public ItemCategory[] itemCategories;

		private string[] currencyTypes = new string[] { "Gold", "Silver", "Bronze" };
	}
}
