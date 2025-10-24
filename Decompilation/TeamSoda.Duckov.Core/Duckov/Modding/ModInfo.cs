using System;
using System.IO;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026C RID: 620
	[Serializable]
	public struct ModInfo
	{
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x00048273 File Offset: 0x00046473
		public string dllPath
		{
			get
			{
				return Path.Combine(this.path, this.name + ".dll");
			}
		}

		// Token: 0x04000E59 RID: 3673
		public string path;

		// Token: 0x04000E5A RID: 3674
		public string name;

		// Token: 0x04000E5B RID: 3675
		public string displayName;

		// Token: 0x04000E5C RID: 3676
		public string description;

		// Token: 0x04000E5D RID: 3677
		public Texture2D preview;

		// Token: 0x04000E5E RID: 3678
		public bool dllFound;

		// Token: 0x04000E5F RID: 3679
		public bool isSteamItem;

		// Token: 0x04000E60 RID: 3680
		public ulong publishedFileId;
	}
}
