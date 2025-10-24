using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000419 RID: 1049
	public class StopMoving : ActionTask<AICharacterController>
	{
		// Token: 0x060025C1 RID: 9665 RVA: 0x00081F9B File Offset: 0x0008019B
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x00081F9E File Offset: 0x0008019E
		protected override void OnExecute()
		{
			base.agent.StopMove();
			base.EndAction(true);
		}
	}
}
