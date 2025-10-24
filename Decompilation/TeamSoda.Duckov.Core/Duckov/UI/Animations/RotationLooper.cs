using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DC RID: 988
	public class RotationLooper : LooperElement
	{
		// Token: 0x060023E8 RID: 9192 RVA: 0x0007D2A0 File Offset: 0x0007B4A0
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector3 euler = Vector3.Lerp(this.eulerRotationA, this.eulerRotationB, this.curve.Evaluate(t));
			base.transform.localRotation = Quaternion.Euler(euler);
		}

		// Token: 0x04001857 RID: 6231
		[SerializeField]
		private Vector3 eulerRotationA;

		// Token: 0x04001858 RID: 6232
		[SerializeField]
		private Vector3 eulerRotationB;

		// Token: 0x04001859 RID: 6233
		[SerializeField]
		private AnimationCurve curve;
	}
}
