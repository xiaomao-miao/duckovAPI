using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B8 RID: 696
	public class GMA_012 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001675 RID: 5749 RVA: 0x00052AB1 File Offset: 0x00050CB1
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.AddModifier(new Modifier(ModifierType.Add, 1f, this));
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00052AD8 File Offset: 0x00050CD8
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.defuse.RemoveAllModifiersFromSource(this);
		}
	}
}
