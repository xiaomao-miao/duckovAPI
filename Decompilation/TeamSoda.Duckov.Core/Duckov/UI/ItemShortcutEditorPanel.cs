using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AB RID: 939
	public class ItemShortcutEditorPanel : MonoBehaviour
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00075B7C File Offset: 0x00073D7C
		private PrefabPool<ItemShortcutEditorEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemShortcutEditorEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00075BD5 File Offset: 0x00073DD5
		private void OnEnable()
		{
			this.Setup();
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00075BE0 File Offset: 0x00073DE0
		private void Setup()
		{
			this.EntryPool.ReleaseAll();
			for (int i = 0; i <= ItemShortcut.MaxIndex; i++)
			{
				ItemShortcutEditorEntry itemShortcutEditorEntry = this.EntryPool.Get(this.entryTemplate.transform.parent);
				itemShortcutEditorEntry.Setup(i);
				itemShortcutEditorEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x040016CE RID: 5838
		[SerializeField]
		private ItemShortcutEditorEntry entryTemplate;

		// Token: 0x040016CF RID: 5839
		private PrefabPool<ItemShortcutEditorEntry> _entryPool;
	}
}
