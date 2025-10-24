using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x0200036A RID: 874
	[MenuPath("医疗/药")]
	public class Drug : UsageBehavior
	{
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0006ADC4 File Offset: 0x00068FC4
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = string.Format("{0} : {1}", this.healValueDescriptionKey.ToPlainText(), this.healValue);
				if (this.useDurability)
				{
					result.description += string.Format(" ({0} : {1})", this.durabilityUsageDescriptionKey.ToPlainText(), this.durabilityUsage);
				}
				return result;
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0006AE40 File Offset: 0x00069040
		public override bool CanBeUsed(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return characterMainControl && this.CheckCanHeal(characterMainControl);
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0006AE6C File Offset: 0x0006906C
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			float num = (float)this.healValue;
			if (this.useDurability && item.UseDurability)
			{
				float num2 = this.durabilityUsage;
				if (this.canUsePart)
				{
					num = characterMainControl.Health.MaxHealth - characterMainControl.Health.CurrentHealth;
					if (num > (float)this.healValue)
					{
						num = (float)this.healValue;
					}
					num2 = num / (float)this.healValue * this.durabilityUsage;
					if (num2 > item.Durability)
					{
						num2 = item.Durability;
						num = (float)this.healValue * item.Durability / this.durabilityUsage;
					}
					Debug.Log(string.Format("治疗：{0}耐久消耗：{1}", num, num2));
					item.Durability -= num2;
				}
			}
			this.Heal(characterMainControl, item, num);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0006AF4C File Offset: 0x0006914C
		private bool CheckCanHeal(CharacterMainControl character)
		{
			return this.healValue <= 0 || character.Health.CurrentHealth < character.Health.MaxHealth;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0006AF74 File Offset: 0x00069174
		private void Heal(CharacterMainControl character, Item selfItem, float _healValue)
		{
			if (_healValue > 0f)
			{
				character.AddHealth((float)Mathf.CeilToInt(_healValue));
				return;
			}
			if (_healValue < 0f)
			{
				DamageInfo damageInfo = new DamageInfo(null);
				damageInfo.damageValue = -_healValue;
				damageInfo.damagePoint = character.transform.position;
				damageInfo.damageNormal = Vector3.up;
				character.Health.Hurt(damageInfo);
			}
		}

		// Token: 0x040014B2 RID: 5298
		public int healValue;

		// Token: 0x040014B3 RID: 5299
		[LocalizationKey("Default")]
		public string healValueDescriptionKey = "Usage_HealValue";

		// Token: 0x040014B4 RID: 5300
		[LocalizationKey("Default")]
		public string durabilityUsageDescriptionKey = "Usage_Durability";

		// Token: 0x040014B5 RID: 5301
		public bool useDurability;

		// Token: 0x040014B6 RID: 5302
		public float durabilityUsage;

		// Token: 0x040014B7 RID: 5303
		public bool canUsePart;
	}
}
