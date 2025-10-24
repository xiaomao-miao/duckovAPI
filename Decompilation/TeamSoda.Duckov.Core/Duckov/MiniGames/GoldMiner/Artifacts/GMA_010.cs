using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B6 RID: 694
	public class GMA_010 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600166E RID: 5742 RVA: 0x00052A0B File Offset: 0x00050C0B
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 1000;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00052A26 File Offset: 0x00050C26
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.minMoneySum = 0;
		}
	}
}
