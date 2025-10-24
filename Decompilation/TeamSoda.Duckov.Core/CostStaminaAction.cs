using System;
using ItemStatsSystem;

// Token: 0x02000082 RID: 130
public class CostStaminaAction : EffectAction
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00015A50 File Offset: 0x00013C50
	private CharacterMainControl MainControl
	{
		get
		{
			Effect master = base.Master;
			if (master == null)
			{
				return null;
			}
			Item item = master.Item;
			if (item == null)
			{
				return null;
			}
			return item.GetCharacterMainControl();
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00015A6E File Offset: 0x00013C6E
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.UseStamina(this.staminaCost);
	}

	// Token: 0x040003FC RID: 1020
	public float staminaCost;
}
