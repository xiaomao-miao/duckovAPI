using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BD RID: 701
	public class GMA_017 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x00052CB1 File Offset: 0x00050EB1
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.AddModifier(new Modifier(ModifierType.Add, 0.2f, this));
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00052CD8 File Offset: 0x00050ED8
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.goldValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
