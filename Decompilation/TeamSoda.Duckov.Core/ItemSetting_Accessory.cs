using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class ItemSetting_Accessory : ItemSettingBase
{
	// Token: 0x060007CE RID: 1998 RVA: 0x00022F93 File Offset: 0x00021193
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsBullet", true, true);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00022FA2 File Offset: 0x000211A2
	public override void OnInit()
	{
		base.Item.onPluggedIntoSlot += this.OnPluggedIntoSlot;
		base.Item.onUnpluggedFromSlot += this.OnUnpluggedIntoSlot;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00022FD4 File Offset: 0x000211D4
	private void OnPluggedIntoSlot(Item selfItem)
	{
		Slot pluggedIntoSlot = selfItem.PluggedIntoSlot;
		if (pluggedIntoSlot == null)
		{
			return;
		}
		this.masterItem = pluggedIntoSlot.Master;
		if (!this.masterItem)
		{
			return;
		}
		this.masterItem.AgentUtilities.onCreateAgent += this.OnMasterCreateAgent;
		this.CreateAccessory(this.masterItem.AgentUtilities.ActiveAgent as DuckovItemAgent);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002303D File Offset: 0x0002123D
	private void OnUnpluggedIntoSlot(Item selfItem)
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002306E File Offset: 0x0002126E
	private void OnDestroy()
	{
		if (this.masterItem)
		{
			this.masterItem.AgentUtilities.onCreateAgent -= this.OnMasterCreateAgent;
		}
		this.DestroyAccessory();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002309F File Offset: 0x0002129F
	private void OnMasterCreateAgent(Item _masterItem, ItemAgent newAgnet)
	{
		if (this.masterItem != _masterItem)
		{
			Debug.LogError("缓存了错误的Item");
		}
		if (newAgnet.AgentType != ItemAgent.AgentTypes.handheld)
		{
			return;
		}
		this.CreateAccessory(newAgnet as DuckovItemAgent);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000230D0 File Offset: 0x000212D0
	private void CreateAccessory(DuckovItemAgent parentAgent)
	{
		this.DestroyAccessory();
		if (this.accessoryPfb == null || parentAgent == null || parentAgent.AgentType != ItemAgent.AgentTypes.handheld)
		{
			return;
		}
		this.accessoryInstance = UnityEngine.Object.Instantiate<AccessoryBase>(this.accessoryPfb);
		this.accessoryInstance.Init(parentAgent, base.Item);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00023127 File Offset: 0x00021327
	private void DestroyAccessory()
	{
		if (this.accessoryInstance)
		{
			UnityEngine.Object.Destroy(this.accessoryInstance.gameObject);
		}
	}

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	private AccessoryBase accessoryPfb;

	// Token: 0x04000747 RID: 1863
	public ADSAimMarker overrideAdsAimMarker;

	// Token: 0x04000748 RID: 1864
	private AccessoryBase accessoryInstance;

	// Token: 0x04000749 RID: 1865
	private bool created;

	// Token: 0x0400074A RID: 1866
	private Item masterItem;
}
