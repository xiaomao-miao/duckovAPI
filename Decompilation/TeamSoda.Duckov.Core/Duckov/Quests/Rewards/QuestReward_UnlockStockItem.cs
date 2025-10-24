using System;
using Duckov.Economy;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x0200035B RID: 859
	public class QuestReward_UnlockStockItem : Reward
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0006A647 File Offset: 0x00068847
		public int UnlockItem
		{
			get
			{
				return this.unlockItem;
			}
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0006A64F File Offset: 0x0006884F
		private ItemMetaData GetItemMeta()
		{
			return ItemAssetsCollection.GetMetaData(this.unlockItem);
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0006A65C File Offset: 0x0006885C
		public override Sprite Icon
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).icon;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0006A66E File Offset: 0x0006886E
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_UnlockStockItem";
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0006A675 File Offset: 0x00068875
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0006A684 File Offset: 0x00068884
		private string ItemDisplayName
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).DisplayName;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0006A6A4 File Offset: 0x000688A4
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

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0006A6BC File Offset: 0x000688BC
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0006A6C4 File Offset: 0x000688C4
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0006A6C7 File Offset: 0x000688C7
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0006A6D4 File Offset: 0x000688D4
		public override void OnClaim()
		{
			EconomyManager.Unlock(this.unlockItem, true, true);
			this.claimed = true;
			base.ReportStatusChanged();
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0006A6F0 File Offset: 0x000688F0
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0006A713 File Offset: 0x00068913
		public override void NotifyReload(Quest questInstance)
		{
			if (questInstance.Complete)
			{
				EconomyManager.Unlock(this.unlockItem, true, true);
			}
		}

		// Token: 0x04001499 RID: 5273
		[SerializeField]
		[ItemTypeID]
		private int unlockItem;

		// Token: 0x0400149A RID: 5274
		private bool claimed;
	}
}
