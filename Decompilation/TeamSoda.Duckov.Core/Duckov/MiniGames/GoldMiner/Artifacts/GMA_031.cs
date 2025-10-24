using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CB RID: 715
	public class GMA_031 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016AA RID: 5802 RVA: 0x000530FF File Offset: 0x000512FF
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraGold = Mathf.MoveTowards(base.Run.extraGold, 5f, 1f);
		}
	}
}
