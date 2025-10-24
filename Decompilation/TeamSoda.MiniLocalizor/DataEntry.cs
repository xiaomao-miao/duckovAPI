using System;

namespace MiniLocalizor
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class DataEntry
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022EB File Offset: 0x000004EB
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000022F3 File Offset: 0x000004F3
		public string key { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022FC File Offset: 0x000004FC
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002304 File Offset: 0x00000504
		public string value { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000230D File Offset: 0x0000050D
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002315 File Offset: 0x00000515
		public string version { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000231E File Offset: 0x0000051E
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002326 File Offset: 0x00000526
		public string sheet { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x00002330 File Offset: 0x00000530
		public bool IsNewerThan(string version)
		{
			if (string.IsNullOrEmpty(this.version))
			{
				return false;
			}
			bool flag = this.version.StartsWith('#');
			bool flag2 = version.StartsWith('#');
			if (flag && !flag2)
			{
				return true;
			}
			string text = this.version;
			if (flag)
			{
				text = text.Substring(1);
			}
			string text2 = version;
			if (flag2)
			{
				text2 = text2.Substring(1);
			}
			long num;
			long num2;
			return long.TryParse(text, out num) && (!long.TryParse(text2, out num2) || num > num2);
		}
	}
}
