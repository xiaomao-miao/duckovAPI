using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003AA RID: 938
	public class ItemShortcutEditorEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x000758B0 File Offset: 0x00073AB0
		private Item TargetItem
		{
			get
			{
				return ItemShortcut.Get(this.index);
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000758C0 File Offset: 0x00073AC0
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			ItemShortcut.OnSetItem += this.OnSetItem;
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00075919 File Offset: 0x00073B19
		private void OnSetItem(int index)
		{
			if (index == this.index)
			{
				this.Refresh();
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0007592A File Offset: 0x00073B2A
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
			data.Use();
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00075939 File Offset: 0x00073B39
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ItemUIUtilities.SelectedItem != null && ItemShortcut.Set(this.index, ItemUIUtilities.SelectedItem))
			{
				this.Refresh();
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00075960 File Offset: 0x00073B60
		internal void Refresh()
		{
			this.UnregisterEvents();
			if (this.displayingItem != this.TargetItem)
			{
				this.itemDisplay.Punch();
			}
			this.displayingItem = this.TargetItem;
			this.itemDisplay.Setup(this.displayingItem);
			this.itemDisplay.ShowOperationButtons = false;
			this.RegisterEvents();
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000759C0 File Offset: 0x00073BC0
		private void RegisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged += this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount += this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000759FE File Offset: 0x00073BFE
		private void UnregisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged -= this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount -= this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00075A3C File Offset: 0x00073C3C
		private void OnTargetStackCountChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00075A44 File Offset: 0x00073C44
		private void OnTargetParentChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00075A4C File Offset: 0x00073C4C
		private void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00075A55 File Offset: 0x00073C55
		private void Update()
		{
			if (this.dirty)
			{
				this.Refresh();
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00075A65 File Offset: 0x00073C65
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemShortcut.OnSetItem -= this.OnSetItem;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00075A80 File Offset: 0x00073C80
		internal void Setup(int i)
		{
			this.index = i;
			this.Refresh();
			InputActionReference inputActionRef = InputActionReference.Create(GameplayDataSettings.InputActions[string.Format("Character/ItemShortcut{0}", i + 3)]);
			this.indicator.Setup(inputActionRef, -1);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x00075ACC File Offset: 0x00073CCC
		public void OnDrop(PointerEventData eventData)
		{
			eventData.Use();
			IItemDragSource component = eventData.pointerDrag.gameObject.GetComponent<IItemDragSource>();
			if (component == null)
			{
				return;
			}
			if (!component.IsEditable())
			{
				return;
			}
			Item item = component.GetItem();
			if (item == null)
			{
				return;
			}
			if (!item.IsInPlayerCharacter())
			{
				ItemUtilities.SendToPlayer(item, false, false);
			}
			if (ItemShortcut.Set(this.index, item))
			{
				this.Refresh();
				AudioManager.Post("UI/click");
			}
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00075B3D File Offset: 0x00073D3D
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(true);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00075B4B File Offset: 0x00073D4B
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00075B59 File Offset: 0x00073D59
		public bool IsEditable()
		{
			return this.TargetItem != null;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00075B67 File Offset: 0x00073D67
		public Item GetItem()
		{
			return this.TargetItem;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x00075B6F File Offset: 0x00073D6F
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x040016C8 RID: 5832
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040016C9 RID: 5833
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x040016CA RID: 5834
		[SerializeField]
		private int index;

		// Token: 0x040016CB RID: 5835
		[SerializeField]
		private InputIndicator indicator;

		// Token: 0x040016CC RID: 5836
		private Item displayingItem;

		// Token: 0x040016CD RID: 5837
		private bool dirty;
	}
}
