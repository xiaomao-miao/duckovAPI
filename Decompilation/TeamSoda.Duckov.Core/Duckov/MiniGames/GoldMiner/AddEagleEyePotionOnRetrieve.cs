using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000297 RID: 663
	public class AddEagleEyePotionOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x060015C4 RID: 5572 RVA: 0x000508D8 File Offset: 0x0004EAD8
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00050926 File Offset: 0x0004EB26
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.eagleEyePotion += this.amount;
		}

		// Token: 0x04001012 RID: 4114
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001013 RID: 4115
		[SerializeField]
		private int amount = 1;
	}
}
