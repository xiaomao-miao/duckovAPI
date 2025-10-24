using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000411 RID: 1041
	public class ReleaseSkill : ActionTask<AICharacterController>
	{
		// Token: 0x0600258D RID: 9613 RVA: 0x00081655 File Offset: 0x0007F855
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00081658 File Offset: 0x0007F858
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetSkill(SkillTypes.characterSkill, base.agent.skillInstance, base.agent.skillInstance.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.characterSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = base.agent.skillInstance.SkillContext.skillReadyTime;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000816C8 File Offset: 0x0007F8C8
		protected override void OnUpdate()
		{
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime <= this.readyTime + 0.1f)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) < base.agent.skillSuccessChance)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.characterSkill);
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.CancleSkill();
			base.EndAction(false);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x0008176F File Offset: 0x0007F96F
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x04001987 RID: 6535
		private float readyTime;

		// Token: 0x04001988 RID: 6536
		private float tryReleaseSkillTimeMarker = -1f;
	}
}
