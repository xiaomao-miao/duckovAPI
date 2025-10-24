using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000296 RID: 662
	public class AddBombOnRetrieve : MiniGameBehaviour
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x00050860 File Offset: 0x0004EA60
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnResolved = (Action<GoldMinerEntity, GoldMiner>)Delegate.Combine(goldMinerEntity.OnResolved, new Action<GoldMinerEntity, GoldMiner>(this.OnResolved));
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x000508AE File Offset: 0x0004EAAE
		private void OnResolved(GoldMinerEntity entity, GoldMiner game)
		{
			game.run.bomb += this.amount;
		}

		// Token: 0x04001010 RID: 4112
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001011 RID: 4113
		[SerializeField]
		private int amount = 1;
	}
}
