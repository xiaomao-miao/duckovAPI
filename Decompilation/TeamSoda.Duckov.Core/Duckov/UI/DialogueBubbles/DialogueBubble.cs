using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x020003E5 RID: 997
	public class DialogueBubble : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0007D962 File Offset: 0x0007BB62
		public Transform Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x0007D96A File Offset: 0x0007BB6A
		private float YOffset
		{
			get
			{
				if (this._yOffset < 0f)
				{
					return this.defaultYOffset;
				}
				return this._yOffset;
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0007D986 File Offset: 0x0007BB86
		private void LateUpdate()
		{
			this.UpdatePosition();
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0007D990 File Offset: 0x0007BB90
		private void UpdatePosition()
		{
			if (this.target == null)
			{
				return;
			}
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, this.target.position + Vector3.up * this.YOffset);
			screenPoint.y += this.screenYOffset * (float)Screen.height;
			Vector2 v;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPoint, null, out v))
			{
				return;
			}
			base.transform.localPosition = v;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0007DA1C File Offset: 0x0007BC1C
		public UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			this.task = this.ShowTask(text, target, yOffset, needInteraction, skippable, speed, duration);
			return this.task;
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x0007DA48 File Offset: 0x0007BC48
		public UniTask ShowTask(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubble.<ShowTask>d__20 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.text = text;
			<ShowTask>d__.target = target;
			<ShowTask>d__.yOffset = yOffset;
			<ShowTask>d__.needInteraction = needInteraction;
			<ShowTask>d__.skippable = skippable;
			<ShowTask>d__.speed = speed;
			<ShowTask>d__.duration = duration;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<DialogueBubble.<ShowTask>d__20>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0007DAC8 File Offset: 0x0007BCC8
		private UniTask WaitForInteraction(int currentToken)
		{
			DialogueBubble.<WaitForInteraction>d__21 <WaitForInteraction>d__;
			<WaitForInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForInteraction>d__.<>4__this = this;
			<WaitForInteraction>d__.currentToken = currentToken;
			<WaitForInteraction>d__.<>1__state = -1;
			<WaitForInteraction>d__.<>t__builder.Start<DialogueBubble.<WaitForInteraction>d__21>(ref <WaitForInteraction>d__);
			return <WaitForInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0007DB13 File Offset: 0x0007BD13
		public void Interact()
		{
			this.interacted = true;
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x0007DB1C File Offset: 0x0007BD1C
		private UniTask Hide()
		{
			DialogueBubble.<Hide>d__23 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<DialogueBubble.<Hide>d__23>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0007DB5F File Offset: 0x0007BD5F
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Interact();
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0007DB67 File Offset: 0x0007BD67
		private void Awake()
		{
			DialogueBubblesManager.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x04001877 RID: 6263
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001878 RID: 6264
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001879 RID: 6265
		[SerializeField]
		private float defaultSpeed = 10f;

		// Token: 0x0400187A RID: 6266
		[SerializeField]
		private float sustainDuration = 2f;

		// Token: 0x0400187B RID: 6267
		[SerializeField]
		private float defaultYOffset = 2f;

		// Token: 0x0400187C RID: 6268
		[SerializeField]
		private GameObject interactIndicator;

		// Token: 0x0400187D RID: 6269
		private bool interacted;

		// Token: 0x0400187E RID: 6270
		private bool animating;

		// Token: 0x0400187F RID: 6271
		private int taskToken;

		// Token: 0x04001880 RID: 6272
		private Transform target;

		// Token: 0x04001881 RID: 6273
		private float _yOffset;

		// Token: 0x04001882 RID: 6274
		private float screenYOffset = 0.06f;

		// Token: 0x04001883 RID: 6275
		private UniTask task;
	}
}
