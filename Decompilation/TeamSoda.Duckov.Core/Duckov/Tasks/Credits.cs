using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.Tasks
{
	// Token: 0x02000370 RID: 880
	public class Credits : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001E79 RID: 7801 RVA: 0x0006B5BA File Offset: 0x000697BA
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0006B5CD File Offset: 0x000697CD
		public void Begin()
		{
			if (this.task.Status == UniTaskStatus.Pending)
			{
				return;
			}
			this.skip = false;
			this.fadeGroup.SkipHide();
			this.fadeGroup.gameObject.SetActive(true);
			this.task = this.Task();
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0006B60C File Offset: 0x0006980C
		public bool IsPending()
		{
			return this.task.Status == UniTaskStatus.Pending;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0006B61C File Offset: 0x0006981C
		public bool IsComplete()
		{
			return !this.IsPending();
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0006B628 File Offset: 0x00069828
		private UniTask Task()
		{
			Credits.<Task>d__13 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<Credits.<Task>d__13>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0006B66B File Offset: 0x0006986B
		public void Skip()
		{
			this.skip = true;
			if (this.fadeOut && this.fadeGroup.IsFading)
			{
				this.fadeGroup.SkipHide();
			}
			if (!this.mute)
			{
				AudioManager.StopBGM();
			}
		}

		// Token: 0x040014C8 RID: 5320
		private RectTransform rectTransform;

		// Token: 0x040014C9 RID: 5321
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040014CA RID: 5322
		[SerializeField]
		private RectTransform content;

		// Token: 0x040014CB RID: 5323
		[SerializeField]
		private float scrollSpeed;

		// Token: 0x040014CC RID: 5324
		[SerializeField]
		private float holdForSeconds;

		// Token: 0x040014CD RID: 5325
		[SerializeField]
		private bool fadeOut;

		// Token: 0x040014CE RID: 5326
		[SerializeField]
		private bool mute;

		// Token: 0x040014CF RID: 5327
		private UniTask task;

		// Token: 0x040014D0 RID: 5328
		private bool skip;
	}
}
