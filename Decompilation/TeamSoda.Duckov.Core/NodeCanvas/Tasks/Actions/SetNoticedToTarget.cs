using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000415 RID: 1045
	public class SetNoticedToTarget : ActionTask<AICharacterController>
	{
		// Token: 0x060025A7 RID: 9639 RVA: 0x00081CC5 File Offset: 0x0007FEC5
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x00081CC8 File Offset: 0x0007FEC8
		protected override string info
		{
			get
			{
				return "set noticed to";
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00081CCF File Offset: 0x0007FECF
		protected override void OnExecute()
		{
			base.agent.SetNoticedToTarget(this.target.value);
			base.EndAction(true);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00081CEE File Offset: 0x0007FEEE
		protected override void OnStop()
		{
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00081CF0 File Offset: 0x0007FEF0
		protected override void OnPause()
		{
		}

		// Token: 0x040019A1 RID: 6561
		public BBParameter<DamageReceiver> target;
	}
}
