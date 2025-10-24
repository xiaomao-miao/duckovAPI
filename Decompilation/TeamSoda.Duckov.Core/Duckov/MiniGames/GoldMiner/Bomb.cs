using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200028D RID: 653
	public class Bomb : MiniGameBehaviour
	{
		// Token: 0x06001522 RID: 5410 RVA: 0x0004E2EC File Offset: 0x0004C4EC
		protected override void OnUpdate(float deltaTime)
		{
			base.transform.position += base.transform.up * this.moveSpeed * deltaTime;
			this.hoveringTargets.RemoveAll((GoldMinerEntity e) => e == null);
			if (this.hoveringTargets.Count > 0)
			{
				this.Explode(this.hoveringTargets[0]);
			}
			this.lifeTime += deltaTime;
			if (this.lifeTime > this.maxLifeTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0004E39D File Offset: 0x0004C59D
		private void Explode(GoldMinerEntity goldMinerTarget)
		{
			goldMinerTarget.Explode(base.transform.position);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0004E3E0 File Offset: 0x0004C5E0
		private void OnCollisionEnter2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Add(component);
			}
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0004E410 File Offset: 0x0004C610
		private void OnCollisionExit2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Remove(component);
			}
		}

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private float moveSpeed;

		// Token: 0x04000F7B RID: 3963
		[SerializeField]
		private float maxLifeTime = 10f;

		// Token: 0x04000F7C RID: 3964
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x04000F7D RID: 3965
		private float lifeTime;

		// Token: 0x04000F7E RID: 3966
		private List<GoldMinerEntity> hoveringTargets = new List<GoldMinerEntity>();
	}
}
