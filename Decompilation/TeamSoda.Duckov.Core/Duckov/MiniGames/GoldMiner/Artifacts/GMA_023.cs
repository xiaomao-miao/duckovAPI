using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C3 RID: 707
	public class GMA_023 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001695 RID: 5781 RVA: 0x00052EDF File Offset: 0x000510DF
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.AddModifier(new Modifier(ModifierType.Add, 10f, this));
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00052F06 File Offset: 0x00051106
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strength.RemoveAllModifiersFromSource(this);
		}
	}
}
