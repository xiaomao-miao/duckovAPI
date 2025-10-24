using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000266 RID: 614
	[Serializable]
	public class Note
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x000474EA File Offset: 0x000456EA
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x00047501 File Offset: 0x00045701
		[LocalizationKey("Default")]
		public string titleKey
		{
			get
			{
				return "Note_" + this.key + "_Title";
			}
			set
			{
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00047503 File Offset: 0x00045703
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x0004751A File Offset: 0x0004571A
		[LocalizationKey("Default")]
		public string contentKey
		{
			get
			{
				return "Note_" + this.key + "_Content";
			}
			set
			{
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x0004751C File Offset: 0x0004571C
		public string Title
		{
			get
			{
				return this.titleKey.ToPlainText();
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x00047529 File Offset: 0x00045729
		private Sprite previewSprite
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x00047531 File Offset: 0x00045731
		public string Content
		{
			get
			{
				return this.contentKey.ToPlainText();
			}
		}

		// Token: 0x04000E47 RID: 3655
		[SerializeField]
		public string key;

		// Token: 0x04000E48 RID: 3656
		[SerializeField]
		public Sprite image;
	}
}
