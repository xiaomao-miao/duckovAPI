using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000400 RID: 1024
	public class CheckHeathLessThan : ConditionTask<AICharacterController>
	{
		// Token: 0x06002531 RID: 9521 RVA: 0x00080755 File Offset: 0x0007E955
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x00080758 File Offset: 0x0007E958
		protected override void OnEnable()
		{
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0008075A File Offset: 0x0007E95A
		protected override void OnDisable()
		{
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0008075C File Offset: 0x0007E95C
		protected override bool OnCheck()
		{
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				return false;
			}
			this.checkTimeMarker = Time.time;
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				return false;
			}
			Health health = base.agent.CharacterMainControl.Health;
			return health && health.CurrentHealth / health.MaxHealth <= this.percent;
		}

		// Token: 0x04001953 RID: 6483
		public float percent;

		// Token: 0x04001954 RID: 6484
		private float checkTimeMarker = -1f;

		// Token: 0x04001955 RID: 6485
		public float checkTimeSpace = 1.5f;
	}
}
