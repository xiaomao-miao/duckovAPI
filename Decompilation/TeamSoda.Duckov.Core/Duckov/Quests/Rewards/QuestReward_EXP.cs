using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000359 RID: 857
	public class QuestReward_EXP : Reward
	{
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x0006A518 File Offset: 0x00068718
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0006A520 File Offset: 0x00068720
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0006A528 File Offset: 0x00068728
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Exp";
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0006A52F File Offset: 0x0006872F
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0006A53C File Offset: 0x0006873C
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

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x0006A554 File Offset: 0x00068754
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0006A557 File Offset: 0x00068757
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x0006A564 File Offset: 0x00068764
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EXPManager.AddExp(this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0006A584 File Offset: 0x00068784
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x04001495 RID: 5269
		[SerializeField]
		private int amount;

		// Token: 0x04001496 RID: 5270
		[SerializeField]
		private bool claimed;
	}
}
