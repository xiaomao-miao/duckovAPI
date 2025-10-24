using System;
using System.Collections.Generic;
using Duckov.Economy;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B5 RID: 949
	public class ItemRepairView : View
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x00077638 File Offset: 0x00075838
		public static ItemRepairView Instance
		{
			get
			{
				return View.GetViewInstance<ItemRepairView>();
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x0007763F File Offset: 0x0007583F
		private Item CharacterItem
		{
			get
			{
				LevelManager instance = LevelManager.Instance;
				if (instance == null)
				{
					return null;
				}
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					return null;
				}
				return mainCharacter.CharacterItem;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x0007765C File Offset: 0x0007585C
		private bool CanRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					return false;
				}
				if (!selectedItem.UseDurability)
				{
					return false;
				}
				if (selectedItem.MaxDurabilityWithLoss < 1f)
				{
					return false;
				}
				if (!selectedItem.Tags.Contains("Repairable"))
				{
					Debug.Log(selectedItem.DisplayName + " 不包含tag Repairable");
					return false;
				}
				return selectedItem.Durability < selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x000776CC File Offset: 0x000758CC
		private bool NoNeedToRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.Durability >= selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x00077708 File Offset: 0x00075908
		private bool Broken
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.MaxDurabilityWithLoss < 1f;
			}
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06002249 RID: 8777 RVA: 0x00077740 File Offset: 0x00075940
		// (remove) Token: 0x0600224A RID: 8778 RVA: 0x00077774 File Offset: 0x00075974
		public static event Action OnRepaireOptionDone;

		// Token: 0x0600224B RID: 8779 RVA: 0x000777A7 File Offset: 0x000759A7
		protected override void Awake()
		{
			base.Awake();
			this.repairButton.onClick.AddListener(new UnityAction(this.OnRepairButtonClicked));
			this.itemDetailsFadeGroup.SkipHide();
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000777D8 File Offset: 0x000759D8
		private List<Inventory> GetAvaliableInventories()
		{
			this.avaliableInventories.Clear();
			LevelManager instance = LevelManager.Instance;
			Inventory inventory;
			if (instance == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			if (inventory2 != null)
			{
				this.avaliableInventories.Add(inventory2);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.avaliableInventories.Add(inventory3);
			}
			return this.avaliableInventories;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00077850 File Offset: 0x00075A50
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				return;
			}
			base.gameObject.SetActive(true);
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
			this.repairButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
			this.repairAllPanel.Setup(this);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000778F1 File Offset: 0x00075AF1
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00077915 File Offset: 0x00075B15
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x00077928 File Offset: 0x00075B28
		private void OnItemSelectionChanged()
		{
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00077930 File Offset: 0x00075B30
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00077943 File Offset: 0x00075B43
		public static void Show()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Open(null);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x0007795E File Offset: 0x00075B5E
		public static void Hide()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Close();
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00077978 File Offset: 0x00075B78
		private void RefreshSelectedItemInfo()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
			}
			else
			{
				this.itemDetailsFadeGroup.Hide();
			}
			if (this.CanRepair)
			{
				this.placeHolderFadeGroup.Hide();
				this.repairButtonFadeGroup.Show();
			}
			else
			{
				this.repairButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			Item selectedItem = ItemUIUtilities.SelectedItem;
			this.willLoseDurabilityText.text = "";
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.noNeedToRepairIndicator.SetActive(false);
				this.brokenIndicator.SetActive(false);
				this.cannotRepairIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				this.barFill.fillAmount = 0f;
				this.lossBarFill.fillAmount = 0f;
				this.durabilityText.text = "-";
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.noNeedToRepairIndicator.SetActive(!this.Broken && this.NoNeedToRepair && selectedItem.Repairable);
			this.cannotRepairIndicator.SetActive(selectedItem.UseDurability && !selectedItem.Repairable && !this.Broken);
			this.brokenIndicator.SetActive(this.Broken);
			if (this.CanRepair)
			{
				float num2;
				float num3;
				float num4;
				int num = this.CalculateRepairPrice(selectedItem, out num2, out num3, out num4);
				this.repairPriceText.text = num.ToString();
				this.willLoseDurabilityText.text = "UI_MaxDurability".ToPlainText() + " -" + num3.ToString("0.#");
				this.repairButton.interactable = (EconomyManager.Money >= (long)num);
			}
			if (selectedItem.UseDurability)
			{
				float durability = selectedItem.Durability;
				float maxDurability = selectedItem.MaxDurability;
				float maxDurabilityWithLoss = selectedItem.MaxDurabilityWithLoss;
				float num5 = durability / maxDurability;
				this.barFill.fillAmount = num5;
				this.lossBarFill.fillAmount = selectedItem.DurabilityLoss;
				this.durabilityText.text = string.Format("{0:0.#} / {1} ", durability, maxDurabilityWithLoss.ToString("0.#"));
				this.barFill.color = this.barFillColorOverT.Evaluate(num5);
				return;
			}
			this.barFill.fillAmount = 0f;
			this.lossBarFill.fillAmount = 0f;
			this.durabilityText.text = "-";
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00077C7C File Offset: 0x00075E7C
		private void OnRepairButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				return;
			}
			if (!selectedItem.UseDurability)
			{
				return;
			}
			this.Repair(selectedItem, false);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00077CB0 File Offset: 0x00075EB0
		private void Repair(Item item, bool prepaied = false)
		{
			float num2;
			float num3;
			float num4;
			int num = this.CalculateRepairPrice(item, out num2, out num3, out num4);
			if (!prepaied && !EconomyManager.Pay(new Cost((long)num), true, true))
			{
				return;
			}
			item.DurabilityLoss += num4;
			item.Durability = item.MaxDurability * (1f - item.DurabilityLoss);
			Action onRepaireOptionDone = ItemRepairView.OnRepaireOptionDone;
			if (onRepaireOptionDone == null)
			{
				return;
			}
			onRepaireOptionDone();
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00077D18 File Offset: 0x00075F18
		private int CalculateRepairPrice(Item item, out float repairAmount, out float lostAmount, out float lostPercentage)
		{
			repairAmount = 0f;
			lostAmount = 0f;
			lostPercentage = 0f;
			if (item == null)
			{
				return 0;
			}
			if (!item.UseDurability)
			{
				return 0;
			}
			float maxDurability = item.MaxDurability;
			float durabilityLoss = item.DurabilityLoss;
			float num = maxDurability * (1f - durabilityLoss);
			float durability = item.Durability;
			repairAmount = num - durability;
			float repairLossRatio = item.GetRepairLossRatio();
			lostAmount = repairAmount * repairLossRatio;
			repairAmount -= lostAmount;
			if (repairAmount <= 0f)
			{
				return 0;
			}
			lostPercentage = lostAmount / maxDurability;
			float num2 = repairAmount / maxDurability;
			return Mathf.CeilToInt((float)item.Value * num2 * 0.5f);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00077DB8 File Offset: 0x00075FB8
		public List<Item> GetAllEquippedItems()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return null;
			}
			Item characterItem = main.CharacterItem;
			if (characterItem == null)
			{
				return null;
			}
			SlotCollection slots = characterItem.Slots;
			if (slots == null)
			{
				return null;
			}
			List<Item> list = new List<Item>();
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					Item content = slot.Content;
					if (!(content == null))
					{
						list.Add(content);
					}
				}
			}
			return list;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00077E5C File Offset: 0x0007605C
		public int CalculateRepairPrice(List<Item> itemsToRepair)
		{
			int num = 0;
			foreach (Item item in itemsToRepair)
			{
				float num2;
				float num3;
				float num4;
				num += this.CalculateRepairPrice(item, out num2, out num3, out num4);
			}
			return num;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00077EB8 File Offset: 0x000760B8
		public void RepairItems(List<Item> itemsToRepair)
		{
			if (!EconomyManager.Pay(new Cost((long)this.CalculateRepairPrice(itemsToRepair)), true, true))
			{
				return;
			}
			foreach (Item item in itemsToRepair)
			{
				this.Repair(item, true);
			}
		}

		// Token: 0x04001739 RID: 5945
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400173A RID: 5946
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x0400173B RID: 5947
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400173C RID: 5948
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400173D RID: 5949
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x0400173E RID: 5950
		[SerializeField]
		private ItemRepair_RepairAllPanel repairAllPanel;

		// Token: 0x0400173F RID: 5951
		[SerializeField]
		private FadeGroup repairButtonFadeGroup;

		// Token: 0x04001740 RID: 5952
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x04001741 RID: 5953
		[SerializeField]
		private Button repairButton;

		// Token: 0x04001742 RID: 5954
		[SerializeField]
		private TextMeshProUGUI repairPriceText;

		// Token: 0x04001743 RID: 5955
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x04001744 RID: 5956
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x04001745 RID: 5957
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x04001746 RID: 5958
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x04001747 RID: 5959
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x04001748 RID: 5960
		[SerializeField]
		private GameObject noNeedToRepairIndicator;

		// Token: 0x04001749 RID: 5961
		[SerializeField]
		private GameObject brokenIndicator;

		// Token: 0x0400174A RID: 5962
		[SerializeField]
		private GameObject cannotRepairIndicator;

		// Token: 0x0400174B RID: 5963
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x0400174C RID: 5964
		[SerializeField]
		private TextMeshProUGUI willLoseDurabilityText;

		// Token: 0x0400174D RID: 5965
		[SerializeField]
		private Image barFill;

		// Token: 0x0400174E RID: 5966
		[SerializeField]
		private Image lossBarFill;

		// Token: 0x0400174F RID: 5967
		[SerializeField]
		private Gradient barFillColorOverT;

		// Token: 0x04001750 RID: 5968
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
