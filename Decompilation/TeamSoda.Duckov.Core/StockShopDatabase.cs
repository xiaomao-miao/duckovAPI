using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000152 RID: 338
[CreateAssetMenu(menuName = "Duckov/Stock Shop Database")]
public class StockShopDatabase : ScriptableObject
{
	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0002DB4C File Offset: 0x0002BD4C
	public static StockShopDatabase Instance
	{
		get
		{
			return GameplayDataSettings.StockshopDatabase;
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0002DB54 File Offset: 0x0002BD54
	public StockShopDatabase.MerchantProfile GetMerchantProfile(string merchantID)
	{
		return this.merchantProfiles.Find((StockShopDatabase.MerchantProfile e) => e.merchantID == merchantID);
	}

	// Token: 0x0400091A RID: 2330
	public List<StockShopDatabase.MerchantProfile> merchantProfiles;

	// Token: 0x020004A8 RID: 1192
	[Serializable]
	public class MerchantProfile
	{
		// Token: 0x04001C39 RID: 7225
		public string merchantID;

		// Token: 0x04001C3A RID: 7226
		public List<StockShopDatabase.ItemEntry> entries = new List<StockShopDatabase.ItemEntry>();
	}

	// Token: 0x020004A9 RID: 1193
	[Serializable]
	public class ItemEntry
	{
		// Token: 0x04001C3B RID: 7227
		[ItemTypeID]
		public int typeID;

		// Token: 0x04001C3C RID: 7228
		public int maxStock;

		// Token: 0x04001C3D RID: 7229
		public bool forceUnlock;

		// Token: 0x04001C3E RID: 7230
		public float priceFactor;

		// Token: 0x04001C3F RID: 7231
		public float possibility;

		// Token: 0x04001C40 RID: 7232
		public bool lockInDemo;
	}
}
