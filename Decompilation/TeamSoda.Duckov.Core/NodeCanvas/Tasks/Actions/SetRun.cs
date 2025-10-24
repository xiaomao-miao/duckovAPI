using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000416 RID: 1046
	public class SetRun : ActionTask<AICharacterController>
	{
		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x00081CFA File Offset: 0x0007FEFA
		protected override string info
		{
			get
			{
				return string.Format("SetRun:{0}", this.run.value);
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x00081D16 File Offset: 0x0007FF16
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00081D19 File Offset: 0x0007FF19
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetRunInput(this.run.value);
			base.EndAction(true);
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00081D3D File Offset: 0x0007FF3D
		protected override void OnStop()
		{
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00081D3F File Offset: 0x0007FF3F
		protected override void OnPause()
		{
		}

		// Token: 0x040019A2 RID: 6562
		public BBParameter<bool> run;
	}
}
