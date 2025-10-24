using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038E RID: 910
	public class InventoryDisplay : MonoBehaviour, IPoolable
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0006EA9B File Offset: 0x0006CC9B
		private bool shortcuts
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0006EA9E File Offset: 0x0006CC9E
		public bool UsePages
		{
			get
			{
				return this.usePages;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x0006EAA6 File Offset: 0x0006CCA6
		// (set) Token: 0x06001FAC RID: 8108 RVA: 0x0006EAAE File Offset: 0x0006CCAE
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x0006EAB7 File Offset: 0x0006CCB7
		// (set) Token: 0x06001FAE RID: 8110 RVA: 0x0006EABF File Offset: 0x0006CCBF
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x0006EAC8 File Offset: 0x0006CCC8
		// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x0006EAD0 File Offset: 0x0006CCD0
		public bool Movable { get; private set; }

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x0006EAD9 File Offset: 0x0006CCD9
		// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x0006EAE1 File Offset: 0x0006CCE1
		public Inventory Target { get; private set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x0006EAEC File Offset: 0x0006CCEC
		private PrefabPool<InventoryEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null && this.entryPrefab != null)
				{
					this._entryPool = new PrefabPool<InventoryEntry>(this.entryPrefab, this.contentLayout.transform, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06001FB4 RID: 8116 RVA: 0x0006EB40 File Offset: 0x0006CD40
		// (remove) Token: 0x06001FB5 RID: 8117 RVA: 0x0006EB78 File Offset: 0x0006CD78
		public event Action<InventoryDisplay, InventoryEntry, PointerEventData> onDisplayDoubleClicked;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06001FB6 RID: 8118 RVA: 0x0006EBB0 File Offset: 0x0006CDB0
		// (remove) Token: 0x06001FB7 RID: 8119 RVA: 0x0006EBE8 File Offset: 0x0006CDE8
		public event Action onPageInfoRefreshed;

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x0006EC1D File Offset: 0x0006CE1D
		public Func<Item, bool> Func_ShouldHighlight
		{
			get
			{
				return this._func_ShouldHighlight;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001FB9 RID: 8121 RVA: 0x0006EC25 File Offset: 0x0006CE25
		public Func<Item, bool> Func_CanOperate
		{
			get
			{
				return this._func_CanOperate;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x0006EC2D File Offset: 0x0006CE2D
		// (set) Token: 0x06001FBB RID: 8123 RVA: 0x0006EC35 File Offset: 0x0006CE35
		public bool ShowSortButton
		{
			get
			{
				return this.showSortButton;
			}
			internal set
			{
				this.showSortButton = value;
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0006EC40 File Offset: 0x0006CE40
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onContentChanged += this.OnTargetContentChanged;
			this.Target.onInventorySorted += this.OnTargetSorted;
			this.Target.onSetIndexLock += this.OnTargetSetIndexLock;
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0006ECA8 File Offset: 0x0006CEA8
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onContentChanged -= this.OnTargetContentChanged;
			this.Target.onInventorySorted -= this.OnTargetSorted;
			this.Target.onSetIndexLock -= this.OnTargetSetIndexLock;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0006ED0C File Offset: 0x0006CF0C
		private void OnTargetSetIndexLock(Inventory inventory, int index)
		{
			foreach (InventoryEntry inventoryEntry in this.entries)
			{
				if (!(inventoryEntry == null) && inventoryEntry.isActiveAndEnabled && inventoryEntry.Index == index)
				{
					inventoryEntry.Refresh();
				}
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0006ED78 File Offset: 0x0006CF78
		private void OnTargetSorted(Inventory inventory)
		{
			if (this.filter == null)
			{
				using (List<InventoryEntry>.Enumerator enumerator = this.entries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InventoryEntry inventoryEntry = enumerator.Current;
						inventoryEntry.Refresh();
					}
					return;
				}
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0006EDDC File Offset: 0x0006CFDC
		private void OnTargetContentChanged(Inventory inventory, int position)
		{
			if (this.Target.Loading)
			{
				return;
			}
			if (this.filter != null)
			{
				this.RefreshCapacityText();
				this.LoadEntriesTask().Forget();
				return;
			}
			this.RefreshCapacityText();
			InventoryEntry inventoryEntry = this.entries.Find((InventoryEntry e) => e != null && e.Index == position);
			if (!inventoryEntry)
			{
				return;
			}
			InventoryEntry inventoryEntry2 = inventoryEntry;
			inventoryEntry2.Refresh();
			inventoryEntry2.Punch();
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0006EE54 File Offset: 0x0006D054
		private void RefreshCapacityText()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.capacityText)
			{
				return;
			}
			this.capacityText.text = string.Format(this.capacityTextFormat, this.Target.Capacity, this.Target.GetItemCount());
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0006EEB4 File Offset: 0x0006D0B4
		public void Setup(Inventory target, Func<Item, bool> funcShouldHighLight = null, Func<Item, bool> funcCanOperate = null, bool movable = false, Func<Item, bool> filter = null)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			if (funcShouldHighLight == null)
			{
				this._func_ShouldHighlight = ((Item e) => false);
			}
			else
			{
				this._func_ShouldHighlight = funcShouldHighLight;
			}
			if (funcCanOperate == null)
			{
				this._func_CanOperate = ((Item e) => true);
			}
			else
			{
				this._func_CanOperate = funcCanOperate;
			}
			this.displayNameText.text = target.DisplayName;
			this.Movable = movable;
			this.cachedCapacity = target.Capacity;
			this.filter = filter;
			this.RefreshCapacityText();
			this.RegisterEvents();
			this.sortButton.gameObject.SetActive(this.editable && this.showSortButton);
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0006EFB8 File Offset: 0x0006D1B8
		private void RefreshGridLayoutPreferredHeight()
		{
			if (this.Target == null)
			{
				this.placeHolder.gameObject.SetActive(true);
				return;
			}
			int num = this.cachedIndexesToDisplay.Count;
			if (this.usePages && num > 0)
			{
				int num2 = this.cachedSelectedPage * this.itemsEachPage;
				int num3 = Mathf.Min(num2 + this.itemsEachPage, this.cachedIndexesToDisplay.Count);
				num = Mathf.Max(0, num3 - num2);
			}
			float preferredHeight = (float)Mathf.CeilToInt((float)num / (float)this.contentLayout.constraintCount) * this.contentLayout.cellSize.y + (float)this.contentLayout.padding.top + (float)this.contentLayout.padding.bottom;
			this.gridLayoutElement.preferredHeight = preferredHeight;
			this.placeHolder.gameObject.SetActive(num <= 0);
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x0006F09C File Offset: 0x0006D29C
		public int MaxPage
		{
			get
			{
				return this.cachedMaxPage;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0006F0A4 File Offset: 0x0006D2A4
		public int SelectedPage
		{
			get
			{
				return this.cachedSelectedPage;
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0006F0AC File Offset: 0x0006D2AC
		public void SetPage(int page)
		{
			this.cachedSelectedPage = page;
			Action action = this.onPageInfoRefreshed;
			if (action != null)
			{
				action();
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0006F0D4 File Offset: 0x0006D2D4
		public void NextPage()
		{
			int num = this.cachedSelectedPage + 1;
			if (num >= this.cachedMaxPage)
			{
				num = 0;
			}
			this.SetPage(num);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0006F0FC File Offset: 0x0006D2FC
		public void PreviousPage()
		{
			int num = this.cachedSelectedPage - 1;
			if (num < 0)
			{
				num = this.cachedMaxPage - 1;
			}
			this.SetPage(num);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0006F128 File Offset: 0x0006D328
		private void CacheIndexesToDisplay()
		{
			this.cachedIndexesToDisplay.Clear();
			int i = 0;
			while (i < this.Target.Capacity)
			{
				if (this.filter == null)
				{
					goto IL_32;
				}
				Item itemAt = this.Target.GetItemAt(i);
				if (this.filter(itemAt))
				{
					goto IL_32;
				}
				IL_3E:
				i++;
				continue;
				IL_32:
				this.cachedIndexesToDisplay.Add(i);
				goto IL_3E;
			}
			int count = this.cachedIndexesToDisplay.Count;
			this.cachedMaxPage = count / this.itemsEachPage + ((count % this.itemsEachPage > 0) ? 1 : 0);
			if (this.cachedSelectedPage >= this.cachedMaxPage)
			{
				this.cachedSelectedPage = Mathf.Max(0, this.cachedMaxPage - 1);
			}
			Action action = this.onPageInfoRefreshed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0006F1E4 File Offset: 0x0006D3E4
		private UniTask LoadEntriesTask()
		{
			InventoryDisplay.<LoadEntriesTask>d__76 <LoadEntriesTask>d__;
			<LoadEntriesTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadEntriesTask>d__.<>4__this = this;
			<LoadEntriesTask>d__.<>1__state = -1;
			<LoadEntriesTask>d__.<>t__builder.Start<InventoryDisplay.<LoadEntriesTask>d__76>(ref <LoadEntriesTask>d__);
			return <LoadEntriesTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0006F227 File Offset: 0x0006D427
		public void SetFilter(Func<Item, bool> filter)
		{
			this.filter = filter;
			this.cachedSelectedPage = 0;
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0006F242 File Offset: 0x0006D442
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			this.entries.Clear();
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0006F25A File Offset: 0x0006D45A
		private void Awake()
		{
			this.sortButton.onClick.AddListener(new UnityAction(this.OnSortButtonClicked));
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0006F278 File Offset: 0x0006D478
		private void OnSortButtonClicked()
		{
			if (!this.Editable)
			{
				return;
			}
			if (!this.Target)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			this.Target.Sort();
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0006F2AA File Offset: 0x0006D4AA
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0006F2B2 File Offset: 0x0006D4B2
		private void OnDisable()
		{
			this.UnregisterEvents();
			this.activeTaskToken++;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0006F2C8 File Offset: 0x0006D4C8
		private void Update()
		{
			if (this.Target && this.cachedCapacity != this.Target.Capacity)
			{
				this.OnCapacityChanged();
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0006F2F0 File Offset: 0x0006D4F0
		private void OnCapacityChanged()
		{
			if (this.Target == null)
			{
				return;
			}
			this.cachedCapacity = this.Target.Capacity;
			this.RefreshCapacityText();
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0006F323 File Offset: 0x0006D523
		public bool IsShortcut(int index)
		{
			return this.shortcuts && index >= this.shortcutsRange.x && index <= this.shortcutsRange.y;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0006F350 File Offset: 0x0006D550
		private InventoryEntry GetNewInventoryEntry()
		{
			return this.EntryPool.Get(null);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0006F35E File Offset: 0x0006D55E
		internal void NotifyItemDoubleClicked(InventoryEntry inventoryEntry, PointerEventData data)
		{
			Action<InventoryDisplay, InventoryEntry, PointerEventData> action = this.onDisplayDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, inventoryEntry, data);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0006F373 File Offset: 0x0006D573
		public void NotifyPooled()
		{
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0006F375 File Offset: 0x0006D575
		public void NotifyReleased()
		{
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0006F378 File Offset: 0x0006D578
		public void DisableItem(Item item)
		{
			foreach (InventoryEntry inventoryEntry in from e in this.entries
			where e.Content == item
			select e)
			{
				inventoryEntry.Disabled = true;
			}
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0006F3E4 File Offset: 0x0006D5E4
		internal bool EvaluateShouldHighlight(Item content)
		{
			if (this.Func_ShouldHighlight != null && this.Func_ShouldHighlight(content))
			{
				return true;
			}
			content == null;
			return false;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0006F46D File Offset: 0x0006D66D
		[CompilerGenerated]
		private bool <LoadEntriesTask>g__TaskValid|76_0(ref InventoryDisplay.<>c__DisplayClass76_0 A_1)
		{
			return Application.isPlaying && A_1.token == this.activeTaskToken;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0006F488 File Offset: 0x0006D688
		[CompilerGenerated]
		private List<int> <LoadEntriesTask>g__GetRange|76_1(int begin, int end_exclusive, List<int> list, ref InventoryDisplay.<>c__DisplayClass76_0 A_4)
		{
			if (begin < 0)
			{
				begin = 0;
			}
			if (end_exclusive < 0)
			{
				end_exclusive = 0;
			}
			A_4.indexes = new List<int>();
			if (end_exclusive > list.Count)
			{
				end_exclusive = list.Count;
			}
			if (begin >= end_exclusive)
			{
				return A_4.indexes;
			}
			for (int i = begin; i < end_exclusive; i++)
			{
				A_4.indexes.Add(list[i]);
			}
			return A_4.indexes;
		}

		// Token: 0x040015A2 RID: 5538
		[SerializeField]
		private InventoryEntry entryPrefab;

		// Token: 0x040015A3 RID: 5539
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040015A4 RID: 5540
		[SerializeField]
		private TextMeshProUGUI capacityText;

		// Token: 0x040015A5 RID: 5541
		[SerializeField]
		private string capacityTextFormat = "({1}/{0})";

		// Token: 0x040015A6 RID: 5542
		[SerializeField]
		private FadeGroup loadingIndcator;

		// Token: 0x040015A7 RID: 5543
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x040015A8 RID: 5544
		[SerializeField]
		private GridLayoutGroup contentLayout;

		// Token: 0x040015A9 RID: 5545
		[SerializeField]
		private LayoutElement gridLayoutElement;

		// Token: 0x040015AA RID: 5546
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x040015AB RID: 5547
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x040015AC RID: 5548
		[SerializeField]
		private Button sortButton;

		// Token: 0x040015AD RID: 5549
		[SerializeField]
		private Vector2Int shortcutsRange = new Vector2Int(0, 3);

		// Token: 0x040015AE RID: 5550
		[SerializeField]
		private bool editable = true;

		// Token: 0x040015AF RID: 5551
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		private bool showSortButton;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		private bool usePages;

		// Token: 0x040015B2 RID: 5554
		[SerializeField]
		private int itemsEachPage = 30;

		// Token: 0x040015B3 RID: 5555
		public Func<Item, bool> filter;

		// Token: 0x040015B6 RID: 5558
		[SerializeField]
		private List<InventoryEntry> entries = new List<InventoryEntry>();

		// Token: 0x040015B7 RID: 5559
		private PrefabPool<InventoryEntry> _entryPool;

		// Token: 0x040015BA RID: 5562
		private Func<Item, bool> _func_ShouldHighlight;

		// Token: 0x040015BB RID: 5563
		private Func<Item, bool> _func_CanOperate;

		// Token: 0x040015BC RID: 5564
		private int cachedCapacity = -1;

		// Token: 0x040015BD RID: 5565
		private int activeTaskToken;

		// Token: 0x040015BE RID: 5566
		private int cachedMaxPage = 1;

		// Token: 0x040015BF RID: 5567
		private int cachedSelectedPage;

		// Token: 0x040015C0 RID: 5568
		private List<int> cachedIndexesToDisplay = new List<int>();
	}
}
