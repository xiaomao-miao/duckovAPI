using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x0200038F RID: 911
	public class InventoryEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IDropHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x0006F4F2 File Offset: 0x0006D6F2
		// (set) Token: 0x06001FDE RID: 8158 RVA: 0x0006F4FA File Offset: 0x0006D6FA
		public InventoryDisplay Master { get; private set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x0006F503 File Offset: 0x0006D703
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x0006F50B File Offset: 0x0006D70B
		// (set) Token: 0x06001FE1 RID: 8161 RVA: 0x0006F513 File Offset: 0x0006D713
		public bool Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
				this.Refresh();
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x0006F524 File Offset: 0x0006D724
		public Item Content
		{
			get
			{
				InventoryDisplay master = this.Master;
				Inventory inventory = (master != null) ? master.Target : null;
				if (inventory == null)
				{
					return null;
				}
				if (this.index >= inventory.Capacity)
				{
					return null;
				}
				InventoryDisplay master2 = this.Master;
				if (master2 == null)
				{
					return null;
				}
				Inventory target = master2.Target;
				if (target == null)
				{
					return null;
				}
				return target.GetItemAt(this.index);
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x0006F584 File Offset: 0x0006D784
		public bool ShouldHighlight
		{
			get
			{
				return !(this.Master == null) && !(this.Content == null) && (this.Master.EvaluateShouldHighlight(this.Content) || (this.Editable && ItemUIUtilities.IsGunSelected && !this.cacheContentIsGun && this.IsCaliberMatchItemSelected()));
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0006F5E5 File Offset: 0x0006D7E5
		private bool IsCaliberMatchItemSelected()
		{
			return !(this.Content == null) && ItemUIUtilities.SelectedItemCaliber == this.cachedMeta.caliber;
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x0006F60C File Offset: 0x0006D80C
		public bool CanOperate
		{
			get
			{
				return !(this.Master == null) && this.Master.Func_CanOperate(this.Content);
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x0006F634 File Offset: 0x0006D834
		public bool Editable
		{
			get
			{
				return !(this.Master == null) && this.Master.Editable && this.CanOperate;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x0006F65B File Offset: 0x0006D85B
		public bool Movable
		{
			get
			{
				return !(this.Master == null) && this.Master.Movable;
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06001FE8 RID: 8168 RVA: 0x0006F678 File Offset: 0x0006D878
		// (remove) Token: 0x06001FE9 RID: 8169 RVA: 0x0006F6B0 File Offset: 0x0006D8B0
		public event Action<InventoryEntry> onRefresh;

		// Token: 0x06001FEA RID: 8170 RVA: 0x0006F6E8 File Offset: 0x0006D8E8
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClicked;
			this.itemDisplay.onDoubleClicked += this.OnDisplayDoubleClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnDropItemButton;
			UIInputManager.OnUseItem += this.OnUseItemButton;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0006F780 File Offset: 0x0006D980
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex += this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput += this.OnShortcutInput;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0006F7B8 File Offset: 0x0006D9B8
		private void OnDisable()
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex -= this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput -= this.OnShortcutInput;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x0006F811 File Offset: 0x0006DA11
		private void OnShortcutInput(UIInputEventData data, int shortcutIndex)
		{
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			ItemShortcut.Set(shortcutIndex, this.Item);
			ItemUIUtilities.NotifyPutItem(this.Item, false);
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0006F844 File Offset: 0x0006DA44
		private void OnInputLockInventoryIndex(UIInputEventData data)
		{
			if (!this.hovering)
			{
				return;
			}
			this.ToggleLock();
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0006F855 File Offset: 0x0006DA55
		private void OnSelectionChanged()
		{
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			if (ItemUIUtilities.SelectedItemDisplay == this.itemDisplay)
			{
				this.Refresh();
			}
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0006F880 File Offset: 0x0006DA80
		private void OnDestroy()
		{
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnDropItemButton;
			UIInputManager.OnUseItem -= this.OnUseItemButton;
			if (this.itemDisplay != null)
			{
				this.itemDisplay.onPointerClick -= this.OnItemDisplayPointerClicked;
				this.itemDisplay.onDoubleClicked -= this.OnDisplayDoubleClicked;
				this.itemDisplay.onReceiveDrop -= this.OnDrop;
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0006F914 File Offset: 0x0006DB14
		private void OnFastPick(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.Master.NotifyItemDoubleClicked(this, new PointerEventData(EventSystem.current));
			data.Use();
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0006F950 File Offset: 0x0006DB50
		private void OnDropItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.CanDrop)
			{
				return;
			}
			if (this.CanOperate)
			{
				this.Item.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0006F9A8 File Offset: 0x0006DBA8
		private void OnUseItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			if (this.CanOperate)
			{
				CharacterMainControl.Main.UseItem(this.Item);
			}
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0006FA04 File Offset: 0x0006DC04
		private void OnItemDisplayPointerClicked(ItemDisplay display, PointerEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.disabled || !this.CanOperate)
			{
				data.Use();
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (data.button == PointerEventData.InputButton.Left)
			{
				if (this.Content == null)
				{
					return;
				}
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					data.Use();
					if (ItemUIUtilities.SelectedItem != null)
					{
						ItemUIUtilities.SelectedItem.TryPlug(this.Content, false, null, 0);
					}
					CharacterMainControl.Main.CharacterItem.TryPlug(this.Content, false, null, 0);
					return;
				}
				if (ItemUIUtilities.SelectedItem == null)
				{
					return;
				}
				if (this.Content.Stackable && ItemUIUtilities.SelectedItem != this.Content && ItemUIUtilities.SelectedItem.TypeID == this.Content.TypeID)
				{
					ItemUIUtilities.SelectedItem.CombineInto(this.Content);
					return;
				}
			}
			else if (data.button == PointerEventData.InputButton.Right && this.Editable && this.Content != null)
			{
				ItemOperationMenu.Show(this.itemDisplay);
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0006FB2C File Offset: 0x0006DD2C
		private void OnDisplayDoubleClicked(ItemDisplay display, PointerEventData data)
		{
			this.Master.NotifyItemDoubleClicked(this, data);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0006FB3B File Offset: 0x0006DD3B
		public void Setup(InventoryDisplay master, int index, bool disabled = false)
		{
			this.Master = master;
			this.index = index;
			this.disabled = disabled;
			this.Refresh();
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0006FB58 File Offset: 0x0006DD58
		internal void Refresh()
		{
			Item content = this.Content;
			if (content != null)
			{
				this.cachedMeta = ItemAssetsCollection.GetMetaData(content.TypeID);
				this.cacheContentIsGun = content.Tags.Contains("Gun");
			}
			else
			{
				this.cacheContentIsGun = false;
				this.cachedMeta = default(ItemMetaData);
			}
			this.itemDisplay.Setup(content);
			this.itemDisplay.CanDrop = this.CanOperate;
			this.itemDisplay.Movable = this.Movable;
			this.itemDisplay.Editable = (this.Editable && this.CanOperate);
			this.itemDisplay.CanLockSort = true;
			if (!this.Master.Target.NeedInspection && content != null)
			{
				content.Inspected = true;
			}
			this.itemDisplay.ShowOperationButtons = this.Master.ShowOperationButtons;
			this.shortcutIndicator.gameObject.SetActive(this.Master.IsShortcut(this.index));
			this.disabledIndicator.SetActive(this.disabled || !this.CanOperate);
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			bool active = this.Master.Target.IsIndexLocked(this.Index);
			this.lockIndicator.SetActive(active);
			Action<InventoryEntry> action = this.onRefresh;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x0006FCC4 File Offset: 0x0006DEC4
		public static PrefabPool<InventoryEntry> Pool
		{
			get
			{
				return GameplayUIManager.Instance.InventoryEntryPool;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0006FCD0 File Offset: 0x0006DED0
		public Item Item
		{
			get
			{
				if (this.itemDisplay != null && this.itemDisplay.isActiveAndEnabled)
				{
					return this.itemDisplay.Target;
				}
				return null;
			}
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0006FCFA File Offset: 0x0006DEFA
		public static InventoryEntry Get()
		{
			return InventoryEntry.Pool.Get(null);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0006FD07 File Offset: 0x0006DF07
		public static void Release(InventoryEntry item)
		{
			InventoryEntry.Pool.Release(item);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0006FD14 File Offset: 0x0006DF14
		public void NotifyPooled()
		{
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0006FD16 File Offset: 0x0006DF16
		public void NotifyReleased()
		{
			this.Master = null;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0006FD20 File Offset: 0x0006DF20
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.lastClickTime = eventData.clickTime;
				if (this.Editable)
				{
					Item selectedItem = ItemUIUtilities.SelectedItem;
					if (!(selectedItem == null))
					{
						if (this.Content != null)
						{
							Debug.Log(string.Format("{0}(Inventory) 的 {1} 已经有物品。操作已取消。", this.Master.Target.name, this.index));
						}
						else
						{
							eventData.Use();
							selectedItem.Detach();
							this.Master.Target.AddAt(selectedItem, this.index);
							ItemUIUtilities.NotifyPutItem(selectedItem, false);
						}
					}
				}
				this.lastClickTime = eventData.clickTime;
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0006FDD2 File Offset: 0x0006DFD2
		internal void Punch()
		{
			this.itemDisplay.Punch();
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0006FDDF File Offset: 0x0006DFDF
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0006FDE4 File Offset: 0x0006DFE4
		public void OnDrop(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
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
			if (item.Sticky && !this.Master.Target.AcceptSticky)
			{
				return;
			}
			if (Keyboard.current != null && Keyboard.current.ctrlKey.isPressed)
			{
				if (this.Content != null)
				{
					NotificationText.Push("UI_Inventory_TargetOccupiedCannotSplit".ToPlainText());
					return;
				}
				Debug.Log("SPLIT");
				SplitDialogue.SetupAndShow(item, this.Master.Target, this.index);
				return;
			}
			else
			{
				ItemUIUtilities.NotifyPutItem(item, false);
				if (this.Content == null)
				{
					item.Detach();
					this.Master.Target.AddAt(item, this.index);
					return;
				}
				if (this.Content.TypeID == item.TypeID && this.Content.Stackable)
				{
					this.Content.Combine(item);
					return;
				}
				Inventory inInventory = item.InInventory;
				Inventory target = this.Master.Target;
				if (inInventory != null)
				{
					int atPosition = inInventory.GetIndex(item);
					int atPosition2 = this.index;
					Item content = this.Content;
					if (content != item)
					{
						item.Detach();
						content.Detach();
						inInventory.AddAt(content, atPosition);
						target.AddAt(item, atPosition2);
					}
				}
				return;
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0006FF73 File Offset: 0x0006E173
		public bool IsEditable()
		{
			return !(this.Content == null) && !this.Content.NeedInspection && this.Editable;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0006FF9A File Offset: 0x0006E19A
		public Item GetItem()
		{
			return this.Content;
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0006FFA2 File Offset: 0x0006E1A2
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(this.Editable);
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0006FFC1 File Offset: 0x0006E1C1
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0006FFDB File Offset: 0x0006E1DB
		public void ToggleLock()
		{
			this.Master.Target.ToggleLockIndex(this.Index);
		}

		// Token: 0x040015C1 RID: 5569
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040015C2 RID: 5570
		[SerializeField]
		private GameObject shortcutIndicator;

		// Token: 0x040015C3 RID: 5571
		[SerializeField]
		private GameObject disabledIndicator;

		// Token: 0x040015C4 RID: 5572
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x040015C5 RID: 5573
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x040015C6 RID: 5574
		[SerializeField]
		private GameObject lockIndicator;

		// Token: 0x040015C8 RID: 5576
		[SerializeField]
		private int index;

		// Token: 0x040015C9 RID: 5577
		[SerializeField]
		private bool disabled;

		// Token: 0x040015CB RID: 5579
		private bool cacheContentIsGun;

		// Token: 0x040015CC RID: 5580
		private ItemMetaData cachedMeta;

		// Token: 0x040015CD RID: 5581
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x040015CE RID: 5582
		private float lastClickTime;

		// Token: 0x040015CF RID: 5583
		private bool hovering;
	}
}
