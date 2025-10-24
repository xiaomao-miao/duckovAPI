using System;
using System.Linq;
using Duckov.Utilities;
using SodaCraft.Localizations;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public struct ItemMetaData
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002AF9 File Offset: 0x00000CF9
		public string Catagory
		{
			get
			{
				if (this.tags != null && this.tags.Length != 0)
				{
					return this.tags[0].name;
				}
				return "None";
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002B1F File Offset: 0x00000D1F
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002B27 File Offset: 0x00000D27
		public string DisplayNameKey
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002B2F File Offset: 0x00000D2F
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002B3C File Offset: 0x00000D3C
		public string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B4C File Offset: 0x00000D4C
		public ItemMetaData(int id, int quality, Tag[] tags, string name, string displayName, Sprite icon, string caliber = "", string description = "", DisplayQuality displayQuality = DisplayQuality.None, int maxStackCount = 1, int defaultStackCount = 1, int priceEach = 0)
		{
			this.id = id;
			this.quality = quality;
			this.tags = tags;
			this.name = name;
			this.displayName = displayName;
			this.icon = icon;
			this.caliber = caliber;
			this.description = description;
			this.displayQuality = displayQuality;
			this.maxStackCount = maxStackCount;
			this.defaultStackCount = defaultStackCount;
			this.priceEach = priceEach;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public ItemMetaData(Item from)
		{
			this.id = from.TypeID;
			this.quality = from.Quality;
			this.tags = from.Tags.ToArray<Tag>();
			this.name = from.name;
			this.displayName = from.DisplayNameRaw;
			this.icon = from.Icon;
			string @string = from.Constants.GetString("Caliber", "");
			this.caliber = @string;
			this.description = from.DescriptionRaw;
			this.displayQuality = from.DisplayQuality;
			this.maxStackCount = from.MaxStackCount;
			this.defaultStackCount = from.StackCount;
			this.priceEach = from.Value;
		}

		// Token: 0x04000014 RID: 20
		[ItemTypeID]
		public int id;

		// Token: 0x04000015 RID: 21
		public int quality;

		// Token: 0x04000016 RID: 22
		public Tag[] tags;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		private string name;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private string displayName;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private string description;

		// Token: 0x0400001A RID: 26
		public int maxStackCount;

		// Token: 0x0400001B RID: 27
		public int defaultStackCount;

		// Token: 0x0400001C RID: 28
		public Sprite icon;

		// Token: 0x0400001D RID: 29
		public DisplayQuality displayQuality;

		// Token: 0x0400001E RID: 30
		public int priceEach;

		// Token: 0x0400001F RID: 31
		public string caliber;
	}
}
