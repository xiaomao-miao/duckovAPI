using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BF RID: 703
	public class GMA_019 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600168C RID: 5772 RVA: 0x00052E15 File Offset: 0x00051015
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.bomb += 3;
		}
	}
}
