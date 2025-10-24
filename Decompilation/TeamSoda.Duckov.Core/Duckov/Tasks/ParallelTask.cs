using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x02000373 RID: 883
	public class ParallelTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001E8B RID: 7819 RVA: 0x0006B804 File Offset: 0x00069A04
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0006B814 File Offset: 0x00069A14
		private UniTask MainTask()
		{
			ParallelTask.<MainTask>d__7 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<ParallelTask.<MainTask>d__7>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0006B857 File Offset: 0x00069A57
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0006B88C File Offset: 0x00069A8C
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0006B894 File Offset: 0x00069A94
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x040014DE RID: 5342
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x040014DF RID: 5343
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x040014E0 RID: 5344
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x040014E1 RID: 5345
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x040014E2 RID: 5346
		private bool running;

		// Token: 0x040014E3 RID: 5347
		private bool complete;
	}
}
