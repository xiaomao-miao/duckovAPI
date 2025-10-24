using System;
using ItemStatsSystem;

// Token: 0x020000ED RID: 237
public class ItemSetting_Bullet : ItemSettingBase
{
	// Token: 0x060007D7 RID: 2007 RVA: 0x0002314E File Offset: 0x0002134E
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsBullet", true, true);
	}
}
