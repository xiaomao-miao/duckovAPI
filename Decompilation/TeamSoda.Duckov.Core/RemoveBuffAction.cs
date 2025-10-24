using System;
using ItemStatsSystem;

// Token: 0x02000086 RID: 134
public class RemoveBuffAction : EffectAction
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x00015DBA File Offset: 0x00013FBA
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

	// Token: 0x060004CF RID: 1231 RVA: 0x00015DD8 File Offset: 0x00013FD8
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.RemoveBuff(this.buffID, this.removeOneLayer);
	}

	// Token: 0x0400040B RID: 1035
	public int buffID;

	// Token: 0x0400040C RID: 1036
	public bool removeOneLayer;
}
