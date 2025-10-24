using System;
using ItemStatsSystem;

// Token: 0x020000F5 RID: 245
public class ItemSetting_Skill : ItemSettingBase
{
	// Token: 0x06000803 RID: 2051 RVA: 0x00023A70 File Offset: 0x00021C70
	public override void OnInit()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Combine(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
			this.Skill.fromItem = base.Item;
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00023AC4 File Offset: 0x00021CC4
	private void OnSkillReleased()
	{
		ItemSetting_Skill.OnReleaseAction onReleaseAction = this.onRelease;
		if (onReleaseAction != ItemSetting_Skill.OnReleaseAction.none && onReleaseAction == ItemSetting_Skill.OnReleaseAction.reduceCount && (!LevelManager.Instance || !LevelManager.Instance.IsBaseLevel))
		{
			if (base.Item.Stackable)
			{
				base.Item.StackCount--;
				return;
			}
			base.Item.Detach();
			base.Item.DestroyTree();
		}
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00023B2E File Offset: 0x00021D2E
	private void OnDestroy()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Remove(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00023B64 File Offset: 0x00021D64
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsSkill", true, true);
	}

	// Token: 0x0400076A RID: 1898
	public ItemSetting_Skill.OnReleaseAction onRelease;

	// Token: 0x0400076B RID: 1899
	public SkillBase Skill;

	// Token: 0x0200046E RID: 1134
	public enum OnReleaseAction
	{
		// Token: 0x04001B53 RID: 6995
		none,
		// Token: 0x04001B54 RID: 6996
		reduceCount
	}
}
