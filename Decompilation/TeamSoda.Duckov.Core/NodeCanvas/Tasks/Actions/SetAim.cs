using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000414 RID: 1044
	public class SetAim : ActionTask<AICharacterController>
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x00081BB8 File Offset: 0x0007FDB8
		protected override string info
		{
			get
			{
				if (this.useTransfom && string.IsNullOrEmpty(this.aimTarget.name))
				{
					return "Set aim to null";
				}
				if (!this.useTransfom)
				{
					return "Set aim to " + this.aimPos.name;
				}
				return "Set aim to " + this.aimTarget.name;
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00081C18 File Offset: 0x0007FE18
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00081C1C File Offset: 0x0007FE1C
		protected override void OnExecute()
		{
			base.agent.SetTarget(this.aimTarget.value);
			if (!this.useTransfom || !(this.aimTarget.value != null))
			{
				if (!this.useTransfom)
				{
					base.agent.SetAimInput((this.aimPos.value - base.agent.transform.position).normalized, AimTypes.normalAim);
				}
				else
				{
					base.agent.SetAimInput(Vector3.zero, AimTypes.normalAim);
				}
			}
			base.EndAction(true);
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00081CB0 File Offset: 0x0007FEB0
		protected override void OnUpdate()
		{
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00081CB2 File Offset: 0x0007FEB2
		protected override void OnStop()
		{
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00081CB4 File Offset: 0x0007FEB4
		protected override void OnPause()
		{
		}

		// Token: 0x0400199D RID: 6557
		public bool useTransfom = true;

		// Token: 0x0400199E RID: 6558
		[ShowIf("useTransfom", 1)]
		public BBParameter<Transform> aimTarget;

		// Token: 0x0400199F RID: 6559
		[ShowIf("useTransfom", 0)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x040019A0 RID: 6560
		private bool waitingSearchResult;
	}
}
