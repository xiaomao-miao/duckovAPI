using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000369 RID: 873
	[MenuPath("概率死亡")]
	public class DeadByChance : UsageBehavior
	{
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x0006AC9C File Offset: 0x00068E9C
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return new UsageBehavior.DisplaySettingsData
				{
					display = true,
					description = string.Format("{0}:  {1:0}%", this.descriptionKey.ToPlainText(), this.chance * 100f)
				};
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0006ACE7 File Offset: 0x00068EE7
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0006ACFC File Offset: 0x00068EFC
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			if (UnityEngine.Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			this.KillSelf(characterMainControl, item.TypeID).Forget();
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0006AD48 File Offset: 0x00068F48
		private UniTaskVoid KillSelf(CharacterMainControl character, int weaponID)
		{
			DeadByChance.<KillSelf>d__8 <KillSelf>d__;
			<KillSelf>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<KillSelf>d__.<>4__this = this;
			<KillSelf>d__.character = character;
			<KillSelf>d__.weaponID = weaponID;
			<KillSelf>d__.<>1__state = -1;
			<KillSelf>d__.<>t__builder.Start<DeadByChance.<KillSelf>d__8>(ref <KillSelf>d__);
			return <KillSelf>d__.<>t__builder.Task;
		}

		// Token: 0x040014AE RID: 5294
		public int damageValue = 9999;

		// Token: 0x040014AF RID: 5295
		public float chance;

		// Token: 0x040014B0 RID: 5296
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_DeadByChance";

		// Token: 0x040014B1 RID: 5297
		[LocalizationKey("Default")]
		public string popTextKey = "Usage_DeadByChance_PopText";
	}
}
