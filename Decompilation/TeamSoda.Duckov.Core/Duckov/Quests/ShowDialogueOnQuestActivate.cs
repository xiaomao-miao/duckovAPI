using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033E RID: 830
	public class ShowDialogueOnQuestActivate : MonoBehaviour
	{
		// Token: 0x06001C96 RID: 7318 RVA: 0x00066EF4 File Offset: 0x000650F4
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x00066F27 File Offset: 0x00065127
		private void OnQuestActivated(Quest quest)
		{
			this.ShowDIalogue().Forget();
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00066F34 File Offset: 0x00065134
		private UniTask ShowDIalogue()
		{
			ShowDialogueOnQuestActivate.<ShowDIalogue>d__6 <ShowDIalogue>d__;
			<ShowDIalogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDIalogue>d__.<>4__this = this;
			<ShowDIalogue>d__.<>1__state = -1;
			<ShowDIalogue>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDIalogue>d__6>(ref <ShowDIalogue>d__);
			return <ShowDIalogue>d__.<>t__builder.Task;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x00066F78 File Offset: 0x00065178
		private UniTask ShowDialogueEntry(ShowDialogueOnQuestActivate.DialogueEntry cur)
		{
			ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7 <ShowDialogueEntry>d__;
			<ShowDialogueEntry>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDialogueEntry>d__.<>4__this = this;
			<ShowDialogueEntry>d__.cur = cur;
			<ShowDialogueEntry>d__.<>1__state = -1;
			<ShowDialogueEntry>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7>(ref <ShowDialogueEntry>d__);
			return <ShowDialogueEntry>d__.<>t__builder.Task;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x00066FC4 File Offset: 0x000651C4
		private Transform GetQuestGiverTransform(Quest quest)
		{
			QuestGiverID id = quest.QuestGiverID;
			QuestGiver questGiver = UnityEngine.Object.FindObjectsByType<QuestGiver>(FindObjectsSortMode.None).FirstOrDefault((QuestGiver e) => e != null && e.ID == id);
			if (questGiver == null)
			{
				return null;
			}
			return questGiver.transform;
		}

		// Token: 0x040013E4 RID: 5092
		[SerializeField]
		private Quest quest;

		// Token: 0x040013E5 RID: 5093
		[SerializeField]
		private List<ShowDialogueOnQuestActivate.DialogueEntry> dialogueEntries;

		// Token: 0x040013E6 RID: 5094
		private Transform cachedQuestGiverTransform;

		// Token: 0x020005F9 RID: 1529
		[Serializable]
		public class DialogueEntry
		{
			// Token: 0x04002120 RID: 8480
			[TextArea]
			public string content;
		}
	}
}
