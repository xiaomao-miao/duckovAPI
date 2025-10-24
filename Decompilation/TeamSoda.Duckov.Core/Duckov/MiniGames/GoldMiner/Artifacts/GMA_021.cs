using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C1 RID: 705
	public class GMA_021 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001690 RID: 5776 RVA: 0x00052E61 File Offset: 0x00051061
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.eagleEyePotion += 3;
		}
	}
}
