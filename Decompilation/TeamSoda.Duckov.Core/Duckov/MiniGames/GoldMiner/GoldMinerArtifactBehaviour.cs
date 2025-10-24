using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200028C RID: 652
	public abstract class GoldMinerArtifactBehaviour : MiniGameBehaviour
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0004E212 File Offset: 0x0004C412
		protected GoldMinerRunData Run
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				if (this.master.Master == null)
				{
					return null;
				}
				return this.master.Master.run;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0004E249 File Offset: 0x0004C449
		protected GoldMiner GoldMiner
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Master;
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0004E268 File Offset: 0x0004C468
		private void Awake()
		{
			if (!this.master)
			{
				this.master = base.GetComponent<GoldMinerArtifact>();
			}
			GoldMinerArtifact goldMinerArtifact = this.master;
			goldMinerArtifact.OnAttached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact.OnAttached, new Action<GoldMinerArtifact>(this.OnAttached));
			GoldMinerArtifact goldMinerArtifact2 = this.master;
			goldMinerArtifact2.OnDetached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact2.OnDetached, new Action<GoldMinerArtifact>(this.OnDetached));
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0004E2DE File Offset: 0x0004C4DE
		protected virtual void OnAttached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0004E2E0 File Offset: 0x0004C4E0
		protected virtual void OnDetached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		protected GoldMinerArtifact master;
	}
}
