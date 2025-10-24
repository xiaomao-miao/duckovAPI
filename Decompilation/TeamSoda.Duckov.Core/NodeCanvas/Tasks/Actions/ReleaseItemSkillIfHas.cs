using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000410 RID: 1040
	public class ReleaseItemSkillIfHas : ActionTask<AICharacterController>
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x0008142A File Offset: 0x0007F62A
		private float chance
		{
			get
			{
				if (!base.agent)
				{
					return 0f;
				}
				return base.agent.itemSkillChance;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x0008144A File Offset: 0x0007F64A
		public float checkTimeSpace
		{
			get
			{
				if (!base.agent)
				{
					return 999f;
				}
				return base.agent.itemSkillCoolTime;
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0008146A File Offset: 0x0007F66A
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00081470 File Offset: 0x0007F670
		protected override void OnExecute()
		{
			this.skillRefrence = null;
			if (Time.time - this.checkTimeMarker < this.checkTimeSpace)
			{
				base.EndAction(false);
				return;
			}
			this.checkTimeMarker = Time.time;
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				base.EndAction(false);
				return;
			}
			ItemSetting_Skill itemSkill = base.agent.GetItemSkill(this.random);
			if (!itemSkill)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.CharacterMainControl.CurrentAction && base.agent.CharacterMainControl.CurrentAction.Running)
			{
				base.EndAction(false);
				return;
			}
			this.skillRefrence = itemSkill;
			base.agent.CharacterMainControl.ChangeHoldItem(itemSkill.Item);
			base.agent.CharacterMainControl.SetSkill(SkillTypes.itemSkill, itemSkill.Skill, itemSkill.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.itemSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = itemSkill.Skill.SkillContext.skillReadyTime;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00081590 File Offset: 0x0007F790
		protected override void OnUpdate()
		{
			if (!this.skillRefrence)
			{
				base.EndAction(false);
				return;
			}
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime > this.readyTime + 0.1f)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.itemSkill);
				base.EndAction(true);
				return;
			}
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00081617 File Offset: 0x0007F817
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x04001983 RID: 6531
		public bool random = true;

		// Token: 0x04001984 RID: 6532
		private float checkTimeMarker = -1f;

		// Token: 0x04001985 RID: 6533
		private float readyTime;

		// Token: 0x04001986 RID: 6534
		private ItemSetting_Skill skillRefrence;
	}
}
