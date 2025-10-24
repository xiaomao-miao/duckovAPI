using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D7 RID: 983
	public class AnchoredPositionLooper : LooperElement
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x0007D041 File Offset: 0x0007B241
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0007D054 File Offset: 0x0007B254
		protected override void OnTick(LooperClock clock, float t)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 anchoredPosition = Vector2.Lerp(this.anchoredPositionA, this.anchoredPositionB, this.curve.Evaluate(t));
			this.rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04001849 RID: 6217
		[SerializeField]
		private Vector2 anchoredPositionA;

		// Token: 0x0400184A RID: 6218
		[SerializeField]
		private Vector2 anchoredPositionB;

		// Token: 0x0400184B RID: 6219
		[SerializeField]
		private AnimationCurve curve;

		// Token: 0x0400184C RID: 6220
		private RectTransform rectTransform;
	}
}
