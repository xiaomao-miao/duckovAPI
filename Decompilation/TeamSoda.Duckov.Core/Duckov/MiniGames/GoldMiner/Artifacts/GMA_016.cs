using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BC RID: 700
	public class GMA_016 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x00052C65 File Offset: 0x00050E65
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00052C8C File Offset: 0x00050E8C
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.rockValueFactor.RemoveAllModifiersFromSource(this);
		}
	}
}
