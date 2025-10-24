using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x0200026E RID: 622
	public class ModEntry : MonoBehaviour
	{
		// Token: 0x06001370 RID: 4976 RVA: 0x00048678 File Offset: 0x00046878
		private void Awake()
		{
			this.toggleButton.onClick.AddListener(new UnityAction(this.OnToggleButtonClicked));
			this.uploadButton.onClick.AddListener(new UnityAction(this.OnUploadButtonClicked));
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Combine(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
			this.failedIndicator.SetActive(false);
			this.btnReorderDown.onClick.AddListener(new UnityAction(this.OnButtonReorderDownClicked));
			this.btnReorderUp.onClick.AddListener(new UnityAction(this.OnButtonReorderUpClicked));
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00048721 File Offset: 0x00046921
		private void OnButtonReorderUpClicked()
		{
			ModManager.Reorder(this.index, this.index - 1);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00048737 File Offset: 0x00046937
		private void OnButtonReorderDownClicked()
		{
			ModManager.Reorder(this.index, this.index + 1);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0004874D File Offset: 0x0004694D
		private void OnDestroy()
		{
			ModManager.OnModLoadingFailed = (Action<string, string>)Delegate.Remove(ModManager.OnModLoadingFailed, new Action<string, string>(this.OnModLoadingFailed));
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0004876F File Offset: 0x0004696F
		private void OnModLoadingFailed(string dllPath, string message)
		{
			if (dllPath != this.info.dllPath)
			{
				return;
			}
			Debug.LogError(message);
			this.failedIndicator.SetActive(true);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00048797 File Offset: 0x00046997
		private void OnUploadButtonClicked()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.BeginUpload(this.info).Forget();
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000487C0 File Offset: 0x000469C0
		private void OnToggleButtonClicked()
		{
			if (ModManager.Instance == null)
			{
				Debug.LogError("ModManager.Instance Not Found");
				return;
			}
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			if (flag && flag2)
			{
				ModManager.Instance.DeactivateMod(this.info);
				return;
			}
			ModManager.Instance.ActivateMod(this.info);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00048844 File Offset: 0x00046A44
		private void OnEnable()
		{
			ModManager.OnModStatusChanged += this.OnModStatusChanged;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00048857 File Offset: 0x00046A57
		private void OnDisable()
		{
			ModManager.OnModStatusChanged -= this.OnModStatusChanged;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004886A File Offset: 0x00046A6A
		private void OnModStatusChanged()
		{
			this.RefreshStatus();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00048874 File Offset: 0x00046A74
		private void RefreshStatus()
		{
			ModBehaviour modBehaviour;
			bool flag = ModManager.IsModActive(this.info, out modBehaviour);
			bool flag2 = flag && modBehaviour.info.path.Trim() == this.info.path.Trim();
			bool active = flag && !flag2;
			this.activeIndicator.SetActive(flag2);
			this.nameCollisionIndicator.SetActive(active);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x000488DC File Offset: 0x00046ADC
		private void RefreshInfo()
		{
			this.textTitle.text = this.info.displayName;
			this.textName.text = this.info.name;
			this.textDescription.text = this.info.description;
			this.preview.texture = this.info.preview;
			this.steamItemIndicator.SetActive(this.info.isSteamItem);
			this.notSteamItemIndicator.SetActive(!this.info.isSteamItem);
			bool flag = SteamWorkshopManager.IsOwner(this.info);
			this.steamItemOwnerIndicator.SetActive(flag);
			bool active = flag || !this.info.isSteamItem;
			this.uploadButton.gameObject.SetActive(active);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000489AE File Offset: 0x00046BAE
		public void Setup(ModManagerUI master, ModInfo modInfo, int index)
		{
			this.master = master;
			this.info = modInfo;
			this.index = index;
			this.RefreshInfo();
			this.RefreshStatus();
		}

		// Token: 0x04000E6B RID: 3691
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x04000E6C RID: 3692
		[SerializeField]
		private TextMeshProUGUI textName;

		// Token: 0x04000E6D RID: 3693
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x04000E6E RID: 3694
		[SerializeField]
		private RawImage preview;

		// Token: 0x04000E6F RID: 3695
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x04000E70 RID: 3696
		[SerializeField]
		private GameObject nameCollisionIndicator;

		// Token: 0x04000E71 RID: 3697
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04000E72 RID: 3698
		[SerializeField]
		private GameObject steamItemIndicator;

		// Token: 0x04000E73 RID: 3699
		[SerializeField]
		private GameObject steamItemOwnerIndicator;

		// Token: 0x04000E74 RID: 3700
		[SerializeField]
		private GameObject notSteamItemIndicator;

		// Token: 0x04000E75 RID: 3701
		[SerializeField]
		private Button uploadButton;

		// Token: 0x04000E76 RID: 3702
		[SerializeField]
		private GameObject failedIndicator;

		// Token: 0x04000E77 RID: 3703
		[SerializeField]
		private Button btnReorderUp;

		// Token: 0x04000E78 RID: 3704
		[SerializeField]
		private Button btnReorderDown;

		// Token: 0x04000E79 RID: 3705
		[SerializeField]
		private int index;

		// Token: 0x04000E7A RID: 3706
		private ModManagerUI master;

		// Token: 0x04000E7B RID: 3707
		private ModInfo info;
	}
}
