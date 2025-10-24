using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200034F RID: 847
	public class QuestTask_CheckSaveData : Task
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001D5E RID: 7518 RVA: 0x000691D1 File Offset: 0x000673D1
		public string SaveDataKey
		{
			get
			{
				return this.saveDataKey;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x000691D9 File Offset: 0x000673D9
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x000691E6 File Offset: 0x000673E6
		private bool SaveDataTrue
		{
			get
			{
				return SavesSystem.Load<bool>(this.saveDataKey);
			}
			set
			{
				SavesSystem.Save<bool>(this.saveDataKey, value);
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x000691F4 File Offset: 0x000673F4
		public override string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00069201 File Offset: 0x00067401
		protected override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x00069209 File Offset: 0x00067409
		private void OnDisable()
		{
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0006920B File Offset: 0x0006740B
		protected override bool CheckFinished()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x00069213 File Offset: 0x00067413
		public override object GenerateSaveData()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00069220 File Offset: 0x00067420
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x04001468 RID: 5224
		[SerializeField]
		private string saveDataKey;

		// Token: 0x04001469 RID: 5225
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;
	}
}
