using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x02000371 RID: 881
	[Obsolete]
	public class EndingFlow : MonoBehaviour
	{
		// Token: 0x06001E80 RID: 7808 RVA: 0x0006B6A9 File Offset: 0x000698A9
		private void Start()
		{
			this.Task().Forget();
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0006B6B8 File Offset: 0x000698B8
		private UniTask Task()
		{
			EndingFlow.<Task>d__4 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<EndingFlow.<Task>d__4>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0006B6FC File Offset: 0x000698FC
		private UniTask WaitForTaskBehaviour(MonoBehaviour mono)
		{
			EndingFlow.<WaitForTaskBehaviour>d__5 <WaitForTaskBehaviour>d__;
			<WaitForTaskBehaviour>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForTaskBehaviour>d__.mono = mono;
			<WaitForTaskBehaviour>d__.<>1__state = -1;
			<WaitForTaskBehaviour>d__.<>t__builder.Start<EndingFlow.<WaitForTaskBehaviour>d__5>(ref <WaitForTaskBehaviour>d__);
			return <WaitForTaskBehaviour>d__.<>t__builder.Task;
		}

		// Token: 0x040014D1 RID: 5329
		[SerializeField]
		private List<MonoBehaviour> taskBehaviours = new List<MonoBehaviour>();

		// Token: 0x040014D2 RID: 5330
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x040014D3 RID: 5331
		[SerializeField]
		private UnityEvent onEnd;
	}
}
