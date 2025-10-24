using System;
using Duckov.MiniGames.GoldMiner;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000342 RID: 834
	public class QuestTask_GoldMiner_Level : Task
	{
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x0006756C File Offset: 0x0006576C
		// (set) Token: 0x06001CBC RID: 7356 RVA: 0x00067573 File Offset: 0x00065773
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_GoldMiner_Level";
			}
			set
			{
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x00067575 File Offset: 0x00065775
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText().Format(new
				{
					level = this.targetLevel
				});
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x00067592 File Offset: 0x00065792
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0006759F File Offset: 0x0006579F
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000675A1 File Offset: 0x000657A1
		protected override bool CheckFinished()
		{
			return GoldMiner.HighLevel + 1 >= this.targetLevel;
		}

		// Token: 0x040013F4 RID: 5108
		[SerializeField]
		private int targetLevel;

		// Token: 0x040013F5 RID: 5109
		private bool finished;
	}
}
