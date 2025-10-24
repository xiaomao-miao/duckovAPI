using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DD RID: 989
	public class ScaleLooper : LooperElement
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x0007D2F4 File Offset: 0x0007B4F4
		protected override void OnTick(LooperClock clock, float t)
		{
			float num = this.xOverT.Evaluate(t);
			float num2 = this.yOverT.Evaluate(t);
			float num3 = this.zOverT.Evaluate(t);
			float num4 = this.uniformScaleOverT.Evaluate(t);
			num *= num4;
			num2 *= num4;
			num3 *= num4;
			base.transform.localScale = new Vector3(num, num2, num3);
		}

		// Token: 0x0400185A RID: 6234
		[SerializeField]
		private AnimationCurve uniformScaleOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400185B RID: 6235
		[SerializeField]
		private AnimationCurve xOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400185C RID: 6236
		[SerializeField]
		private AnimationCurve yOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400185D RID: 6237
		[SerializeField]
		private AnimationCurve zOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	}
}
