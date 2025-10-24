using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200039C RID: 924
	public class SlotDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x060020D6 RID: 8406 RVA: 0x000729B4 File Offset: 0x00070BB4
		// (remove) Token: 0x060020D7 RID: 8407 RVA: 0x000729EC File Offset: 0x00070BEC
		internal event Action<SlotDisplay> onSlotDisplayClicked;

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x060020D8 RID: 8408 RVA: 0x00072A24 File Offset: 0x00070C24
		// (remove) Token: 0x060020D9 RID: 8409 RVA: 0x00072A5C File Offset: 0x00070C5C
		internal event Action<SlotDisplay> onSlotDisplayDoubleClicked;

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x00072A91 File Offset: 0x00070C91
		// (set) Token: 0x060020DB RID: 8411 RVA: 0x00072A99 File Offset: 0x00070C99
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

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x00072AA2 File Offset: 0x00070CA2
		// (set) Token: 0x060020DD RID: 8413 RVA: 0x00072AAA File Offset: 0x00070CAA
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			internal set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00072AB3 File Offset: 0x00070CB3
		// (set) Token: 0x060020DF RID: 8415 RVA: 0x00072ABB File Offset: 0x00070CBB
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
			internal set
			{
				this.showOperationMenu = value;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00072AC4 File Offset: 0x00070CC4
		// (set) Token: 0x060020E1 RID: 8417 RVA: 0x00072ACC File Offset: 0x00070CCC
		public Slot Target { get; private set; }

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x060020E2 RID: 8418 RVA: 0x00072AD8 File Offset: 0x00070CD8
		// (remove) Token: 0x060020E3 RID: 8419 RVA: 0x00072B0C File Offset: 0x00070D0C
		public static event Action<SlotDisplayOperationContext> onOperation;

		// Token: 0x060020E4 RID: 8420 RVA: 0x00072B40 File Offset: 0x00070D40
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged += this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked += this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem += this.OnStartDragItem;
			IItemDragSource.OnEndDragItem += this.OnEndDragItem;
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnFastDrop;
			UIInputManager.OnUseItem += this.OnFastUse;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00072C10 File Offset: 0x00070E10
		private void UnregisterEvents()
		{
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged -= this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked -= this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
			IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnFastDrop;
			UIInputManager.OnUseItem -= this.OnFastUse;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00072CD0 File Offset: 0x00070ED0
		private void OnFastDrop(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.CanDrop)
			{
				return;
			}
			if (this.Editable)
			{
				this.Target.Content.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00072D40 File Offset: 0x00070F40
		private void OnFastUse(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			CharacterMainControl.Main.UseItem(this.Target.Content);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00072DA9 File Offset: 0x00070FA9
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.OnItemDisplayDoubleClicked(this.itemDisplay, new PointerEventData(EventSystem.current));
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00072DD3 File Offset: 0x00070FD3
		private void OnEndDragItem(Item item)
		{
			this.pluggableIndicator.Hide();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00072DE0 File Offset: 0x00070FE0
		private void OnStartDragItem(Item item)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (item != this.Target.Content && this.Target.CanPlug(item))
			{
				this.pluggableIndicator.Show();
				return;
			}
			this.pluggableIndicator.Hide();
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00072E37 File Offset: 0x00071037
		private void OnItemDisplayDoubleClicked(ItemDisplay arg1, PointerEventData arg2)
		{
			Action<SlotDisplay> action = this.onSlotDisplayDoubleClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.ContentSelectable)
			{
				arg2.Use();
			}
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00072E5C File Offset: 0x0007105C
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (data.button == PointerEventData.InputButton.Left)
			{
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					if (this.Editable && this.Target.Content != null)
					{
						Item content = this.Target.Content;
						content.Detach();
						if (!ItemUtilities.SendToPlayerCharacterInventory(content, false))
						{
							if (PlayerStorage.IsAccessableAndNotFull())
							{
								ItemUtilities.SendToPlayerStorage(content, false);
							}
							else
							{
								ItemUtilities.SendToPlayer(content, false, false);
							}
						}
						data.Use();
						return;
					}
				}
				else if (!this.ContentSelectable)
				{
					data.Use();
					return;
				}
			}
			else if (data.button == PointerEventData.InputButton.Right && this.Editable)
			{
				Slot target = this.Target;
				if (((target != null) ? target.Content : null) != null)
				{
					ItemOperationMenu.Show(this.itemDisplay);
				}
			}
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00072F38 File Offset: 0x00071138
		private void OnTargetContentChanged(Slot slot)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00072F46 File Offset: 0x00071146
		private void OnItemSelectionChanged()
		{
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00072F48 File Offset: 0x00071148
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.label.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
			this.pluggableIndicator.Hide();
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00072F80 File Offset: 0x00071180
		private void Refresh()
		{
			if (this.Target.Content == null)
			{
				this.slotIcon.gameObject.SetActive(true);
				if (this.Target.SlotIcon != null)
				{
					this.slotIcon.sprite = this.Target.SlotIcon;
				}
				else
				{
					this.slotIcon.sprite = this.defaultSlotIcon;
				}
			}
			else
			{
				this.slotIcon.gameObject.SetActive(false);
			}
			this.itemDisplay.ShowOperationButtons = this.showOperationMenu;
			this.itemDisplay.Setup(this.Target.Content);
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x00073027 File Offset: 0x00071227
		public static PrefabPool<SlotDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.SlotDisplayPool;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00073033 File Offset: 0x00071233
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x00073040 File Offset: 0x00071240
		public bool Movable
		{
			get
			{
				return this.itemDisplay.Movable;
			}
			set
			{
				this.itemDisplay.Movable = value;
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x0007304E File Offset: 0x0007124E
		public static SlotDisplay Get()
		{
			return SlotDisplay.Pool.Get(null);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x0007305B File Offset: 0x0007125B
		public static void Release(SlotDisplay item)
		{
			SlotDisplay.Pool.Release(item);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00073068 File Offset: 0x00071268
		public void NotifyPooled()
		{
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x0007306A File Offset: 0x0007126A
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00073079 File Offset: 0x00071279
		private void Awake()
		{
			this.itemDisplay.onReceiveDrop += this.OnDrop;
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00073093 File Offset: 0x00071293
		private void OnEnable()
		{
			this.RegisterEvents();
			this.iconInitialColor = this.slotIcon.color;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000730BD File Offset: 0x000712BD
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000730C8 File Offset: 0x000712C8
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.Editable)
			{
				this.Punch();
				eventData.Use();
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					this.Punch();
					return;
				}
				if (this.Target.Content != null)
				{
					Debug.Log("槽位 " + this.Target.DisplayName + " 中已经有物品。操作已取消。");
					this.DenialPunch();
					return;
				}
				if (!this.Target.CanPlug(selectedItem))
				{
					Debug.Log(string.Concat(new string[]
					{
						"物品 ",
						selectedItem.DisplayName,
						" 未通过槽位 ",
						this.Target.DisplayName,
						" 安装检测。操作已取消。"
					}));
					this.DenialPunch();
					return;
				}
				eventData.Use();
				selectedItem.Detach();
				Item item;
				this.Target.Plug(selectedItem, out item);
				ItemUIUtilities.NotifyPutItem(selectedItem, false);
				if (item != null)
				{
					ItemUIUtilities.RaiseOrphan(item);
				}
				this.Punch();
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000731E4 File Offset: 0x000713E4
		public void Punch()
		{
			if (this.slotIcon != null)
			{
				this.slotIcon.transform.DOKill(false);
				this.slotIcon.color = this.iconInitialColor;
				this.slotIcon.transform.localScale = Vector3.one;
				this.slotIcon.transform.DOPunchScale(Vector3.one * this.slotIconPunchScale, this.punchDuration, 10, 1f);
			}
			if (this.itemDisplay != null)
			{
				this.itemDisplay.Punch();
			}
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00073280 File Offset: 0x00071480
		public void DenialPunch()
		{
			if (this.slotIcon == null)
			{
				return;
			}
			this.slotIcon.transform.DOKill(false);
			this.slotIcon.color = this.iconInitialColor;
			this.slotIcon.DOColor(this.slotIconDenialColor, this.denialPunchDuration).From<TweenerCore<Color, Color, ColorOptions>>();
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Deny, false));
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000732F3 File Offset: 0x000714F3
		public bool IsEditable()
		{
			return this.Editable;
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000732FB File Offset: 0x000714FB
		public Item GetItem()
		{
			Slot target = this.Target;
			if (target == null)
			{
				return null;
			}
			return target.Content;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00073310 File Offset: 0x00071510
		public void OnDrop(PointerEventData eventData)
		{
			if (!this.Editable)
			{
				return;
			}
			if (eventData.used)
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
			if (this.SetAmmo(item))
			{
				return;
			}
			if (!this.Target.CanPlug(item))
			{
				Debug.Log(string.Concat(new string[]
				{
					"物品 ",
					item.DisplayName,
					" 未通过槽位 ",
					this.Target.DisplayName,
					" 安装检测。操作已取消。"
				}));
				this.DenialPunch();
				return;
			}
			Inventory inInventory = item.InInventory;
			Slot pluggedIntoSlot = item.PluggedIntoSlot;
			if (pluggedIntoSlot == this.Target)
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			Item item2;
			bool succeed = this.Target.Plug(item, out item2);
			if (item2 != null && (!(inInventory != null) || !inInventory.AddAndMerge(item2, 0)))
			{
				Item item3;
				if (pluggedIntoSlot != null && pluggedIntoSlot.CanPlug(item2) && pluggedIntoSlot.Plug(item2, out item3))
				{
					if (item3)
					{
						Debug.LogError("Source slot spit out an unplugged item! " + item3.DisplayName);
					}
				}
				else if (!ItemUtilities.SendToPlayerCharacter(item2, false))
				{
					LootView lootView = View.ActiveView as LootView;
					if (lootView == null || !(lootView.TargetInventory != null) || !lootView.TargetInventory.AddAndMerge(item2, 0))
					{
						if (PlayerStorage.IsAccessableAndNotFull())
						{
							ItemUtilities.SendToPlayerStorage(item2, false);
						}
						else
						{
							item2.Drop(CharacterMainControl.Main, true);
						}
					}
				}
			}
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Equip, succeed));
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000734C5 File Offset: 0x000716C5
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000734C7 File Offset: 0x000716C7
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

		// Token: 0x06002103 RID: 8451 RVA: 0x000734E6 File Offset: 0x000716E6
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

		// Token: 0x06002104 RID: 8452 RVA: 0x00073500 File Offset: 0x00071700
		private bool SetAmmo(Item incomming)
		{
			Slot target = this.Target;
			ItemSetting_Gun itemSetting_Gun;
			if (target == null)
			{
				itemSetting_Gun = null;
			}
			else
			{
				Item content = target.Content;
				itemSetting_Gun = ((content != null) ? content.GetComponent<ItemSetting_Gun>() : null);
			}
			ItemSetting_Gun itemSetting_Gun2 = itemSetting_Gun;
			if (itemSetting_Gun2 == null)
			{
				return false;
			}
			if (!itemSetting_Gun2.IsValidBullet(incomming))
			{
				return false;
			}
			if (View.ActiveView is InventoryView || View.ActiveView is LootView)
			{
				View.ActiveView.Close();
			}
			return itemSetting_Gun2.LoadSpecificBullet(incomming);
		}

		// Token: 0x04001659 RID: 5721
		[SerializeField]
		private Sprite defaultSlotIcon;

		// Token: 0x0400165A RID: 5722
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x0400165B RID: 5723
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400165C RID: 5724
		[SerializeField]
		private Image slotIcon;

		// Token: 0x0400165D RID: 5725
		[SerializeField]
		private FadeGroup pluggableIndicator;

		// Token: 0x0400165E RID: 5726
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x0400165F RID: 5727
		[SerializeField]
		private bool editable = true;

		// Token: 0x04001660 RID: 5728
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x04001661 RID: 5729
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x04001664 RID: 5732
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.1f;

		// Token: 0x04001665 RID: 5733
		[SerializeField]
		[Range(-1f, 1f)]
		private float slotIconPunchScale = -0.1f;

		// Token: 0x04001666 RID: 5734
		[SerializeField]
		[Range(0f, 1f)]
		private float denialPunchDuration = 0.2f;

		// Token: 0x04001667 RID: 5735
		[SerializeField]
		private Color slotIconDenialColor = Color.red;

		// Token: 0x04001668 RID: 5736
		private Color iconInitialColor;

		// Token: 0x0400166B RID: 5739
		private bool hovering;
	}
}
