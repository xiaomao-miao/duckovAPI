using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000304 RID: 772
	public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerMoveHandler
	{
		// Token: 0x06001911 RID: 6417 RVA: 0x0005B17A File Offset: 0x0005937A
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.RefreshFadeGroups();
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0005B194 File Offset: 0x00059394
		private void CacheRadius()
		{
			this.cachedRect = this.rectTransform.rect;
			Rect rect = this.cachedRect;
			this.cachedRadius = Mathf.Sqrt(rect.width * rect.width + rect.height * rect.height) / 2f;
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0005B1E9 File Offset: 0x000593E9
		private void Update()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			this.HandleAnimation();
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0005B210 File Offset: 0x00059410
		private void HandleAnimation()
		{
			Quaternion quaternion = this.cardTransform.rotation;
			if ((this.facingFront && !this.frontFadeGroup.IsShown) || (!this.facingFront && !this.backFadeGroup.IsShown))
			{
				quaternion = Quaternion.RotateTowards(quaternion, Quaternion.Euler(0f, 90f, 0f), this.flipSpeed * Time.deltaTime);
				if (Mathf.Approximately(Quaternion.Angle(quaternion, Quaternion.Euler(0f, 90f, 0f)), 0f))
				{
					quaternion = Quaternion.Euler(0f, -90f, 0f);
					this.RefreshFadeGroups();
				}
			}
			else
			{
				quaternion = Quaternion.RotateTowards(quaternion, this.GetIdealRotation(), this.rotateSpeed * Time.deltaTime);
			}
			this.cardTransform.rotation = quaternion;
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0005B2EB File Offset: 0x000594EB
		private void OnEnable()
		{
			this.CacheRadius();
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0005B2F4 File Offset: 0x000594F4
		private Quaternion GetIdealRotation()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			if (this.hovering && !Mathf.Approximately(this.cachedRadius, 0f))
			{
				Vector2 a;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, this.pointerPosition, null, out a);
				Vector2 center = this.rectTransform.rect.center;
				Vector2 a2 = a - center;
				float d = Mathf.Max(10f, this.cachedRadius);
				Vector2 vector = Vector2.ClampMagnitude(a2 / d, 1f);
				return Quaternion.Euler(-vector.y * this.idleAmp, -vector.x * this.idleAmp, 0f);
			}
			return Quaternion.Euler(Mathf.Sin(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, Mathf.Cos(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, 0f);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0005B408 File Offset: 0x00059608
		private void SkipAnimation()
		{
			this.RefreshFadeGroups();
			this.cardTransform.rotation = this.GetIdealRotation();
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0005B421 File Offset: 0x00059621
		public void SetFacing(bool facingFront, bool skipAnimation = false)
		{
			this.facingFront = facingFront;
			if (skipAnimation)
			{
				this.SkipAnimation();
			}
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0005B433 File Offset: 0x00059633
		public void Flip()
		{
			this.SetFacing(!this.facingFront, false);
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0005B445 File Offset: 0x00059645
		private void RefreshFadeGroups()
		{
			if (this.facingFront)
			{
				this.frontFadeGroup.Show();
				this.backFadeGroup.Hide();
				return;
			}
			this.frontFadeGroup.Hide();
			this.backFadeGroup.Show();
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0005B47C File Offset: 0x0005967C
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0005B485 File Offset: 0x00059685
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0005B48E File Offset: 0x0005968E
		public void OnPointerMove(PointerEventData eventData)
		{
			this.pointerPosition = eventData.position;
		}

		// Token: 0x0400122D RID: 4653
		private RectTransform rectTransform;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private RectTransform cardTransform;

		// Token: 0x0400122F RID: 4655
		[SerializeField]
		private FadeGroup frontFadeGroup;

		// Token: 0x04001230 RID: 4656
		[SerializeField]
		private FadeGroup backFadeGroup;

		// Token: 0x04001231 RID: 4657
		[SerializeField]
		private float idleAmp = 10f;

		// Token: 0x04001232 RID: 4658
		[SerializeField]
		private float idleFrequency = 0.5f;

		// Token: 0x04001233 RID: 4659
		[SerializeField]
		private float rotateSpeed = 300f;

		// Token: 0x04001234 RID: 4660
		[SerializeField]
		private float flipSpeed = 300f;

		// Token: 0x04001235 RID: 4661
		private bool facingFront;

		// Token: 0x04001236 RID: 4662
		private bool hovering;

		// Token: 0x04001237 RID: 4663
		private Vector2 pointerPosition;

		// Token: 0x04001238 RID: 4664
		private Rect cachedRect;

		// Token: 0x04001239 RID: 4665
		private float cachedRadius;
	}
}
