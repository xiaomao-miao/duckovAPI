using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D8 RID: 984
	public class ImageColorLooper : LooperElement
	{
		// Token: 0x060023DA RID: 9178 RVA: 0x0007D0A4 File Offset: 0x0007B2A4
		protected override void OnTick(LooperClock clock, float t)
		{
			Color color = this.colorOverT.Evaluate(t);
			float num = this.alphaOverT.Evaluate(t);
			color.a *= num;
			this.image.color = color;
		}

		// Token: 0x0400184D RID: 6221
		[SerializeField]
		private Image image;

		// Token: 0x0400184E RID: 6222
		[GradientUsage(true)]
		[SerializeField]
		private Gradient colorOverT;

		// Token: 0x0400184F RID: 6223
		[SerializeField]
		private AnimationCurve alphaOverT;
	}
}
