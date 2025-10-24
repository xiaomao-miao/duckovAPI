using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C4 RID: 708
	public class GMA_024 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001698 RID: 5784 RVA: 0x00052F2B File Offset: 0x0005112B
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.AddModifier(new Modifier(ModifierType.Add, 1.5f, this));
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00052F52 File Offset: 0x00051152
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.maxStamina.RemoveAllModifiersFromSource(this);
		}
	}
}
