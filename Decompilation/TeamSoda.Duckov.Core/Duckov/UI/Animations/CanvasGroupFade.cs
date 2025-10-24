using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D1 RID: 977
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupFade : FadeElement
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x0007BF48 File Offset: 0x0007A148
		private float ShowingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x0007BF50 File Offset: 0x0007A150
		private float HidingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0007BF58 File Offset: 0x0007A158
		private void Awake()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			this.awaked = true;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0007BF93 File Offset: 0x0007A193
		private void OnValidate()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0007BFC8 File Offset: 0x0007A1C8
		protected override UniTask ShowTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (!this.awaked)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 0f, 1f, this.showingCurve, this.ShowingDuration);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0007C04C File Offset: 0x0007A24C
		protected override UniTask HideTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 1f, 0f, this.hidingCurve, this.HidingDuration);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0007C0B8 File Offset: 0x0007A2B8
		private UniTask FadeTask(int token, float beginAlpha, float targetAlpha, AnimationCurve animationCurve, float duration)
		{
			CanvasGroupFade.<FadeTask>d__14 <FadeTask>d__;
			<FadeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<FadeTask>d__.<>4__this = this;
			<FadeTask>d__.token = token;
			<FadeTask>d__.beginAlpha = beginAlpha;
			<FadeTask>d__.targetAlpha = targetAlpha;
			<FadeTask>d__.animationCurve = animationCurve;
			<FadeTask>d__.duration = duration;
			<FadeTask>d__.<>1__state = -1;
			<FadeTask>d__.<>t__builder.Start<CanvasGroupFade.<FadeTask>d__14>(ref <FadeTask>d__);
			return <FadeTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0007C125 File Offset: 0x0007A325
		protected override void OnSkipHide()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0007C159 File Offset: 0x0007A359
		protected override void OnSkipShow()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 1f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0007C1A0 File Offset: 0x0007A3A0
		[CompilerGenerated]
		private bool <FadeTask>g__CheckTaskValid|14_0(ref CanvasGroupFade.<>c__DisplayClass14_0 A_1)
		{
			return this.canvasGroup != null && A_1.token == base.ActiveTaskToken;
		}

		// Token: 0x04001814 RID: 6164
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001815 RID: 6165
		[SerializeField]
		private AnimationCurve showingCurve;

		// Token: 0x04001816 RID: 6166
		[SerializeField]
		private AnimationCurve hidingCurve;

		// Token: 0x04001817 RID: 6167
		[SerializeField]
		private float fadeDuration = 0.2f;

		// Token: 0x04001818 RID: 6168
		[SerializeField]
		private bool manageBlockRaycast;

		// Token: 0x04001819 RID: 6169
		private bool awaked;
	}
}
