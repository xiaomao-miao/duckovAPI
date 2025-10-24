using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000011 RID: 17
	[CreateAssetMenu(menuName = "Items/Tag")]
	public class Tag : ScriptableObject
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003B80 File Offset: 0x00001D80
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003B7E File Offset: 0x00001D7E
		[LocalizationKey("Tags")]
		private string displayNameKey
		{
			get
			{
				return "Tag_" + base.name;
			}
			set
			{
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003B94 File Offset: 0x00001D94
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003B92 File Offset: 0x00001D92
		[LocalizationKey("Tags")]
		private string descriptionKey
		{
			get
			{
				return "Tag_" + base.name + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003BAB File Offset: 0x00001DAB
		public bool Show
		{
			get
			{
				return this.show;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003BB3 File Offset: 0x00001DB3
		public bool ShowDescription
		{
			get
			{
				return this.showDescription;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003BBB File Offset: 0x00001DBB
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003BC3 File Offset: 0x00001DC3
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003BDD File Offset: 0x00001DDD
		public int Hash
		{
			get
			{
				if (this._hash == null)
				{
					this._hash = new int?(Tag.GetHash(base.name));
				}
				return this._hash.Value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003C0D File Offset: 0x00001E0D
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003C15 File Offset: 0x00001E15
		public override string ToString()
		{
			return base.name;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003C1D File Offset: 0x00001E1D
		private static int GetHash(string name)
		{
			return name.GetHashCode();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003C25 File Offset: 0x00001E25
		public static bool Match(Tag tag, string name)
		{
			return !(tag == null) && tag.Hash == Tag.GetHash(name);
		}

		// Token: 0x0400002E RID: 46
		[SerializeField]
		private bool show;

		// Token: 0x0400002F RID: 47
		[SerializeField]
		private bool showDescription;

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private Color color = Color.black;

		// Token: 0x04000031 RID: 49
		[SerializeField]
		private int priority;

		// Token: 0x04000032 RID: 50
		private int? _hash;
	}
}
