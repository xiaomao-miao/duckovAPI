using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CA RID: 714
	public class GMA_030 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x000530C7 File Offset: 0x000512C7
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraRocks = Mathf.MoveTowards(base.Run.extraRocks, 5f, 1f);
		}
	}
}
