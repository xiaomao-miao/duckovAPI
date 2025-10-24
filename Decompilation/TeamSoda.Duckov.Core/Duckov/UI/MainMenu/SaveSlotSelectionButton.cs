using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003EC RID: 1004
	public class SaveSlotSelectionButton : MonoBehaviour
	{
		// Token: 0x06002448 RID: 9288 RVA: 0x0007E269 File Offset: 0x0007C469
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0007E287 File Offset: 0x0007C487
		private void OnDestroy()
		{
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0007E289 File Offset: 0x0007C489
		private void OnEnable()
		{
			SavesSystem.OnSetFile += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0007E2A2 File Offset: 0x0007C4A2
		private void OnDisable()
		{
			SavesSystem.OnSetFile -= this.Refresh;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0007E2B5 File Offset: 0x0007C4B5
		private void OnButtonClick()
		{
			SavesSystem.SetFile(this.index);
			this.menu.Finish();
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0007E2CD File Offset: 0x0007C4CD
		private void OnValidate()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
			if (this.text == null)
			{
				this.text = base.GetComponentInChildren<TextMeshProUGUI>();
			}
			this.Refresh();
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0007E30C File Offset: 0x0007C50C
		private void Refresh()
		{
			new ES3Settings(SavesSystem.GetFilePath(this.index), null).location = ES3.Location.File;
			this.text.text = this.format.Format(new
			{
				slotText = this.slotTextKey.ToPlainText(),
				index = this.index
			});
			bool active = SavesSystem.CurrentSlot == this.index;
			GameObject gameObject = this.activeIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
			if (SavesSystem.IsOldGame(this.index))
			{
				this.difficultyText.text = (GameRulesManager.GetRuleIndexDisplayNameOfSlot(this.index) ?? "");
				this.playTimeText.gameObject.SetActive(true);
				TimeSpan realTimePlayedOfSaveSlot = GameClock.GetRealTimePlayedOfSaveSlot(this.index);
				this.playTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt((float)realTimePlayedOfSaveSlot.TotalHours), realTimePlayedOfSaveSlot.Minutes);
				bool active2 = SavesSystem.IsOldSave(this.index);
				this.oldSlotIndicator.SetActive(active2);
				long num = SavesSystem.Load<long>("SaveTime", this.index);
				string text = (num > 0L) ? DateTime.FromBinary(num).ToLocalTime().ToString("yyyy/MM/dd HH:mm") : "???";
				this.saveTimeText.text = text;
				return;
			}
			this.difficultyText.text = this.newGameTextKey.ToPlainText();
			this.playTimeText.gameObject.SetActive(false);
			this.oldSlotIndicator.SetActive(false);
			this.saveTimeText.text = "----/--/-- --:--";
		}

		// Token: 0x040018A6 RID: 6310
		[SerializeField]
		private SaveSlotSelectionMenu menu;

		// Token: 0x040018A7 RID: 6311
		[SerializeField]
		private Button button;

		// Token: 0x040018A8 RID: 6312
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018A9 RID: 6313
		[SerializeField]
		private TextMeshProUGUI difficultyText;

		// Token: 0x040018AA RID: 6314
		[SerializeField]
		private TextMeshProUGUI playTimeText;

		// Token: 0x040018AB RID: 6315
		[SerializeField]
		private TextMeshProUGUI saveTimeText;

		// Token: 0x040018AC RID: 6316
		[SerializeField]
		private string slotTextKey = "MainMenu_SaveSelection_Slot";

		// Token: 0x040018AD RID: 6317
		[SerializeField]
		private string format = "{slotText} {index}";

		// Token: 0x040018AE RID: 6318
		[LocalizationKey("Default")]
		[SerializeField]
		private string newGameTextKey = "NewGame";

		// Token: 0x040018AF RID: 6319
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x040018B0 RID: 6320
		[SerializeField]
		private GameObject oldSlotIndicator;

		// Token: 0x040018B1 RID: 6321
		[Min(1f)]
		[SerializeField]
		private int index;
	}
}
