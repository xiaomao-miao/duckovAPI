using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E9 RID: 745
	[Serializable]
	public struct CropInfo
	{
		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x00057880 File Offset: 0x00055A80
		public string DisplayName
		{
			get
			{
				if (this._normalMetaData == null)
				{
					this._normalMetaData = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.resultNormal));
				}
				return this._normalMetaData.Value.DisplayName;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x000578C3 File Offset: 0x00055AC3
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.totalGrowTicks);
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x000578D0 File Offset: 0x00055AD0
		public int GetProduct(ProductRanking ranking)
		{
			int num = 0;
			switch (ranking)
			{
			case ProductRanking.Poor:
				num = this.resultPoor;
				break;
			case ProductRanking.Normal:
				num = this.resultNormal;
				break;
			case ProductRanking.Good:
				num = this.resultGood;
				break;
			}
			if (num == 0)
			{
				if (this.resultNormal != 0)
				{
					return this.resultNormal;
				}
				if (this.resultPoor != 0)
				{
					return this.resultPoor;
				}
			}
			return num;
		}

		// Token: 0x0400116A RID: 4458
		public string id;

		// Token: 0x0400116B RID: 4459
		public GameObject displayPrefab;

		// Token: 0x0400116C RID: 4460
		[ItemTypeID]
		public int resultPoor;

		// Token: 0x0400116D RID: 4461
		[ItemTypeID]
		public int resultNormal;

		// Token: 0x0400116E RID: 4462
		[ItemTypeID]
		public int resultGood;

		// Token: 0x0400116F RID: 4463
		private ItemMetaData? _normalMetaData;

		// Token: 0x04001170 RID: 4464
		public int resultAmount;

		// Token: 0x04001171 RID: 4465
		[TimeSpan]
		public long totalGrowTicks;
	}
}
