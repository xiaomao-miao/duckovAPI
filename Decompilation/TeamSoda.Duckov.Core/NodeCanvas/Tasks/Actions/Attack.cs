using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000408 RID: 1032
	public class Attack : ActionTask<AICharacterController>
	{
		// Token: 0x06002553 RID: 9555 RVA: 0x00080A6A File Offset: 0x0007EC6A
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x00080A6D File Offset: 0x0007EC6D
		protected override string info
		{
			get
			{
				return string.Format("Attack", Array.Empty<object>());
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00080A7E File Offset: 0x0007EC7E
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.Attack();
			base.EndAction(true);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x00080A98 File Offset: 0x0007EC98
		protected override void OnStop()
		{
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00080A9A File Offset: 0x0007EC9A
		protected override void OnPause()
		{
		}
	}
}
