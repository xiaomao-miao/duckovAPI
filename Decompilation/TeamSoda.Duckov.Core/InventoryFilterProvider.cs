using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class InventoryFilterProvider : MonoBehaviour
{
	// Token: 0x04000C14 RID: 3092
	public InventoryFilterProvider.FilterEntry[] entries;

	// Token: 0x020004DD RID: 1245
	[Serializable]
	public struct FilterEntry
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x0008E4BA File Offset: 0x0008C6BA
		public string DisplayName
		{
			get
			{
				return this.name.ToPlainText();
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x0008E4C8 File Offset: 0x0008C6C8
		private bool FilterFunction(Item item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.requireTags.Length == 0)
			{
				return true;
			}
			foreach (Tag tag in this.requireTags)
			{
				if (!(tag == null) && item.Tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x0008E51E File Offset: 0x0008C71E
		public Func<Item, bool> GetFunction()
		{
			if (this.requireTags.Length == 0)
			{
				return null;
			}
			return new Func<Item, bool>(this.FilterFunction);
		}

		// Token: 0x04001D0E RID: 7438
		[LocalizationKey("Default")]
		public string name;

		// Token: 0x04001D0F RID: 7439
		public Sprite icon;

		// Token: 0x04001D10 RID: 7440
		public Tag[] requireTags;
	}
}
