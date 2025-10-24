using System;
using Duckov.Utilities;
using ItemStatsSystem;

namespace Duckov.Crops
{
	// Token: 0x020002E8 RID: 744
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x0005786E File Offset: 0x00055A6E
		public string GetRandomCropID()
		{
			return this.cropIDs.GetRandom(0f);
		}

		// Token: 0x04001168 RID: 4456
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x04001169 RID: 4457
		public RandomContainer<string> cropIDs;
	}
}
