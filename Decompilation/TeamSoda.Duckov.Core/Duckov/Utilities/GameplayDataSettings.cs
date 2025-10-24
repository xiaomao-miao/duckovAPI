using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Duckov.Achievements;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Crops;
using Duckov.Quests;
using Duckov.Quests.Relations;
using Duckov.UI;
using Eflatun.SceneReference;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Duckov.Utilities
{
	// Token: 0x020003F7 RID: 1015
	[CreateAssetMenu(menuName = "Settings/Gameplay Data Settings")]
	public class GameplayDataSettings : ScriptableObject
	{
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x0007F287 File Offset: 0x0007D487
		private static GameplayDataSettings Default
		{
			get
			{
				if (GameplayDataSettings.cachedDefault == null)
				{
					GameplayDataSettings.cachedDefault = Resources.Load<GameplayDataSettings>("GameplayDataSettings");
				}
				return GameplayDataSettings.cachedDefault;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060024B5 RID: 9397 RVA: 0x0007F2AA File Offset: 0x0007D4AA
		public static InputActionAsset InputActions
		{
			get
			{
				return GameplayDataSettings.Default.inputActions;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x0007F2B6 File Offset: 0x0007D4B6
		public static CustomFaceData CustomFaceData
		{
			get
			{
				return GameplayDataSettings.Default.customFaceData;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x0007F2C2 File Offset: 0x0007D4C2
		public static GameplayDataSettings.TagsData Tags
		{
			get
			{
				return GameplayDataSettings.Default.tags;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x0007F2CE File Offset: 0x0007D4CE
		public static GameplayDataSettings.PrefabsData Prefabs
		{
			get
			{
				return GameplayDataSettings.Default.prefabs;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060024B9 RID: 9401 RVA: 0x0007F2DA File Offset: 0x0007D4DA
		public static UIPrefabsReference UIPrefabs
		{
			get
			{
				return GameplayDataSettings.Default.uiPrefabs;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x0007F2E6 File Offset: 0x0007D4E6
		public static GameplayDataSettings.ItemAssetsData ItemAssets
		{
			get
			{
				return GameplayDataSettings.Default.itemAssets;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060024BB RID: 9403 RVA: 0x0007F2F2 File Offset: 0x0007D4F2
		public static GameplayDataSettings.StringListsData StringLists
		{
			get
			{
				return GameplayDataSettings.Default.stringLists;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x0007F2FE File Offset: 0x0007D4FE
		public static GameplayDataSettings.LayersData Layers
		{
			get
			{
				return GameplayDataSettings.Default.layers;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060024BD RID: 9405 RVA: 0x0007F30A File Offset: 0x0007D50A
		public static GameplayDataSettings.SceneManagementData SceneManagement
		{
			get
			{
				return GameplayDataSettings.Default.sceneManagement;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x0007F316 File Offset: 0x0007D516
		public static GameplayDataSettings.BuffsData Buffs
		{
			get
			{
				return GameplayDataSettings.Default.buffs;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x0007F322 File Offset: 0x0007D522
		public static GameplayDataSettings.QuestsData Quests
		{
			get
			{
				return GameplayDataSettings.Default.quests;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x0007F32E File Offset: 0x0007D52E
		public static QuestCollection QuestCollection
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestCollection;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060024C1 RID: 9409 RVA: 0x0007F33F File Offset: 0x0007D53F
		public static QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestRelation;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060024C2 RID: 9410 RVA: 0x0007F350 File Offset: 0x0007D550
		public static GameplayDataSettings.EconomyData Economy
		{
			get
			{
				return GameplayDataSettings.Default.economyData;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x0007F35C File Offset: 0x0007D55C
		public static GameplayDataSettings.UIStyleData UIStyle
		{
			get
			{
				return GameplayDataSettings.Default.uiStyleData;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x0007F368 File Offset: 0x0007D568
		public static BuildingDataCollection BuildingDataCollection
		{
			get
			{
				return GameplayDataSettings.Default.buildingDataCollection;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x0007F374 File Offset: 0x0007D574
		public static CraftingFormulaCollection CraftingFormulas
		{
			get
			{
				return GameplayDataSettings.Default.craftingFormulas;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0007F380 File Offset: 0x0007D580
		public static DecomposeDatabase DecomposeDatabase
		{
			get
			{
				return GameplayDataSettings.Default.decomposeDatabase;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x0007F38C File Offset: 0x0007D58C
		public static StatInfoDatabase StatInfo
		{
			get
			{
				return GameplayDataSettings.Default.statInfo;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x0007F398 File Offset: 0x0007D598
		public static StockShopDatabase StockshopDatabase
		{
			get
			{
				return GameplayDataSettings.Default.stockShopDatabase;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x0007F3A4 File Offset: 0x0007D5A4
		public static GameplayDataSettings.LootingData Looting
		{
			get
			{
				return GameplayDataSettings.Default.looting;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060024CA RID: 9418 RVA: 0x0007F3B0 File Offset: 0x0007D5B0
		public static AchievementDatabase AchievementDatabase
		{
			get
			{
				return GameplayDataSettings.Default.achivementDatabase;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x0007F3BC File Offset: 0x0007D5BC
		public static CropDatabase CropDatabase
		{
			get
			{
				return GameplayDataSettings.Default.cropDatabase;
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0007F3C8 File Offset: 0x0007D5C8
		internal static Sprite GetSprite(string key)
		{
			return GameplayDataSettings.Default.spriteData.GetSprite(key);
		}

		// Token: 0x040018F4 RID: 6388
		private static GameplayDataSettings cachedDefault;

		// Token: 0x040018F5 RID: 6389
		[SerializeField]
		private GameplayDataSettings.TagsData tags;

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		private GameplayDataSettings.PrefabsData prefabs;

		// Token: 0x040018F7 RID: 6391
		[SerializeField]
		private UIPrefabsReference uiPrefabs;

		// Token: 0x040018F8 RID: 6392
		[SerializeField]
		private GameplayDataSettings.ItemAssetsData itemAssets;

		// Token: 0x040018F9 RID: 6393
		[SerializeField]
		private GameplayDataSettings.StringListsData stringLists;

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		private GameplayDataSettings.LayersData layers;

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private GameplayDataSettings.SceneManagementData sceneManagement;

		// Token: 0x040018FC RID: 6396
		[SerializeField]
		private GameplayDataSettings.BuffsData buffs;

		// Token: 0x040018FD RID: 6397
		[SerializeField]
		private GameplayDataSettings.QuestsData quests;

		// Token: 0x040018FE RID: 6398
		[SerializeField]
		private GameplayDataSettings.EconomyData economyData;

		// Token: 0x040018FF RID: 6399
		[SerializeField]
		private GameplayDataSettings.UIStyleData uiStyleData;

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private InputActionAsset inputActions;

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		private BuildingDataCollection buildingDataCollection;

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		private CustomFaceData customFaceData;

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		private CraftingFormulaCollection craftingFormulas;

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		private DecomposeDatabase decomposeDatabase;

		// Token: 0x04001905 RID: 6405
		[SerializeField]
		private StatInfoDatabase statInfo;

		// Token: 0x04001906 RID: 6406
		[SerializeField]
		private StockShopDatabase stockShopDatabase;

		// Token: 0x04001907 RID: 6407
		[SerializeField]
		private GameplayDataSettings.LootingData looting;

		// Token: 0x04001908 RID: 6408
		[SerializeField]
		private AchievementDatabase achivementDatabase;

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		private CropDatabase cropDatabase;

		// Token: 0x0400190A RID: 6410
		[SerializeField]
		private GameplayDataSettings.SpritesData spriteData;

		// Token: 0x02000653 RID: 1619
		[Serializable]
		public class LootingData
		{
			// Token: 0x06002A35 RID: 10805 RVA: 0x000A0224 File Offset: 0x0009E424
			public float MGetInspectingTime(Item item)
			{
				int num = item.Quality;
				if (num < 0)
				{
					num = 0;
				}
				if (num >= this.inspectingTimes.Length)
				{
					num = this.inspectingTimes.Length - 1;
				}
				return this.inspectingTimes[num];
			}

			// Token: 0x06002A36 RID: 10806 RVA: 0x000A025C File Offset: 0x0009E45C
			public static float GetInspectingTime(Item item)
			{
				GameplayDataSettings.LootingData looting = GameplayDataSettings.Looting;
				if (looting == null)
				{
					return 1f;
				}
				return looting.MGetInspectingTime(item);
			}

			// Token: 0x0400229C RID: 8860
			public float[] inspectingTimes;
		}

		// Token: 0x02000654 RID: 1620
		[Serializable]
		public class TagsData
		{
			// Token: 0x17000793 RID: 1939
			// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000A0287 File Offset: 0x0009E487
			public Tag Character
			{
				get
				{
					return this.character;
				}
			}

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000A028F File Offset: 0x0009E48F
			public Tag LockInDemoTag
			{
				get
				{
					return this.lockInDemoTag;
				}
			}

			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x06002A3A RID: 10810 RVA: 0x000A0297 File Offset: 0x0009E497
			public Tag Helmat
			{
				get
				{
					return this.helmat;
				}
			}

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x06002A3B RID: 10811 RVA: 0x000A029F File Offset: 0x0009E49F
			public Tag Armor
			{
				get
				{
					return this.armor;
				}
			}

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x06002A3C RID: 10812 RVA: 0x000A02A7 File Offset: 0x0009E4A7
			public Tag Backpack
			{
				get
				{
					return this.backpack;
				}
			}

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06002A3D RID: 10813 RVA: 0x000A02AF File Offset: 0x0009E4AF
			public Tag Bullet
			{
				get
				{
					return this.bullet;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000A02B7 File Offset: 0x0009E4B7
			public Tag Bait
			{
				get
				{
					return this.bait;
				}
			}

			// Token: 0x1700079A RID: 1946
			// (get) Token: 0x06002A3F RID: 10815 RVA: 0x000A02BF File Offset: 0x0009E4BF
			public Tag AdvancedDebuffMode
			{
				get
				{
					return this.advancedDebuffMode;
				}
			}

			// Token: 0x1700079B RID: 1947
			// (get) Token: 0x06002A40 RID: 10816 RVA: 0x000A02C7 File Offset: 0x0009E4C7
			public Tag Special
			{
				get
				{
					return this.special;
				}
			}

			// Token: 0x1700079C RID: 1948
			// (get) Token: 0x06002A41 RID: 10817 RVA: 0x000A02CF File Offset: 0x0009E4CF
			public Tag DestroyOnLootBox
			{
				get
				{
					return this.destroyOnLootBox;
				}
			}

			// Token: 0x1700079D RID: 1949
			// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000A02D7 File Offset: 0x0009E4D7
			public Tag DontDropOnDeadInSlot
			{
				get
				{
					return this.dontDropOnDeadInSlot;
				}
			}

			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x06002A43 RID: 10819 RVA: 0x000A02DF File Offset: 0x0009E4DF
			public ReadOnlyCollection<Tag> AllTags
			{
				get
				{
					if (this.tagsReadOnly == null)
					{
						this.tagsReadOnly = this.allTags.AsReadOnly();
					}
					return this.tagsReadOnly;
				}
			}

			// Token: 0x1700079F RID: 1951
			// (get) Token: 0x06002A44 RID: 10820 RVA: 0x000A0300 File Offset: 0x0009E500
			public Tag Gun
			{
				get
				{
					if (this.gun == null)
					{
						this.gun = this.Get("Gun");
					}
					return this.gun;
				}
			}

			// Token: 0x06002A45 RID: 10821 RVA: 0x000A0328 File Offset: 0x0009E528
			internal Tag Get(string name)
			{
				foreach (Tag tag in this.AllTags)
				{
					if (tag.name == name)
					{
						return tag;
					}
				}
				return null;
			}

			// Token: 0x0400229D RID: 8861
			[SerializeField]
			private Tag character;

			// Token: 0x0400229E RID: 8862
			[SerializeField]
			private Tag lockInDemoTag;

			// Token: 0x0400229F RID: 8863
			[SerializeField]
			private Tag helmat;

			// Token: 0x040022A0 RID: 8864
			[SerializeField]
			private Tag armor;

			// Token: 0x040022A1 RID: 8865
			[SerializeField]
			private Tag backpack;

			// Token: 0x040022A2 RID: 8866
			[SerializeField]
			private Tag bullet;

			// Token: 0x040022A3 RID: 8867
			[SerializeField]
			private Tag bait;

			// Token: 0x040022A4 RID: 8868
			[SerializeField]
			private Tag advancedDebuffMode;

			// Token: 0x040022A5 RID: 8869
			[SerializeField]
			private Tag special;

			// Token: 0x040022A6 RID: 8870
			[SerializeField]
			private Tag destroyOnLootBox;

			// Token: 0x040022A7 RID: 8871
			[FormerlySerializedAs("dontDropOnDead")]
			[SerializeField]
			private Tag dontDropOnDeadInSlot;

			// Token: 0x040022A8 RID: 8872
			[SerializeField]
			private List<Tag> allTags = new List<Tag>();

			// Token: 0x040022A9 RID: 8873
			private ReadOnlyCollection<Tag> tagsReadOnly;

			// Token: 0x040022AA RID: 8874
			private Tag gun;
		}

		// Token: 0x02000655 RID: 1621
		[Serializable]
		public class PrefabsData
		{
			// Token: 0x170007A0 RID: 1952
			// (get) Token: 0x06002A47 RID: 10823 RVA: 0x000A0397 File Offset: 0x0009E597
			public LevelManager LevelManagerPrefab
			{
				get
				{
					return this.levelManagerPrefab;
				}
			}

			// Token: 0x170007A1 RID: 1953
			// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000A039F File Offset: 0x0009E59F
			public CharacterMainControl CharacterPrefab
			{
				get
				{
					return this.characterPrefab;
				}
			}

			// Token: 0x170007A2 RID: 1954
			// (get) Token: 0x06002A49 RID: 10825 RVA: 0x000A03A7 File Offset: 0x0009E5A7
			public GameObject BulletHitObsticleFx
			{
				get
				{
					return this.bulletHitObsticleFx;
				}
			}

			// Token: 0x170007A3 RID: 1955
			// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000A03AF File Offset: 0x0009E5AF
			public GameObject QuestMarker
			{
				get
				{
					return this.questMarker;
				}
			}

			// Token: 0x170007A4 RID: 1956
			// (get) Token: 0x06002A4B RID: 10827 RVA: 0x000A03B7 File Offset: 0x0009E5B7
			public DuckovItemAgent PickupAgentPrefab
			{
				get
				{
					return this.pickupAgentPrefab;
				}
			}

			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x06002A4C RID: 10828 RVA: 0x000A03BF File Offset: 0x0009E5BF
			public DuckovItemAgent PickupAgentNoRendererPrefab
			{
				get
				{
					return this.pickupAgentNoRendererPrefab;
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x06002A4D RID: 10829 RVA: 0x000A03C7 File Offset: 0x0009E5C7
			public DuckovItemAgent HandheldAgentPrefab
			{
				get
				{
					return this.handheldAgentPrefab;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x06002A4E RID: 10830 RVA: 0x000A03CF File Offset: 0x0009E5CF
			public InteractableLootbox LootBoxPrefab
			{
				get
				{
					return this.lootBoxPrefab;
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x06002A4F RID: 10831 RVA: 0x000A03D7 File Offset: 0x0009E5D7
			public InteractableLootbox LootBoxPrefab_Tomb
			{
				get
				{
					return this.lootBoxPrefab_Tomb;
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000A03DF File Offset: 0x0009E5DF
			public InteractMarker InteractMarker
			{
				get
				{
					return this.interactMarker;
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x06002A51 RID: 10833 RVA: 0x000A03E7 File Offset: 0x0009E5E7
			public HeadCollider HeadCollider
			{
				get
				{
					return this.headCollider;
				}
			}

			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x06002A52 RID: 10834 RVA: 0x000A03EF File Offset: 0x0009E5EF
			public Projectile DefaultBullet
			{
				get
				{
					return this.defaultBullet;
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000A03F7 File Offset: 0x0009E5F7
			public GameObject BuildingBlockAreaMesh
			{
				get
				{
					return this.buildingBlockAreaMesh;
				}
			}

			// Token: 0x170007AD RID: 1965
			// (get) Token: 0x06002A54 RID: 10836 RVA: 0x000A03FF File Offset: 0x0009E5FF
			public GameObject AlertFxPrefab
			{
				get
				{
					return this.alertFxPrefab;
				}
			}

			// Token: 0x170007AE RID: 1966
			// (get) Token: 0x06002A55 RID: 10837 RVA: 0x000A0407 File Offset: 0x0009E607
			public UIInputManager UIInputManagerPrefab
			{
				get
				{
					return this.uiInputManagerPrefab;
				}
			}

			// Token: 0x040022AB RID: 8875
			[SerializeField]
			private LevelManager levelManagerPrefab;

			// Token: 0x040022AC RID: 8876
			[SerializeField]
			private CharacterMainControl characterPrefab;

			// Token: 0x040022AD RID: 8877
			[SerializeField]
			private GameObject bulletHitObsticleFx;

			// Token: 0x040022AE RID: 8878
			[SerializeField]
			private GameObject questMarker;

			// Token: 0x040022AF RID: 8879
			[SerializeField]
			private DuckovItemAgent pickupAgentPrefab;

			// Token: 0x040022B0 RID: 8880
			[SerializeField]
			private DuckovItemAgent pickupAgentNoRendererPrefab;

			// Token: 0x040022B1 RID: 8881
			[SerializeField]
			private DuckovItemAgent handheldAgentPrefab;

			// Token: 0x040022B2 RID: 8882
			[SerializeField]
			private InteractableLootbox lootBoxPrefab;

			// Token: 0x040022B3 RID: 8883
			[SerializeField]
			private InteractableLootbox lootBoxPrefab_Tomb;

			// Token: 0x040022B4 RID: 8884
			[SerializeField]
			private InteractMarker interactMarker;

			// Token: 0x040022B5 RID: 8885
			[SerializeField]
			private HeadCollider headCollider;

			// Token: 0x040022B6 RID: 8886
			[SerializeField]
			private Projectile defaultBullet;

			// Token: 0x040022B7 RID: 8887
			[SerializeField]
			private UIInputManager uiInputManagerPrefab;

			// Token: 0x040022B8 RID: 8888
			[SerializeField]
			private GameObject buildingBlockAreaMesh;

			// Token: 0x040022B9 RID: 8889
			[SerializeField]
			private GameObject alertFxPrefab;
		}

		// Token: 0x02000656 RID: 1622
		[Serializable]
		public class BuffsData
		{
			// Token: 0x170007AF RID: 1967
			// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000A0417 File Offset: 0x0009E617
			public Buff BleedSBuff
			{
				get
				{
					return this.bleedSBuff;
				}
			}

			// Token: 0x170007B0 RID: 1968
			// (get) Token: 0x06002A58 RID: 10840 RVA: 0x000A041F File Offset: 0x0009E61F
			public Buff UnlimitBleedBuff
			{
				get
				{
					return this.unlimitBleedBuff;
				}
			}

			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x06002A59 RID: 10841 RVA: 0x000A0427 File Offset: 0x0009E627
			public Buff BoneCrackBuff
			{
				get
				{
					return this.boneCrackBuff;
				}
			}

			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x06002A5A RID: 10842 RVA: 0x000A042F File Offset: 0x0009E62F
			public Buff WoundBuff
			{
				get
				{
					return this.woundBuff;
				}
			}

			// Token: 0x170007B3 RID: 1971
			// (get) Token: 0x06002A5B RID: 10843 RVA: 0x000A0437 File Offset: 0x0009E637
			public Buff Weight_Light
			{
				get
				{
					return this.weight_Light;
				}
			}

			// Token: 0x170007B4 RID: 1972
			// (get) Token: 0x06002A5C RID: 10844 RVA: 0x000A043F File Offset: 0x0009E63F
			public Buff Weight_Heavy
			{
				get
				{
					return this.weight_Heavy;
				}
			}

			// Token: 0x170007B5 RID: 1973
			// (get) Token: 0x06002A5D RID: 10845 RVA: 0x000A0447 File Offset: 0x0009E647
			public Buff Weight_SuperHeavy
			{
				get
				{
					return this.weight_SuperHeavy;
				}
			}

			// Token: 0x170007B6 RID: 1974
			// (get) Token: 0x06002A5E RID: 10846 RVA: 0x000A044F File Offset: 0x0009E64F
			public Buff Weight_Overweight
			{
				get
				{
					return this.weight_Overweight;
				}
			}

			// Token: 0x170007B7 RID: 1975
			// (get) Token: 0x06002A5F RID: 10847 RVA: 0x000A0457 File Offset: 0x0009E657
			public Buff Pain
			{
				get
				{
					return this.pain;
				}
			}

			// Token: 0x170007B8 RID: 1976
			// (get) Token: 0x06002A60 RID: 10848 RVA: 0x000A045F File Offset: 0x0009E65F
			public Buff BaseBuff
			{
				get
				{
					return this.baseBuff;
				}
			}

			// Token: 0x170007B9 RID: 1977
			// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000A0467 File Offset: 0x0009E667
			public Buff Starve
			{
				get
				{
					return this.starve;
				}
			}

			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000A046F File Offset: 0x0009E66F
			public Buff Thirsty
			{
				get
				{
					return this.thirsty;
				}
			}

			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x06002A63 RID: 10851 RVA: 0x000A0477 File Offset: 0x0009E677
			public Buff Burn
			{
				get
				{
					return this.burn;
				}
			}

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x06002A64 RID: 10852 RVA: 0x000A047F File Offset: 0x0009E67F
			public Buff Poison
			{
				get
				{
					return this.poison;
				}
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06002A65 RID: 10853 RVA: 0x000A0487 File Offset: 0x0009E687
			public Buff Electric
			{
				get
				{
					return this.electric;
				}
			}

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06002A66 RID: 10854 RVA: 0x000A048F File Offset: 0x0009E68F
			public Buff Space
			{
				get
				{
					return this.space;
				}
			}

			// Token: 0x06002A67 RID: 10855 RVA: 0x000A0498 File Offset: 0x0009E698
			public string GetBuffDisplayName(int id)
			{
				Buff buff = this.allBuffs.Find((Buff e) => e != null && e.ID == id);
				if (buff == null)
				{
					return "?";
				}
				return buff.DisplayName;
			}

			// Token: 0x040022BA RID: 8890
			[SerializeField]
			private Buff bleedSBuff;

			// Token: 0x040022BB RID: 8891
			[SerializeField]
			private Buff unlimitBleedBuff;

			// Token: 0x040022BC RID: 8892
			[SerializeField]
			private Buff boneCrackBuff;

			// Token: 0x040022BD RID: 8893
			[SerializeField]
			private Buff woundBuff;

			// Token: 0x040022BE RID: 8894
			[SerializeField]
			private Buff weight_Light;

			// Token: 0x040022BF RID: 8895
			[SerializeField]
			private Buff weight_Heavy;

			// Token: 0x040022C0 RID: 8896
			[SerializeField]
			private Buff weight_SuperHeavy;

			// Token: 0x040022C1 RID: 8897
			[SerializeField]
			private Buff weight_Overweight;

			// Token: 0x040022C2 RID: 8898
			[SerializeField]
			private Buff pain;

			// Token: 0x040022C3 RID: 8899
			[SerializeField]
			private Buff baseBuff;

			// Token: 0x040022C4 RID: 8900
			[SerializeField]
			private Buff starve;

			// Token: 0x040022C5 RID: 8901
			[SerializeField]
			private Buff thirsty;

			// Token: 0x040022C6 RID: 8902
			[SerializeField]
			private Buff burn;

			// Token: 0x040022C7 RID: 8903
			[SerializeField]
			private Buff poison;

			// Token: 0x040022C8 RID: 8904
			[SerializeField]
			private Buff electric;

			// Token: 0x040022C9 RID: 8905
			[SerializeField]
			private Buff space;

			// Token: 0x040022CA RID: 8906
			[SerializeField]
			private List<Buff> allBuffs;
		}

		// Token: 0x02000657 RID: 1623
		[Serializable]
		public class ItemAssetsData
		{
			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000A04E7 File Offset: 0x0009E6E7
			public int DefaultCharacterItemTypeID
			{
				get
				{
					return this.defaultCharacterItemTypeID;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000A04EF File Offset: 0x0009E6EF
			public int CashItemTypeID
			{
				get
				{
					return this.cashItemTypeID;
				}
			}

			// Token: 0x040022CB RID: 8907
			[SerializeField]
			[ItemTypeID]
			private int defaultCharacterItemTypeID;

			// Token: 0x040022CC RID: 8908
			[SerializeField]
			[ItemTypeID]
			private int cashItemTypeID;
		}

		// Token: 0x02000658 RID: 1624
		public class StringListsData
		{
			// Token: 0x040022CD RID: 8909
			public static StringList StatKeys;

			// Token: 0x040022CE RID: 8910
			public static StringList SlotTypes;

			// Token: 0x040022CF RID: 8911
			public static StringList ItemAgentKeys;
		}

		// Token: 0x02000659 RID: 1625
		[Serializable]
		public class LayersData
		{
			// Token: 0x06002A6D RID: 10861 RVA: 0x000A0507 File Offset: 0x0009E707
			public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
			{
				return (1 << layer & layerMask) != 0;
			}

			// Token: 0x040022D0 RID: 8912
			public LayerMask damageReceiverLayerMask;

			// Token: 0x040022D1 RID: 8913
			public LayerMask wallLayerMask;

			// Token: 0x040022D2 RID: 8914
			public LayerMask groundLayerMask;

			// Token: 0x040022D3 RID: 8915
			public LayerMask halfObsticleLayer;

			// Token: 0x040022D4 RID: 8916
			public LayerMask fowBlockLayers;

			// Token: 0x040022D5 RID: 8917
			public LayerMask fowBlockLayersWithThermal;
		}

		// Token: 0x0200065A RID: 1626
		[Serializable]
		public class SceneManagementData
		{
			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000A0523 File Offset: 0x0009E723
			public SceneInfoCollection SceneInfoCollection
			{
				get
				{
					return this.sceneInfoCollection;
				}
			}

			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000A052B File Offset: 0x0009E72B
			public SceneReference PrologueScene
			{
				get
				{
					return this.prologueScene;
				}
			}

			// Token: 0x170007C3 RID: 1987
			// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000A0533 File Offset: 0x0009E733
			public SceneReference MainMenuScene
			{
				get
				{
					return this.mainMenuScene;
				}
			}

			// Token: 0x170007C4 RID: 1988
			// (get) Token: 0x06002A72 RID: 10866 RVA: 0x000A053B File Offset: 0x0009E73B
			public SceneReference BaseScene
			{
				get
				{
					return this.baseScene;
				}
			}

			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000A0543 File Offset: 0x0009E743
			public SceneReference FailLoadingScreenScene
			{
				get
				{
					return this.failLoadingScreenScene;
				}
			}

			// Token: 0x170007C6 RID: 1990
			// (get) Token: 0x06002A74 RID: 10868 RVA: 0x000A054B File Offset: 0x0009E74B
			public SceneReference EvacuateScreenScene
			{
				get
				{
					return this.evacuateScreenScene;
				}
			}

			// Token: 0x040022D6 RID: 8918
			[SerializeField]
			private SceneInfoCollection sceneInfoCollection;

			// Token: 0x040022D7 RID: 8919
			[SerializeField]
			private SceneReference prologueScene;

			// Token: 0x040022D8 RID: 8920
			[SerializeField]
			private SceneReference mainMenuScene;

			// Token: 0x040022D9 RID: 8921
			[SerializeField]
			private SceneReference baseScene;

			// Token: 0x040022DA RID: 8922
			[SerializeField]
			private SceneReference failLoadingScreenScene;

			// Token: 0x040022DB RID: 8923
			[SerializeField]
			private SceneReference evacuateScreenScene;
		}

		// Token: 0x0200065B RID: 1627
		[Serializable]
		public class QuestsData
		{
			// Token: 0x170007C7 RID: 1991
			// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000A055B File Offset: 0x0009E75B
			private string DefaultQuestGiverDisplayName
			{
				get
				{
					return this.defaultQuestGiverDisplayName;
				}
			}

			// Token: 0x170007C8 RID: 1992
			// (get) Token: 0x06002A77 RID: 10871 RVA: 0x000A0563 File Offset: 0x0009E763
			public QuestCollection QuestCollection
			{
				get
				{
					return this.questCollection;
				}
			}

			// Token: 0x170007C9 RID: 1993
			// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000A056B File Offset: 0x0009E76B
			public QuestRelationGraph QuestRelation
			{
				get
				{
					return this.questRelation;
				}
			}

			// Token: 0x06002A79 RID: 10873 RVA: 0x000A0574 File Offset: 0x0009E774
			public GameplayDataSettings.QuestsData.QuestGiverInfo GetInfo(QuestGiverID id)
			{
				return this.questGiverInfos.Find((GameplayDataSettings.QuestsData.QuestGiverInfo e) => e != null && e.id == id);
			}

			// Token: 0x06002A7A RID: 10874 RVA: 0x000A05A8 File Offset: 0x0009E7A8
			public string GetDisplayName(QuestGiverID id)
			{
				return string.Format("Character_{0}", id).ToPlainText();
			}

			// Token: 0x040022DC RID: 8924
			[SerializeField]
			private QuestCollection questCollection;

			// Token: 0x040022DD RID: 8925
			[SerializeField]
			private QuestRelationGraph questRelation;

			// Token: 0x040022DE RID: 8926
			[SerializeField]
			private List<GameplayDataSettings.QuestsData.QuestGiverInfo> questGiverInfos;

			// Token: 0x040022DF RID: 8927
			[SerializeField]
			private string defaultQuestGiverDisplayName = "佚名";

			// Token: 0x02000678 RID: 1656
			[Serializable]
			public class QuestGiverInfo
			{
				// Token: 0x170007D6 RID: 2006
				// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000A1599 File Offset: 0x0009F799
				public string DisplayName
				{
					get
					{
						return this.displayName;
					}
				}

				// Token: 0x0400233A RID: 9018
				public QuestGiverID id;

				// Token: 0x0400233B RID: 9019
				[SerializeField]
				private string displayName;
			}
		}

		// Token: 0x0200065C RID: 1628
		[Serializable]
		public class EconomyData
		{
			// Token: 0x170007CA RID: 1994
			// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000A05DD File Offset: 0x0009E7DD
			public ReadOnlyCollection<int> UnlockedItemByDefault
			{
				get
				{
					return this.unlockItemByDefault.AsReadOnly();
				}
			}

			// Token: 0x040022E0 RID: 8928
			[SerializeField]
			[ItemTypeID]
			private List<int> unlockItemByDefault = new List<int>();
		}

		// Token: 0x0200065D RID: 1629
		[Serializable]
		public class UIStyleData
		{
			// Token: 0x170007CB RID: 1995
			// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000A05FD File Offset: 0x0009E7FD
			public Sprite CritPopSprite
			{
				get
				{
					return this.critPopSprite;
				}
			}

			// Token: 0x170007CC RID: 1996
			// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000A0605 File Offset: 0x0009E805
			public Sprite DefaultTeleporterIcon
			{
				get
				{
					return this.defaultTeleporterIcon;
				}
			}

			// Token: 0x170007CD RID: 1997
			// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000A060D File Offset: 0x0009E80D
			public Sprite EleteCharacterIcon
			{
				get
				{
					return this.eleteCharacterIcon;
				}
			}

			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000A0615 File Offset: 0x0009E815
			public Sprite BossCharacterIcon
			{
				get
				{
					return this.bossCharacterIcon;
				}
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000A061D File Offset: 0x0009E81D
			public Sprite PmcCharacterIcon
			{
				get
				{
					return this.pmcCharacterIcon;
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000A0625 File Offset: 0x0009E825
			public Sprite MerchantCharacterIcon
			{
				get
				{
					return this.merchantCharacterIcon;
				}
			}

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000A062D File Offset: 0x0009E82D
			public Sprite PetCharacterIcon
			{
				get
				{
					return this.petCharacterIcon;
				}
			}

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000A0635 File Offset: 0x0009E835
			public float TeleporterIconScale
			{
				get
				{
					return this.teleporterIconScale;
				}
			}

			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x06002A86 RID: 10886 RVA: 0x000A063D File Offset: 0x0009E83D
			public Sprite FallbackItemIcon
			{
				get
				{
					return this.fallbackItemIcon;
				}
			}

			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000A0645 File Offset: 0x0009E845
			public TextMeshProUGUI TemplateTextUGUI
			{
				get
				{
					return this.templateTextUGUI;
				}
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000A064D File Offset: 0x0009E84D
			[SerializeField]
			private TMP_Asset DefaultFont
			{
				get
				{
					return this.defaultFont;
				}
			}

			// Token: 0x06002A89 RID: 10889 RVA: 0x000A0658 File Offset: 0x0009E858
			[return: TupleElementNames(new string[]
			{
				"shadowOffset",
				"color",
				"innerGlow"
			})]
			public ValueTuple<float, Color, bool> GetShadowOffsetAndColorOfQuality(DisplayQuality displayQuality)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == displayQuality);
				if (displayQualityLook == null)
				{
					return new ValueTuple<float, Color, bool>(this.defaultDisplayQualityShadowOffset, this.defaultDisplayQualityShadowColor, this.defaultDIsplayQualityShadowInnerGlow);
				}
				return new ValueTuple<float, Color, bool>(displayQualityLook.shadowOffset, displayQualityLook.shadowColor, displayQualityLook.innerGlow);
			}

			// Token: 0x06002A8A RID: 10890 RVA: 0x000A06BC File Offset: 0x0009E8BC
			public void ApplyDisplayQualityShadow(DisplayQuality displayQuality, TrueShadow target)
			{
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = this.GetShadowOffsetAndColorOfQuality(displayQuality);
				target.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				target.Color = shadowOffsetAndColorOfQuality.Item2;
				target.Inset = shadowOffsetAndColorOfQuality.Item3;
			}

			// Token: 0x06002A8B RID: 10891 RVA: 0x000A06F8 File Offset: 0x0009E8F8
			public GameplayDataSettings.UIStyleData.DisplayQualityLook GetDisplayQualityLook(DisplayQuality q)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == q);
				if (displayQualityLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayQualityLook
					{
						quality = q,
						shadowOffset = this.defaultDisplayQualityShadowOffset,
						shadowColor = this.defaultDisplayQualityShadowColor,
						innerGlow = this.defaultDIsplayQualityShadowInnerGlow
					};
				}
				return displayQualityLook;
			}

			// Token: 0x06002A8C RID: 10892 RVA: 0x000A0764 File Offset: 0x0009E964
			public GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook GetElementDamagePopTextLook(ElementTypes elementType)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook displayElementDamagePopTextLook = this.elementDamagePopTextLook.Find((GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook e) => e != null && e.elementType == elementType);
				if (displayElementDamagePopTextLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook
					{
						elementType = ElementTypes.physics,
						normalSize = 1f,
						critSize = 1.6f,
						color = Color.white
					};
				}
				return displayElementDamagePopTextLook;
			}

			// Token: 0x040022E1 RID: 8929
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayQualityLook> displayQualityLooks = new List<GameplayDataSettings.UIStyleData.DisplayQualityLook>();

			// Token: 0x040022E2 RID: 8930
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook> elementDamagePopTextLook = new List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook>();

			// Token: 0x040022E3 RID: 8931
			[SerializeField]
			private float defaultDisplayQualityShadowOffset = 8f;

			// Token: 0x040022E4 RID: 8932
			[SerializeField]
			private Color defaultDisplayQualityShadowColor = Color.black;

			// Token: 0x040022E5 RID: 8933
			[SerializeField]
			private bool defaultDIsplayQualityShadowInnerGlow;

			// Token: 0x040022E6 RID: 8934
			[SerializeField]
			private Sprite defaultTeleporterIcon;

			// Token: 0x040022E7 RID: 8935
			[SerializeField]
			private float teleporterIconScale = 0.5f;

			// Token: 0x040022E8 RID: 8936
			[SerializeField]
			private Sprite critPopSprite;

			// Token: 0x040022E9 RID: 8937
			[SerializeField]
			private Sprite fallbackItemIcon;

			// Token: 0x040022EA RID: 8938
			[SerializeField]
			private Sprite eleteCharacterIcon;

			// Token: 0x040022EB RID: 8939
			[SerializeField]
			private Sprite bossCharacterIcon;

			// Token: 0x040022EC RID: 8940
			[SerializeField]
			private Sprite pmcCharacterIcon;

			// Token: 0x040022ED RID: 8941
			[SerializeField]
			private Sprite merchantCharacterIcon;

			// Token: 0x040022EE RID: 8942
			[SerializeField]
			private Sprite petCharacterIcon;

			// Token: 0x040022EF RID: 8943
			[SerializeField]
			private TMP_Asset defaultFont;

			// Token: 0x040022F0 RID: 8944
			[SerializeField]
			private TextMeshProUGUI templateTextUGUI;

			// Token: 0x0200067A RID: 1658
			[Serializable]
			public class DisplayQualityLook
			{
				// Token: 0x06002AB6 RID: 10934 RVA: 0x000A15C6 File Offset: 0x0009F7C6
				public void Apply(TrueShadow trueShadow)
				{
					trueShadow.OffsetDistance = this.shadowOffset;
					trueShadow.Color = this.shadowColor;
					trueShadow.Inset = this.innerGlow;
				}

				// Token: 0x0400233D RID: 9021
				public DisplayQuality quality;

				// Token: 0x0400233E RID: 9022
				public float shadowOffset;

				// Token: 0x0400233F RID: 9023
				public Color shadowColor;

				// Token: 0x04002340 RID: 9024
				public bool innerGlow;
			}

			// Token: 0x0200067B RID: 1659
			[Serializable]
			public class DisplayElementDamagePopTextLook
			{
				// Token: 0x04002341 RID: 9025
				public ElementTypes elementType;

				// Token: 0x04002342 RID: 9026
				public float normalSize;

				// Token: 0x04002343 RID: 9027
				public float critSize;

				// Token: 0x04002344 RID: 9028
				public Color color;
			}
		}

		// Token: 0x0200065E RID: 1630
		[Serializable]
		public class SpritesData
		{
			// Token: 0x06002A8E RID: 10894 RVA: 0x000A0808 File Offset: 0x0009EA08
			public Sprite GetSprite(string key)
			{
				foreach (GameplayDataSettings.SpritesData.Entry entry in this.entries)
				{
					if (entry.key == key)
					{
						return entry.sprite;
					}
				}
				return null;
			}

			// Token: 0x040022F1 RID: 8945
			public List<GameplayDataSettings.SpritesData.Entry> entries;

			// Token: 0x0200067F RID: 1663
			[Serializable]
			public struct Entry
			{
				// Token: 0x04002348 RID: 9032
				public string key;

				// Token: 0x04002349 RID: 9033
				public Sprite sprite;
			}
		}
	}
}
