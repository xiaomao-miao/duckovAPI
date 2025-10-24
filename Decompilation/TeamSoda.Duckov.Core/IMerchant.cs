using System;
using ItemStatsSystem;

// Token: 0x02000200 RID: 512
public interface IMerchant
{
	// Token: 0x06000F01 RID: 3841
	int ConvertPrice(Item item, bool selling = false);
}
