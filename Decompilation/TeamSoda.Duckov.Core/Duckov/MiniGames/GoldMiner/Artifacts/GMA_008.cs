using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B4 RID: 692
	public class GMA_008 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001665 RID: 5733 RVA: 0x00052808 File Offset: 0x00050A08
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00052874 File Offset: 0x00050A74
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Remove(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000528DE File Offset: 0x00050ADE
		private void OnLevelBegin(GoldMiner miner)
		{
			this.triggered = false;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000528E8 File Offset: 0x00050AE8
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			if (this.triggered)
			{
				return;
			}
			if (base.GoldMiner.activeEntities.Count <= 0)
			{
				this.triggered = true;
				base.Run.charm.BaseValue += 0.5f;
			}
		}

		// Token: 0x0400108A RID: 4234
		private bool triggered;
	}
}
