using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D4 RID: 980
	public class ScaleFade : FadeElement
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060023A3 RID: 9123 RVA: 0x0007C7BF File Offset: 0x0007A9BF
		private Vector3 HiddenScale
		{
			get
			{
				return Vector3.one + Vector3.one * this.uniformScale + this.scale;
			}
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0007C7E6 File Offset: 0x0007A9E6
		private void CachePose()
		{
			this.cachedScale = base.transform.localScale;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0007C7F9 File Offset: 0x0007A9F9
		private void RestorePose()
		{
			base.transform.localScale = this.cachedScale;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0007C80C File Offset: 0x0007AA0C
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.CachePose();
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0007C824 File Offset: 0x0007AA24
		protected override UniTask HideTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!base.transform)
			{
				return UniTask.CompletedTask;
			}
			return base.transform.DOScale(this.HiddenScale, this.duration).SetEase(this.hideCurve).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0007C883 File Offset: 0x0007AA83
		protected override void OnSkipHide()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			base.transform.localScale = this.HiddenScale;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0007C8A4 File Offset: 0x0007AAA4
		protected override void OnSkipShow()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.RestorePose();
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0007C8BC File Offset: 0x0007AABC
		protected override UniTask ShowTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return base.transform.DOScale(this.cachedScale, this.duration).SetEase(this.showCurve).OnComplete(new TweenCallback(this.RestorePose)).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x0400182D RID: 6189
		[SerializeField]
		private float duration = 0.1f;

		// Token: 0x0400182E RID: 6190
		[SerializeField]
		private Vector3 scale = Vector3.zero;

		// Token: 0x0400182F RID: 6191
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x04001830 RID: 6192
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001831 RID: 6193
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001832 RID: 6194
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x04001833 RID: 6195
		private bool initialized;
	}
}
