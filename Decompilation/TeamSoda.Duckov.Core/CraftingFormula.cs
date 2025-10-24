using System;
using Duckov.Economy;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001A4 RID: 420
[Serializable]
public struct CraftingFormula
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0003475B File Offset: 0x0003295B
	public bool IDValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.id);
		}
	}

	// Token: 0x04000ACA RID: 2762
	public string id;

	// Token: 0x04000ACB RID: 2763
	public CraftingFormula.ItemEntry result;

	// Token: 0x04000ACC RID: 2764
	public string[] tags;

	// Token: 0x04000ACD RID: 2765
	[SerializeField]
	public Cost cost;

	// Token: 0x04000ACE RID: 2766
	public bool unlockByDefault;

	// Token: 0x04000ACF RID: 2767
	public bool lockInDemo;

	// Token: 0x04000AD0 RID: 2768
	public string requirePerk;

	// Token: 0x04000AD1 RID: 2769
	public bool hideInIndex;

	// Token: 0x020004BF RID: 1215
	[Serializable]
	public struct ItemEntry
	{
		// Token: 0x04001C9A RID: 7322
		[ItemTypeID]
		public int id;

		// Token: 0x04001C9B RID: 7323
		public int amount;
	}
}
