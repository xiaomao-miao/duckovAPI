using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C6 RID: 710
	public class GMA_026 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600169E RID: 5790 RVA: 0x00052FD0 File Offset: 0x000511D0
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.AddModifier(new Modifier(ModifierType.PercentageMultiply, -1f, this));
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00052FFB File Offset: 0x000511FB
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshPrice.RemoveAllModifiersFromSource(this);
		}
	}
}
