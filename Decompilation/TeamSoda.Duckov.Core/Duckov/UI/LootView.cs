using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B6 RID: 950
	public class LootView : View
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x00077F3E File Offset: 0x0007613E
		public static LootView Instance
		{
			get
			{
				return View.GetViewInstance<LootView>();
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x00077F45 File Offset: 0x00076145
		private CharacterMainControl Character
		{
			get
			{
				return LevelManager.Instance.MainCharacter;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x00077F51 File Offset: 0x00076151
		private Item CharacterItem
		{
			get
			{
				if (this.Character == null)
				{
					return null;
				}
				return this.Character.CharacterItem;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x00077F6E File Offset: 0x0007616E
		public Inventory TargetInventory
		{
			get
			{
				if (this.targetLootBox != null)
				{
					return this.targetLootBox.Inventory;
				}
				if (this.targetInventory)
				{
					return this.targetInventory;
				}
				return null;
			}
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00077F9F File Offset: 0x0007619F
		public static bool HasInventoryEverBeenLooted(Inventory inventory)
		{
			return !(LootView.Instance == null) && LootView.Instance.lootedInventories != null && !(inventory == null) && LootView.Instance.lootedInventories.Contains(inventory);
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x00077FDC File Offset: 0x000761DC
		protected override void Awake()
		{
			base.Awake();
			InteractableLootbox.OnStartLoot += this.OnStartLoot;
			this.pickAllButton.onClick.AddListener(new UnityAction(this.OnPickAllButtonClicked));
			CharacterMainControl.OnMainCharacterStartUseItem += this.OnMainCharacterStartUseItem;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			this.storeAllButton.onClick.AddListener(new UnityAction(this.OnStoreAllButtonClicked));
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x0007805C File Offset: 0x0007625C
		private void OnStoreAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (this.TargetInventory != PlayerStorage.Inventory)
			{
				return;
			}
			if (this.CharacterItem == null)
			{
				return;
			}
			Inventory inventory = this.CharacterItem.Inventory;
			if (inventory == null)
			{
				return;
			}
			int lastItemPosition = inventory.GetLastItemPosition();
			for (int i = 0; i <= lastItemPosition; i++)
			{
				if (!inventory.lockedIndexes.Contains(i))
				{
					Item itemAt = inventory.GetItemAt(i);
					if (!(itemAt == null))
					{
						if (!this.TargetInventory.AddAndMerge(itemAt, 0))
						{
							break;
						}
						if (i == 0)
						{
							AudioManager.PlayPutItemSFX(itemAt, false);
						}
					}
				}
			}
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000780FB File Offset: 0x000762FB
		protected override void OnDestroy()
		{
			this.UnregisterEvents();
			InteractableLootbox.OnStartLoot -= this.OnStartLoot;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			base.OnDestroy();
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x0007812B File Offset: 0x0007632B
		private void OnMainCharacterStartUseItem(Item _item)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x0007813B File Offset: 0x0007633B
		private void OnMainCharacterDead(DamageInfo dmgInfo)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0007814B File Offset: 0x0007634B
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00078153 File Offset: 0x00076353
		private void OnDisable()
		{
			this.UnregisterEvents();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00078173 File Offset: 0x00076373
		public void Show()
		{
			base.Open(null);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x0007817C File Offset: 0x0007637C
		private void OnStartLoot(InteractableLootbox lootbox)
		{
			this.targetLootBox = lootbox;
			if (this.targetLootBox == null || this.targetLootBox.Inventory == null)
			{
				Debug.LogError("Target loot box could not be found");
				return;
			}
			base.Open(null);
			if (this.TargetInventory != null)
			{
				this.lootedInventories.Add(this.TargetInventory);
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000781E3 File Offset: 0x000763E3
		private void OnStopLoot(InteractableLootbox lootbox)
		{
			if (lootbox == this.targetLootBox)
			{
				this.targetLootBox = null;
				base.Close();
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00078200 File Offset: 0x00076400
		public static void LootItem(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (LootView.Instance == null)
			{
				return;
			}
			LootView.Instance.targetInventory = item.Inventory;
			LootView.Instance.Open(null);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00078238 File Offset: 0x00076438
		protected override void OnOpen()
		{
			base.OnOpen();
			this.UnregisterEvents();
			base.gameObject.SetActive(true);
			this.characterSlotCollectionDisplay.Setup(this.CharacterItem, true);
			if (PetProxy.PetInventory)
			{
				this.petInventoryDisplay.gameObject.SetActive(true);
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, null, false, null);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			this.characterInventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, true, null);
			if (this.targetLootBox != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = this.targetLootBox.ShowSortButton;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetDisplayName.text = this.TargetInventory.DisplayName;
				if (this.TargetInventory.GetComponent<InventoryFilterProvider>())
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(true);
					this.lootTargetFilterDisplay.Setup(this.lootTargetInventoryDisplay);
					this.lootTargetFilterDisplay.Select(0);
				}
				else
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(false);
				}
				this.lootTargetFadeGroup.Show();
			}
			else if (this.targetInventory != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = false;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetFadeGroup.Show();
				this.lootTargetFilterDisplay.gameObject.SetActive(false);
			}
			else
			{
				this.lootTargetFadeGroup.SkipHide();
			}
			bool active = this.TargetInventory != null && this.TargetInventory == PlayerStorage.Inventory;
			this.storeAllButton.gameObject.SetActive(active);
			this.fadeGroup.Show();
			this.RefreshDetails();
			this.RefreshPickAllButton();
			this.RegisterEvents();
			this.RefreshCapacityText();
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00078430 File Offset: 0x00076630
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
			this.targetInventory = null;
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
			this.UnregisterEvents();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000784A0 File Offset: 0x000766A0
		private void OnTargetInventoryContentChanged(Inventory inventory, int arg2)
		{
			this.RefreshPickAllButton();
			this.RefreshCapacityText();
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000784B0 File Offset: 0x000766B0
		private void RefreshCapacityText()
		{
			if (this.targetLootBox != null)
			{
				this.lootTargetCapacityText.text = this.lootTargetCapacityTextFormat.Format(new
				{
					itemCount = this.TargetInventory.GetItemCount(),
					capacity = this.TargetInventory.Capacity
				});
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000784FC File Offset: 0x000766FC
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			this.lootTargetInventoryDisplay.onDisplayDoubleClicked += this.OnLootTargetItemDoubleClicked;
			this.characterInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.petInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.characterSlotCollectionDisplay.onElementDoubleClicked += this.OnCharacterSlotItemDoubleClicked;
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged += this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000785C2 File Offset: 0x000767C2
		private void OnPreviousPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.PreviousPage();
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000785EC File Offset: 0x000767EC
		private void OnNextPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.NextPage();
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00078618 File Offset: 0x00076818
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			if (this.lootTargetInventoryDisplay)
			{
				this.lootTargetInventoryDisplay.onDisplayDoubleClicked -= this.OnLootTargetItemDoubleClicked;
			}
			if (this.characterInventoryDisplay)
			{
				this.characterInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.petInventoryDisplay)
			{
				this.petInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.characterSlotCollectionDisplay)
			{
				this.characterSlotCollectionDisplay.onElementDoubleClicked -= this.OnCharacterSlotItemDoubleClicked;
			}
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged -= this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x0007870C File Offset: 0x0007690C
		private void OnCharacterSlotItemDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (slotDisplay == null)
			{
				return;
			}
			Slot target = slotDisplay.Target;
			if (target == null)
			{
				return;
			}
			Item content = target.Content;
			if (content == null)
			{
				return;
			}
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			Item x;
			if (!target.Plug(content, out x))
			{
				Debug.LogError("Failed plugging back!");
			}
			if (x != null)
			{
				Debug.Log("Unplugged item should be null!");
			}
			this.RefreshDetails();
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000787B8 File Offset: 0x000769B8
		private void OnCharacterInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item content = entry.Content;
			if (content == null)
			{
				return;
			}
			Inventory inInventory = content.InInventory;
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			if (!inInventory.AddAndMerge(content, 0))
			{
				Debug.LogError("Failed sending back item");
			}
			this.RefreshDetails();
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0007883F File Offset: 0x00076A3F
		private void OnSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00078847 File Offset: 0x00076A47
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsFadeGroup.Show();
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00078880 File Offset: 0x00076A80
		private void OnLootTargetItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!item.IsInPlayerCharacter())
			{
				if (this.targetLootBox != null && this.targetLootBox.needInspect && !item.Inspected)
				{
					data.Use();
					return;
				}
				data.Use();
				bool flag = false;
				LevelManager instance = LevelManager.Instance;
				bool? flag2;
				if (instance == null)
				{
					flag2 = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag2 = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag2 = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
					}
				}
				bool? flag3 = flag2;
				flag |= flag3.Value;
				if (flag3 == null || !flag3.Value)
				{
					flag |= ItemUtilities.SendToPlayerCharacterInventory(item, false);
				}
				if (flag)
				{
					AudioManager.PlayPutItemSFX(item, false);
					this.RefreshDetails();
				}
			}
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0007895C File Offset: 0x00076B5C
		private void RefreshPickAllButton()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			this.pickAllButton.gameObject.SetActive(false);
			bool interactable = this.TargetInventory.GetItemCount() > 0;
			this.pickAllButton.interactable = interactable;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000789A4 File Offset: 0x00076BA4
		private void OnPickAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			List<Item> list = new List<Item>();
			list.AddRange(this.TargetInventory);
			foreach (Item item in list)
			{
				if (!(item == null) && (!this.targetLootBox.needInspect || item.Inspected))
				{
					LevelManager instance = LevelManager.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						CharacterMainControl mainCharacter = instance.MainCharacter;
						if (mainCharacter == null)
						{
							flag = null;
						}
						else
						{
							Item characterItem = mainCharacter.CharacterItem;
							flag = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
						}
					}
					bool? flag2 = flag;
					if (flag2 == null || !flag2.Value)
					{
						ItemUtilities.SendToPlayerCharacterInventory(item, false);
					}
				}
			}
			AudioManager.Post("UI/confirm");
		}

		// Token: 0x04001752 RID: 5970
		[SerializeField]
		private ItemSlotCollectionDisplay characterSlotCollectionDisplay;

		// Token: 0x04001753 RID: 5971
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x04001754 RID: 5972
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x04001755 RID: 5973
		[SerializeField]
		private InventoryDisplay lootTargetInventoryDisplay;

		// Token: 0x04001756 RID: 5974
		[SerializeField]
		private InventoryFilterDisplay lootTargetFilterDisplay;

		// Token: 0x04001757 RID: 5975
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001758 RID: 5976
		[SerializeField]
		private Button pickAllButton;

		// Token: 0x04001759 RID: 5977
		[SerializeField]
		private TextMeshProUGUI lootTargetDisplayName;

		// Token: 0x0400175A RID: 5978
		[SerializeField]
		private TextMeshProUGUI lootTargetCapacityText;

		// Token: 0x0400175B RID: 5979
		[SerializeField]
		private string lootTargetCapacityTextFormat = "({itemCount}/{capacity})";

		// Token: 0x0400175C RID: 5980
		[SerializeField]
		private Button storeAllButton;

		// Token: 0x0400175D RID: 5981
		[SerializeField]
		private FadeGroup lootTargetFadeGroup;

		// Token: 0x0400175E RID: 5982
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400175F RID: 5983
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001760 RID: 5984
		[SerializeField]
		private InteractableLootbox targetLootBox;

		// Token: 0x04001761 RID: 5985
		private Inventory targetInventory;

		// Token: 0x04001762 RID: 5986
		private HashSet<Inventory> lootedInventories = new HashSet<Inventory>();
	}
}
