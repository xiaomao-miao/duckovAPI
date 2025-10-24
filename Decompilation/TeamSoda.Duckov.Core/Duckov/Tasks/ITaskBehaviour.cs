using System;

namespace Duckov.Tasks
{
	// Token: 0x0200036E RID: 878
	public interface ITaskBehaviour
	{
		// Token: 0x06001E6F RID: 7791
		void Begin();

		// Token: 0x06001E70 RID: 7792
		bool IsPending();

		// Token: 0x06001E71 RID: 7793
		bool IsComplete();

		// Token: 0x06001E72 RID: 7794 RVA: 0x0006B380 File Offset: 0x00069580
		void Skip()
		{
		}
	}
}
