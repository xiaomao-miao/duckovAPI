using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200038B RID: 907
	public class PunchReceiver : MonoBehaviour
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0006E4E7 File Offset: 0x0006C6E7
		private float PunchAnchorPositionDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x0006E4EF File Offset: 0x0006C6EF
		private float PunchScaleDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0006E4F7 File Offset: 0x0006C6F7
		private float PunchRotationDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0006E4FF File Offset: 0x0006C6FF
		private bool ShouldPunchPosition
		{
			get
			{
				return this.randomAnchorPosition.magnitude > 0.001f && this.punchAnchorPosition.magnitude > 0.001f;
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0006E527 File Offset: 0x0006C727
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.CachePose();
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0006E549 File Offset: 0x0006C749
		private void Start()
		{
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0006E54C File Offset: 0x0006C74C
		[ContextMenu("Punch")]
		public void Punch()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.particle != null)
			{
				this.particle.Play();
			}
			this.rectTransform.DOKill(false);
			if (this.cacheWhenPunched)
			{
				this.CachePose();
			}
			Vector2 punch = this.punchAnchorPosition + new Vector2(UnityEngine.Random.Range(-this.randomAnchorPosition.x, this.randomAnchorPosition.x), UnityEngine.Random.Range(-this.randomAnchorPosition.y, this.randomAnchorPosition.y));
			float d = this.punchScaleUniform;
			float d2 = this.punchRotationZ + UnityEngine.Random.Range(-this.randomRotationZ, this.randomRotationZ);
			if (this.ShouldPunchPosition)
			{
				this.rectTransform.DOPunchAnchorPos(punch, this.PunchAnchorPositionDuration, this.vibrato, this.elasticity, false).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			}
			this.rectTransform.DOPunchScale(Vector3.one * d, this.PunchScaleDuration, this.vibrato, this.elasticity).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			this.rectTransform.DOPunchRotation(Vector3.forward * d2, this.PunchRotationDuration, this.vibrato, this.elasticity).SetEase(this.animationCurve).OnKill(new TweenCallback(this.RestorePose));
			if (!string.IsNullOrWhiteSpace(this.sfx))
			{
				AudioManager.Post(this.sfx);
			}
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0006E6F8 File Offset: 0x0006C8F8
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchorPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0006E75C File Offset: 0x0006C95C
		private void RestorePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.ShouldPunchPosition)
			{
				this.rectTransform.anchoredPosition = this.cachedAnchorPosition;
			}
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0006E7C2 File Offset: 0x0006C9C2
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.DOKill(false);
		}

		// Token: 0x04001587 RID: 5511
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001588 RID: 5512
		[SerializeField]
		private ParticleSystem particle;

		// Token: 0x04001589 RID: 5513
		[Min(0.0001f)]
		[SerializeField]
		private float duration = 0.01f;

		// Token: 0x0400158A RID: 5514
		public int vibrato = 10;

		// Token: 0x0400158B RID: 5515
		public float elasticity = 1f;

		// Token: 0x0400158C RID: 5516
		[SerializeField]
		private Vector2 punchAnchorPosition;

		// Token: 0x0400158D RID: 5517
		[SerializeField]
		[Range(-1f, 1f)]
		private float punchScaleUniform;

		// Token: 0x0400158E RID: 5518
		[SerializeField]
		[Range(-180f, 180f)]
		private float punchRotationZ;

		// Token: 0x0400158F RID: 5519
		[SerializeField]
		private Vector2 randomAnchorPosition;

		// Token: 0x04001590 RID: 5520
		[SerializeField]
		[Range(0f, 180f)]
		private float randomRotationZ;

		// Token: 0x04001591 RID: 5521
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001592 RID: 5522
		[SerializeField]
		private bool cacheWhenPunched;

		// Token: 0x04001593 RID: 5523
		[SerializeField]
		private string sfx;

		// Token: 0x04001594 RID: 5524
		private Vector2 cachedAnchorPosition;

		// Token: 0x04001595 RID: 5525
		private Vector2 cachedScale;

		// Token: 0x04001596 RID: 5526
		private Vector2 cachedRotation;
	}
}
