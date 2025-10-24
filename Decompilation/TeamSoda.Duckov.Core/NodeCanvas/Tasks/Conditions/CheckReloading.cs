using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000404 RID: 1028
	public class CheckReloading : ConditionTask<AICharacterController>
	{
		// Token: 0x06002545 RID: 9541 RVA: 0x000808EA File Offset: 0x0007EAEA
		protected override bool OnCheck()
		{
			return !(base.agent == null) && !(base.agent.CharacterMainControl == null) && base.agent.CharacterMainControl.reloadAction.Running;
		}
	}
}
