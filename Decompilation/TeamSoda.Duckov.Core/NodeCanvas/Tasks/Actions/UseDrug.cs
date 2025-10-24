using System;
using ItemStatsSystem;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041C RID: 1052
	public class UseDrug : ActionTask<AICharacterController>
	{
		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x000823A9 File Offset: 0x000805A9
		protected override string info
		{
			get
			{
				if (!this.stopMove)
				{
					return "打药";
				}
				return "原地打药";
			}
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000823BE File Offset: 0x000805BE
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000823C4 File Offset: 0x000805C4
		protected override void OnExecute()
		{
			Item drugItem = base.agent.GetDrugItem();
			if (drugItem == null)
			{
				base.EndAction(false);
				return;
			}
			base.agent.CharacterMainControl.UseItem(drugItem);
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00082400 File Offset: 0x00080600
		protected override void OnUpdate()
		{
			if (this.stopMove && base.agent.IsMoving())
			{
				base.agent.StopMove();
			}
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(false);
				return;
			}
			if (!base.agent.CharacterMainControl.useItemAction.Running)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00082472 File Offset: 0x00080672
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x040019B7 RID: 6583
		public bool stopMove;
	}
}
