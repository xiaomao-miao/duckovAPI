using System;
using Duckov.MiniGames.SnakeForces;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000343 RID: 835
	public class QuestTask_Snake_Score : Task
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000675BD File Offset: 0x000657BD
		// (set) Token: 0x06001CC3 RID: 7363 RVA: 0x000675C4 File Offset: 0x000657C4
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_Snake_Score";
			}
			set
			{
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x000675C6 File Offset: 0x000657C6
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText().Format(new
				{
					score = this.targetScore
				});
			}
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000675E3 File Offset: 0x000657E3
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000675F0 File Offset: 0x000657F0
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000675F2 File Offset: 0x000657F2
		protected override bool CheckFinished()
		{
			return SnakeForce.HighScore >= this.targetScore;
		}

		// Token: 0x040013F6 RID: 5110
		[SerializeField]
		private int targetScore;

		// Token: 0x040013F7 RID: 5111
		private bool finished;
	}
}
