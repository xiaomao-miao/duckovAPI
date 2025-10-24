using System;
using Duckov.Economy;
using Duckov.PerkTrees;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class UnlockStockShopItem : PerkBehaviour
{
	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00039905 File Offset: 0x00037B05
	private string DescriptionFormat
	{
		get
		{
			return "PerkBehaviour_UnlockStockShopItem".ToPlainText();
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00039911 File Offset: 0x00037B11
	public override string Description
	{
		get
		{
			return this.DescriptionFormat.Format(new
			{
				this.ItemDisplayName
			});
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0003992C File Offset: 0x00037B2C
	private string ItemDisplayName
	{
		get
		{
			return ItemAssetsCollection.GetMetaData(this.itemTypeID).DisplayName;
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0003994C File Offset: 0x00037B4C
	private void Start()
	{
		if (base.Master.Unlocked && !EconomyManager.IsUnlocked(this.itemTypeID))
		{
			EconomyManager.Unlock(this.itemTypeID, false, false);
		}
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x00039975 File Offset: 0x00037B75
	protected override void OnUnlocked()
	{
		base.OnUnlocked();
		EconomyManager.Unlock(this.itemTypeID, false, true);
	}

	// Token: 0x04000BCE RID: 3022
	[ItemTypeID]
	[SerializeField]
	private int itemTypeID;
}
