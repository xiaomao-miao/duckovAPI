using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016D RID: 365
public class Button_QuitGame : MonoBehaviour
{
	// Token: 0x06000B01 RID: 2817 RVA: 0x0002EF07 File Offset: 0x0002D107
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		if (this.dialogue)
		{
			this.dialogue.SkipHide();
		}
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0002EF3D File Offset: 0x0002D13D
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0002EF64 File Offset: 0x0002D164
	private UniTask QuitTask()
	{
		Button_QuitGame.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_QuitGame.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Button button;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x04000973 RID: 2419
	private UniTask task;
}
