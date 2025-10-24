using System;

namespace Duckov.Utilities
{
	// Token: 0x0200000A RID: 10
	public interface IPoolable
	{
		// Token: 0x0600005D RID: 93
		void NotifyPooled();

		// Token: 0x0600005E RID: 94
		void NotifyReleased();
	}
}
