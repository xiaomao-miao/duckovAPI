using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001F0 RID: 496
public class InventoryFilterDisplay : MonoBehaviour, ISingleSelectionMenu<InventoryFilterDisplayEntry>
{
	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0003A4F8 File Offset: 0x000386F8
	private PrefabPool<InventoryFilterDisplayEntry> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<InventoryFilterDisplayEntry>(this.template, null, null, null, null, true, 10, 10000, null);
			}
			return this._pool;
		}
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0003A531 File Offset: 0x00038731
	private void Awake()
	{
		this.template.gameObject.SetActive(false);
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0003A544 File Offset: 0x00038744
	public void Setup(InventoryDisplay target)
	{
		this.Pool.ReleaseAll();
		this.entries.Clear();
		if (target == null)
		{
			return;
		}
		this.targetDisplay = target;
		this.provider = target.Target.GetComponent<InventoryFilterProvider>();
		if (this.provider == null)
		{
			return;
		}
		foreach (InventoryFilterProvider.FilterEntry filter in this.provider.entries)
		{
			InventoryFilterDisplayEntry inventoryFilterDisplayEntry = this.Pool.Get(null);
			inventoryFilterDisplayEntry.Setup(new Action<InventoryFilterDisplayEntry, PointerEventData>(this.OnEntryClicked), filter);
			this.entries.Add(inventoryFilterDisplayEntry);
		}
		this.selection = null;
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0003A5ED File Offset: 0x000387ED
	private void OnEntryClicked(InventoryFilterDisplayEntry entry, PointerEventData data)
	{
		this.SetSelection(entry);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0003A5F7 File Offset: 0x000387F7
	internal void Select(int i)
	{
		if (i < 0 || i >= this.entries.Count)
		{
			return;
		}
		this.SetSelection(this.entries[i]);
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0003A61F File Offset: 0x0003881F
	public InventoryFilterDisplayEntry GetSelection()
	{
		return this.selection;
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0003A628 File Offset: 0x00038828
	public bool SetSelection(InventoryFilterDisplayEntry selection)
	{
		if (selection == null)
		{
			return false;
		}
		this.selection = selection;
		InventoryFilterProvider.FilterEntry filter = selection.Filter;
		this.targetDisplay.SetFilter(filter.GetFunction());
		foreach (InventoryFilterDisplayEntry inventoryFilterDisplayEntry in this.entries)
		{
			inventoryFilterDisplayEntry.NotifySelectionChanged(inventoryFilterDisplayEntry == selection);
		}
		return true;
	}

	// Token: 0x04000C09 RID: 3081
	[SerializeField]
	private InventoryFilterDisplayEntry template;

	// Token: 0x04000C0A RID: 3082
	private PrefabPool<InventoryFilterDisplayEntry> _pool;

	// Token: 0x04000C0B RID: 3083
	private InventoryDisplay targetDisplay;

	// Token: 0x04000C0C RID: 3084
	private InventoryFilterProvider provider;

	// Token: 0x04000C0D RID: 3085
	private List<InventoryFilterDisplayEntry> entries = new List<InventoryFilterDisplayEntry>();

	// Token: 0x04000C0E RID: 3086
	private InventoryFilterDisplayEntry selection;
}
