using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.UI.Animations;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003EB RID: 1003
	public class SavesButton : MonoBehaviour
	{
		// Token: 0x06002440 RID: 9280 RVA: 0x0007E0E0 File Offset: 0x0007C2E0
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
			SavesSystem.OnSetFile += this.Refresh;
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			SavesSystem.OnSaveDeleted += this.Refresh;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0007E13C File Offset: 0x0007C33C
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
			SavesSystem.OnSaveDeleted -= this.Refresh;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0007E171 File Offset: 0x0007C371
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0007E179 File Offset: 0x0007C379
		private void OnButtonClick()
		{
			if (!this.executing)
			{
				this.SavesSelectionTask().Forget();
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0007E190 File Offset: 0x0007C390
		private UniTask SavesSelectionTask()
		{
			SavesButton.<SavesSelectionTask>d__12 <SavesSelectionTask>d__;
			<SavesSelectionTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SavesSelectionTask>d__.<>4__this = this;
			<SavesSelectionTask>d__.<>1__state = -1;
			<SavesSelectionTask>d__.<>t__builder.Start<SavesButton.<SavesSelectionTask>d__12>(ref <SavesSelectionTask>d__);
			return <SavesSelectionTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0007E1D3 File Offset: 0x0007C3D3
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0007E1DC File Offset: 0x0007C3DC
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			string difficulty = flag ? GameRulesManager.Current.DisplayName : "";
			this.text.text = this.textFormat.Format(new
			{
				text = this.textKey.ToPlainText(),
				slotNumber = SavesSystem.CurrentSlot,
				difficulty = difficulty
			});
			bool active = flag && SavesSystem.IsOldSave(SavesSystem.CurrentSlot);
			this.oldSaveIndicator.SetActive(active);
		}

		// Token: 0x0400189E RID: 6302
		[SerializeField]
		private FadeGroup currentMenuFadeGroup;

		// Token: 0x0400189F RID: 6303
		[SerializeField]
		private SaveSlotSelectionMenu selectionMenu;

		// Token: 0x040018A0 RID: 6304
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x040018A1 RID: 6305
		[SerializeField]
		private Button button;

		// Token: 0x040018A2 RID: 6306
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018A3 RID: 6307
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey = "MainMenu_SaveSlot";

		// Token: 0x040018A4 RID: 6308
		[SerializeField]
		private string textFormat = "{text}: {slotNumber}";

		// Token: 0x040018A5 RID: 6309
		private bool executing;
	}
}
