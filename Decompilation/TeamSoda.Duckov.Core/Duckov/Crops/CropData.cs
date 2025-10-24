using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EB RID: 747
	[Serializable]
	public struct CropData
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x0005792E File Offset: 0x00055B2E
		public ProductRanking Ranking
		{
			get
			{
				if (this.score < 33)
				{
					return ProductRanking.Poor;
				}
				if (this.score < 66)
				{
					return ProductRanking.Normal;
				}
				return ProductRanking.Good;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x00057949 File Offset: 0x00055B49
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.growTicks);
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00057956 File Offset: 0x00055B56
		// (set) Token: 0x060017F3 RID: 6131 RVA: 0x00057963 File Offset: 0x00055B63
		public DateTime LastUpdateDateTime
		{
			get
			{
				return DateTime.FromBinary(this.lastUpdateDateTimeRaw);
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x04001176 RID: 4470
		public string gardenID;

		// Token: 0x04001177 RID: 4471
		public Vector2Int coord;

		// Token: 0x04001178 RID: 4472
		public string cropID;

		// Token: 0x04001179 RID: 4473
		public int score;

		// Token: 0x0400117A RID: 4474
		public bool watered;

		// Token: 0x0400117B RID: 4475
		[TimeSpan]
		public long growTicks;

		// Token: 0x0400117C RID: 4476
		[DateTime]
		public long lastUpdateDateTimeRaw;
	}
}
