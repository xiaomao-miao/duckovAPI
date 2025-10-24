using System;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using NodeCanvas.Framework;

// Token: 0x020001B2 RID: 434
public class AT_SetBlackScreen : ActionTask
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x00035A72 File Offset: 0x00033C72
	protected override void OnExecute()
	{
		if (this.show)
		{
			this.task = BlackScreen.ShowAndReturnTask(null, 0f, 0.5f);
			return;
		}
		this.task = BlackScreen.HideAndReturnTask(null, 0f, 0.5f);
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00035AA9 File Offset: 0x00033CA9
	protected override void OnUpdate()
	{
		if (this.task.Status != UniTaskStatus.Pending)
		{
			base.EndAction();
		}
	}

	// Token: 0x04000B19 RID: 2841
	public bool show;

	// Token: 0x04000B1A RID: 2842
	private UniTask task;
}
