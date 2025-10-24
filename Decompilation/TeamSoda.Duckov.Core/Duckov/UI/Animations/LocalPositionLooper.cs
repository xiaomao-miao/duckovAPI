using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D9 RID: 985
	public class LocalPositionLooper : LooperElement
	{
		// Token: 0x060023DC RID: 9180 RVA: 0x0007D0EC File Offset: 0x0007B2EC
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector2 v = Vector2.Lerp(this.localPositionA, this.localPositionB, this.curve.Evaluate(t));
			base.transform.localPosition = v;
		}

		// Token: 0x04001850 RID: 6224
		[SerializeField]
		private Vector3 localPositionA;

		// Token: 0x04001851 RID: 6225
		[SerializeField]
		private Vector3 localPositionB;

		// Token: 0x04001852 RID: 6226
		[SerializeField]
		private AnimationCurve curve;
	}
}
