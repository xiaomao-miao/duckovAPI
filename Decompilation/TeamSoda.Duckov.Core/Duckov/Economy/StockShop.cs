using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x02000326 RID: 806
	public class StockShop : MonoBehaviour, IMerchant, ISaveDataProvider
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x000617DC File Offset: 0x0005F9DC
		public string MerchantID
		{
			get
			{
				return this.merchantID;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000617E4 File Offset: 0x0005F9E4
		public string OpinionKey
		{
			get
			{
				return "Opinion_" + this.merchantID;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x000617F6 File Offset: 0x0005F9F6
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00061803 File Offset: 0x0005FA03
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x0006181A File Offset: 0x0005FA1A
		private int Opinion
		{
			get
			{
				return Mathf.Clamp(CommonVariables.GetInt(this.OpinionKey, 0), -100, 100);
			}
			set
			{
				CommonVariables.SetInt(this.OpinionKey, value);
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x00061828 File Offset: 0x0005FA28
		public string PurchaseNotificationTextFormat
		{
			get
			{
				return this.purchaseNotificationTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x00061835 File Offset: 0x0005FA35
		public bool AccountAvaliable
		{
			get
			{
				return this.accountAvaliable;
			}
		}

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001B03 RID: 6915 RVA: 0x00061840 File Offset: 0x0005FA40
		// (remove) Token: 0x06001B04 RID: 6916 RVA: 0x00061874 File Offset: 0x0005FA74
		public static event Action<StockShop> OnAfterItemSold;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001B05 RID: 6917 RVA: 0x000618A8 File Offset: 0x0005FAA8
		// (remove) Token: 0x06001B06 RID: 6918 RVA: 0x000618DC File Offset: 0x0005FADC
		public static event Action<StockShop, Item> OnItemPurchased;

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001B07 RID: 6919 RVA: 0x00061910 File Offset: 0x0005FB10
		// (remove) Token: 0x06001B08 RID: 6920 RVA: 0x00061944 File Offset: 0x0005FB44
		public static event Action<StockShop, Item, int> OnItemSoldByPlayer;

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00061978 File Offset: 0x0005FB78
		public TimeSpan TimeSinceLastRefresh
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return DateTime.UtcNow - dateTime;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x000619C8 File Offset: 0x0005FBC8
		public TimeSpan NextRefreshETA
		{
			get
			{
				TimeSpan timeSinceLastRefresh = this.TimeSinceLastRefresh;
				TimeSpan timeSpan = TimeSpan.FromTicks(this.refreshAfterTimeSpan) - timeSinceLastRefresh;
				if (timeSpan < TimeSpan.Zero)
				{
					timeSpan = TimeSpan.Zero;
				}
				return timeSpan;
			}
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x00061A04 File Offset: 0x0005FC04
		private UniTask<Item> GetItemInstance(int typeID)
		{
			StockShop.<GetItemInstance>d__40 <GetItemInstance>d__;
			<GetItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<GetItemInstance>d__.<>4__this = this;
			<GetItemInstance>d__.typeID = typeID;
			<GetItemInstance>d__.<>1__state = -1;
			<GetItemInstance>d__.<>t__builder.Start<StockShop.<GetItemInstance>d__40>(ref <GetItemInstance>d__);
			return <GetItemInstance>d__.<>t__builder.Task;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x00061A50 File Offset: 0x0005FC50
		public Item GetItemInstanceDirect(int typeID)
		{
			Item result;
			if (this.itemInstances.TryGetValue(typeID, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x00061A70 File Offset: 0x0005FC70
		private void Awake()
		{
			this.InitializeEntries();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x00061AA0 File Offset: 0x0005FCA0
		private void InitializeEntries()
		{
			StockShopDatabase.MerchantProfile merchantProfile = StockShopDatabase.Instance.GetMerchantProfile(this.merchantID);
			if (merchantProfile == null)
			{
				Debug.Log("未配置商人 " + this.merchantID);
				return;
			}
			foreach (StockShopDatabase.ItemEntry cur in merchantProfile.entries)
			{
				this.entries.Add(new StockShop.Entry(cur));
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x00061B28 File Offset: 0x0005FD28
		private string SaveKey
		{
			get
			{
				return "StockShop_" + this.merchantID;
			}
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00061B3C File Offset: 0x0005FD3C
		private void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			StockShop.SaveData dataRaw = SavesSystem.Load<StockShop.SaveData>(this.SaveKey);
			this.SetupSaveData(dataRaw);
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00061B6C File Offset: 0x0005FD6C
		private void Save()
		{
			StockShop.SaveData saveData = this.GenerateSaveData() as StockShop.SaveData;
			if (saveData == null)
			{
				Debug.LogError("没法正确生成StockShop的SaveData");
				return;
			}
			SavesSystem.Save<StockShop.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00061B9F File Offset: 0x0005FD9F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x00061BC4 File Offset: 0x0005FDC4
		private void Start()
		{
			this.CacheItemInstances().Forget();
			if (this.refreshStockOnStart)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00061C00 File Offset: 0x0005FE00
		private UniTask CacheItemInstances()
		{
			StockShop.<CacheItemInstances>d__50 <CacheItemInstances>d__;
			<CacheItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CacheItemInstances>d__.<>4__this = this;
			<CacheItemInstances>d__.<>1__state = -1;
			<CacheItemInstances>d__.<>t__builder.Start<StockShop.<CacheItemInstances>d__50>(ref <CacheItemInstances>d__);
			return <CacheItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00061C44 File Offset: 0x0005FE44
		internal void RefreshIfNeeded()
		{
			TimeSpan t = TimeSpan.FromTicks(this.refreshAfterTimeSpan);
			DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
			if (dateTime > DateTime.UtcNow)
			{
				dateTime = DateTime.UtcNow;
				this.lastTimeRefreshedStock = dateTime.ToBinary();
			}
			DateTime t2 = DateTime.UtcNow - TimeSpan.FromDays(2.0);
			if (dateTime < t2)
			{
				this.lastTimeRefreshedStock = t2.ToBinary();
			}
			if (DateTime.UtcNow - dateTime > t)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00061CE4 File Offset: 0x0005FEE4
		private void DoRefreshStock()
		{
			bool advancedDebuffMode = LevelManager.Rule.AdvancedDebuffMode;
			foreach (StockShop.Entry entry in this.entries)
			{
				if (entry.Possibility > 0f && entry.Possibility < 1f && UnityEngine.Random.Range(0f, 1f) > entry.Possibility)
				{
					entry.Show = false;
					entry.CurrentStock = 0;
				}
				else
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(entry.ItemTypeID);
					if (!advancedDebuffMode && metaData.tags.Contains(GameplayDataSettings.Tags.AdvancedDebuffMode))
					{
						entry.Show = false;
						entry.CurrentStock = 0;
					}
					else
					{
						entry.Show = true;
						entry.CurrentStock = entry.MaxStock;
					}
				}
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00061DCC File Offset: 0x0005FFCC
		public bool Busy
		{
			get
			{
				return this.buying || this.selling;
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00061DE4 File Offset: 0x0005FFE4
		public UniTask<bool> Buy(int itemTypeID, int amount = 1)
		{
			StockShop.<Buy>d__57 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.itemTypeID = itemTypeID;
			<Buy>d__.amount = amount;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<StockShop.<Buy>d__57>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00061E38 File Offset: 0x00060038
		private UniTask<bool> BuyTask(int itemTypeID, int amount = 1)
		{
			StockShop.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.amount = amount;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShop.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00061E8C File Offset: 0x0006008C
		internal UniTask Sell(Item target)
		{
			StockShop.<Sell>d__59 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.target = target;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<StockShop.<Sell>d__59>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00061ED7 File Offset: 0x000600D7
		public void ShowUI()
		{
			if (!StockShopView.Instance)
			{
				return;
			}
			this.RefreshIfNeeded();
			StockShopView.Instance.SetupAndShow(this);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00061EF8 File Offset: 0x000600F8
		public int ConvertPrice(Item item, bool selling = false)
		{
			int num = item.GetTotalRawValue();
			if (!selling)
			{
				StockShop.Entry entry = this.entries.Find((StockShop.Entry e) => e != null && e.ItemTypeID == item.TypeID);
				if (entry != null)
				{
					num = Mathf.FloorToInt((float)num * entry.PriceFactor);
				}
			}
			if (selling)
			{
				float factor = this.sellFactor;
				StockShop.OverrideSellingPriceEntry overrideSellingPriceEntry = this.overrideSellingPrice.Find((StockShop.OverrideSellingPriceEntry e) => e.typeID == item.TypeID);
				if (overrideSellingPriceEntry != null)
				{
					factor = overrideSellingPriceEntry.factor;
				}
				return Mathf.FloorToInt((float)num * factor);
			}
			return num;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00061F88 File Offset: 0x00060188
		public object GenerateSaveData()
		{
			StockShop.SaveData saveData = new StockShop.SaveData();
			saveData.lastTimeRefreshedStock = this.lastTimeRefreshedStock;
			foreach (StockShop.Entry entry in this.entries)
			{
				saveData.stockCounts.Add(new StockShop.SaveData.StockCountEntry
				{
					itemTypeID = entry.ItemTypeID,
					stock = entry.CurrentStock
				});
			}
			return saveData;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00062010 File Offset: 0x00060210
		public void SetupSaveData(object dataRaw)
		{
			StockShop.SaveData saveData = dataRaw as StockShop.SaveData;
			if (saveData == null)
			{
				return;
			}
			this.lastTimeRefreshedStock = saveData.lastTimeRefreshedStock;
			using (List<StockShop.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StockShop.Entry cur = enumerator.Current;
					StockShop.SaveData.StockCountEntry stockCountEntry = saveData.stockCounts.Find((StockShop.SaveData.StockCountEntry e) => e != null && e.itemTypeID == cur.ItemTypeID);
					if (stockCountEntry != null)
					{
						cur.Show = (stockCountEntry.stock > 0);
						cur.CurrentStock = stockCountEntry.stock;
					}
				}
			}
		}

		// Token: 0x0400132C RID: 4908
		[SerializeField]
		private string merchantID = "Albert";

		// Token: 0x0400132D RID: 4909
		[LocalizationKey("Default")]
		public string DisplayNameKey;

		// Token: 0x0400132E RID: 4910
		[TimeSpan]
		[SerializeField]
		private long refreshAfterTimeSpan;

		// Token: 0x0400132F RID: 4911
		[SerializeField]
		private string purchaseNotificationTextFormatKey = "UI_StockShop_PurchasedNotification";

		// Token: 0x04001330 RID: 4912
		[SerializeField]
		private bool accountAvaliable;

		// Token: 0x04001331 RID: 4913
		[SerializeField]
		private bool returnCash;

		// Token: 0x04001332 RID: 4914
		[SerializeField]
		private bool refreshStockOnStart;

		// Token: 0x04001333 RID: 4915
		public float sellFactor = 0.5f;

		// Token: 0x04001334 RID: 4916
		public List<StockShop.Entry> entries = new List<StockShop.Entry>();

		// Token: 0x04001335 RID: 4917
		public List<StockShop.OverrideSellingPriceEntry> overrideSellingPrice = new List<StockShop.OverrideSellingPriceEntry>();

		// Token: 0x04001339 RID: 4921
		[DateTime]
		[SerializeField]
		private long lastTimeRefreshedStock;

		// Token: 0x0400133A RID: 4922
		private Dictionary<int, Item> itemInstances = new Dictionary<int, Item>();

		// Token: 0x0400133B RID: 4923
		private bool buying;

		// Token: 0x0400133C RID: 4924
		private bool selling;

		// Token: 0x020005C5 RID: 1477
		public class Entry
		{
			// Token: 0x06002901 RID: 10497 RVA: 0x00097C7A File Offset: 0x00095E7A
			public Entry(StockShopDatabase.ItemEntry cur)
			{
				this.entry = cur;
			}

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x06002902 RID: 10498 RVA: 0x00097C90 File Offset: 0x00095E90
			public int MaxStock
			{
				get
				{
					if (this.entry.maxStock < 1)
					{
						this.entry.maxStock = 1;
					}
					return this.entry.maxStock;
				}
			}

			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x06002903 RID: 10499 RVA: 0x00097CB7 File Offset: 0x00095EB7
			public int ItemTypeID
			{
				get
				{
					return this.entry.typeID;
				}
			}

			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x06002904 RID: 10500 RVA: 0x00097CC4 File Offset: 0x00095EC4
			public bool ForceUnlock
			{
				get
				{
					return (!GameMetaData.Instance.IsDemo || !this.entry.lockInDemo) && this.entry.forceUnlock;
				}
			}

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x06002905 RID: 10501 RVA: 0x00097CEC File Offset: 0x00095EEC
			public float PriceFactor
			{
				get
				{
					return this.entry.priceFactor;
				}
			}

			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x06002906 RID: 10502 RVA: 0x00097CF9 File Offset: 0x00095EF9
			public float Possibility
			{
				get
				{
					return this.entry.possibility;
				}
			}

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06002907 RID: 10503 RVA: 0x00097D06 File Offset: 0x00095F06
			// (set) Token: 0x06002908 RID: 10504 RVA: 0x00097D0E File Offset: 0x00095F0E
			public bool Show
			{
				get
				{
					return this.show;
				}
				set
				{
					this.show = value;
				}
			}

			// Token: 0x17000789 RID: 1929
			// (get) Token: 0x06002909 RID: 10505 RVA: 0x00097D17 File Offset: 0x00095F17
			// (set) Token: 0x0600290A RID: 10506 RVA: 0x00097D1F File Offset: 0x00095F1F
			public int CurrentStock
			{
				get
				{
					return this.currentStock;
				}
				set
				{
					this.currentStock = value;
					Action<StockShop.Entry> action = this.onStockChanged;
					if (action == null)
					{
						return;
					}
					action(this);
				}
			}

			// Token: 0x140000F9 RID: 249
			// (add) Token: 0x0600290B RID: 10507 RVA: 0x00097D3C File Offset: 0x00095F3C
			// (remove) Token: 0x0600290C RID: 10508 RVA: 0x00097D74 File Offset: 0x00095F74
			public event Action<StockShop.Entry> onStockChanged;

			// Token: 0x04002090 RID: 8336
			private StockShopDatabase.ItemEntry entry;

			// Token: 0x04002091 RID: 8337
			[SerializeField]
			private bool show = true;

			// Token: 0x04002092 RID: 8338
			[SerializeField]
			private int currentStock;
		}

		// Token: 0x020005C6 RID: 1478
		[Serializable]
		public class OverrideSellingPriceEntry
		{
			// Token: 0x04002094 RID: 8340
			[ItemTypeID]
			public int typeID;

			// Token: 0x04002095 RID: 8341
			public float factor = 0.5f;
		}

		// Token: 0x020005C7 RID: 1479
		[Serializable]
		private class SaveData
		{
			// Token: 0x04002096 RID: 8342
			[DateTime]
			public long lastTimeRefreshedStock;

			// Token: 0x04002097 RID: 8343
			public List<StockShop.SaveData.StockCountEntry> stockCounts = new List<StockShop.SaveData.StockCountEntry>();

			// Token: 0x02000676 RID: 1654
			public class StockCountEntry
			{
				// Token: 0x04002337 RID: 9015
				public int itemTypeID;

				// Token: 0x04002338 RID: 9016
				public int stock;
			}
		}
	}
}
