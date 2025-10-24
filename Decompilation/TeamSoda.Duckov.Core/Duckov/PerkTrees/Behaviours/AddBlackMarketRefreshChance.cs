using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000257 RID: 599
	public class AddBlackMarketRefreshChance : PerkBehaviour
	{
		// Token: 0x060012AC RID: 4780 RVA: 0x00046456 File Offset: 0x00044656
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestMaxRefreshChance += this.HandleEvent;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0004646F File Offset: 0x0004466F
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestMaxRefreshChance -= this.HandleEvent;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00046488 File Offset: 0x00044688
		private void HandleEvent(BlackMarket.OnRequestMaxRefreshChanceEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.addAmount);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000464B3 File Offset: 0x000446B3
		protected override void OnUnlocked()
		{
			BlackMarket.NotifyMaxRefreshChanceChanged();
		}

		// Token: 0x04000E1F RID: 3615
		[SerializeField]
		private int addAmount = 1;
	}
}
