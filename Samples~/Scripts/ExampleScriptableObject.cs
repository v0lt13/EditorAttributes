using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace EditorAttributesSamples
{
	[CreateAssetMenu(fileName = "ExampleScriptableObject", menuName = "ScriptableObjects/ExampleScriptableObject")]
	public class ExampleScriptableObject : ScriptableObject
	{
		public enum Season { Winter, Spring, Summer, Fall }

		[Serializable]
		public class PlayerClass
		{
			public string className;
			[Range(0f, 100f)] public float playerHealth;
			public GameObject playerPrefab;
		}

		[Serializable]
		public class EnemyData
		{
			public string enemyName;
			[Range(0, 100)] public int enemyHealth;
			public float enemyDamage;
			public GameObject enemyPrefab;
		}

		[Title("General Settings")]
		public string gameName;
		public bool isMultiplayer;
		[ShowField(nameof(isMultiplayer)), Clamp(0, 5), IndentProperty(20f)] 
		public int maxPlayers;
		[Space]
		[Title("Entity Settings", titleSpace: 20f)]
		[SerializeField] private Void titleHolder;

		[HelpBox("Will apply randomly to each player in the list", MessageMode.None)]
		[MinMaxSlider(0, 100)] public Vector2Int startingCurrency;

		[DataTable] public PlayerClass[] playerClasses;
		[Space]
		[DataTable(true)] public EnemyData[] enemies;

		[Title("Level Settings")]
#if UNITY_6000_OR_NEWER
		[EnumButtons]
#else
		[SelectionButtons]
#endif
		public Season season;
		[Suffix(nameof(GetTimeOfDay), stringInputMode: StringInputMode.Dynamic), TimeField(TimeFormat.HourMinuteSecond, ConvertTo.Minutes)] 
		public float timeOfDay;
		[Space]
		[Dropdown(nameof(GetAudioClips))] public string backgroundMusic;
		[AssetPreview] public Sprite levelBackground;

		private string[] GetAudioClips() => new string[] { "Music/BackgroundMusic1", "Music/BackgroundMusic2", "SFX/Explosion" };
		private string GetTimeOfDay() => $"{timeOfDay} minutes";
	}
}
