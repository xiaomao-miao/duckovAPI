using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B2 RID: 946
	public class ItemCustomizeSelectionView : View
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00076773 File Offset: 0x00074973
		public static ItemCustomizeSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeSelectionView>();
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600220A RID: 8714 RVA: 0x0007677A File Offset: 0x0007497A
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

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x00076798 File Offset: 0x00074998
		private bool CanCustomize
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && !(selectedItem.Slots == null) && selectedItem.Slots.Count >= 1;
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000767D7 File Offset: 0x000749D7
		protected override void Awake()
		{
			base.Awake();
			this.beginCustomizeButton.onClick.AddListener(new UnityAction(this.OnBeginCustomizeButtonClicked));
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000767FC File Offset: 0x000749FC
		private void OnBeginCustomizeButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Setup(ItemUIUtilities.SelectedItem, this.GetAvaliableInventories());
			instance.Open(null);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00076838 File Offset: 0x00074A38
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

		// Token: 0x0600220F RID: 8719 RVA: 0x000768B0 File Offset: 0x00074AB0
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
			this.customizeButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00076945 File Offset: 0x00074B45
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00076969 File Offset: 0x00074B69
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0007697C File Offset: 0x00074B7C
		private void OnItemSelectionChanged()
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
			if (this.CanCustomize)
			{
				this.placeHolderFadeGroup.Hide();
				this.customizeButtonFadeGroup.Show();
			}
			else
			{
				this.customizeButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000769FA File Offset: 0x00074BFA
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00076A0D File Offset: 0x00074C0D
		public static void Show()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Open(null);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x00076A28 File Offset: 0x00074C28
		public static void Hide()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Close();
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x00076A44 File Offset: 0x00074C44
		private void RefreshSelectedItemInfo()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.customizableIndicator.SetActive(false);
				this.uncustomizableIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.customizableIndicator.SetActive(this.CanCustomize);
			this.uncustomizableIndicator.SetActive(!this.CanCustomize);
		}

		// Token: 0x0400170C RID: 5900
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400170D RID: 5901
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x0400170E RID: 5902
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400170F RID: 5903
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001710 RID: 5904
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x04001711 RID: 5905
		[SerializeField]
		private FadeGroup customizeButtonFadeGroup;

		// Token: 0x04001712 RID: 5906
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x04001713 RID: 5907
		[SerializeField]
		private Button beginCustomizeButton;

		// Token: 0x04001714 RID: 5908
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x04001715 RID: 5909
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x04001716 RID: 5910
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x04001717 RID: 5911
		[SerializeField]
		private GameObject customizableIndicator;

		// Token: 0x04001718 RID: 5912
		[SerializeField]
		private GameObject uncustomizableIndicator;

		// Token: 0x04001719 RID: 5913
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x0400171A RID: 5914
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x0400171B RID: 5915
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
