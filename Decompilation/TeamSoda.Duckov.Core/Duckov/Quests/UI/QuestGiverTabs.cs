using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000349 RID: 841
	public class QuestGiverTabs : MonoBehaviour, ISingleSelectionMenu<QuestGiverTabButton>
	{
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06001D08 RID: 7432 RVA: 0x00068294 File Offset: 0x00066494
		// (remove) Token: 0x06001D09 RID: 7433 RVA: 0x000682CC File Offset: 0x000664CC
		public event Action<QuestGiverTabs> onSelectionChanged;

		// Token: 0x06001D0A RID: 7434 RVA: 0x00068301 File Offset: 0x00066501
		public QuestGiverTabButton GetSelection()
		{
			return this.selectedButton;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x00068309 File Offset: 0x00066509
		public QuestStatus GetStatus()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return this.selectedButton.Status;
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00068324 File Offset: 0x00066524
		public bool SetSelection(QuestGiverTabButton selection)
		{
			this.selectedButton = selection;
			this.RefreshAllButtons();
			Action<QuestGiverTabs> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this);
			}
			return true;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00068348 File Offset: 0x00066548
		private void Initialize()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Setup(this);
			}
			if (this.buttons.Count > 0)
			{
				this.SetSelection(this.buttons[0]);
			}
			this.initialized = true;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000683C4 File Offset: 0x000665C4
		private void Awake()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000683D4 File Offset: 0x000665D4
		private void RefreshAllButtons()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Refresh();
			}
		}

		// Token: 0x0400142C RID: 5164
		[SerializeField]
		private List<QuestGiverTabButton> buttons = new List<QuestGiverTabButton>();

		// Token: 0x0400142D RID: 5165
		private QuestGiverTabButton selectedButton;

		// Token: 0x0400142F RID: 5167
		private bool initialized;
	}
}
