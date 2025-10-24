using System;
using Duckov.UI;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033D RID: 829
	public class PlayDialogueGraphOnQuestActive : MonoBehaviour
	{
		// Token: 0x06001C91 RID: 7313 RVA: 0x00066DCE File Offset: 0x00064FCE
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00066E01 File Offset: 0x00065001
		private void OnQuestActivated(Quest quest)
		{
			if (View.ActiveView != null)
			{
				View.ActiveView.Close();
			}
			this.SetupActors();
			this.PlayDialogue();
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00066E26 File Offset: 0x00065026
		private void PlayDialogue()
		{
			this.dialogueTreeController.StartDialogue();
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00066E34 File Offset: 0x00065034
		private void SetupActors()
		{
			if (this.dialogueTreeController.behaviour == null)
			{
				Debug.LogError("Dialoguetree没有配置", this.dialogueTreeController);
				return;
			}
			foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeController.behaviour.actorParameters)
			{
				string name = actorParameter.name;
				if (!string.IsNullOrEmpty(name))
				{
					DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
					if (duckovDialogueActor == null)
					{
						Debug.LogError("未找到actor ID:" + name);
					}
					else
					{
						this.dialogueTreeController.SetActorReference(name, duckovDialogueActor);
					}
				}
			}
		}

		// Token: 0x040013E2 RID: 5090
		[SerializeField]
		private Quest quest;

		// Token: 0x040013E3 RID: 5091
		[SerializeField]
		private DialogueTreeController dialogueTreeController;
	}
}
