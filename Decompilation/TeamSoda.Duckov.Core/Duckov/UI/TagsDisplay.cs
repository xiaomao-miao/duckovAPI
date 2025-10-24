using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039F RID: 927
	public class TagsDisplay : MonoBehaviour
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x000736E8 File Offset: 0x000718E8
		private PrefabPool<TagsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<TagsDisplayEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00073721 File Offset: 0x00071921
		private void Awake()
		{
			this.entryTemplate.gameObject.SetActive(false);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00073734 File Offset: 0x00071934
		public void Setup(Item item)
		{
			this.EntryPool.ReleaseAll();
			if (item == null)
			{
				return;
			}
			foreach (Tag tag in item.Tags)
			{
				if (!(tag == null) && tag.Show)
				{
					this.EntryPool.Get(null).Setup(tag);
				}
			}
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000737B4 File Offset: 0x000719B4
		internal void Clear()
		{
			this.EntryPool.ReleaseAll();
		}

		// Token: 0x04001671 RID: 5745
		[SerializeField]
		private TagsDisplayEntry entryTemplate;

		// Token: 0x04001672 RID: 5746
		private PrefabPool<TagsDisplayEntry> _entryPool;
	}
}
