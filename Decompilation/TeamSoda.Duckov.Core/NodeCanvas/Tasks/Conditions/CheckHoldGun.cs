using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000401 RID: 1025
	public class CheckHoldGun : ConditionTask<AICharacterController>
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x000807FD File Offset: 0x0007E9FD
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00080800 File Offset: 0x0007EA00
		protected override void OnEnable()
		{
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00080802 File Offset: 0x0007EA02
		protected override void OnDisable()
		{
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00080804 File Offset: 0x0007EA04
		protected override bool OnCheck()
		{
			return base.agent.CharacterMainControl.GetGun() != null;
		}
	}
}
