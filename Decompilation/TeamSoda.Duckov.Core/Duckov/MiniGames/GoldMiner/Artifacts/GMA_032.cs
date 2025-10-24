using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002CC RID: 716
	public class GMA_032 : GoldMinerArtifactBehaviour
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x00053137 File Offset: 0x00051337
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.extraDiamond = Mathf.MoveTowards(base.Run.extraDiamond, 5f, 0.5f);
		}
	}
}
