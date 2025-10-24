using System;
using Duckvo.Beacons;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033B RID: 827
	public class QuestTask_UnlockBeacon : Task
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x00066AF7 File Offset: 0x00064CF7
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x00066B09 File Offset: 0x00064D09
		[LocalizationKey("Default")]
		private string DescriptionKey
		{
			get
			{
				return "Task_Beacon_" + this.beaconID;
			}
			set
			{
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00066B0B File Offset: 0x00064D0B
		public override string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00066B18 File Offset: 0x00064D18
		public override object GenerateSaveData()
		{
			return 0;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00066B20 File Offset: 0x00064D20
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00066B22 File Offset: 0x00064D22
		protected override bool CheckFinished()
		{
			return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
		}

		// Token: 0x040013D7 RID: 5079
		[SerializeField]
		private string beaconID;

		// Token: 0x040013D8 RID: 5080
		[SerializeField]
		private int beaconIndex;
	}
}
