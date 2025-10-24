using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BB RID: 699
	public class GMA_015 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600167D RID: 5757 RVA: 0x00052BB8 File Offset: 0x00050DB8
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00052BF0 File Offset: 0x00050DF0
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00052C28 File Offset: 0x00050E28
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (base.Run == null)
			{
				return;
			}
			if (!base.Run.IsPig(entity))
			{
				return;
			}
			entity.Value += this.amount;
		}

		// Token: 0x0400108C RID: 4236
		[SerializeField]
		private int amount = 20;
	}
}
