using System;
using ItemStatsSystem;
using SodaCraft.Localizations;

namespace Duckov.ItemUsage
{
	// Token: 0x0200036B RID: 875
	[MenuPath("食物/食物")]
	public class FoodDrink : UsageBehavior
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0006AFFC File Offset: 0x000691FC
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				if (this.energyValue != 0f && this.waterValue != 0f)
				{
					result.description = string.Concat(new string[]
					{
						this.energyKey.ToPlainText(),
						": ",
						this.energyValue.ToString(),
						"  ",
						this.waterKey.ToPlainText(),
						": ",
						this.waterValue.ToString()
					});
				}
				else if (this.energyValue != 0f)
				{
					result.description = this.energyKey.ToPlainText() + ": " + this.energyValue.ToString();
				}
				else
				{
					result.description = this.waterKey.ToPlainText() + ": " + this.waterValue.ToString();
				}
				return result;
			}
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0006B0F5 File Offset: 0x000692F5
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0006B108 File Offset: 0x00069308
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			this.Eat(characterMainControl);
			if (this.UseDurability > 0f && item.UseDurability)
			{
				item.Durability -= this.UseDurability;
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0006B154 File Offset: 0x00069354
		private void Eat(CharacterMainControl character)
		{
			if (this.energyValue != 0f)
			{
				character.AddEnergy(this.energyValue);
			}
			if (this.waterValue != 0f)
			{
				character.AddWater(this.waterValue);
			}
		}

		// Token: 0x040014B8 RID: 5304
		public float energyValue;

		// Token: 0x040014B9 RID: 5305
		public float waterValue;

		// Token: 0x040014BA RID: 5306
		[LocalizationKey("Default")]
		public string energyKey = "Usage_Energy";

		// Token: 0x040014BB RID: 5307
		[LocalizationKey("Default")]
		public string waterKey = "Usage_Water";

		// Token: 0x040014BC RID: 5308
		public float UseDurability;
	}
}
