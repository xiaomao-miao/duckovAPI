using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002AE RID: 686
	public class GMA_002 : GoldMinerArtifactBehaviour
	{
		// Token: 0x0600164B RID: 5707 RVA: 0x000522A0 File Offset: 0x000504A0
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (this.master == null)
			{
				return;
			}
			if (base.GoldMiner == null)
			{
				return;
			}
			this.modifer = new Modifier(ModifierType.PercentageMultiply, -0.5f, this);
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Combine(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00052358 File Offset: 0x00050558
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.GoldMiner == null)
			{
				return;
			}
			GoldMiner goldMiner = base.GoldMiner;
			goldMiner.onResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Remove(goldMiner.onResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnResolveEntity));
			GoldMiner goldMiner2 = base.GoldMiner;
			goldMiner2.onHookBeginRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner2.onHookBeginRetrieve, new Action<GoldMiner, Hook>(this.OnBeginRetrieve));
			GoldMiner goldMiner3 = base.GoldMiner;
			goldMiner3.onHookEndRetrieve = (Action<GoldMiner, Hook>)Delegate.Remove(goldMiner3.onHookEndRetrieve, new Action<GoldMiner, Hook>(this.OnEndRetrieve));
			if (base.Run != null)
			{
				base.Run.staminaDrain.RemoveModifier(this.modifer);
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00052408 File Offset: 0x00050608
		private void OnBeginRetrieve(GoldMiner miner, Hook hook)
		{
			if (!this.effectActive)
			{
				return;
			}
			base.Run.staminaDrain.AddModifier(this.modifer);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00052429 File Offset: 0x00050629
		private void OnEndRetrieve(GoldMiner miner, Hook hook)
		{
			base.Run.staminaDrain.RemoveModifier(this.modifer);
			this.effectActive = false;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00052449 File Offset: 0x00050649
		private void OnResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.effectActive = true;
		}

		// Token: 0x04001086 RID: 4230
		private Modifier modifer;

		// Token: 0x04001087 RID: 4231
		private bool effectActive;
	}
}
