using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000354 RID: 852
	public class QuestTask_SubmitMoney : Task
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x00069A66 File Offset: 0x00067C66
		public string DescriptionFormat
		{
			get
			{
				return this.decriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x00069A73 File Offset: 0x00067C73
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.money
				});
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x00069A8B File Offset: 0x00067C8B
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x00069A8E File Offset: 0x00067C8E
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckMoneyEnough();
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x00069A96 File Offset: 0x00067C96
		public override string InteractText
		{
			get
			{
				return this.interactTextKey.ToPlainText();
			}
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x00069AA4 File Offset: 0x00067CA4
		public override void Interact()
		{
			Cost cost = new Cost((long)this.money);
			if (cost.Pay(true, true))
			{
				this.submitted = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00069AD7 File Offset: 0x00067CD7
		private bool CheckMoneyEnough()
		{
			return EconomyManager.Money >= (long)this.money;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x00069AEA File Offset: 0x00067CEA
		public override object GenerateSaveData()
		{
			return this.submitted;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00069AF8 File Offset: 0x00067CF8
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.submitted = flag;
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00069B1B File Offset: 0x00067D1B
		protected override bool CheckFinished()
		{
			return this.submitted;
		}

		// Token: 0x0400147D RID: 5245
		[SerializeField]
		private int money;

		// Token: 0x0400147E RID: 5246
		[SerializeField]
		[LocalizationKey("Default")]
		private string decriptionFormatKey = "QuestTask_SubmitMoney";

		// Token: 0x0400147F RID: 5247
		[SerializeField]
		[LocalizationKey("Default")]
		private string interactTextKey = "QuestTask_SubmitMoney_Interact";

		// Token: 0x04001480 RID: 5248
		private bool submitted;
	}
}
