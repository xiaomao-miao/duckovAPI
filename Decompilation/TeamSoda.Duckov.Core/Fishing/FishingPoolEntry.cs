using System;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing
{
	// Token: 0x02000215 RID: 533
	[Serializable]
	internal struct FishingPoolEntry
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0003E2ED File Offset: 0x0003C4ED
		public int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0003E2F5 File Offset: 0x0003C4F5
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x04000CBC RID: 3260
		[SerializeField]
		[ItemTypeID]
		private int id;

		// Token: 0x04000CBD RID: 3261
		[SerializeField]
		private float weight;
	}
}
