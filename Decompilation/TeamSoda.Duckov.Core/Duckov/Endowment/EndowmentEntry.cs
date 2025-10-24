using System;
using System.Text;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Endowment
{
	// Token: 0x020002F3 RID: 755
	public class EndowmentEntry : MonoBehaviour
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x00059737 File Offset: 0x00057937
		public EndowmentIndex Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0005973F File Offset: 0x0005793F
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x00059756 File Offset: 0x00057956
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return string.Format("Endowmment_{0}", this.index);
			}
			set
			{
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x00059758 File Offset: 0x00057958
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x0005976F File Offset: 0x0005796F
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return string.Format("Endowmment_{0}_Desc", this.index);
			}
			set
			{
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x00059771 File Offset: 0x00057971
		public string RequirementText
		{
			get
			{
				return this.requirementTextKey.ToPlainText();
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x0005977E File Offset: 0x0005797E
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x00059786 File Offset: 0x00057986
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x00059793 File Offset: 0x00057993
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x000597A0 File Offset: 0x000579A0
		public string DescriptionAndEffects
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string description = this.Description;
				stringBuilder.AppendLine(description);
				foreach (EndowmentEntry.ModifierDescription modifierDescription in this.Modifiers)
				{
					stringBuilder.AppendLine("- " + modifierDescription.DescriptionText);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600188A RID: 6282 RVA: 0x000597FE File Offset: 0x000579FE
		public EndowmentEntry.ModifierDescription[] Modifiers
		{
			get
			{
				return this.modifiers;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x00059806 File Offset: 0x00057A06
		private Item CharacterItem
		{
			get
			{
				if (CharacterMainControl.Main == null)
				{
					return null;
				}
				return CharacterMainControl.Main.CharacterItem;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600188C RID: 6284 RVA: 0x00059821 File Offset: 0x00057A21
		public bool UnlockedByDefault
		{
			get
			{
				return this.unlockedByDefault;
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00059829 File Offset: 0x00057A29
		public void Activate()
		{
			this.ApplyModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onActivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00059842 File Offset: 0x00057A42
		public void Deactivate()
		{
			this.DeleteModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onDeactivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0005985C File Offset: 0x00057A5C
		private void ApplyModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.DeleteModifiers();
			foreach (EndowmentEntry.ModifierDescription modifierDescription in this.modifiers)
			{
				this.CharacterItem.AddModifier(modifierDescription.statKey, new Modifier(modifierDescription.type, modifierDescription.value, this));
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x000598BF File Offset: 0x00057ABF
		private void DeleteModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.CharacterItem.RemoveAllModifiersFrom(this);
		}

		// Token: 0x040011DA RID: 4570
		[SerializeField]
		private EndowmentIndex index;

		// Token: 0x040011DB RID: 4571
		[SerializeField]
		private Sprite icon;

		// Token: 0x040011DC RID: 4572
		[SerializeField]
		[LocalizationKey("Default")]
		private string requirementTextKey;

		// Token: 0x040011DD RID: 4573
		[SerializeField]
		private bool unlockedByDefault;

		// Token: 0x040011DE RID: 4574
		[SerializeField]
		private EndowmentEntry.ModifierDescription[] modifiers;

		// Token: 0x040011DF RID: 4575
		public UnityEvent<EndowmentEntry> onActivate;

		// Token: 0x040011E0 RID: 4576
		public UnityEvent<EndowmentEntry> onDeactivate;

		// Token: 0x02000587 RID: 1415
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x17000766 RID: 1894
			// (get) Token: 0x0600286C RID: 10348 RVA: 0x00095174 File Offset: 0x00093374
			// (set) Token: 0x0600286D RID: 10349 RVA: 0x00095186 File Offset: 0x00093386
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					return "Stat_" + this.statKey;
				}
				set
				{
				}
			}

			// Token: 0x17000767 RID: 1895
			// (get) Token: 0x0600286E RID: 10350 RVA: 0x00095188 File Offset: 0x00093388
			public string DescriptionText
			{
				get
				{
					string str = this.DisplayNameKey.ToPlainText();
					string str2 = "";
					ModifierType modifierType = this.type;
					if (modifierType != ModifierType.Add)
					{
						if (modifierType != ModifierType.PercentageAdd)
						{
							if (modifierType == ModifierType.PercentageMultiply)
							{
								str2 = string.Format("x{0:00.#}%", (1f + this.value) * 100f);
							}
						}
						else if (this.value >= 0f)
						{
							str2 = string.Format("+{0:00.#}%", this.value * 100f);
						}
						else
						{
							str2 = string.Format("-{0:00.#}%", -this.value * 100f);
						}
					}
					else if (this.value >= 0f)
					{
						str2 = string.Format("+{0}", this.value);
					}
					else
					{
						str2 = string.Format("{0}", this.value);
					}
					return str + " " + str2;
				}
			}

			// Token: 0x04001FBF RID: 8127
			public string statKey;

			// Token: 0x04001FC0 RID: 8128
			public ModifierType type;

			// Token: 0x04001FC1 RID: 8129
			public float value;
		}
	}
}
