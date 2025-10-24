using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000402 RID: 1026
	public class CheckHurt : ConditionTask<AICharacterController>
	{
		// Token: 0x0600253B RID: 9531 RVA: 0x00080824 File Offset: 0x0007EA24
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x00080827 File Offset: 0x0007EA27
		protected override void OnEnable()
		{
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x00080829 File Offset: 0x0007EA29
		protected override void OnDisable()
		{
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x0008082C File Offset: 0x0007EA2C
		protected override bool OnCheck()
		{
			if (base.agent == null || this.cacheFromCharacterDmgReceiver == null)
			{
				return false;
			}
			bool result = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(this.hurtTimeThreshold, this.damageThreshold, ref damageInfo))
			{
				this.cacheFromCharacterDmgReceiver.value = damageInfo.fromCharacter.mainDamageReceiver;
				result = true;
			}
			return result;
		}

		// Token: 0x04001956 RID: 6486
		public float hurtTimeThreshold = 0.2f;

		// Token: 0x04001957 RID: 6487
		public int damageThreshold = 3;

		// Token: 0x04001958 RID: 6488
		public BBParameter<DamageReceiver> cacheFromCharacterDmgReceiver;
	}
}
