using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C5 RID: 709
	public class GMA_025 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x00052F77 File Offset: 0x00051177
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.AddModifier(new Modifier(ModifierType.PercentageAdd, this.addAmount, this));
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00052FA0 File Offset: 0x000511A0
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.emptySpeed.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x0400108F RID: 4239
		[SerializeField]
		private float addAmount = 1f;
	}
}
