using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000298 RID: 664
	public class AddStrengthPotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x060015C7 RID: 5575 RVA: 0x00050950 File Offset: 0x0004EB50
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0005099E File Offset: 0x0004EB9E
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.strengthPotion += this.amount;
		}

		// Token: 0x04001014 RID: 4116
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001015 RID: 4117
		[SerializeField]
		private int amount = 1;
	}
}
