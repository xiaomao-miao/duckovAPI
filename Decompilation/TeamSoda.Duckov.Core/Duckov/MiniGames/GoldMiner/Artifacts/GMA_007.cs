using System;
using System.Linq;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B3 RID: 691
	public class GMA_007 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001661 RID: 5729 RVA: 0x0005273A File Offset: 0x0005093A
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Add(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00052761 File Offset: 0x00050961
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.additionalFactorFuncs.Remove(new Func<float>(this.AddFactorIfResolved3DifferentKindsOfGold));
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0005278C File Offset: 0x0005098C
		private float AddFactorIfResolved3DifferentKindsOfGold()
		{
			if ((from e in base.GoldMiner.resolvedEntities
			where e != null && e.tags.Contains(GoldMinerEntity.Tag.Gold)
			group e by e.size).Count<IGrouping<GoldMinerEntity.Size, GoldMinerEntity>>() >= 3)
			{
				return 0.5f;
			}
			return 0f;
		}
	}
}
