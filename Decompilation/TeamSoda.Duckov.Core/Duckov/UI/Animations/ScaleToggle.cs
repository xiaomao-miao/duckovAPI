using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E1 RID: 993
	public class ScaleToggle : ToggleAnimation
	{
		// Token: 0x060023FA RID: 9210 RVA: 0x0007D62E File Offset: 0x0007B82E
		private void CachePose()
		{
			this.cachedScale = this.rectTransform.localScale;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0007D641 File Offset: 0x0007B841
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.CachePose();
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0007D65C File Offset: 0x0007B85C
		protected override void OnSetToggle(bool status)
		{
			float d = status ? this.activeScale : this.idleScale;
			d * this.cachedScale;
			this.rectTransform.DOKill(false);
			this.rectTransform.DOScale(this.cachedScale * d, this.duration).SetEase(this.animationCurve);
		}

		// Token: 0x04001868 RID: 6248
		public float idleScale = 1f;

		// Token: 0x04001869 RID: 6249
		public float activeScale = 0.9f;

		// Token: 0x0400186A RID: 6250
		public float duration = 0.1f;

		// Token: 0x0400186B RID: 6251
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400186C RID: 6252
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x0400186D RID: 6253
		private RectTransform rectTransform;
	}
}
