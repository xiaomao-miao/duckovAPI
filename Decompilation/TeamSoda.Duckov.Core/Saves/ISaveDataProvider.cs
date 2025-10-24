using System;

namespace Saves
{
	// Token: 0x02000224 RID: 548
	public interface ISaveDataProvider
	{
		// Token: 0x06001074 RID: 4212
		object GenerateSaveData();

		// Token: 0x06001075 RID: 4213
		void SetupSaveData(object data);
	}
}
