using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x0200026F RID: 623
	public class ModManagerUI : MonoBehaviour
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x000489D9 File Offset: 0x00046BD9
		private ModManager Master
		{
			get
			{
				return ModManager.Instance;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x000489E0 File Offset: 0x00046BE0
		private PrefabPool<ModEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<ModEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00048A1C File Offset: 0x00046C1C
		private void Awake()
		{
			this.agreementBtn.onClick.AddListener(new UnityAction(this.OnAgreementBtnClicked));
			this.quitBtn.onClick.AddListener(new UnityAction(this.Quit));
			this.rejectBtn.onClick.AddListener(new UnityAction(this.OnRejectBtnClicked));
			this.needRebootIndicator.SetActive(false);
			ModManager.OnReorder += this.OnReorder;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00048A9A File Offset: 0x00046C9A
		private void OnDestroy()
		{
			ModManager.OnReorder -= this.OnReorder;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00048AAD File Offset: 0x00046CAD
		private void OnReorder()
		{
			this.Refresh();
			this.needRebootIndicator.SetActive(true);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00048AC1 File Offset: 0x00046CC1
		private void OnRejectBtnClicked()
		{
			ModManager.AllowActivatingMod = false;
			this.Quit();
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00048ACF File Offset: 0x00046CCF
		private void OnAgreementBtnClicked()
		{
			ModManager.AllowActivatingMod = true;
			this.agreementFadeGroup.Hide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00048AED File Offset: 0x00046CED
		private void Show()
		{
			this.mainFadeGroup.Show();
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00048AFC File Offset: 0x00046CFC
		private void OnEnable()
		{
			ModManager.Rescan();
			this.Refresh();
			this.uploaderFadeGroup.SkipHide();
			if (!ModManager.AllowActivatingMod)
			{
				this.contentFadeGroup.SkipHide();
				this.agreementFadeGroup.Show();
				return;
			}
			this.agreementFadeGroup.SkipHide();
			this.contentFadeGroup.Show();
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00048B54 File Offset: 0x00046D54
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			int num = 0;
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				this.Pool.Get(null).Setup(this, modInfo, num);
				num++;
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00048BC4 File Offset: 0x00046DC4
		private void Hide()
		{
			this.mainFadeGroup.Hide();
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00048BD1 File Offset: 0x00046DD1
		private void Quit()
		{
			UnityEvent unityEvent = this.onQuit;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.Hide();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00048BEC File Offset: 0x00046DEC
		internal UniTask BeginUpload(ModInfo info)
		{
			ModManagerUI.<BeginUpload>d__27 <BeginUpload>d__;
			<BeginUpload>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BeginUpload>d__.<>4__this = this;
			<BeginUpload>d__.info = info;
			<BeginUpload>d__.<>1__state = -1;
			<BeginUpload>d__.<>t__builder.Start<ModManagerUI.<BeginUpload>d__27>(ref <BeginUpload>d__);
			return <BeginUpload>d__.<>t__builder.Task;
		}

		// Token: 0x04000E7C RID: 3708
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000E7D RID: 3709
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000E7E RID: 3710
		[SerializeField]
		private FadeGroup agreementFadeGroup;

		// Token: 0x04000E7F RID: 3711
		[SerializeField]
		private FadeGroup uploaderFadeGroup;

		// Token: 0x04000E80 RID: 3712
		[SerializeField]
		private ModUploadPanel uploadPanel;

		// Token: 0x04000E81 RID: 3713
		[SerializeField]
		private Button rejectBtn;

		// Token: 0x04000E82 RID: 3714
		[SerializeField]
		private Button agreementBtn;

		// Token: 0x04000E83 RID: 3715
		[SerializeField]
		private ModEntry entryTemplate;

		// Token: 0x04000E84 RID: 3716
		[SerializeField]
		private Button quitBtn;

		// Token: 0x04000E85 RID: 3717
		[SerializeField]
		private GameObject needRebootIndicator;

		// Token: 0x04000E86 RID: 3718
		public UnityEvent onQuit;

		// Token: 0x04000E87 RID: 3719
		private PrefabPool<ModEntry> _pool;

		// Token: 0x04000E88 RID: 3720
		private bool uploading;
	}
}
