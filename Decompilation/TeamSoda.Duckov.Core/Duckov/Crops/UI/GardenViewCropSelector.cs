using System;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F1 RID: 753
	public class GardenViewCropSelector : MonoBehaviour
	{
		// Token: 0x06001872 RID: 6258 RVA: 0x000595AA File Offset: 0x000577AA
		private void Awake()
		{
			this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirm));
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000595C8 File Offset: 0x000577C8
		private void OnConfirm()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem != null)
			{
				this.master.SelectSeed(selectedItem.TypeID);
			}
			this.Hide();
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000595FC File Offset: 0x000577FC
		public void Show()
		{
			this.fadeGroup.Show();
			if (LevelManager.Instance == null)
			{
				return;
			}
			ItemUIUtilities.Select(null);
			this.playerInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
			this.storageInventoryDisplay.Setup(PlayerStorage.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00059696 File Offset: 0x00057896
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000596A9 File Offset: 0x000578A9
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x000596BC File Offset: 0x000578BC
		private void OnSelectionChanged()
		{
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000596BE File Offset: 0x000578BE
		public void Hide()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x040011D2 RID: 4562
		[SerializeField]
		private GardenView master;

		// Token: 0x040011D3 RID: 4563
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011D4 RID: 4564
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x040011D5 RID: 4565
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x040011D6 RID: 4566
		[SerializeField]
		private InventoryDisplay storageInventoryDisplay;
	}
}
