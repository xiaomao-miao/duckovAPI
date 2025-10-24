using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000403 RID: 1027
	public class CheckNoticed : ConditionTask<AICharacterController>
	{
		// Token: 0x06002540 RID: 9536 RVA: 0x000808A9 File Offset: 0x0007EAA9
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000808AC File Offset: 0x0007EAAC
		protected override void OnEnable()
		{
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000808AE File Offset: 0x0007EAAE
		protected override void OnDisable()
		{
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000808B0 File Offset: 0x0007EAB0
		protected override bool OnCheck()
		{
			bool result = base.agent.isNoticing(this.noticedTimeThreshold);
			if (this.resetNotice)
			{
				base.agent.noticed = false;
			}
			return result;
		}

		// Token: 0x04001959 RID: 6489
		public float noticedTimeThreshold = 0.2f;

		// Token: 0x0400195A RID: 6490
		public bool resetNotice;
	}
}
