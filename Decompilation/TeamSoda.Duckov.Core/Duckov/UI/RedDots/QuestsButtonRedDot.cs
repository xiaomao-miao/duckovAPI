using System;
using Duckov.Quests;
using UnityEngine;

namespace Duckov.UI.RedDots
{
	// Token: 0x020003E7 RID: 999
	public class QuestsButtonRedDot : MonoBehaviour
	{
		// Token: 0x06002423 RID: 9251 RVA: 0x0007DCF7 File Offset: 0x0007BEF7
		private void Awake()
		{
			Quest.onQuestNeedInspectionChanged += this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0007DD0A File Offset: 0x0007BF0A
		private void OnDestroy()
		{
			Quest.onQuestNeedInspectionChanged -= this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0007DD1D File Offset: 0x0007BF1D
		private void OnQuestNeedInspectionChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0007DD25 File Offset: 0x0007BF25
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0007DD2D File Offset: 0x0007BF2D
		private void Refresh()
		{
			this.dot.SetActive(QuestManager.AnyQuestNeedsInspection);
		}

		// Token: 0x04001889 RID: 6281
		public GameObject dot;
	}
}
