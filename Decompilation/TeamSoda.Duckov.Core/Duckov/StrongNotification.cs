using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x02000240 RID: 576
	public class StrongNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0004483C File Offset: 0x00042A3C
		// (set) Token: 0x060011E2 RID: 4578 RVA: 0x00044843 File Offset: 0x00042A43
		public static StrongNotification Instance { get; private set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0004484B File Offset: 0x00042A4B
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0004485B File Offset: 0x00042A5B
		public static bool Showing
		{
			get
			{
				return !(StrongNotification.Instance == null) && StrongNotification.Instance.showing;
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00044878 File Offset: 0x00042A78
		private void Awake()
		{
			if (StrongNotification.Instance == null)
			{
				StrongNotification.Instance = this;
			}
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			View.OnActiveViewChanged += this.View_OnActiveViewChanged;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x000448CB File Offset: 0x00042ACB
		private void OnDestroy()
		{
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
			View.OnActiveViewChanged -= this.View_OnActiveViewChanged;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00044900 File Offset: 0x00042B00
		private void View_OnActiveViewChanged()
		{
			this.confirmed = true;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00044909 File Offset: 0x00042B09
		private void OnCancel(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00044912 File Offset: 0x00042B12
		private void OnConfirm(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004491B File Offset: 0x00042B1B
		private void Update()
		{
			if (!this.showing && StrongNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00044938 File Offset: 0x00042B38
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00044948 File Offset: 0x00042B48
		private UniTask ShowTask()
		{
			StrongNotification.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<StrongNotification.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004498C File Offset: 0x00042B8C
		private UniTask DisplayContent(StrongNotificationContent cur)
		{
			StrongNotification.<DisplayContent>d__24 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.cur = cur;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<StrongNotification.<DisplayContent>d__24>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x000449D7 File Offset: 0x00042BD7
		public void OnPointerClick(PointerEventData eventData)
		{
			this.confirmed = true;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x000449E0 File Offset: 0x00042BE0
		public static void Push(StrongNotificationContent content)
		{
			StrongNotification.pending.Add(content);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x000449ED File Offset: 0x00042BED
		public static void Push(string mainText, string subText = "")
		{
			StrongNotification.pending.Add(new StrongNotificationContent(mainText, subText, null));
		}

		// Token: 0x04000DC1 RID: 3521
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000DC3 RID: 3523
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000DC5 RID: 3525
		[SerializeField]
		private Image image;

		// Token: 0x04000DC6 RID: 3526
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000DC7 RID: 3527
		private static List<StrongNotificationContent> pending = new List<StrongNotificationContent>();

		// Token: 0x04000DC9 RID: 3529
		private UniTask showingTask;

		// Token: 0x04000DCA RID: 3530
		private bool confirmed;
	}
}
