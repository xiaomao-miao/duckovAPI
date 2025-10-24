using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B3 RID: 947
	public class ItemCustomizeView : View, ISingleSelectionMenu<SlotDisplay>
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x00076B53 File Offset: 0x00074D53
		public static ItemCustomizeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeView>();
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x00076B5C File Offset: 0x00074D5C
		private PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this._itemDisplayPool == null)
				{
					this.itemDisplayTemplate.gameObject.SetActive(false);
					this._itemDisplayPool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, this.itemDisplayTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				}
				return this._itemDisplayPool;
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00076BB5 File Offset: 0x00074DB5
		private void OnGetInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked += this.OnInventoryDoubleClicked;
			display.ShowOperationButtons = false;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00076BD0 File Offset: 0x00074DD0
		private void OnReleaseInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked -= this.OnInventoryDoubleClicked;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00076BE4 File Offset: 0x00074DE4
		private void OnInventoryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (entry.Item != null)
			{
				this.target.TryPlug(entry.Item, false, entry.Master.Target, 0);
				data.Use();
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x00076C19 File Offset: 0x00074E19
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00076C21 File Offset: 0x00074E21
		public void Setup(Item target, List<Inventory> avaliableInventories)
		{
			this.target = target;
			this.customizingTargetDisplay.Setup(target);
			this.avaliableInventories.Clear();
			this.avaliableInventories.AddRange(avaliableInventories);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00076C4D File Offset: 0x00074E4D
		public void DebugSetup(Item target, Inventory inventory1, Inventory inventory2)
		{
			this.Setup(target, new List<Inventory>
			{
				inventory1,
				inventory2
			});
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00076C69 File Offset: 0x00074E69
		protected override void OnOpen()
		{
			base.OnOpen();
			ItemUIUtilities.Select(null);
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00076CA1 File Offset: 0x00074EA1
		protected override void OnClose()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			base.OnClose();
			this.fadeGroup.Hide();
			this.selectedItemDisplayFadeGroup.Hide();
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00076CD0 File Offset: 0x00074ED0
		private void OnItemSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00076CD8 File Offset: 0x00074ED8
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.selectedItemDisplayFadeGroup.Show();
				this.selectedItemDisplay.Setup(ItemUIUtilities.SelectedItem);
				Item y = this.selectedItemDisplay.Target;
				bool flag = this.selectedSlotDisplay.Target.Content != y;
				this.equipButton.gameObject.SetActive(flag);
				this.unequipButton.gameObject.SetActive(!flag);
				return;
			}
			this.selectedItemDisplayFadeGroup.Hide();
			this.equipButton.gameObject.SetActive(false);
			this.unequipButton.gameObject.SetActive(false);
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00076D84 File Offset: 0x00074F84
		protected override void Awake()
		{
			base.Awake();
			this.equipButton.onClick.AddListener(new UnityAction(this.OnEquipButtonClicked));
			this.unequipButton.onClick.AddListener(new UnityAction(this.OnUnequipButtonClicked));
			this.customizingTargetDisplay.SlotCollectionDisplay.onElementClicked += this.OnSlotElementClicked;
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00076DEC File Offset: 0x00074FEC
		private void OnUnequipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			if (slot.Content != null)
			{
				Item item = slot.Unplug();
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x00076E48 File Offset: 0x00075048
		private void OnEquipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			Item item = this.selectedItemDisplay.Target;
			if (slot == null)
			{
				return;
			}
			if (item == null)
			{
				return;
			}
			if (slot.Content != null)
			{
				Item item2 = slot.Unplug();
				this.HandleUnpluggledItem(item2);
			}
			item.Detach();
			Item item3;
			if (!slot.Plug(item, out item3))
			{
				Debug.LogError("装备失败！");
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00076EDD File Offset: 0x000750DD
		private void HandleUnpluggledItem(Item item)
		{
			if (PlayerStorage.Inventory)
			{
				ItemUtilities.SendToPlayerStorage(item, false);
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacterInventory(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x00076F03 File Offset: 0x00075103
		private void OnSlotElementClicked(ItemSlotCollectionDisplay collection, SlotDisplay slot)
		{
			this.SetSelection(slot);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x00076F0D File Offset: 0x0007510D
		public SlotDisplay GetSelection()
		{
			return this.selectedSlotDisplay;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00076F15 File Offset: 0x00075115
		public bool SetSelection(SlotDisplay selection)
		{
			this.selectedSlotDisplay = selection;
			this.RefreshSelectionIndicator();
			this.OnSlotSelectionChanged();
			return true;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00076F2C File Offset: 0x0007512C
		private void RefreshSelectionIndicator()
		{
			this.slotSelectionIndicator.gameObject.SetActive(this.selectedSlotDisplay);
			if (this.selectedSlotDisplay != null)
			{
				this.slotSelectionIndicator.position = this.selectedSlotDisplay.transform.position;
			}
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00076F7D File Offset: 0x0007517D
		private void OnSlotSelectionChanged()
		{
			ItemUIUtilities.Select(null);
			this.RefreshAvaliableItems();
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00076F8C File Offset: 0x0007518C
		private void RefreshAvaliableItems()
		{
			this.avaliableItems.Clear();
			if (!(this.selectedSlotDisplay == null))
			{
				Slot slot = this.selectedSlotDisplay.Target;
				if (!(this.selectedSlotDisplay == null))
				{
					foreach (Inventory inventory in this.avaliableInventories)
					{
						foreach (Item item in inventory)
						{
							if (!(item == null) && slot.CanPlug(item))
							{
								this.avaliableItems.Add(item);
							}
						}
					}
				}
			}
			this.RefreshItemListGraphics();
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00077060 File Offset: 0x00075260
		private void RefreshItemListGraphics()
		{
			Debug.Log("Refreshing Item List Graphics");
			bool flag = this.selectedSlotDisplay != null;
			bool flag2 = this.avaliableItems.Count > 0;
			this.selectSlotPlaceHolder.SetActive(!flag);
			this.noAvaliableItemPlaceHolder.SetActive(flag && !flag2);
			this.avaliableItemsContainer.SetActive(flag2);
			this.ItemDisplayPool.ReleaseAll();
			if (flag2)
			{
				foreach (Item x in this.avaliableItems)
				{
					if (!(x == null))
					{
						ItemDisplay itemDisplay = this.ItemDisplayPool.Get(null);
						itemDisplay.ShowOperationButtons = false;
						itemDisplay.Setup(x);
					}
				}
			}
		}

		// Token: 0x0400171C RID: 5916
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400171D RID: 5917
		[SerializeField]
		private Button equipButton;

		// Token: 0x0400171E RID: 5918
		[SerializeField]
		private Button unequipButton;

		// Token: 0x0400171F RID: 5919
		[SerializeField]
		private ItemDetailsDisplay customizingTargetDisplay;

		// Token: 0x04001720 RID: 5920
		[SerializeField]
		private ItemDetailsDisplay selectedItemDisplay;

		// Token: 0x04001721 RID: 5921
		[SerializeField]
		private FadeGroup selectedItemDisplayFadeGroup;

		// Token: 0x04001722 RID: 5922
		[SerializeField]
		private RectTransform slotSelectionIndicator;

		// Token: 0x04001723 RID: 5923
		[SerializeField]
		private GameObject selectSlotPlaceHolder;

		// Token: 0x04001724 RID: 5924
		[SerializeField]
		private GameObject avaliableItemsContainer;

		// Token: 0x04001725 RID: 5925
		[SerializeField]
		private GameObject noAvaliableItemPlaceHolder;

		// Token: 0x04001726 RID: 5926
		[SerializeField]
		private ItemDisplay itemDisplayTemplate;

		// Token: 0x04001727 RID: 5927
		private PrefabPool<ItemDisplay> _itemDisplayPool;

		// Token: 0x04001728 RID: 5928
		private Item target;

		// Token: 0x04001729 RID: 5929
		private SlotDisplay selectedSlotDisplay;

		// Token: 0x0400172A RID: 5930
		private List<Inventory> avaliableInventories = new List<Inventory>();

		// Token: 0x0400172B RID: 5931
		private List<Item> avaliableItems = new List<Item>();
	}
}
