using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x02000372 RID: 882
	public class TaskList : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001E84 RID: 7812 RVA: 0x0006B752 File Offset: 0x00069952
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0006B764 File Offset: 0x00069964
		private UniTask MainTask()
		{
			TaskList.<MainTask>d__10 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<TaskList.<MainTask>d__10>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0006B7A7 File Offset: 0x000699A7
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.skip = false;
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0006B7E3 File Offset: 0x000699E3
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0006B7EB File Offset: 0x000699EB
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0006B7F3 File Offset: 0x000699F3
		public void Skip()
		{
			this.skip = true;
		}

		// Token: 0x040014D4 RID: 5332
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x040014D5 RID: 5333
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x040014D6 RID: 5334
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x040014D7 RID: 5335
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x040014D8 RID: 5336
		[SerializeField]
		private bool listenToSkipSignal;

		// Token: 0x040014D9 RID: 5337
		private bool running;

		// Token: 0x040014DA RID: 5338
		private bool complete;

		// Token: 0x040014DB RID: 5339
		private int currentTaskIndex;

		// Token: 0x040014DC RID: 5340
		private ITaskBehaviour currentTask;

		// Token: 0x040014DD RID: 5341
		private bool skip;
	}
}
