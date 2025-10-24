using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B5 RID: 693
	public class GMA_009 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600166A RID: 5738 RVA: 0x0005293C File Offset: 0x00050B3C
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00052974 File Offset: 0x00050B74
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000529AC File Offset: 0x00050BAC
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity))
			{
				this.effectActive = true;
			}
			if (this.effectActive && base.Run.IsGold(entity))
			{
				this.effectActive = false;
				entity.Value *= 2;
			}
		}

		// Token: 0x0400108B RID: 4235
		private bool effectActive;
	}
}
