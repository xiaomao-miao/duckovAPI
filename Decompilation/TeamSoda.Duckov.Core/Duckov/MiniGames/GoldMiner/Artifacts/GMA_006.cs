using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B2 RID: 690
	public class GMA_006 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600165D RID: 5725 RVA: 0x000526C7 File Offset: 0x000508C7
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Add(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000526EE File Offset: 0x000508EE
		private bool SmallRockIsGold(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock) && entity.size < GoldMinerEntity.Size.M;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0005270A File Offset: 0x0005090A
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.isGoldPredicators.Remove(new Func<GoldMinerEntity, bool>(this.SmallRockIsGold));
		}
	}
}
