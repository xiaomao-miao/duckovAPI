using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000025 RID: 37
	public abstract class UsageBehavior : MonoBehaviour
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00007E4C File Offset: 0x0000604C
		public virtual UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				return default(UsageBehavior.DisplaySettingsData);
			}
		}

		// Token: 0x060001F9 RID: 505
		public abstract bool CanBeUsed(Item item, object user);

		// Token: 0x060001FA RID: 506
		protected abstract void OnUse(Item item, object user);

		// Token: 0x060001FB RID: 507 RVA: 0x00007E62 File Offset: 0x00006062
		public void Use(Item item, object user)
		{
			this.OnUse(item, user);
		}

		// Token: 0x0200004A RID: 74
		public struct DisplaySettingsData
		{
			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x0600028A RID: 650 RVA: 0x000096E3 File Offset: 0x000078E3
			public string Description
			{
				get
				{
					return this.description;
				}
			}

			// Token: 0x04000124 RID: 292
			public bool display;

			// Token: 0x04000125 RID: 293
			public string description;
		}
	}
}
