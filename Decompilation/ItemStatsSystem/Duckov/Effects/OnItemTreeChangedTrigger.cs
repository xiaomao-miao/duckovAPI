using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Effects
{
	// Token: 0x02000004 RID: 4
	public class OnItemTreeChangedTrigger : EffectTrigger
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000022C0 File Offset: 0x000004C0
		protected override void Awake()
		{
			base.Awake();
			base.Master.onItemTreeChanged += this.OnItemTreeChanged;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022DF File Offset: 0x000004DF
		private void OnDestroy()
		{
			if (base.Master == null)
			{
				return;
			}
			base.Master.onItemTreeChanged -= this.OnItemTreeChanged;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002308 File Offset: 0x00000508
		private void OnItemTreeChanged(Effect effect, Item item)
		{
			UnityEngine.Object characterItem = item.GetCharacterItem();
			if (this.triggerFalseWheneverChanged)
			{
				base.Trigger(false);
			}
			if (characterItem == null)
			{
				if (!this.triggerFalseWheneverChanged)
				{
					base.Trigger(false);
				}
				return;
			}
			if (this.triggerInInventory)
			{
				base.Trigger(true);
				return;
			}
			if (item.IsInCharacterSlot())
			{
				base.Trigger(true);
				return;
			}
			if (!this.triggerFalseWheneverChanged)
			{
				base.Trigger(false);
			}
		}

		// Token: 0x04000005 RID: 5
		[SerializeField]
		private bool triggerFalseWheneverChanged = true;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private bool triggerInInventory;
	}
}
