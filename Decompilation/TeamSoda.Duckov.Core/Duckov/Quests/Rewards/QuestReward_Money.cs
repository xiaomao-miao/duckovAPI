using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x0200035A RID: 858
	public class QuestReward_Money : Reward
	{
		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0006A5AF File Offset: 0x000687AF
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x0006A5B7 File Offset: 0x000687B7
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0006A5BF File Offset: 0x000687BF
		[SerializeField]
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Money";
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0006A5C6 File Offset: 0x000687C6
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x0006A5D3 File Offset: 0x000687D3
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0006A5D6 File Offset: 0x000687D6
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.amount
				});
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0006A5EE File Offset: 0x000687EE
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0006A5FB File Offset: 0x000687FB
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EconomyManager.Add((long)this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0006A61C File Offset: 0x0006881C
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x04001497 RID: 5271
		[Min(0f)]
		[SerializeField]
		private int amount;

		// Token: 0x04001498 RID: 5272
		[SerializeField]
		private bool claimed;
	}
}
