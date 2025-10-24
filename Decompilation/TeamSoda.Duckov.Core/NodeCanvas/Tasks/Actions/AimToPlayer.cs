using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000407 RID: 1031
	public class AimToPlayer : ActionTask<AICharacterController>
	{
		// Token: 0x0600254F RID: 9551 RVA: 0x00080A23 File Offset: 0x0007EC23
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00080A26 File Offset: 0x0007EC26
		protected override void OnExecute()
		{
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x00080A28 File Offset: 0x0007EC28
		protected override void OnUpdate()
		{
			if (!this.target)
			{
				this.target = CharacterMainControl.Main;
			}
			base.agent.CharacterMainControl.SetAimPoint(this.target.transform.position);
		}

		// Token: 0x04001964 RID: 6500
		private CharacterMainControl target;
	}
}
