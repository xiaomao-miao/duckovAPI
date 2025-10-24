using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x0200021F RID: 543
	public class LocalizedStatementSequence : DTNode
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0003F403 File Offset: 0x0003D603
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003F406 File Offset: 0x0003D606
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			this.Begin();
			return Status.Running;
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0003F40F File Offset: 0x0003D60F
		private void Begin()
		{
			this.index = this.beginIndex.value - 1;
			this.Next();
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0003F42C File Offset: 0x0003D62C
		private void Next()
		{
			this.index++;
			if (this.index > this.endIndex.value)
			{
				base.status = Status.Success;
				base.DLGTree.Continue(0);
				return;
			}
			LocalizedStatement statement = new LocalizedStatement(this.format.value.Format(new
			{
				keyPrefix = this.keyPrefix.value,
				index = this.index
			}));
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, new Action(this.OnStatementFinish)));
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0003F4B7 File Offset: 0x0003D6B7
		private void OnStatementFinish()
		{
			this.Next();
		}

		// Token: 0x04000CFF RID: 3327
		public BBParameter<string> keyPrefix;

		// Token: 0x04000D00 RID: 3328
		public BBParameter<int> beginIndex;

		// Token: 0x04000D01 RID: 3329
		public BBParameter<int> endIndex;

		// Token: 0x04000D02 RID: 3330
		public BBParameter<string> format = new BBParameter<string>("{keyPrefix}_{index}");

		// Token: 0x04000D03 RID: 3331
		private int index;
	}
}
