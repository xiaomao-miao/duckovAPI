using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034C RID: 844
	public class QuestViewFlagButton : MonoBehaviour
	{
		// Token: 0x06001D40 RID: 7488 RVA: 0x00068C57 File Offset: 0x00066E57
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.master.onShowingContentChanged += this.OnMasterShowingContentChanged;
			this.Refresh();
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x00068C92 File Offset: 0x00066E92
		private void OnButtonClicked()
		{
			this.master.SetShowingContent(this.content);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00068CA5 File Offset: 0x00066EA5
		private void OnMasterShowingContentChanged(QuestView view, QuestView.ShowContent content)
		{
			this.Refresh();
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00068CB0 File Offset: 0x00066EB0
		private void Refresh()
		{
			bool active = this.master.ShowingContentType == this.content;
			this.selectionIndicator.SetActive(active);
		}

		// Token: 0x0400144C RID: 5196
		[SerializeField]
		private QuestView master;

		// Token: 0x0400144D RID: 5197
		[SerializeField]
		private Button button;

		// Token: 0x0400144E RID: 5198
		[SerializeField]
		private QuestView.ShowContent content;

		// Token: 0x0400144F RID: 5199
		[SerializeField]
		private GameObject selectionIndicator;
	}
}
