using System;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000292 RID: 658
	[Serializable]
	public class ShopEntity
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x0004FC28 File Offset: 0x0004DE28
		public string ID
		{
			get
			{
				if (!this.artifact)
				{
					return null;
				}
				return this.artifact.ID;
			}
		}

		// Token: 0x04000FE6 RID: 4070
		public GoldMinerArtifact artifact;

		// Token: 0x04000FE7 RID: 4071
		public bool locked;

		// Token: 0x04000FE8 RID: 4072
		public bool sold;

		// Token: 0x04000FE9 RID: 4073
		public float priceFactor = 1f;
	}
}
