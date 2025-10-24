using System;
using ItemStatsSystem;
using UnityEngine.Serialization;

namespace Duckov.ItemUsage
{
	// Token: 0x0200036C RID: 876
	public class RemoveBuff : UsageBehavior
	{
		// Token: 0x06001E68 RID: 7784 RVA: 0x0006B1A8 File Offset: 0x000693A8
		public override bool CanBeUsed(Item item, object user)
		{
			if (!item)
			{
				return false;
			}
			if (this.useDurability && item.Durability < (float)this.durabilityUsage)
			{
				return false;
			}
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return !(characterMainControl == null) && characterMainControl.HasBuff(this.buffID);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0006B1F8 File Offset: 0x000693F8
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (!this.litmitRemoveLayerCount)
			{
				characterMainControl.RemoveBuff(this.buffID, false);
			}
			for (int i = 0; i < this.removeLayerCount; i++)
			{
				characterMainControl.RemoveBuff(this.buffID, this.litmitRemoveLayerCount);
			}
			if (this.useDurability && item.Durability > 0f)
			{
				item.Durability -= (float)this.durabilityUsage;
			}
		}

		// Token: 0x040014BD RID: 5309
		public int buffID;

		// Token: 0x040014BE RID: 5310
		[FormerlySerializedAs("removeOneLayer")]
		public bool litmitRemoveLayerCount;

		// Token: 0x040014BF RID: 5311
		public int removeLayerCount = 2;

		// Token: 0x040014C0 RID: 5312
		public bool useDurability;

		// Token: 0x040014C1 RID: 5313
		public int durabilityUsage = 1;
	}
}
