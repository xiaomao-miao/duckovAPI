using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002AF RID: 687
	public class GMA_003 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001651 RID: 5713 RVA: 0x0005245A File Offset: 0x0005065A
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00052492 File Offset: 0x00050692
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000524CC File Offset: 0x000506CC
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				Debug.Log("Enity is Rock ", entity);
				this.streak++;
			}
			else
			{
				this.streak = 0;
			}
			if (this.streak > 1)
			{
				base.Run.levelScoreFactor += 0.1f;
			}
		}

		// Token: 0x04001088 RID: 4232
		private int streak;
	}
}
