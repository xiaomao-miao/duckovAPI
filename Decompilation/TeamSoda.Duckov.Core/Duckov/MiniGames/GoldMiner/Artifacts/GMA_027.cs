using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C7 RID: 711
	public class GMA_027 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A1 RID: 5793 RVA: 0x00053020 File Offset: 0x00051220
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00053047 File Offset: 0x00051247
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopRefreshChances.RemoveAllModifiersFromSource(this);
		}
	}
}
