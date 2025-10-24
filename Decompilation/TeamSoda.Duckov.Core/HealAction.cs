using System;
using ItemStatsSystem;

// Token: 0x02000084 RID: 132
public class HealAction : EffectAction
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00015B96 File Offset: 0x00013D96
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00015BD0 File Offset: 0x00013DD0
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl)
		{
			return;
		}
		this.MainControl.Health.AddHealth((float)this.healValue);
	}

	// Token: 0x04000400 RID: 1024
	private CharacterMainControl _mainControl;

	// Token: 0x04000401 RID: 1025
	public int healValue = 10;
}
