using System;
using Duckov.Buffs;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000368 RID: 872
	public class AddBuff : UsageBehavior
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0006AB70 File Offset: 0x00068D70
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = "";
				result.description = (this.buffPrefab.DisplayName ?? "");
				if (this.buffPrefab.LimitedLifeTime)
				{
					result.description += string.Format(" : {0}s ", this.buffPrefab.TotalLifeTime);
				}
				if (this.chance < 1f)
				{
					result.description += string.Format(" ({0} : {1}%)", this.chanceKey.ToPlainText(), Mathf.RoundToInt(this.chance * 100f));
				}
				return result;
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0006AC32 File Offset: 0x00068E32
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0006AC38 File Offset: 0x00068E38
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			characterMainControl.AddBuff(this.buffPrefab, characterMainControl, 0);
		}

		// Token: 0x040014AB RID: 5291
		public Buff buffPrefab;

		// Token: 0x040014AC RID: 5292
		[Range(0.01f, 1f)]
		public float chance = 1f;

		// Token: 0x040014AD RID: 5293
		[LocalizationKey("Default")]
		private string chanceKey = "UI_AddBuffChance";
	}
}
