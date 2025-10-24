using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000232 RID: 562
	[Serializable]
	public class CursorData
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00043B2C File Offset: 0x00041D2C
		public Texture2D texture
		{
			get
			{
				if (this.textures.Length == 0)
				{
					return null;
				}
				return this.textures[0];
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00043B44 File Offset: 0x00041D44
		internal void Apply(int frame)
		{
			if (this.textures == null || this.textures.Length < 1)
			{
				Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
				return;
			}
			if (frame < 0)
			{
				int num = this.textures.Length;
				frame = (-frame / this.textures.Length + 1) * num + frame;
			}
			frame %= this.textures.Length;
			Cursor.SetCursor(this.textures[frame], this.hotspot, CursorMode.Auto);
		}

		// Token: 0x04000D88 RID: 3464
		public Texture2D[] textures;

		// Token: 0x04000D89 RID: 3465
		public Vector2 hotspot;

		// Token: 0x04000D8A RID: 3466
		public float fps;
	}
}
