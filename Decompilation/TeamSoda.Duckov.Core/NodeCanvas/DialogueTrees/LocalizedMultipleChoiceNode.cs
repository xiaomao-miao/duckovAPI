using System;
using System.Collections.Generic;
using Dialogues;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020003FE RID: 1022
	[ParadoxNotion.Design.Icon("List", false, "")]
	[Name("Multiple Choice Localized", 0)]
	[Category("Branch")]
	[Description("Prompt a Dialogue Multiple Choice. A choice will be available if the choice condition(s) are true or there is no choice conditions. The Actor selected is used for the condition checks and will also Say the selection if the option is checked.")]
	[Color("b3ff7f")]
	public class LocalizedMultipleChoiceNode : DTNode
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x000804F6 File Offset: 0x0007E6F6
		public override int maxOutConnections
		{
			get
			{
				return this.availableChoices.Count;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x00080503 File Offset: 0x0007E703
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00080508 File Offset: 0x0007E708
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (base.outConnections.Count == 0)
			{
				return base.Error("There are no connections to the Multiple Choice Node!");
			}
			Dictionary<IStatement, int> dictionary = new Dictionary<IStatement, int>();
			for (int i = 0; i < this.availableChoices.Count; i++)
			{
				ConditionTask condition = this.availableChoices[i].condition;
				if (condition == null || condition.CheckOnce(base.finalActor.transform, bb))
				{
					LocalizedStatement statement = this.availableChoices[i].statement;
					dictionary[statement] = i;
				}
			}
			if (dictionary.Count == 0)
			{
				base.DLGTree.Stop(false);
				return Status.Failure;
			}
			DialogueTree.RequestMultipleChoices(new MultipleChoiceRequestInfo(base.finalActor, dictionary, this.availableTime, new Action<int>(this.OnOptionSelected))
			{
				showLastStatement = true
			});
			return Status.Running;
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000805D0 File Offset: 0x0007E7D0
		private void OnOptionSelected(int index)
		{
			base.status = Status.Success;
			Action action = delegate()
			{
				this.DLGTree.Continue(index);
			};
			if (this.saySelection)
			{
				LocalizedStatement statement = this.availableChoices[index].statement;
				DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, action));
				return;
			}
			action();
		}

		// Token: 0x04001950 RID: 6480
		[SliderField(0f, 10f)]
		public float availableTime;

		// Token: 0x04001951 RID: 6481
		public bool saySelection;

		// Token: 0x04001952 RID: 6482
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<LocalizedMultipleChoiceNode.Choice> availableChoices = new List<LocalizedMultipleChoiceNode.Choice>();

		// Token: 0x0200066E RID: 1646
		[Serializable]
		public class Choice
		{
			// Token: 0x06002AA8 RID: 10920 RVA: 0x000A14D4 File Offset: 0x0009F6D4
			public Choice()
			{
			}

			// Token: 0x06002AA9 RID: 10921 RVA: 0x000A14E3 File Offset: 0x0009F6E3
			public Choice(LocalizedStatement statement)
			{
				this.statement = statement;
			}

			// Token: 0x04002326 RID: 8998
			public bool isUnfolded = true;

			// Token: 0x04002327 RID: 8999
			public LocalizedStatement statement;

			// Token: 0x04002328 RID: 9000
			public ConditionTask condition;
		}
	}
}
