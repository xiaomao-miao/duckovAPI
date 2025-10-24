using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000405 RID: 1029
	public class CheckTargetDistance : ConditionTask<AICharacterController>
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x0008092E File Offset: 0x0007EB2E
		protected override string info
		{
			get
			{
				return "is target in range";
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x00080938 File Offset: 0x0007EB38
		protected override bool OnCheck()
		{
			if (this.useTransform && this.targetTransform.value == null)
			{
				return false;
			}
			Vector3 b = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			float num;
			if (this.useShootRange)
			{
				num = base.agent.CharacterMainControl.GetAimRange() * this.shootRangeMultiplier.value;
			}
			else
			{
				num = this.distance.value;
			}
			return Vector3.Distance(base.agent.transform.position, b) <= num;
		}

		// Token: 0x0400195B RID: 6491
		public bool useTransform;

		// Token: 0x0400195C RID: 6492
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x0400195D RID: 6493
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x0400195E RID: 6494
		public bool useShootRange;

		// Token: 0x0400195F RID: 6495
		[ShowIf("useShootRange", 1)]
		public BBParameter<float> shootRangeMultiplier = 1f;

		// Token: 0x04001960 RID: 6496
		[ShowIf("useShootRange", 0)]
		public BBParameter<float> distance;
	}
}
