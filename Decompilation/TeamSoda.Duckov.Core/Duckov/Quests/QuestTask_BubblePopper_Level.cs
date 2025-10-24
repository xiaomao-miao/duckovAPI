using System;
using Duckov.MiniGames.BubblePoppers;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000341 RID: 833
	public class QuestTask_BubblePopper_Level : Task
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x0006751D File Offset: 0x0006571D
		// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x00067524 File Offset: 0x00065724
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_BubblePopper_Level";
			}
			set
			{
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x00067526 File Offset: 0x00065726
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

		// Token: 0x06001CB7 RID: 7351 RVA: 0x00067543 File Offset: 0x00065743
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00067550 File Offset: 0x00065750
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00067552 File Offset: 0x00065752
		protected override bool CheckFinished()
		{
			return BubblePopper.HighLevel >= this.targetLevel;
		}

		// Token: 0x040013F2 RID: 5106
		[SerializeField]
		private int targetLevel;

		// Token: 0x040013F3 RID: 5107
		private bool finished;
	}
}
