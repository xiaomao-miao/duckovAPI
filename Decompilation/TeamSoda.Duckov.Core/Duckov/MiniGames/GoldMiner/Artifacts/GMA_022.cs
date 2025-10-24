using System;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002C2 RID: 706
	public class GMA_022 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001692 RID: 5778 RVA: 0x00052E87 File Offset: 0x00051087
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.AddModifier(new Modifier(ModifierType.Add, this.amount, this));
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00052EAF File Offset: 0x000510AF
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.charm.RemoveAllModifiersFromSource(this);
		}

		// Token: 0x0400108E RID: 4238
		[SerializeField]
		private float amount = 0.1f;
	}
}
