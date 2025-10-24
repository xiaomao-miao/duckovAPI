using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029A RID: 666
	public class PigBehaviour : MiniGameBehaviour
	{
		// Token: 0x060015CE RID: 5582 RVA: 0x00050B04 File Offset: 0x0004ED04
		private void Awake()
		{
			if (this.entity == null)
			{
				this.entity = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.entity;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00050B54 File Offset: 0x0004ED54
		protected override void OnUpdate(float deltaTime)
		{
			Quaternion localRotation = Quaternion.AngleAxis((float)(this.movingRight ? 0 : 180), Vector3.up);
			base.transform.localRotation = localRotation;
			base.transform.localPosition += (this.movingRight ? Vector3.right : Vector3.left) * this.moveSpeed * this.entity.master.run.GameSpeedFactor * deltaTime;
			if (base.transform.localPosition.x > this.entity.master.Bounds.max.x)
			{
				this.movingRight = false;
				return;
			}
			if (base.transform.localPosition.x < this.entity.master.Bounds.min.x)
			{
				this.movingRight = true;
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00050C4B File Offset: 0x0004EE4B
		private void OnAttached(GoldMinerEntity entity, Hook hook)
		{
		}

		// Token: 0x04001019 RID: 4121
		[SerializeField]
		private GoldMinerEntity entity;

		// Token: 0x0400101A RID: 4122
		[SerializeField]
		private float moveSpeed = 50f;

		// Token: 0x0400101B RID: 4123
		private bool attached;

		// Token: 0x0400101C RID: 4124
		private bool movingRight;
	}
}
