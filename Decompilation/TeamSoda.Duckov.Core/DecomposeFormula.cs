using System;
using Duckov.Economy;
using ItemStatsSystem;

// Token: 0x020001A8 RID: 424
[Serializable]
public struct DecomposeFormula
{
	// Token: 0x04000ADB RID: 2779
	[ItemTypeID]
	public int item;

	// Token: 0x04000ADC RID: 2780
	public bool valid;

	// Token: 0x04000ADD RID: 2781
	public Cost result;
}
