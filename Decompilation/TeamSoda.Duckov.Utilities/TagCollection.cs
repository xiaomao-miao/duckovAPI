using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Duckov.Utilities
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class TagCollection : ICollection<Tag>, IEnumerable<Tag>, IEnumerable
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00003C54 File Offset: 0x00001E54
		public bool Check(ICollection<Tag> requireTags, ICollection<Tag> excludeTags)
		{
			TagCollection.<Check>g__Print|1_0(this.list);
			TagCollection.<Check>g__Print|1_0(requireTags);
			TagCollection.<Check>g__Print|1_0(excludeTags);
			foreach (Tag tag in requireTags)
			{
				if (!(tag == null) && !this.Contains(tag))
				{
					return false;
				}
			}
			foreach (Tag tag2 in excludeTags)
			{
				if (!(tag2 == null) && this.Contains(tag2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003D10 File Offset: 0x00001F10
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003D1D File Offset: 0x00001F1D
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003D20 File Offset: 0x00001F20
		public void Add(Tag item)
		{
			if (item == null)
			{
				return;
			}
			if (this.Contains(item))
			{
				return;
			}
			this.list.Add(item);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003D42 File Offset: 0x00001F42
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003D50 File Offset: 0x00001F50
		public bool Contains(Tag item)
		{
			return !(item == null) && this.list.Any((Tag e) => e != null && e.Hash == item.Hash);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003D94 File Offset: 0x00001F94
		public bool Contains(string tagName)
		{
			return this.list.Any((Tag e) => e != null && e.name == tagName);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003DC5 File Offset: 0x00001FC5
		public void CopyTo(Tag[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003DD4 File Offset: 0x00001FD4
		public IEnumerator<Tag> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003DE8 File Offset: 0x00001FE8
		public bool Remove(Tag item)
		{
			return this.list.RemoveAll((Tag e) => e.Hash == item.Hash) > 0;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003E1C File Offset: 0x0000201C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003E2E File Offset: 0x0000202E
		public Tag Get(int index)
		{
			if (index < 0 || index >= this.list.Count)
			{
				return null;
			}
			return this.list[index];
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003E64 File Offset: 0x00002064
		[CompilerGenerated]
		internal static string <Check>g__Print|1_0(ICollection<Tag> tags)
		{
			string text = "";
			foreach (Tag tag in tags)
			{
				text += string.Format("{0}({1})", tag.name, tag.GetInstanceID());
			}
			return text;
		}

		// Token: 0x04000033 RID: 51
		public List<Tag> list = new List<Tag>();
	}
}
