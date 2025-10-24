using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C9 RID: 713
	public class GMA_029 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A6 RID: 5798 RVA: 0x00053092 File Offset: 0x00051292
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			if (base.Run.shopCapacity >= 6)
			{
				return;
			}
			base.Run.shopCapacity++;
		}
	}
}
