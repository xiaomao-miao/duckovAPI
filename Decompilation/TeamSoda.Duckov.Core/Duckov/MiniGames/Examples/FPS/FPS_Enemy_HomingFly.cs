using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D1 RID: 721
	public class FPS_Enemy_HomingFly : MiniGameBehaviour
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x0005340E File Offset: 0x0005160E
		private bool CanSeeTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00053411 File Offset: 0x00051611
		private bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0005341E File Offset: 0x0005161E
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
			this.health.onDead += this.OnDead;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00053451 File Offset: 0x00051651
		private void OnDead(FPSHealth health)
		{
			this.rigidbody.useGravity = true;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x0005345F File Offset: 0x0005165F
		protected override void OnUpdate(float deltaTime)
		{
			if (this.Dead)
			{
				this.UpdateDead(deltaTime);
				return;
			}
			if (this.CanSeeTarget)
			{
				this.UpdateHoming(deltaTime);
				return;
			}
			this.UpdateIdle(deltaTime);
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00053488 File Offset: 0x00051688
		private void UpdateIdle(float deltaTime)
		{
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x0005348A File Offset: 0x0005168A
		private void UpdateDead(float deltaTime)
		{
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x0005348C File Offset: 0x0005168C
		private void UpdateHoming(float deltaTime)
		{
		}

		// Token: 0x0400109A RID: 4250
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x0400109B RID: 4251
		[SerializeField]
		private FPSHealth health;
	}
}
