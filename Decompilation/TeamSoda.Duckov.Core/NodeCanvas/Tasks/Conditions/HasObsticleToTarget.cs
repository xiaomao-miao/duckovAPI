using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000406 RID: 1030
	public class HasObsticleToTarget : ConditionTask<AICharacterController>
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x000809F5 File Offset: 0x0007EBF5
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000809F8 File Offset: 0x0007EBF8
		protected override void OnEnable()
		{
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000809FA File Offset: 0x0007EBFA
		protected override void OnDisable()
		{
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000809FC File Offset: 0x0007EBFC
		protected override bool OnCheck()
		{
			return base.agent.hasObsticleToTarget;
		}

		// Token: 0x04001961 RID: 6497
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x04001962 RID: 6498
		public int damageThreshold = 3;

		// Token: 0x04001963 RID: 6499
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
