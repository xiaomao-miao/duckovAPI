using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003EF RID: 1007
	public class GameRulesManager : MonoBehaviour
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x0007E71C File Offset: 0x0007C91C
		public static GameRulesManager Instance
		{
			get
			{
				return GameManager.DifficultyManager;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x0007E723 File Offset: 0x0007C923
		public static Ruleset Current
		{
			get
			{
				return GameRulesManager.Instance.mCurrent;
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x0600245B RID: 9307 RVA: 0x0007E730 File Offset: 0x0007C930
		// (remove) Token: 0x0600245C RID: 9308 RVA: 0x0007E764 File Offset: 0x0007C964
		public static event Action OnRuleChanged;

		// Token: 0x0600245D RID: 9309 RVA: 0x0007E797 File Offset: 0x0007C997
		public static void NotifyRuleChanged()
		{
			Action onRuleChanged = GameRulesManager.OnRuleChanged;
			if (onRuleChanged == null)
			{
				return;
			}
			onRuleChanged();
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x0007E7A8 File Offset: 0x0007C9A8
		private Ruleset mCurrent
		{
			get
			{
				if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom)
				{
					return this.CustomRuleSet;
				}
				foreach (GameRulesManager.RuleIndexFileEntry ruleIndexFileEntry in this.entries)
				{
					if (ruleIndexFileEntry.index == GameRulesManager.SelectedRuleIndex)
					{
						return ruleIndexFileEntry.file.Data;
					}
				}
				return this.entries[0].file.Data;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x0007E810 File Offset: 0x0007CA10
		// (set) Token: 0x06002460 RID: 9312 RVA: 0x0007E82A File Offset: 0x0007CA2A
		public static RuleIndex SelectedRuleIndex
		{
			get
			{
				if (SavesSystem.KeyExisits("GameRulesManager_RuleIndex"))
				{
					return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex");
				}
				return RuleIndex.Standard;
			}
			internal set
			{
				SavesSystem.Save<RuleIndex>("GameRulesManager_RuleIndex", value);
				GameRulesManager.NotifyRuleChanged();
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0007E83C File Offset: 0x0007CA3C
		public static RuleIndex GetRuleIndexOfSaveSlot(int slot)
		{
			return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex", slot);
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0007E849 File Offset: 0x0007CA49
		private Ruleset CustomRuleSet
		{
			get
			{
				if (this.customRuleSet == null)
				{
					this.ReloadCustomRuleSet();
				}
				return this.customRuleSet;
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0007E85F File Offset: 0x0007CA5F
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetFile;
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0007E883 File Offset: 0x0007CA83
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetFile;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0007E8A7 File Offset: 0x0007CAA7
		private void OnSetFile()
		{
			this.ReloadCustomRuleSet();
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0007E8B0 File Offset: 0x0007CAB0
		private void ReloadCustomRuleSet()
		{
			if (SavesSystem.KeyExisits("Rule_Custom"))
			{
				this.customRuleSet = SavesSystem.Load<Ruleset>("Rule_Custom");
			}
			if (this.customRuleSet == null)
			{
				this.customRuleSet = new Ruleset();
				this.customRuleSet.displayNameKey = "Rule_Custom";
			}
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0007E8FC File Offset: 0x0007CAFC
		private void OnCollectSaveData()
		{
			if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom && this.customRuleSet != null)
			{
				SavesSystem.Save<Ruleset>("Rule_Custom", this.customRuleSet);
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0007E920 File Offset: 0x0007CB20
		internal static string GetRuleIndexDisplayNameOfSlot(int slotIndex)
		{
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(slotIndex);
			return string.Format("Rule_{0}", ruleIndexOfSaveSlot).ToPlainText();
		}

		// Token: 0x040018BC RID: 6332
		private const string SelectedRuleIndexSaveKey = "GameRulesManager_RuleIndex";

		// Token: 0x040018BD RID: 6333
		private Ruleset customRuleSet;

		// Token: 0x040018BE RID: 6334
		private const string CustomRuleSetKey = "Rule_Custom";

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		private GameRulesManager.RuleIndexFileEntry[] entries;

		// Token: 0x0200064F RID: 1615
		[Serializable]
		private struct RuleIndexFileEntry
		{
			// Token: 0x0400228E RID: 8846
			public RuleIndex index;

			// Token: 0x0400228F RID: 8847
			public RulesetFile file;
		}
	}
}
