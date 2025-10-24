using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E2 RID: 994
	public class SizeDeltaToggle : ToggleAnimation
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0007D71E File Offset: 0x0007B91E
		private RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0007D740 File Offset: 0x0007B940
		private void CachePose()
		{
			this.cachedSizeDelta = this.RectTransform.sizeDelta;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0007D753 File Offset: 0x0007B953
		private void Awake()
		{
			this.CachePose();
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0007D75C File Offset: 0x0007B95C
		protected override void OnSetToggle(bool status)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 endValue = status ? this.activeSizeDelta : this.idleSizeDelta;
			this.RectTransform.DOKill(false);
			this.RectTransform.DOSizeDelta(endValue, this.duration, false).SetEase(this.animationCurve);
		}

		// Token: 0x0400186E RID: 6254
		public Vector2 idleSizeDelta = Vector2.zero;

		// Token: 0x0400186F RID: 6255
		public Vector2 activeSizeDelta = Vector2.one * 12f;

		// Token: 0x04001870 RID: 6256
		public float duration = 0.1f;

		// Token: 0x04001871 RID: 6257
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001872 RID: 6258
		private Vector2 cachedSizeDelta = Vector3.one;

		// Token: 0x04001873 RID: 6259
		private RectTransform _rectTransform;
	}
}
