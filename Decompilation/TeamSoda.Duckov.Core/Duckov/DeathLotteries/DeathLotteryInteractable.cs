using System;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000303 RID: 771
	public class DeathLotteryInteractable : InteractableBase
	{
		// Token: 0x0600190E RID: 6414 RVA: 0x0005B12F File Offset: 0x0005932F
		protected override bool IsInteractable()
		{
			return !(this.deathLottery == null) && this.deathLottery.CurrentStatus.valid && !this.deathLottery.Loading;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0005B165 File Offset: 0x00059365
		protected override void OnInteractFinished()
		{
			this.deathLottery.RequestUI();
		}

		// Token: 0x0400122C RID: 4652
		[SerializeField]
		private DeathLottery deathLottery;
	}
}
