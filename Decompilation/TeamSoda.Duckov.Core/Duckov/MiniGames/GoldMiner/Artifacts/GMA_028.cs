using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C8 RID: 712
	public class GMA_028 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A4 RID: 5796 RVA: 0x0005306C File Offset: 0x0005126C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.shopTicket++;
		}
	}
}
