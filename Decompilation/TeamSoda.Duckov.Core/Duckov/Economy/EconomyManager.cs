using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x02000324 RID: 804
	public class EconomyManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00060DF4 File Offset: 0x0005EFF4
		public static string ItemUnlockNotificationTextMainFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextMainFormat;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x00060E06 File Offset: 0x0005F006
		public static string ItemUnlockNotificationTextSubFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextSubFormat;
			}
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06001AD1 RID: 6865 RVA: 0x00060E18 File Offset: 0x0005F018
		// (remove) Token: 0x06001AD2 RID: 6866 RVA: 0x00060E4C File Offset: 0x0005F04C
		public static event Action OnEconomyManagerLoaded;

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x00060E7F File Offset: 0x0005F07F
		// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x00060E86 File Offset: 0x0005F086
		public static EconomyManager Instance { get; private set; }

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00060E8E File Offset: 0x0005F08E
		private void Awake()
		{
			if (EconomyManager.Instance == null)
			{
				EconomyManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			this.Load();
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00060ECB File Offset: 0x0005F0CB
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00060ED3 File Offset: 0x0005F0D3
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00060EDC File Offset: 0x0005F0DC
		private void Load()
		{
			if (SavesSystem.KeyExisits("EconomyData"))
			{
				this.SetupSaveData(SavesSystem.Load<EconomyManager.SaveData>("EconomyData"));
			}
			try
			{
				Action onEconomyManagerLoaded = EconomyManager.OnEconomyManagerLoaded;
				if (onEconomyManagerLoaded != null)
				{
					onEconomyManagerLoaded();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00060F34 File Offset: 0x0005F134
		private void Save()
		{
			SavesSystem.Save<EconomyManager.SaveData>("EconomyData", (EconomyManager.SaveData)this.GenerateSaveData());
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00060F4B File Offset: 0x0005F14B
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x00060F6F File Offset: 0x0005F16F
		// (set) Token: 0x06001ADC RID: 6876 RVA: 0x00060F8C File Offset: 0x0005F18C
		public static long Money
		{
			get
			{
				if (EconomyManager.Instance == null)
				{
					return 0L;
				}
				return EconomyManager.Instance.money;
			}
			private set
			{
				long arg = EconomyManager.Money;
				if (EconomyManager.Instance == null)
				{
					return;
				}
				EconomyManager.Instance.money = value;
				Action<long, long> onMoneyChanged = EconomyManager.OnMoneyChanged;
				if (onMoneyChanged == null)
				{
					return;
				}
				onMoneyChanged(arg, value);
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00060FC9 File Offset: 0x0005F1C9
		public static long Cash
		{
			get
			{
				return (long)ItemUtilities.GetItemCount(451);
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x00060FD6 File Offset: 0x0005F1D6
		public ReadOnlyCollection<int> UnlockedItemIds
		{
			get
			{
				return this.unlockedItemIds.AsReadOnly();
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06001ADF RID: 6879 RVA: 0x00060FE4 File Offset: 0x0005F1E4
		// (remove) Token: 0x06001AE0 RID: 6880 RVA: 0x00061018 File Offset: 0x0005F218
		public static event Action<long, long> OnMoneyChanged;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001AE1 RID: 6881 RVA: 0x0006104C File Offset: 0x0005F24C
		// (remove) Token: 0x06001AE2 RID: 6882 RVA: 0x00061080 File Offset: 0x0005F280
		public static event Action<int> OnItemUnlockStateChanged;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001AE3 RID: 6883 RVA: 0x000610B4 File Offset: 0x0005F2B4
		// (remove) Token: 0x06001AE4 RID: 6884 RVA: 0x000610E8 File Offset: 0x0005F2E8
		public static event Action<long> OnMoneyPaid;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001AE5 RID: 6885 RVA: 0x0006111C File Offset: 0x0005F31C
		// (remove) Token: 0x06001AE6 RID: 6886 RVA: 0x00061150 File Offset: 0x0005F350
		public static event Action<Cost> OnCostPaid;

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00061184 File Offset: 0x0005F384
		private static bool Pay(long amount, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < amount)
			{
				return false;
			}
			long num3 = amount;
			if (accountAvaliable)
			{
				if (num > amount)
				{
					num3 = 0L;
					EconomyManager.Money -= amount;
				}
				else
				{
					num3 -= num;
					EconomyManager.Money = 0L;
				}
			}
			if (cashAvaliale && num3 > 0L)
			{
				ItemUtilities.ConsumeItems(451, num3);
			}
			if (amount > 0L)
			{
				Action<long> onMoneyPaid = EconomyManager.OnMoneyPaid;
				if (onMoneyPaid != null)
				{
					onMoneyPaid(amount);
				}
			}
			return true;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00061206 File Offset: 0x0005F406
		public static bool Pay(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			if (!EconomyManager.IsEnough(cost, accountAvaliable, true))
			{
				return false;
			}
			if (!EconomyManager.Pay(cost.money, accountAvaliable, cashAvaliale))
			{
				return false;
			}
			if (!ItemUtilities.ConsumeItems(cost))
			{
				return false;
			}
			Action<Cost> onCostPaid = EconomyManager.OnCostPaid;
			if (onCostPaid != null)
			{
				onCostPaid(cost);
			}
			return true;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00061244 File Offset: 0x0005F444
		public static bool IsEnough(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < cost.money)
			{
				return false;
			}
			if (cost.items != null)
			{
				foreach (Cost.ItemEntry itemEntry in cost.items)
				{
					if ((long)ItemUtilities.GetItemCount(itemEntry.id) < itemEntry.amount)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000612B2 File Offset: 0x0005F4B2
		public static bool Add(long amount)
		{
			if (EconomyManager.Instance == null)
			{
				return false;
			}
			EconomyManager.Money += amount;
			return true;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x000612D0 File Offset: 0x0005F4D0
		public static bool IsWaitingForUnlockConfirm(int itemTypeID)
		{
			return !GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) && !(EconomyManager.Instance == null) && EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00061305 File Offset: 0x0005F505
		public static bool IsUnlocked(int itemTypeID)
		{
			return GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) || (!(EconomyManager.Instance == null) && EconomyManager.Instance.UnlockedItemIds.Contains(itemTypeID));
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0006133C File Offset: 0x0005F53C
		public static void Unlock(int itemTypeID, bool needConfirm = true, bool showUI = true)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			if (EconomyManager.Instance.unlockedItemIds.Contains(itemTypeID))
			{
				return;
			}
			if (EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID))
			{
				return;
			}
			if (needConfirm)
			{
				EconomyManager.Instance.unlockesWaitingForConfirm.Add(itemTypeID);
			}
			else
			{
				EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			}
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged != null)
			{
				onItemUnlockStateChanged(itemTypeID);
			}
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			Debug.Log(EconomyManager.ItemUnlockNotificationTextMainFormat);
			Debug.Log(metaData.DisplayName);
			if (showUI)
			{
				NotificationText.Push("Notification_StockShoopItemUnlockFormat".ToPlainText().Format(new
				{
					displayName = metaData.DisplayName
				}));
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000613F4 File Offset: 0x0005F5F4
		public static void ConfirmUnlock(int itemTypeID)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			EconomyManager.Instance.unlockesWaitingForConfirm.Remove(itemTypeID);
			EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged == null)
			{
				return;
			}
			onItemUnlockStateChanged(itemTypeID);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00061440 File Offset: 0x0005F640
		public object GenerateSaveData()
		{
			return new EconomyManager.SaveData
			{
				money = EconomyManager.Money,
				unlockedItems = this.unlockedItemIds.ToArray(),
				unlockesWaitingForConfirm = this.unlockesWaitingForConfirm.ToArray()
			};
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0006148C File Offset: 0x0005F68C
		public void SetupSaveData(object rawData)
		{
			if (rawData is EconomyManager.SaveData)
			{
				EconomyManager.SaveData saveData = (EconomyManager.SaveData)rawData;
				this.money = saveData.money;
				this.unlockedItemIds.Clear();
				if (saveData.unlockedItems != null)
				{
					this.unlockedItemIds.AddRange(saveData.unlockedItems);
				}
				this.unlockesWaitingForConfirm.Clear();
				if (saveData.unlockesWaitingForConfirm != null)
				{
					this.unlockesWaitingForConfirm.AddRange(saveData.unlockesWaitingForConfirm);
				}
				return;
			}
		}

		// Token: 0x0400131C RID: 4892
		[SerializeField]
		private string itemUnlockNotificationTextMainFormat = "物品 {itemDisplayName} 已解锁";

		// Token: 0x0400131D RID: 4893
		[SerializeField]
		private string itemUnlockNotificationTextSubFormat = "请在对应商店中查看";

		// Token: 0x04001320 RID: 4896
		private const string saveKey = "EconomyData";

		// Token: 0x04001321 RID: 4897
		private long money;

		// Token: 0x04001322 RID: 4898
		[SerializeField]
		private List<int> unlockedItemIds;

		// Token: 0x04001323 RID: 4899
		[SerializeField]
		private List<int> unlockesWaitingForConfirm;

		// Token: 0x04001328 RID: 4904
		public const int CashItemID = 451;

		// Token: 0x020005C2 RID: 1474
		[Serializable]
		public struct SaveData
		{
			// Token: 0x0400207D RID: 8317
			public long money;

			// Token: 0x0400207E RID: 8318
			public int[] unlockedItems;

			// Token: 0x0400207F RID: 8319
			public int[] unlockesWaitingForConfirm;
		}
	}
}
