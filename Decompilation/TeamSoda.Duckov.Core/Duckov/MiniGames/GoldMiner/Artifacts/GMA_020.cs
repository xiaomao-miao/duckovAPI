using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C0 RID: 704
	public class GMA_020 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600168E RID: 5774 RVA: 0x00052E3B File Offset: 0x0005103B
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.strengthPotion += 3;
		}
	}
}
