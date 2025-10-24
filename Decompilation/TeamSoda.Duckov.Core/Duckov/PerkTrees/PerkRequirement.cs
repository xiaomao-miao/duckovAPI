using System;
using Duckov.Economy;
using ItemStatsSystem;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024B RID: 587
	[Serializable]
	public class PerkRequirement
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x00045968 File Offset: 0x00043B68
		public TimeSpan RequireTime
		{
			get
			{
				return TimeSpan.FromTicks(this.requireTime);
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00045975 File Offset: 0x00043B75
		internal bool AreSatisfied()
		{
			return this.level <= EXPManager.Level && this.cost.Enough;
		}

		// Token: 0x04000E07 RID: 3591
		public int level;

		// Token: 0x04000E08 RID: 3592
		public Cost cost;

		// Token: 0x04000E09 RID: 3593
		[TimeSpan]
		public long requireTime;

		// Token: 0x02000533 RID: 1331
		[Serializable]
		public class RequireItemEntry
		{
			// Token: 0x04001E7A RID: 7802
			[ItemTypeID]
			public int id = 1;

			// Token: 0x04001E7B RID: 7803
			public int amount = 1;
		}
	}
}
