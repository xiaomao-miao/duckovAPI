using System;

namespace Duckov
{
	// Token: 0x02000237 RID: 567
	[Serializable]
	public struct VersionData
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x000440E8 File Offset: 0x000422E8
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}{3}", new object[]
			{
				this.mainVersion,
				this.subVersion,
				this.buildVersion,
				this.suffix
			});
		}

		// Token: 0x04000D9E RID: 3486
		public int mainVersion;

		// Token: 0x04000D9F RID: 3487
		public int subVersion;

		// Token: 0x04000DA0 RID: 3488
		public int buildVersion;

		// Token: 0x04000DA1 RID: 3489
		public string suffix;
	}
}
