using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200003D RID: 61
public class Interact_CustomFace : InteractableBase
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600016B RID: 363 RVA: 0x00007034 File Offset: 0x00005234
	// (remove) Token: 0x0600016C RID: 364 RVA: 0x00007068 File Offset: 0x00005268
	public static event Action OnCustomFaceStartEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600016D RID: 365 RVA: 0x0000709C File Offset: 0x0000529C
	// (remove) Token: 0x0600016E RID: 366 RVA: 0x000070D0 File Offset: 0x000052D0
	public static event Action OnCustomFaceFinishedEvent;

	// Token: 0x0600016F RID: 367 RVA: 0x00007103 File Offset: 0x00005303
	protected override void Awake()
	{
		base.Awake();
		this.activePart.SetActive(false);
		this.customFaceUI.SetFace(this.customFaceInstance);
		this.fade.gameObject.SetActive(false);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000713C File Offset: 0x0000533C
	protected override void OnInteractStart(CharacterMainControl interactCharacter)
	{
		this.Show().Forget();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00007158 File Offset: 0x00005358
	protected override void OnInteractStop()
	{
		Debug.Log("Stop custom face");
		this.Hide().Forget();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00007180 File Offset: 0x00005380
	private UniTaskVoid Show()
	{
		Interact_CustomFace.<Show>d__17 <Show>d__;
		<Show>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Show>d__.<>4__this = this;
		<Show>d__.<>1__state = -1;
		<Show>d__.<>t__builder.Start<Interact_CustomFace.<Show>d__17>(ref <Show>d__);
		return <Show>d__.<>t__builder.Task;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x000071C4 File Offset: 0x000053C4
	private UniTaskVoid Hide()
	{
		Interact_CustomFace.<Hide>d__18 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<Interact_CustomFace.<Hide>d__18>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00007208 File Offset: 0x00005408
	public void CopyToClipboard()
	{
		GUIUtility.systemCopyBuffer = this.customFaceInstance.ConvertToSaveData().DataToJson();
		NotificationText.Push(this.OnCopyNotificationKey.ToPlainText());
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00007240 File Offset: 0x00005440
	public void PastyDataAndApply()
	{
		CustomFaceSettingData saveData;
		if (CustomFaceSettingData.JsonToData(GUIUtility.systemCopyBuffer, out saveData))
		{
			this.customFaceInstance.LoadFromData(saveData);
			NotificationText.Push(this.OnPastySuccessNotificationKey.ToPlainText());
			return;
		}
		NotificationText.Push(this.OnPastyFailedNotificationKey.ToPlainText());
	}

	// Token: 0x04000100 RID: 256
	public GameObject activePart;

	// Token: 0x04000101 RID: 257
	public CustomFaceUI customFaceUI;

	// Token: 0x04000102 RID: 258
	public CanvasGroupFade fade;

	// Token: 0x04000103 RID: 259
	public CustomFaceInstance customFaceInstance;

	// Token: 0x04000104 RID: 260
	public UnityEvent OnCustomFaceUiClosedEvent;

	// Token: 0x04000105 RID: 261
	[LocalizationKey("UIText")]
	public string OnCopyNotificationKey;

	// Token: 0x04000106 RID: 262
	[LocalizationKey("UIText")]
	public string OnPastySuccessNotificationKey;

	// Token: 0x04000107 RID: 263
	[LocalizationKey("UIText")]
	public string OnPastyFailedNotificationKey;
}
