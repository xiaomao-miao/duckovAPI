using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000026 RID: 38
	public class UsageUtilities : ItemComponent
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007E74 File Offset: 0x00006074
		public float UseTime
		{
			get
			{
				return this.useTime;
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060001FE RID: 510 RVA: 0x00007E7C File Offset: 0x0000607C
		// (remove) Token: 0x060001FF RID: 511 RVA: 0x00007EB0 File Offset: 0x000060B0
		public static event Action<Item> OnItemUsedStaticEvent;

		// Token: 0x06000200 RID: 512 RVA: 0x00007EE4 File Offset: 0x000060E4
		public bool IsUsable(Item item, object user)
		{
			if (!item)
			{
				return false;
			}
			if (this.useDurability && item.Durability < (float)this.durabilityUsage)
			{
				return false;
			}
			foreach (UsageBehavior usageBehavior in this.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.CanBeUsed(item, user))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007F70 File Offset: 0x00006170
		public void Use(Item item, object user)
		{
			foreach (UsageBehavior usageBehavior in this.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.CanBeUsed(item, user))
				{
					usageBehavior.Use(item, user);
				}
			}
			if (this.useDurability && item.Durability > 0f)
			{
				item.Durability -= (float)this.durabilityUsage;
			}
			Action<Item> onItemUsedStaticEvent = UsageUtilities.OnItemUsedStaticEvent;
			if (onItemUsedStaticEvent == null)
			{
				return;
			}
			onItemUsedStaticEvent(item);
		}

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private float useTime;

		// Token: 0x040000AD RID: 173
		public List<UsageBehavior> behaviors = new List<UsageBehavior>();

		// Token: 0x040000AE RID: 174
		public bool hasSound;

		// Token: 0x040000AF RID: 175
		public string actionSound;

		// Token: 0x040000B0 RID: 176
		public string useSound;

		// Token: 0x040000B1 RID: 177
		public bool useDurability;

		// Token: 0x040000B2 RID: 178
		public int durabilityUsage = 1;
	}
}
