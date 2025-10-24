using System;
using Duckov.BlackMarkets;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000258 RID: 600
	public class ChangeBlackMarketRefreshTimeFactor : PerkBehaviour
	{
		// Token: 0x060012B1 RID: 4785 RVA: 0x000464C9 File Offset: 0x000446C9
		protected override void OnAwake()
		{
			base.OnAwake();
			BlackMarket.onRequestRefreshTime += this.HandleEvent;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000464E2 File Offset: 0x000446E2
		protected override void OnOnDestroy()
		{
			base.OnOnDestroy();
			BlackMarket.onRequestRefreshTime -= this.HandleEvent;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000464FB File Offset: 0x000446FB
		private void HandleEvent(BlackMarket.OnRequestRefreshTimeFactorEventContext context)
		{
			if (base.Master == null)
			{
				return;
			}
			if (!base.Master.Unlocked)
			{
				return;
			}
			context.Add(this.amount);
		}

		// Token: 0x04000E20 RID: 3616
		[SerializeField]
		private float amount = -0.1f;
	}
}
