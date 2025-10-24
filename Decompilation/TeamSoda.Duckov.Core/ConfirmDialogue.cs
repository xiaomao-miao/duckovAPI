using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class ConfirmDialogue : MonoBehaviour
{
	// Token: 0x06000B05 RID: 2821 RVA: 0x0002EFAF File Offset: 0x0002D1AF
	private void Awake()
	{
		this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirmed));
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCanceled));
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0002EFE9 File Offset: 0x0002D1E9
	private void OnCanceled()
	{
		this.canceled = true;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0002EFF2 File Offset: 0x0002D1F2
	private void OnConfirmed()
	{
		this.confirmed = true;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0002EFFC File Offset: 0x0002D1FC
	public UniTask<bool> Execute()
	{
		ConfirmDialogue.<Execute>d__9 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ConfirmDialogue.<Execute>d__9>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0002F040 File Offset: 0x0002D240
	private UniTask<bool> DoExecute()
	{
		ConfirmDialogue.<DoExecute>d__10 <DoExecute>d__;
		<DoExecute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<DoExecute>d__.<>4__this = this;
		<DoExecute>d__.<>1__state = -1;
		<DoExecute>d__.<>t__builder.Start<ConfirmDialogue.<DoExecute>d__10>(ref <DoExecute>d__);
		return <DoExecute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0002F083 File Offset: 0x0002D283
	internal void SkipHide()
	{
		this.fadeGroup.SkipHide();
	}

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000975 RID: 2421
	[SerializeField]
	private Button btnConfirm;

	// Token: 0x04000976 RID: 2422
	[SerializeField]
	private Button btnCancel;

	// Token: 0x04000977 RID: 2423
	private bool canceled;

	// Token: 0x04000978 RID: 2424
	private bool confirmed;

	// Token: 0x04000979 RID: 2425
	private bool executing;
}
