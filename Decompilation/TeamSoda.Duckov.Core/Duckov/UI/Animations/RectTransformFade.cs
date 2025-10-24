using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D3 RID: 979
	public class RectTransformFade : FadeElement
	{
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0007C410 File Offset: 0x0007A610
		private Vector2 TargetAnchoredPosition
		{
			get
			{
				return this.cachedAnchordPosition + this.offset;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0007C423 File Offset: 0x0007A623
		private Vector3 TargetScale
		{
			get
			{
				return this.cachedScale + Vector3.one * this.uniformScale;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x0007C440 File Offset: 0x0007A640
		private Vector3 TargetRotation
		{
			get
			{
				return this.cachedRotation + Vector3.forward * this.rotateZ;
			}
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0007C45D File Offset: 0x0007A65D
		private void Initialize()
		{
			if (this.initialized)
			{
				Debug.LogError("Object Initialized Twice, aborting");
				return;
			}
			this.CachePose();
			this.initialized = true;
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0007C480 File Offset: 0x0007A680
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchordPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0007C4D8 File Offset: 0x0007A6D8
		private void Awake()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0007C525 File Offset: 0x0007A725
		private void OnValidate()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0007C55C File Offset: 0x0007A75C
		protected override UniTask HideTask(int token)
		{
			RectTransformFade.<HideTask>d__22 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<RectTransformFade.<HideTask>d__22>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0007C5A0 File Offset: 0x0007A7A0
		protected override UniTask ShowTask(int token)
		{
			RectTransformFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<RectTransformFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0007C5E4 File Offset: 0x0007A7E4
		protected override void OnSkipHide()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipHide");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.TargetAnchoredPosition;
			this.rectTransform.localScale = this.TargetScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.TargetRotation);
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0007C649 File Offset: 0x0007A849
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.DOKill(false);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0007C660 File Offset: 0x0007A860
		protected override void OnSkipShow()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipShow");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.cachedAnchordPosition;
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x04001821 RID: 6177
		[SerializeField]
		private bool debug;

		// Token: 0x04001822 RID: 6178
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001823 RID: 6179
		[SerializeField]
		private float duration = 0.4f;

		// Token: 0x04001824 RID: 6180
		[SerializeField]
		private Vector2 offset = Vector2.left * 10f;

		// Token: 0x04001825 RID: 6181
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x04001826 RID: 6182
		[SerializeField]
		[Range(-180f, 180f)]
		private float rotateZ;

		// Token: 0x04001827 RID: 6183
		[SerializeField]
		private AnimationCurve showingAnimationCurve;

		// Token: 0x04001828 RID: 6184
		[SerializeField]
		private AnimationCurve hidingAnimationCurve;

		// Token: 0x04001829 RID: 6185
		private Vector2 cachedAnchordPosition = Vector2.zero;

		// Token: 0x0400182A RID: 6186
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x0400182B RID: 6187
		private Vector3 cachedRotation = Vector3.zero;

		// Token: 0x0400182C RID: 6188
		private bool initialized;
	}
}
