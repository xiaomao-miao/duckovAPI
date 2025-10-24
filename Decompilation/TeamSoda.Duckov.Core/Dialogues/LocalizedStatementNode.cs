using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x0200021E RID: 542
	public class LocalizedStatementNode : DTNode
	{
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0003F361 File Offset: 0x0003D561
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0003F364 File Offset: 0x0003D564
		private string Key
		{
			get
			{
				if (this.useSequence.value)
				{
					return string.Format("{0}_{1}", this.key.value, this.sequenceIndex.value);
				}
				return this.key.value;
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0003F3A4 File Offset: 0x0003D5A4
		private LocalizedStatement CreateStatement()
		{
			return new LocalizedStatement(this.Key);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0003F3B4 File Offset: 0x0003D5B4
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			LocalizedStatement statement = this.CreateStatement();
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, new Action(this.OnStatementFinish)));
			return Status.Running;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003F3E6 File Offset: 0x0003D5E6
		private void OnStatementFinish()
		{
			base.status = Status.Success;
			base.DLGTree.Continue(0);
		}

		// Token: 0x04000CFC RID: 3324
		public BBParameter<string> key;

		// Token: 0x04000CFD RID: 3325
		public BBParameter<bool> useSequence;

		// Token: 0x04000CFE RID: 3326
		public BBParameter<int> sequenceIndex;
	}
}
