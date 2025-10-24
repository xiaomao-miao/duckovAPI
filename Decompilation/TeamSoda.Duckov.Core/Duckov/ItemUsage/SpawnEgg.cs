using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x0200036D RID: 877
	public class SpawnEgg : UsageBehavior
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x0006B290 File Offset: 0x00069490
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return new UsageBehavior.DisplaySettingsData
				{
					display = true,
					description = (this.descriptionKey.ToPlainText() ?? "")
				};
			}
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0006B2C9 File Offset: 0x000694C9
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0006B2CC File Offset: 0x000694CC
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			Egg egg = UnityEngine.Object.Instantiate<Egg>(this.eggPrefab, characterMainControl.transform.position, Quaternion.identity);
			Collider component = egg.GetComponent<Collider>();
			Collider component2 = characterMainControl.GetComponent<Collider>();
			if (component && component2)
			{
				Debug.Log("关掉角色和蛋的碰撞");
				Physics.IgnoreCollision(component, component2, true);
			}
			egg.Init(characterMainControl.transform.position, characterMainControl.CurrentAimDirection * 1f, characterMainControl, this.spawnCharacter, this.eggSpawnDelay);
		}

		// Token: 0x040014C2 RID: 5314
		public Egg eggPrefab;

		// Token: 0x040014C3 RID: 5315
		public CharacterRandomPreset spawnCharacter;

		// Token: 0x040014C4 RID: 5316
		public float eggSpawnDelay = 2f;

		// Token: 0x040014C5 RID: 5317
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_SpawnEgg";
	}
}
