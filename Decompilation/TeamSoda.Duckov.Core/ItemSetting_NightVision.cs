using System;
using ItemStatsSystem;

// Token: 0x020000F4 RID: 244
public class ItemSetting_NightVision : ItemSettingBase
{
	// Token: 0x060007FC RID: 2044 RVA: 0x000239B7 File Offset: 0x00021BB7
	public override void OnInit()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot += this.OnplugedIntoSlot;
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000239DD File Offset: 0x00021BDD
	private void OnplugedIntoSlot(Item item)
	{
		this.nightVisionOn = true;
		this.SyncModifiers();
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x000239EC File Offset: 0x00021BEC
	private void OnDestroy()
	{
		if (this._item)
		{
			this._item.onPluggedIntoSlot -= this.OnplugedIntoSlot;
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00023A12 File Offset: 0x00021C12
	public void ToggleNightVison()
	{
		this.nightVisionOn = !this.nightVisionOn;
		this.SyncModifiers();
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00023A29 File Offset: 0x00021C29
	private void SyncModifiers()
	{
		if (!this._item)
		{
			return;
		}
		this._item.Modifiers.ModifierEnable = this.nightVisionOn;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00023A4F File Offset: 0x00021C4F
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsNightVision", true, true);
	}

	// Token: 0x04000769 RID: 1897
	private bool nightVisionOn = true;
}
