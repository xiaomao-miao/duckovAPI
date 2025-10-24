using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

namespace Duckov.Bitcoins
{
	// Token: 0x0200030D RID: 781
	public class BitcoinMiner : MonoBehaviour
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0005CD37 File Offset: 0x0005AF37
		// (set) Token: 0x060019AC RID: 6572 RVA: 0x0005CD3E File Offset: 0x0005AF3E
		public static BitcoinMiner Instance { get; private set; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060019AD RID: 6573 RVA: 0x0005CD46 File Offset: 0x0005AF46
		private double Progress
		{
			get
			{
				return this.work;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x0005CD4E File Offset: 0x0005AF4E
		private static double K_1_12
		{
			get
			{
				if (BitcoinMiner._cached_k == null)
				{
					BitcoinMiner._cached_k = new double?((BitcoinMiner.wps_12 - BitcoinMiner.wps_1) / 11.0);
				}
				return BitcoinMiner._cached_k.Value;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x0005CD88 File Offset: 0x0005AF88
		public double WorkPerSecond
		{
			get
			{
				if (this.IsInventoryFull)
				{
					return 0.0;
				}
				if (this.cachedPerformance < 1f)
				{
					return (double)this.cachedPerformance * BitcoinMiner.wps_1;
				}
				return BitcoinMiner.wps_1 + (double)(this.cachedPerformance - 1f) * BitcoinMiner.K_1_12;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0005CDDB File Offset: 0x0005AFDB
		public double HoursPerCoin
		{
			get
			{
				return this.workPerCoin / 3600.0 / this.WorkPerSecond;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0005CDF4 File Offset: 0x0005AFF4
		public bool IsInventoryFull
		{
			get
			{
				return !(this.item == null) && this.item.Inventory.GetFirstEmptyPosition(0) < 0;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0005CE1A File Offset: 0x0005B01A
		public TimeSpan TimePerCoin
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds(this.workPerCoin / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0005CE45 File Offset: 0x0005B045
		public TimeSpan RemainingTime
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds((this.workPerCoin - this.work) / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0005CE78 File Offset: 0x0005B078
		// (set) Token: 0x060019B5 RID: 6581 RVA: 0x0005CEBD File Offset: 0x0005B0BD
		private DateTime LastUpdateDateTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastUpdateDateTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					this.lastUpdateDateTimeRaw = DateTime.UtcNow.ToBinary();
					dateTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0005CECC File Offset: 0x0005B0CC
		private void Awake()
		{
			if (BitcoinMiner.Instance != null)
			{
				Debug.LogError("存在多个BitcoinMiner");
				return;
			}
			BitcoinMiner.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0005CEFD File Offset: 0x0005B0FD
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0005CF10 File Offset: 0x0005B110
		private void Start()
		{
			this.Load();
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0005CF18 File Offset: 0x0005B118
		// (set) Token: 0x060019BA RID: 6586 RVA: 0x0005CF20 File Offset: 0x0005B120
		public bool Loading { get; private set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0005CF29 File Offset: 0x0005B129
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x0005CF31 File Offset: 0x0005B131
		public bool Initialized { get; private set; }

		// Token: 0x060019BD RID: 6589 RVA: 0x0005CF3C File Offset: 0x0005B13C
		private UniTask Setup(BitcoinMiner.SaveData data)
		{
			BitcoinMiner.<Setup>d__43 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.data = data;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<BitcoinMiner.<Setup>d__43>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0005CF88 File Offset: 0x0005B188
		private UniTask Initialize()
		{
			BitcoinMiner.<Initialize>d__44 <Initialize>d__;
			<Initialize>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Initialize>d__.<>4__this = this;
			<Initialize>d__.<>1__state = -1;
			<Initialize>d__.<>t__builder.Start<BitcoinMiner.<Initialize>d__44>(ref <Initialize>d__);
			return <Initialize>d__.<>t__builder.Task;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0005CFCC File Offset: 0x0005B1CC
		private void Load()
		{
			if (SavesSystem.KeyExisits("BitcoinMiner_Data"))
			{
				BitcoinMiner.SaveData data = SavesSystem.Load<BitcoinMiner.SaveData>("BitcoinMiner_Data");
				this.Setup(data).Forget();
				return;
			}
			this.Initialize().Forget();
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0005D008 File Offset: 0x0005B208
		private void Save()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			BitcoinMiner.SaveData value = new BitcoinMiner.SaveData
			{
				itemData = ItemTreeData.FromItem(this.item),
				work = this.work,
				lastUpdateDateTimeRaw = this.lastUpdateDateTimeRaw,
				cachedPerformance = this.cachedPerformance
			};
			SavesSystem.Save<BitcoinMiner.SaveData>("BitcoinMiner_Data", value);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0005D078 File Offset: 0x0005B278
		private void UpdateWork()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			double totalSeconds = (DateTime.UtcNow - this.LastUpdateDateTime).TotalSeconds;
			double num = this.WorkPerSecond * totalSeconds;
			bool isInventoryFull = this.IsInventoryFull;
			if (this.work < 0.0)
			{
				this.work = 0.0;
			}
			this.work += num;
			if (this.work >= this.workPerCoin && !this.CreatingCoin)
			{
				if (!isInventoryFull)
				{
					this.CreateCoin().Forget();
				}
				else
				{
					this.work = this.workPerCoin;
				}
			}
			this.cachedPerformance = this.item.GetStatValue("Performance".GetHashCode());
			this.LastUpdateDateTime = DateTime.UtcNow;
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0005D146 File Offset: 0x0005B346
		// (set) Token: 0x060019C3 RID: 6595 RVA: 0x0005D14E File Offset: 0x0005B34E
		public bool CreatingCoin { get; private set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0005D157 File Offset: 0x0005B357
		public Item Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0005D15F File Offset: 0x0005B35F
		public float NormalizedProgress
		{
			get
			{
				return (float)(this.work / this.workPerCoin);
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0005D16F File Offset: 0x0005B36F
		public double Performance
		{
			get
			{
				if (this.Item == null)
				{
					return 0.0;
				}
				return (double)this.Item.GetStatValue("Performance".GetHashCode());
			}
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0005D1A0 File Offset: 0x0005B3A0
		private UniTask CreateCoin()
		{
			BitcoinMiner.<CreateCoin>d__60 <CreateCoin>d__;
			<CreateCoin>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCoin>d__.<>4__this = this;
			<CreateCoin>d__.<>1__state = -1;
			<CreateCoin>d__.<>t__builder.Start<BitcoinMiner.<CreateCoin>d__60>(ref <CreateCoin>d__);
			return <CreateCoin>d__.<>t__builder.Task;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0005D1E3 File Offset: 0x0005B3E3
		private void FixedUpdate()
		{
			this.UpdateWork();
		}

		// Token: 0x0400128F RID: 4751
		[SerializeField]
		[ItemTypeID]
		private int minerItemID = 397;

		// Token: 0x04001290 RID: 4752
		[SerializeField]
		[ItemTypeID]
		private int coinItemID = 388;

		// Token: 0x04001291 RID: 4753
		[SerializeField]
		private double workPerCoin = 1.0;

		// Token: 0x04001292 RID: 4754
		private Item item;

		// Token: 0x04001293 RID: 4755
		private double work;

		// Token: 0x04001294 RID: 4756
		private static readonly double wps_1 = 2.3148148148148147E-05;

		// Token: 0x04001295 RID: 4757
		private static readonly double wps_12 = 5.555555555555556E-05;

		// Token: 0x04001296 RID: 4758
		private static double? _cached_k;

		// Token: 0x04001297 RID: 4759
		[DateTime]
		private long lastUpdateDateTimeRaw;

		// Token: 0x04001298 RID: 4760
		private float cachedPerformance;

		// Token: 0x0400129B RID: 4763
		public const string SaveKey = "BitcoinMiner_Data";

		// Token: 0x0400129C RID: 4764
		private const string PerformaceStatKey = "Performance";

		// Token: 0x020005A0 RID: 1440
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04002023 RID: 8227
			public ItemTreeData itemData;

			// Token: 0x04002024 RID: 8228
			public double work;

			// Token: 0x04002025 RID: 8229
			public float cachedPerformance;

			// Token: 0x04002026 RID: 8230
			public long lastUpdateDateTimeRaw;
		}
	}
}
