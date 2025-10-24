using System;
using UnityEngine;
using UnityEngine.Events;

namespace ItemStatsSystem
{
	// Token: 0x0200022A RID: 554
	[MenuPath("General/On Take Damage")]
	public class OnTakeDamageTrigger : EffectTrigger
	{
		// Token: 0x06001112 RID: 4370 RVA: 0x0004222D File Offset: 0x0004042D
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00042235 File Offset: 0x00040435
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00042243 File Offset: 0x00040443
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0004224C File Offset: 0x0004044C
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (base.Master == null)
			{
				return;
			}
			Item item = base.Master.Item;
			if (item == null)
			{
				return;
			}
			CharacterMainControl characterMainControl = item.GetCharacterMainControl();
			if (characterMainControl == null)
			{
				return;
			}
			this.target = characterMainControl.Health;
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x000422BD File Offset: 0x000404BD
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000422EA File Offset: 0x000404EA
		private void OnTookDamage(DamageInfo info)
		{
			if (info.damageValue < (float)this.threshold)
			{
				return;
			}
			base.Trigger(true);
		}

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		public int threshold;

		// Token: 0x04000D51 RID: 3409
		private Health target;
	}
}
