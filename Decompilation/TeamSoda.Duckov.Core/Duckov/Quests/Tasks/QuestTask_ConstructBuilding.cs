using System;
using Duckov.Buildings;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000350 RID: 848
	public class QuestTask_ConstructBuilding : Task
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x0006922A File Offset: 0x0006742A
		[LocalizationKey("Default")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_ConstructBuilding";
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x00069231 File Offset: 0x00067431
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x0006923E File Offset: 0x0006743E
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					BuildingName = Building.GetDisplayName(this.buildingID)
				});
			}
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0006925B File Offset: 0x0006745B
		public override object GenerateSaveData()
		{
			return null;
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0006925E File Offset: 0x0006745E
		protected override bool CheckFinished()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x0006926C File Offset: 0x0006746C
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x0400146A RID: 5226
		[SerializeField]
		private string buildingID;
	}
}
