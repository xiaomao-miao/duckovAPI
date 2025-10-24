using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000299 RID: 665
	public class ExplodeOnAttach : MiniGameBehaviour
	{
		// Token: 0x060015CA RID: 5578 RVA: 0x000509C8 File Offset: 0x0004EBC8
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			if (this.goldMiner == null)
			{
				this.goldMiner = base.GetComponentInParent<GoldMiner>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00050A30 File Offset: 0x0004EC30
		private void OnAttached(GoldMinerEntity target, Hook hook)
		{
			if (this.goldMiner == null)
			{
				return;
			}
			if (this.goldMiner.run == null)
			{
				return;
			}
			if (this.goldMiner.run.defuse.Value > 0.1f)
			{
				return;
			}
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, this.explodeRange);
			for (int i = 0; i < array.Length; i++)
			{
				GoldMinerEntity component = array[i].GetComponent<GoldMinerEntity>();
				if (!(component == null))
				{
					component.Explode(base.transform.position);
				}
			}
			this.master.Explode(base.transform.position);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00050ADA File Offset: 0x0004ECDA
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.explodeRange);
		}

		// Token: 0x04001016 RID: 4118
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x04001017 RID: 4119
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x04001018 RID: 4120
		[SerializeField]
		private float explodeRange;
	}
}
