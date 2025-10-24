using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000347 RID: 839
	public class QuestGiverTabButton : MonoBehaviour
	{
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x00068201 File Offset: 0x00066401
		public QuestStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x00068209 File Offset: 0x00066409
		internal void Setup(QuestGiverTabs questGiverTabs)
		{
			this.master = questGiverTabs;
			this.Refresh();
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x00068218 File Offset: 0x00066418
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00068236 File Offset: 0x00066436
		private void OnClick()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00068254 File Offset: 0x00066454
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00068277 File Offset: 0x00066477
		internal void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x04001423 RID: 5155
		[SerializeField]
		private Button button;

		// Token: 0x04001424 RID: 5156
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001425 RID: 5157
		[SerializeField]
		private QuestStatus status;

		// Token: 0x04001426 RID: 5158
		private QuestGiverTabs master;
	}
}
