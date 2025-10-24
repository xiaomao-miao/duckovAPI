using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B1 RID: 689
	public class GMA_005 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001659 RID: 5721 RVA: 0x000525ED File Offset: 0x000507ED
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00052625 File Offset: 0x00050825
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00052660 File Offset: 0x00050860
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.remaining < 1)
			{
				return;
			}
			if (entity == null)
			{
				return;
			}
			if (base.Run.IsRock(entity) && entity.size < GoldMinerEntity.Size.M)
			{
				entity.Value += 500;
				this.remaining--;
			}
		}

		// Token: 0x04001089 RID: 4233
		private int remaining = 3;
	}
}
