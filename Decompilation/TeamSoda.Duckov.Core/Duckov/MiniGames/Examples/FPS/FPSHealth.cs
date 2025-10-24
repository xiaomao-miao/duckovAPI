using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D7 RID: 727
	public class FPSHealth : MiniGameBehaviour
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00053AC5 File Offset: 0x00051CC5
		public int HP
		{
			get
			{
				return this.hp;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x00053ACD File Offset: 0x00051CCD
		public bool Dead
		{
			get
			{
				return this.dead;
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060016E3 RID: 5859 RVA: 0x00053AD8 File Offset: 0x00051CD8
		// (remove) Token: 0x060016E4 RID: 5860 RVA: 0x00053B10 File Offset: 0x00051D10
		public event Action<FPSHealth> onDead;

		// Token: 0x060016E5 RID: 5861 RVA: 0x00053B48 File Offset: 0x00051D48
		protected override void Start()
		{
			base.Start();
			this.hp = this.maxHp;
			this.materialPropertyBlock = new MaterialPropertyBlock();
			foreach (FPSDamageReceiver fpsdamageReceiver in this.damageReceivers)
			{
				fpsdamageReceiver.onReceiveDamage += this.OnReceiverReceiveDamage;
			}
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00053BC4 File Offset: 0x00051DC4
		protected override void OnUpdate(float deltaTime)
		{
			if (this.hurtValue > 0f)
			{
				this.hurtValue = Mathf.MoveTowards(this.hurtValue, 0f, deltaTime * this.hurtValueDropRate);
			}
			this.materialPropertyBlock.SetFloat("_HurtValue", this.hurtValue);
			this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock, 0);
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00053C24 File Offset: 0x00051E24
		private void OnReceiverReceiveDamage(FPSDamageReceiver receiver, FPSDamageInfo info)
		{
			this.ReceiveDamage(info);
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00053C30 File Offset: 0x00051E30
		private void ReceiveDamage(FPSDamageInfo info)
		{
			if (this.dead)
			{
				return;
			}
			this.hurtValue = 1f;
			this.hp -= Mathf.FloorToInt(info.amount);
			if (this.hp <= 0)
			{
				this.hp = 0;
				this.Die();
			}
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00053C7F File Offset: 0x00051E7F
		private void Die()
		{
			this.dead = true;
			Action<FPSHealth> action = this.onDead;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040010BB RID: 4283
		[SerializeField]
		private int maxHp;

		// Token: 0x040010BC RID: 4284
		[SerializeField]
		private List<FPSDamageReceiver> damageReceivers;

		// Token: 0x040010BD RID: 4285
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x040010BE RID: 4286
		[SerializeField]
		private float hurtValueDropRate = 1f;

		// Token: 0x040010BF RID: 4287
		private int hp;

		// Token: 0x040010C0 RID: 4288
		private bool dead;

		// Token: 0x040010C1 RID: 4289
		private float hurtValue;

		// Token: 0x040010C3 RID: 4291
		private MaterialPropertyBlock materialPropertyBlock;
	}
}
