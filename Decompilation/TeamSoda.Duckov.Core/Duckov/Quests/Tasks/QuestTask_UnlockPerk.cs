using System;
using System.Collections.Generic;
using Duckov.PerkTrees;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000356 RID: 854
	public class QuestTask_UnlockPerk : Task
	{
		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x00069C46 File Offset: 0x00067E46
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x00069C53 File Offset: 0x00067E53
		private string PerkDisplayName
		{
			get
			{
				if (this.perk == null)
				{
					this.BindPerk();
				}
				if (this.perk == null)
				{
					return this.perkObjectName.ToPlainText();
				}
				return this.perk.DisplayName;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x00069C8F File Offset: 0x00067E8F
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.PerkDisplayName
				});
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x00069CA7 File Offset: 0x00067EA7
		public override Sprite Icon
		{
			get
			{
				if (this.perk != null)
				{
					return this.perk.Icon;
				}
				return null;
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00069CC4 File Offset: 0x00067EC4
		protected override void OnInit()
		{
			if (LevelManager.LevelInited)
			{
				this.BindPerk();
				return;
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x00069CE8 File Offset: 0x00067EE8
		private bool BindPerk()
		{
			if (this.perk)
			{
				if (!this.unlocked && this.perk.Unlocked)
				{
					this.OnPerkUnlockStateChanged(this.perk, true);
				}
				return false;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				using (List<Perk>.Enumerator enumerator = perkTree.perks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Perk perk = enumerator.Current;
						if (perk.gameObject.name == this.perkObjectName)
						{
							this.perk = perk;
							if (this.perk.Unlocked)
							{
								this.OnPerkUnlockStateChanged(this.perk, true);
							}
							this.perk.onUnlockStateChanged += this.OnPerkUnlockStateChanged;
							return true;
						}
					}
					goto IL_E6;
				}
			}
			Debug.LogError("PerkTree Not Found " + this.perkTreeID, base.gameObject);
			IL_E6:
			Debug.LogError("Perk Not Found: " + this.perkTreeID + "/" + this.perkObjectName, base.gameObject);
			return false;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00069E14 File Offset: 0x00068014
		private void OnPerkUnlockStateChanged(Perk _perk, bool _unlocked)
		{
			if (base.Master.Complete)
			{
				return;
			}
			if (_unlocked)
			{
				this.unlocked = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00069E34 File Offset: 0x00068034
		private void OnDestroy()
		{
			if (this.perk)
			{
				this.perk.onUnlockStateChanged -= this.OnPerkUnlockStateChanged;
			}
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00069E6B File Offset: 0x0006806B
		private void OnLevelInitialized()
		{
			this.BindPerk();
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00069E74 File Offset: 0x00068074
		public override object GenerateSaveData()
		{
			return this.unlocked;
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00069E81 File Offset: 0x00068081
		protected override bool CheckFinished()
		{
			return this.unlocked;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x00069E8C File Offset: 0x0006808C
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.unlocked = flag;
			}
		}

		// Token: 0x04001485 RID: 5253
		[SerializeField]
		private string perkTreeID;

		// Token: 0x04001486 RID: 5254
		[SerializeField]
		private string perkObjectName;

		// Token: 0x04001487 RID: 5255
		private Perk perk;

		// Token: 0x04001488 RID: 5256
		[NonSerialized]
		private bool unlocked;

		// Token: 0x04001489 RID: 5257
		private string descriptionFormatKey = "Task_UnlockPerk";
	}
}
