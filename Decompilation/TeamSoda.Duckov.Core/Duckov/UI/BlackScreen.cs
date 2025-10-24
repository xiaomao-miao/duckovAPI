using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200037A RID: 890
	public class BlackScreen : MonoBehaviour
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x0006C159 File Offset: 0x0006A359
		public static BlackScreen Instance
		{
			get
			{
				return GameManager.BlackScreen;
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0006C160 File Offset: 0x0006A360
		private void Awake()
		{
			if (BlackScreen.Instance != this)
			{
				Debug.LogError("检测到应当删除的BlackScreen实例", base.gameObject);
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0006C17F File Offset: 0x0006A37F
		private void SetFadeCurve(AnimationCurve curve)
		{
			this.fadeElement.ShowCurve = curve;
			this.fadeElement.HideCurve = curve;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0006C199 File Offset: 0x0006A399
		private void SetCircleFade(float circleFade)
		{
			this.fadeImage.material.SetFloat("_CircleFade", circleFade);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0006C1B4 File Offset: 0x0006A3B4
		private UniTask LShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			this.taskCounter++;
			if (this.taskCounter > 1)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultShowCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0006C224 File Offset: 0x0006A424
		private UniTask LHideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			int num = this.taskCounter - 1;
			this.taskCounter = num;
			if (num > 0)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultHideCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0006C291 File Offset: 0x0006A491
		public static UniTask ShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LShowAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0006C2B3 File Offset: 0x0006A4B3
		public static UniTask HideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LHideAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x04001509 RID: 5385
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400150A RID: 5386
		[SerializeField]
		private MaterialPropertyFade fadeElement;

		// Token: 0x0400150B RID: 5387
		[SerializeField]
		private Image fadeImage;

		// Token: 0x0400150C RID: 5388
		[SerializeField]
		private float defaultDuration = 0.5f;

		// Token: 0x0400150D RID: 5389
		[SerializeField]
		private AnimationCurve defaultShowCurve;

		// Token: 0x0400150E RID: 5390
		[SerializeField]
		private AnimationCurve defaultHideCurve;

		// Token: 0x0400150F RID: 5391
		private int taskCounter;
	}
}
