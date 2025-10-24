using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002BE RID: 702
	public class GMA_018 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x00052D00 File Offset: 0x00050F00
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00052D6C File Offset: 0x00050F6C
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00052DD6 File Offset: 0x00050FD6
		private void OnLevelBegin(GoldMiner miner)
		{
			this.remaining = 5;
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00052DDF File Offset: 0x00050FDF
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (!entity)
			{
				return;
			}
			if (this.remaining < 1)
			{
				return;
			}
			this.remaining--;
			entity.Value = 200;
		}

		// Token: 0x0400108D RID: 4237
		private int remaining;
	}
}
