using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class Button_LoadMainMenu : MonoBehaviour
{
	// Token: 0x06000AFD RID: 2813 RVA: 0x0002EE6B File Offset: 0x0002D06B
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		this.dialogue.SkipHide();
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0002EE94 File Offset: 0x0002D094
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0002EEBC File Offset: 0x0002D0BC
	private UniTask QuitTask()
	{
		Button_LoadMainMenu.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_LoadMainMenu.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private Button button;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x04000970 RID: 2416
	private UniTask task;
}
