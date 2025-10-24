using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B1 RID: 945
	public class InventoryView : View
	{
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x000765A9 File Offset: 0x000747A9
		private static InventoryView Instance
		{
			get
			{
				return View.GetViewInstance<InventoryView>();
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x000765B0 File Offset: 0x000747B0
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

		// Token: 0x060021FF RID: 8703 RVA: 0x000765CD File Offset: 0x000747CD
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000765D8 File Offset: 0x000747D8
		private void Update()
		{
			bool editable = true;
			this.inventoryDisplay.Editable = editable;
			this.slotDisplay.Editable = editable;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00076600 File Offset: 0x00074800
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				base.Close();
				return;
			}
			base.gameObject.SetActive(true);
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0007667C File Offset: 0x0007487C
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000766CD File Offset: 0x000748CD
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000766E0 File Offset: 0x000748E0
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
				return;
			}
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00076716 File Offset: 0x00074916
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00076729 File Offset: 0x00074929
		public static void Show()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LootView instance = LootView.Instance;
			if (instance != null)
			{
				instance.Show();
			}
			if (LootView.Instance == null)
			{
				Debug.Log("LOOTVIEW INSTANCE IS NULL");
			}
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0007675A File Offset: 0x0007495A
		public static void Hide()
		{
			LootView instance = LootView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Close();
		}

		// Token: 0x04001707 RID: 5895
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001708 RID: 5896
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x04001709 RID: 5897
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400170A RID: 5898
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400170B RID: 5899
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;
	}
}
