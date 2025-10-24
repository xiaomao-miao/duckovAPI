using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x020003F3 RID: 1011
	public class DifficultySelection : MonoBehaviour
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x0007EA4C File Offset: 0x0007CC4C
		private PrefabPool<DifficultySelection_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DifficultySelection_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0007EA85 File Offset: 0x0007CC85
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0007EAA3 File Offset: 0x0007CCA3
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0007EAAC File Offset: 0x0007CCAC
		public UniTask Execute()
		{
			DifficultySelection.<Execute>d__15 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<DifficultySelection.<Execute>d__15>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0007EAF0 File Offset: 0x0007CCF0
		private bool CheckUnlocked(DifficultySelection.SettingEntry setting)
		{
			bool flag = !MultiSceneCore.GetVisited("Base");
			RuleIndex ruleIndex = setting.ruleIndex;
			if (ruleIndex <= RuleIndex.Custom)
			{
				if (ruleIndex != RuleIndex.Standard)
				{
					if (ruleIndex != RuleIndex.Custom)
					{
						return false;
					}
					return flag || GameRulesManager.SelectedRuleIndex == RuleIndex.Custom;
				}
			}
			else if (ruleIndex - RuleIndex.Easy > 2 && ruleIndex - RuleIndex.Hard > 1)
			{
				if (ruleIndex != RuleIndex.Rage)
				{
					return false;
				}
				return this.GetRageUnlocked(flag);
			}
			return flag || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex != RuleIndex.Rage);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0007EB67 File Offset: 0x0007CD67
		public static void UnlockRage()
		{
			SavesSystem.SaveGlobal<bool>("Difficulty/RageUnlocked", true);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0007EB74 File Offset: 0x0007CD74
		public bool GetRageUnlocked(bool isFirstSelect)
		{
			return SavesSystem.LoadGlobal<bool>("Difficulty/RageUnlocked", false) && (isFirstSelect || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex == RuleIndex.Rage));
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0007EBA0 File Offset: 0x0007CDA0
		private bool CheckShouldDisplay(DifficultySelection.SettingEntry setting)
		{
			return true;
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002483 RID: 9347 RVA: 0x0007EBA3 File Offset: 0x0007CDA3
		// (set) Token: 0x06002484 RID: 9348 RVA: 0x0007EBAF File Offset: 0x0007CDAF
		public static bool CustomDifficultyMarker
		{
			get
			{
				return SavesSystem.Load<bool>("CustomDifficultyMarker");
			}
			set
			{
				SavesSystem.Save<bool>("CustomDifficultyMarker", value);
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x0007EBBC File Offset: 0x0007CDBC
		public RuleIndex SelectedRuleIndex
		{
			get
			{
				if (this.SelectedEntry == null)
				{
					return RuleIndex.Standard;
				}
				return this.SelectedEntry.Setting.ruleIndex;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x0007EBDE File Offset: 0x0007CDDE
		// (set) Token: 0x06002487 RID: 9351 RVA: 0x0007EBE6 File Offset: 0x0007CDE6
		public DifficultySelection_Entry SelectedEntry { get; private set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x0007EBEF File Offset: 0x0007CDEF
		// (set) Token: 0x06002489 RID: 9353 RVA: 0x0007EBF7 File Offset: 0x0007CDF7
		public DifficultySelection_Entry HoveringEntry { get; private set; }

		// Token: 0x0600248A RID: 9354 RVA: 0x0007EC00 File Offset: 0x0007CE00
		private UniTask<RuleIndex> WaitForConfirmation()
		{
			DifficultySelection.<WaitForConfirmation>d__34 <WaitForConfirmation>d__;
			<WaitForConfirmation>d__.<>t__builder = AsyncUniTaskMethodBuilder<RuleIndex>.Create();
			<WaitForConfirmation>d__.<>4__this = this;
			<WaitForConfirmation>d__.<>1__state = -1;
			<WaitForConfirmation>d__.<>t__builder.Start<DifficultySelection.<WaitForConfirmation>d__34>(ref <WaitForConfirmation>d__);
			return <WaitForConfirmation>d__.<>t__builder.Task;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0007EC44 File Offset: 0x0007CE44
		internal void NotifySelected(DifficultySelection_Entry entry)
		{
			this.SelectedEntry = entry;
			GameRulesManager.SelectedRuleIndex = this.SelectedRuleIndex;
			foreach (DifficultySelection_Entry difficultySelection_Entry in this.EntryPool.ActiveEntries)
			{
				if (!(difficultySelection_Entry == null))
				{
					difficultySelection_Entry.Refresh();
				}
			}
			this.RefreshDescription();
			if (this.SelectedRuleIndex == RuleIndex.Custom)
			{
				this.ShowCustomRuleSetupPanel();
			}
			bool flag = this.SelectedRuleIndex == RuleIndex.Custom;
			this.achievementDisabledIndicator.SetActive(flag || DifficultySelection.CustomDifficultyMarker);
			this.selectedCustomDifficultyBefore.SetActive(DifficultySelection.CustomDifficultyMarker);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0007ECF8 File Offset: 0x0007CEF8
		private void ShowCustomRuleSetupPanel()
		{
			FadeGroup fadeGroup = this.customPanel;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0007ED0A File Offset: 0x0007CF0A
		internal void NotifyEntryPointerEnter(DifficultySelection_Entry entry)
		{
			this.HoveringEntry = entry;
			this.RefreshDescription();
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0007ED19 File Offset: 0x0007CF19
		internal void NotifyEntryPointerExit(DifficultySelection_Entry entry)
		{
			if (this.HoveringEntry == entry)
			{
				this.HoveringEntry = null;
				this.RefreshDescription();
			}
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0007ED38 File Offset: 0x0007CF38
		private void RefreshDescription()
		{
			string text;
			if (this.SelectedEntry != null)
			{
				text = this.SelectedEntry.Setting.Description;
			}
			else
			{
				text = this.description_PlaceHolderKey.ToPlainText();
			}
			this.textDescription.text = text;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0007ED81 File Offset: 0x0007CF81
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x040018D5 RID: 6357
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018D6 RID: 6358
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x040018D7 RID: 6359
		[SerializeField]
		[LocalizationKey("Default")]
		private string description_PlaceHolderKey = "DifficultySelection_Desc_PlaceHolder";

		// Token: 0x040018D8 RID: 6360
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040018D9 RID: 6361
		[SerializeField]
		private FadeGroup customPanel;

		// Token: 0x040018DA RID: 6362
		[SerializeField]
		private DifficultySelection_Entry entryTemplate;

		// Token: 0x040018DB RID: 6363
		[SerializeField]
		private GameObject achievementDisabledIndicator;

		// Token: 0x040018DC RID: 6364
		[SerializeField]
		private GameObject selectedCustomDifficultyBefore;

		// Token: 0x040018DD RID: 6365
		private PrefabPool<DifficultySelection_Entry> _entryPool;

		// Token: 0x040018DE RID: 6366
		[SerializeField]
		private DifficultySelection.SettingEntry[] displaySettings;

		// Token: 0x040018E1 RID: 6369
		private bool confirmed;

		// Token: 0x02000650 RID: 1616
		[Serializable]
		public struct SettingEntry
		{
			// Token: 0x1700078F RID: 1935
			// (get) Token: 0x06002A2B RID: 10795 RVA: 0x0009FECE File Offset: 0x0009E0CE
			// (set) Token: 0x06002A2C RID: 10796 RVA: 0x0009FEE5 File Offset: 0x0009E0E5
			[LocalizationKey("Default")]
			private string TitleKey
			{
				get
				{
					return string.Format("Rule_{0}", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x06002A2D RID: 10797 RVA: 0x0009FEE7 File Offset: 0x0009E0E7
			public string Title
			{
				get
				{
					return this.TitleKey.ToPlainText();
				}
			}

			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x06002A2E RID: 10798 RVA: 0x0009FEF4 File Offset: 0x0009E0F4
			// (set) Token: 0x06002A2F RID: 10799 RVA: 0x0009FF0B File Offset: 0x0009E10B
			[LocalizationKey("Default")]
			private string DescriptionKey
			{
				get
				{
					return string.Format("Rule_{0}_Desc", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x17000792 RID: 1938
			// (get) Token: 0x06002A30 RID: 10800 RVA: 0x0009FF0D File Offset: 0x0009E10D
			public string Description
			{
				get
				{
					return this.DescriptionKey.ToPlainText();
				}
			}

			// Token: 0x04002290 RID: 8848
			public RuleIndex ruleIndex;

			// Token: 0x04002291 RID: 8849
			public Sprite icon;

			// Token: 0x04002292 RID: 8850
			public bool recommended;
		}
	}
}
