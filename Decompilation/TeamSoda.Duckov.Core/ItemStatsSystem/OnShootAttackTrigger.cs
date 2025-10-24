using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000229 RID: 553
	[MenuPath("General/On Shoot&Attack")]
	public class OnShootAttackTrigger : EffectTrigger
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x000420FF File Offset: 0x000402FF
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00042107 File Offset: 0x00040307
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00042115 File Offset: 0x00040315
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00042120 File Offset: 0x00040320
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
			this.target = item.GetCharacterMainControl();
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent += this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent += this.OnShootAttack;
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x000421B4 File Offset: 0x000403B4
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent -= this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent -= this.OnShootAttack;
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0004220E File Offset: 0x0004040E
		private void OnShootAttack(DuckovItemAgent agent)
		{
			base.Trigger(true);
		}

		// Token: 0x04000D4D RID: 3405
		[SerializeField]
		private bool onShoot = true;

		// Token: 0x04000D4E RID: 3406
		[SerializeField]
		private bool onAttack = true;

		// Token: 0x04000D4F RID: 3407
		private CharacterMainControl target;
	}
}
