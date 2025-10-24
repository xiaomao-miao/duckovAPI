using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000379 RID: 889
	public class NotificationText : MonoBehaviour
	{
		// Token: 0x06001EC3 RID: 7875 RVA: 0x0006C089 File Offset: 0x0006A289
		public static void Push(string text)
		{
			if (NotificationText.pendingTexts.Count > 0 && NotificationText.pendingTexts.Peek() == text)
			{
				return;
			}
			NotificationText.pendingTexts.Enqueue(text);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0006C0B6 File Offset: 0x0006A2B6
		private static string Pop()
		{
			return NotificationText.pendingTexts.Dequeue();
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x0006C0C2 File Offset: 0x0006A2C2
		private int PendingCount
		{
			get
			{
				return NotificationText.pendingTexts.Count;
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0006C0CE File Offset: 0x0006A2CE
		private void Update()
		{
			if (!this.showing && this.PendingCount > 0)
			{
				this.ShowNext().Forget();
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0006C0EC File Offset: 0x0006A2EC
		private UniTask ShowNext()
		{
			NotificationText.<ShowNext>d__11 <ShowNext>d__;
			<ShowNext>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNext>d__.<>4__this = this;
			<ShowNext>d__.<>1__state = -1;
			<ShowNext>d__.<>t__builder.Start<NotificationText.<ShowNext>d__11>(ref <ShowNext>d__);
			return <ShowNext>d__.<>t__builder.Task;
		}

		// Token: 0x04001503 RID: 5379
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001504 RID: 5380
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001505 RID: 5381
		[SerializeField]
		private float duration = 1.2f;

		// Token: 0x04001506 RID: 5382
		[SerializeField]
		private float durationIfPending = 0.65f;

		// Token: 0x04001507 RID: 5383
		private static Queue<string> pendingTexts = new Queue<string>();

		// Token: 0x04001508 RID: 5384
		private bool showing;
	}
}
