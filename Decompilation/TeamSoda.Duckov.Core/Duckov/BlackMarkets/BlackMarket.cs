using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.BlackMarkets
{
	// Token: 0x02000307 RID: 775
	public class BlackMarket : MonoBehaviour
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x0005B947 File Offset: 0x00059B47
		// (set) Token: 0x06001939 RID: 6457 RVA: 0x0005B94E File Offset: 0x00059B4E
		public static BlackMarket Instance { get; private set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x0005B956 File Offset: 0x00059B56
		// (set) Token: 0x0600193B RID: 6459 RVA: 0x0005B969 File Offset: 0x00059B69
		public int RefreshChance
		{
			get
			{
				return Mathf.Min(this.refreshChance, this.MaxRefreshChance);
			}
			set
			{
				this.refreshChance = value;
				Action<BlackMarket> action = BlackMarket.onRefreshChanceChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x0600193C RID: 6460 RVA: 0x0005B984 File Offset: 0x00059B84
		// (remove) Token: 0x0600193D RID: 6461 RVA: 0x0005B9B8 File Offset: 0x00059BB8
		public static event Action<BlackMarket> onRefreshChanceChanged;

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x0600193E RID: 6462 RVA: 0x0005B9EC File Offset: 0x00059BEC
		// (remove) Token: 0x0600193F RID: 6463 RVA: 0x0005BA20 File Offset: 0x00059C20
		public static event Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> onRequestMaxRefreshChance;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06001940 RID: 6464 RVA: 0x0005BA54 File Offset: 0x00059C54
		// (remove) Token: 0x06001941 RID: 6465 RVA: 0x0005BA88 File Offset: 0x00059C88
		public static event Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> onRequestRefreshTime;

		// Token: 0x06001942 RID: 6466 RVA: 0x0005BABB File Offset: 0x00059CBB
		public static void NotifyMaxRefreshChanceChanged()
		{
			BlackMarket.dirty = true;
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x0005BAC4 File Offset: 0x00059CC4
		public int MaxRefreshChance
		{
			get
			{
				if (BlackMarket.dirty)
				{
					BlackMarket.OnRequestMaxRefreshChanceEventContext onRequestMaxRefreshChanceEventContext = new BlackMarket.OnRequestMaxRefreshChanceEventContext();
					onRequestMaxRefreshChanceEventContext.Add(1);
					Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> action = BlackMarket.onRequestMaxRefreshChance;
					if (action != null)
					{
						action(onRequestMaxRefreshChanceEventContext);
					}
					this.cachedMaxRefreshChance = onRequestMaxRefreshChanceEventContext.Value;
				}
				return this.cachedMaxRefreshChance;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x0005BB08 File Offset: 0x00059D08
		private TimeSpan TimeToRefresh
		{
			get
			{
				BlackMarket.OnRequestRefreshTimeFactorEventContext onRequestRefreshTimeFactorEventContext = new BlackMarket.OnRequestRefreshTimeFactorEventContext();
				Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> action = BlackMarket.onRequestRefreshTime;
				if (action != null)
				{
					action(onRequestRefreshTimeFactorEventContext);
				}
				float num = Mathf.Max(onRequestRefreshTimeFactorEventContext.Value, 0.01f);
				return TimeSpan.FromTicks((long)((float)this.timeToRefresh * num));
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x0005BB4C File Offset: 0x00059D4C
		// (set) Token: 0x06001946 RID: 6470 RVA: 0x0005BB59 File Offset: 0x00059D59
		private DateTime LastRefreshedTime
		{
			get
			{
				return DateTime.FromBinary(this.lastRefreshedTimeRaw);
			}
			set
			{
				this.lastRefreshedTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x0005BB68 File Offset: 0x00059D68
		private TimeSpan TimeSinceLastRefreshedTime
		{
			get
			{
				if (DateTime.UtcNow < this.LastRefreshedTime)
				{
					this.LastRefreshedTime = DateTime.UtcNow;
				}
				return DateTime.UtcNow - this.LastRefreshedTime;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0005BB97 File Offset: 0x00059D97
		public TimeSpan RemainingTimeBeforeRefresh
		{
			get
			{
				return this.TimeToRefresh - this.TimeSinceLastRefreshedTime;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x0005BBAA File Offset: 0x00059DAA
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Demands
		{
			get
			{
				if (this._demands_readonly == null)
				{
					this._demands_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.demands);
				}
				return this._demands_readonly;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x0005BBCB File Offset: 0x00059DCB
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Supplies
		{
			get
			{
				if (this._supplies_readonly == null)
				{
					this._supplies_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.supplies);
				}
				return this._supplies_readonly;
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x0600194B RID: 6475 RVA: 0x0005BBEC File Offset: 0x00059DEC
		// (remove) Token: 0x0600194C RID: 6476 RVA: 0x0005BC24 File Offset: 0x00059E24
		public event Action onAfterGenerateEntries;

		// Token: 0x0600194D RID: 6477 RVA: 0x0005BC5C File Offset: 0x00059E5C
		private ItemFilter ContructRandomFilter()
		{
			Tag random = this.tags.GetRandom(0f);
			int random2 = this.qualities.GetRandom(0f);
			if (GameMetaData.Instance.IsDemo)
			{
				this.excludeTags.Add(GameplayDataSettings.Tags.LockInDemoTag);
			}
			return new ItemFilter
			{
				requireTags = new Tag[]
				{
					random
				},
				excludeTags = this.excludeTags.ToArray(),
				minQuality = random2,
				maxQuality = random2
			};
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0005BCE8 File Offset: 0x00059EE8
		public UniTask<bool> Buy(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Buy>d__59 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.entry = entry;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<BlackMarket.<Buy>d__59>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0005BD34 File Offset: 0x00059F34
		public UniTask<bool> Sell(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Sell>d__60 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.entry = entry;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<BlackMarket.<Sell>d__60>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0005BD80 File Offset: 0x00059F80
		private void GenerateDemandsAndSupplies()
		{
			this.demands.Clear();
			this.supplies.Clear();
			int num = 0;
			for (int i = 0; i < this.demandsCount; i++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建需求失败。尝试次数超过100次。");
					break;
				}
				int[] array = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array.Length == 0)
				{
					i--;
				}
				else
				{
					int random = array.GetRandom<int>();
					ItemAssetsCollection.GetMetaData(random);
					int random2 = this.demandAmountRand.GetRandom(0f);
					float random3 = this.demandFactorRand.GetRandom(0f);
					int random4 = this.demandBatchCountRand.GetRandom(0f);
					BlackMarket.DemandSupplyEntry item = new BlackMarket.DemandSupplyEntry
					{
						itemID = random,
						remaining = random2,
						priceFactor = random3,
						batchCount = random4
					};
					this.demands.Add(item);
				}
			}
			num = 0;
			for (int j = 0; j < this.suppliesCount; j++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建供应失败。尝试次数超过100次。");
					break;
				}
				int[] array2 = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array2.Length == 0)
				{
					j--;
				}
				else
				{
					int candidate = array2.GetRandom<int>();
					if (this.demands.Any((BlackMarket.DemandSupplyEntry e) => e.ItemID == candidate))
					{
						j--;
					}
					else
					{
						ItemAssetsCollection.GetMetaData(candidate);
						int random5 = this.supplyAmountRand.GetRandom(0f);
						float random6 = this.supplyFactorRand.GetRandom(0f);
						int random7 = this.supplyBatchCountRand.GetRandom(0f);
						BlackMarket.DemandSupplyEntry item2 = new BlackMarket.DemandSupplyEntry
						{
							itemID = candidate,
							remaining = random5,
							priceFactor = random6,
							batchCount = random7
						};
						this.supplies.Add(item2);
					}
				}
			}
			Action action = this.onAfterGenerateEntries;
			if (action != null)
			{
				action();
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LevelManager.Instance.SaveMainCharacter();
			SavesSystem.CollectSaveData();
			SavesSystem.SaveFile(true);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0005BF94 File Offset: 0x0005A194
		public void PayAndRegenerate()
		{
			if (this.RefreshChance <= 0)
			{
				return;
			}
			int num = this.RefreshChance;
			this.RefreshChance = num - 1;
			this.GenerateDemandsAndSupplies();
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0005BFC4 File Offset: 0x0005A1C4
		private void FixedUpdate()
		{
			if (this.RefreshChance >= this.MaxRefreshChance)
			{
				this.LastRefreshedTime = DateTime.UtcNow;
				return;
			}
			TimeSpan timeSpan = this.TimeSinceLastRefreshedTime;
			if (timeSpan > this.TimeToRefresh)
			{
				while (timeSpan > this.TimeToRefresh)
				{
					timeSpan -= this.TimeToRefresh;
					this.RefreshChance++;
					if (this.RefreshChance >= this.MaxRefreshChance)
					{
						break;
					}
				}
				if (timeSpan > TimeSpan.Zero)
				{
					this.LastRefreshedTime = DateTime.UtcNow - timeSpan;
				}
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0005C057 File Offset: 0x0005A257
		private void Awake()
		{
			BlackMarket.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0005C070 File Offset: 0x0005A270
		private void Start()
		{
			this.Load();
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0005C078 File Offset: 0x0005A278
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0005C08C File Offset: 0x0005A28C
		private void Save()
		{
			BlackMarket.SaveData value = new BlackMarket.SaveData(this);
			SavesSystem.Save<BlackMarket.SaveData>("BlackMarket_Data", value);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0005C0AC File Offset: 0x0005A2AC
		private void Load()
		{
			BlackMarket.SaveData saveData = SavesSystem.Load<BlackMarket.SaveData>("BlackMarket_Data");
			if (!saveData.valid)
			{
				this.GenerateDemandsAndSupplies();
				return;
			}
			this.demands.Clear();
			this.demands.AddRange(saveData.demands);
			this.supplies.Clear();
			this.supplies.AddRange(saveData.supplies);
			this.lastRefreshedTimeRaw = saveData.lastRefreshedTimeRaw;
			this.refreshChance = saveData.refreshChance;
		}

		// Token: 0x0400124C RID: 4684
		[SerializeField]
		private int demandsCount = 3;

		// Token: 0x0400124D RID: 4685
		[SerializeField]
		private int suppliesCount = 3;

		// Token: 0x0400124E RID: 4686
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x0400124F RID: 4687
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x04001250 RID: 4688
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001251 RID: 4689
		[SerializeField]
		private RandomContainer<int> demandAmountRand;

		// Token: 0x04001252 RID: 4690
		[SerializeField]
		private RandomContainer<float> demandFactorRand;

		// Token: 0x04001253 RID: 4691
		[SerializeField]
		private RandomContainer<int> demandBatchCountRand;

		// Token: 0x04001254 RID: 4692
		[SerializeField]
		private RandomContainer<int> supplyAmountRand;

		// Token: 0x04001255 RID: 4693
		[SerializeField]
		private RandomContainer<float> supplyFactorRand;

		// Token: 0x04001256 RID: 4694
		[SerializeField]
		private RandomContainer<int> supplyBatchCountRand;

		// Token: 0x04001257 RID: 4695
		[SerializeField]
		[TimeSpan]
		private long timeToRefresh;

		// Token: 0x04001258 RID: 4696
		[SerializeField]
		private int refreshChance;

		// Token: 0x0400125C RID: 4700
		private static bool dirty = true;

		// Token: 0x0400125D RID: 4701
		private int cachedMaxRefreshChance = -1;

		// Token: 0x0400125E RID: 4702
		[DateTime]
		private long lastRefreshedTimeRaw;

		// Token: 0x0400125F RID: 4703
		private List<BlackMarket.DemandSupplyEntry> demands = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x04001260 RID: 4704
		private List<BlackMarket.DemandSupplyEntry> supplies = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x04001261 RID: 4705
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _demands_readonly;

		// Token: 0x04001262 RID: 4706
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _supplies_readonly;

		// Token: 0x04001264 RID: 4708
		private const string SaveKey = "BlackMarket_Data";

		// Token: 0x02000598 RID: 1432
		public class OnRequestMaxRefreshChanceEventContext
		{
			// Token: 0x1700076B RID: 1899
			// (get) Token: 0x0600288F RID: 10383 RVA: 0x000962DA File Offset: 0x000944DA
			public int Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06002890 RID: 10384 RVA: 0x000962E2 File Offset: 0x000944E2
			public void Add(int count = 1)
			{
				this.value += count;
			}

			// Token: 0x04002008 RID: 8200
			private int value;
		}

		// Token: 0x02000599 RID: 1433
		public class OnRequestRefreshTimeFactorEventContext
		{
			// Token: 0x1700076C RID: 1900
			// (get) Token: 0x06002892 RID: 10386 RVA: 0x000962FA File Offset: 0x000944FA
			public float Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06002893 RID: 10387 RVA: 0x00096302 File Offset: 0x00094502
			public void Add(float count = -0.1f)
			{
				this.value += count;
			}

			// Token: 0x04002009 RID: 8201
			private float value = 1f;
		}

		// Token: 0x0200059A RID: 1434
		[Serializable]
		public class DemandSupplyEntry
		{
			// Token: 0x1700076D RID: 1901
			// (get) Token: 0x06002895 RID: 10389 RVA: 0x00096325 File Offset: 0x00094525
			public int ItemID
			{
				get
				{
					return this.itemID;
				}
			}

			// Token: 0x1700076E RID: 1902
			// (get) Token: 0x06002896 RID: 10390 RVA: 0x0009632D File Offset: 0x0009452D
			internal ItemMetaData ItemMetaData
			{
				get
				{
					return ItemAssetsCollection.GetMetaData(this.itemID);
				}
			}

			// Token: 0x1700076F RID: 1903
			// (get) Token: 0x06002897 RID: 10391 RVA: 0x0009633A File Offset: 0x0009453A
			public int Remaining
			{
				get
				{
					return this.remaining;
				}
			}

			// Token: 0x17000770 RID: 1904
			// (get) Token: 0x06002898 RID: 10392 RVA: 0x00096342 File Offset: 0x00094542
			public int TotalPrice
			{
				get
				{
					return Mathf.FloorToInt((float)this.ItemMetaData.priceEach * this.priceFactor * (float)this.ItemMetaData.defaultStackCount * (float)this.batchCount);
				}
			}

			// Token: 0x17000771 RID: 1905
			// (get) Token: 0x06002899 RID: 10393 RVA: 0x00096371 File Offset: 0x00094571
			public Cost BuyCost
			{
				get
				{
					return new Cost((long)this.TotalPrice);
				}
			}

			// Token: 0x17000772 RID: 1906
			// (get) Token: 0x0600289A RID: 10394 RVA: 0x0009637F File Offset: 0x0009457F
			public Cost SellCost
			{
				get
				{
					return new Cost(new ValueTuple<int, long>[]
					{
						new ValueTuple<int, long>(this.ItemMetaData.id, (long)(this.ItemMetaData.defaultStackCount * this.batchCount))
					});
				}
			}

			// Token: 0x17000773 RID: 1907
			// (get) Token: 0x0600289B RID: 10395 RVA: 0x000963B8 File Offset: 0x000945B8
			public string ItemDisplayName
			{
				get
				{
					return this.ItemMetaData.DisplayName;
				}
			}

			// Token: 0x140000F8 RID: 248
			// (add) Token: 0x0600289C RID: 10396 RVA: 0x000963D4 File Offset: 0x000945D4
			// (remove) Token: 0x0600289D RID: 10397 RVA: 0x0009640C File Offset: 0x0009460C
			public event Action<BlackMarket.DemandSupplyEntry> onChanged;

			// Token: 0x0600289E RID: 10398 RVA: 0x00096441 File Offset: 0x00094641
			internal void NotifyChange()
			{
				Action<BlackMarket.DemandSupplyEntry> action = this.onChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}

			// Token: 0x0400200A RID: 8202
			[SerializeField]
			[ItemTypeID]
			internal int itemID;

			// Token: 0x0400200B RID: 8203
			[SerializeField]
			internal int remaining;

			// Token: 0x0400200C RID: 8204
			[SerializeField]
			internal float priceFactor;

			// Token: 0x0400200D RID: 8205
			[SerializeField]
			internal int batchCount;
		}

		// Token: 0x0200059B RID: 1435
		[Serializable]
		public struct SaveData
		{
			// Token: 0x060028A0 RID: 10400 RVA: 0x0009645C File Offset: 0x0009465C
			public SaveData(BlackMarket blackMarket)
			{
				this.valid = true;
				this.lastRefreshedTimeRaw = blackMarket.lastRefreshedTimeRaw;
				this.demands = blackMarket.demands.ToArray();
				this.supplies = blackMarket.supplies.ToArray();
				this.refreshChance = blackMarket.refreshChance;
			}

			// Token: 0x0400200F RID: 8207
			public bool valid;

			// Token: 0x04002010 RID: 8208
			public long lastRefreshedTimeRaw;

			// Token: 0x04002011 RID: 8209
			public int refreshChance;

			// Token: 0x04002012 RID: 8210
			public BlackMarket.DemandSupplyEntry[] demands;

			// Token: 0x04002013 RID: 8211
			public BlackMarket.DemandSupplyEntry[] supplies;
		}
	}
}
