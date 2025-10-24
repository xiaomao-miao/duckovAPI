using System;
using ItemStatsSystem;

// Token: 0x020000F3 RID: 243
public class ItemSetting_MeleeWeapon : ItemSettingBase
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00023998 File Offset: 0x00021B98
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x000239A0 File Offset: 0x00021BA0
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsMeleeWeapon", true, true);
	}

	// Token: 0x04000768 RID: 1896
	public bool dealExplosionDamage;
}
