using System;
using Duckov.Buffs;
using ItemStatsSystem;

// Token: 0x02000081 RID: 129
public class AddBuffAction : EffectAction
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060004BD RID: 1213 RVA: 0x00015A02 File Offset: 0x00013C02
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

	// Token: 0x060004BE RID: 1214 RVA: 0x00015A20 File Offset: 0x00013C20
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.AddBuff(this.buffPfb, this.MainControl, 0);
	}

	// Token: 0x040003FB RID: 1019
	public Buff buffPfb;
}
