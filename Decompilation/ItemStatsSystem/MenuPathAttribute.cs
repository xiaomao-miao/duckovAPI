using System;

namespace ItemStatsSystem
{
	// Token: 0x0200000B RID: 11
	public class MenuPathAttribute : Attribute
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002FA4 File Offset: 0x000011A4
		public MenuPathAttribute(string path)
		{
			this.path = path;
		}

		// Token: 0x04000027 RID: 39
		public string path;
	}
}
