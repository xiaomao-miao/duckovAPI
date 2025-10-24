using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Modding;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001C4 RID: 452
public class ModUploadPanel : MonoBehaviour
{
	// Token: 0x06000D74 RID: 3444 RVA: 0x000377D3 File Offset: 0x000359D3
	private void Awake()
	{
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCancelBtnClick));
		this.btnUpload.onClick.AddListener(new UnityAction(this.OnUploadBtnClick));
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0003780D File Offset: 0x00035A0D
	private void OnUploadBtnClick()
	{
		this.uploadClicked = true;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00037816 File Offset: 0x00035A16
	private void OnCancelBtnClick()
	{
		this.cancelClicked = true;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x00037820 File Offset: 0x00035A20
	public UniTask Execute(ModInfo info)
	{
		ModUploadPanel.<Execute>d__29 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.info = info;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ModUploadPanel.<Execute>d__29>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x0003786C File Offset: 0x00035A6C
	private void Update()
	{
		if (this.waitingForUpload)
		{
			this.progressBarFill.fillAmount = SteamWorkshopManager.UploadingProgress;
			ulong punBytesProcess = SteamWorkshopManager.punBytesProcess;
			ulong punBytesTotal = SteamWorkshopManager.punBytesTotal;
			this.progressText.text = ModUploadPanel.FormatBytes(punBytesProcess) + " / " + ModUploadPanel.FormatBytes(punBytesTotal);
		}
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x000378C0 File Offset: 0x00035AC0
	private static string FormatBytes(ulong bytes)
	{
		if (bytes < 1024UL)
		{
			return string.Format("{0}bytes", bytes);
		}
		if (bytes < 1048576UL)
		{
			return string.Format("{0:0.0}KB", bytes / 1024f);
		}
		if (bytes < 1073741824UL)
		{
			return string.Format("{0:0.0}MB", bytes / 1048576f);
		}
		return string.Format("{0:0.0}GB", bytes / 1.0737418E+09f);
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x00037944 File Offset: 0x00035B44
	private void Clean()
	{
		this.fgLoading.SkipHide();
		this.fgContent.SkipHide();
		this.indicatorNew.SetActive(false);
		this.indicatorUpdate.SetActive(false);
		this.indicatorOwnershipWarning.SetActive(false);
		this.indicatorInvalidContent.SetActive(false);
		this.txtPublishedFileID.text = "-";
		this.txtPath.text = "-";
		this.fgButtonMain.SkipHide();
		this.fgProgressBar.SkipHide();
		this.fgSucceed.SkipHide();
		this.fgFailed.SkipHide();
		this.waitingForUpload = false;
	}

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private FadeGroup fgMain;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private FadeGroup fgLoading;

	// Token: 0x04000B6F RID: 2927
	[SerializeField]
	private FadeGroup fgContent;

	// Token: 0x04000B70 RID: 2928
	[SerializeField]
	private TextMeshProUGUI txtTitle;

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	private TextMeshProUGUI txtDescription;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private RawImage preview;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private TextMeshProUGUI txtModName;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private TextMeshProUGUI txtPath;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private TextMeshProUGUI txtPublishedFileID;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private GameObject indicatorNew;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private GameObject indicatorUpdate;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private GameObject indicatorOwnershipWarning;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private GameObject indicatorInvalidContent;

	// Token: 0x04000B7A RID: 2938
	[SerializeField]
	private Button btnUpload;

	// Token: 0x04000B7B RID: 2939
	[SerializeField]
	private Button btnCancel;

	// Token: 0x04000B7C RID: 2940
	[SerializeField]
	private FadeGroup fgButtonMain;

	// Token: 0x04000B7D RID: 2941
	[SerializeField]
	private FadeGroup fgProgressBar;

	// Token: 0x04000B7E RID: 2942
	[SerializeField]
	private TextMeshProUGUI progressText;

	// Token: 0x04000B7F RID: 2943
	[SerializeField]
	private Image progressBarFill;

	// Token: 0x04000B80 RID: 2944
	[SerializeField]
	private FadeGroup fgSucceed;

	// Token: 0x04000B81 RID: 2945
	[SerializeField]
	private FadeGroup fgFailed;

	// Token: 0x04000B82 RID: 2946
	[SerializeField]
	private float closeAfterSeconds = 2f;

	// Token: 0x04000B83 RID: 2947
	[SerializeField]
	private Texture2D defaultPreviewTexture;

	// Token: 0x04000B84 RID: 2948
	private bool cancelClicked;

	// Token: 0x04000B85 RID: 2949
	private bool uploadClicked;

	// Token: 0x04000B86 RID: 2950
	private bool waitingForUpload;
}
