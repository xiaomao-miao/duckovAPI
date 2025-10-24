using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E2 RID: 738
	public class MasterKeysRegisterView : View
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x00056804 File Offset: 0x00054A04
		public static MasterKeysRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysRegisterView>();
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x0005680B File Offset: 0x00054A0B
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

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x00056828 File Offset: 0x00054A28
		private Slot KeySlot
		{
			get
			{
				if (this.keySlotItem == null)
				{
					return null;
				}
				if (this.keySlotItem.Slots == null)
				{
					return null;
				}
				return this.keySlotItem.Slots[this.keySlotKey];
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00056868 File Offset: 0x00054A68
		protected override void Awake()
		{
			base.Awake();
			this.submitButton.onClick.AddListener(new UnityAction(this.OnSubmitButtonClicked));
			this.succeedIndicator.SkipHide();
			this.detailsFadeGroup.SkipHide();
			this.registerSlotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDoubleClicked;
			this.inventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
			this.playerStorageInventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000568F4 File Offset: 0x00054AF4
		private void OnInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (!entry.Editable)
			{
				return;
			}
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!this.KeySlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.KeySlot.Plug(item, out item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00056950 File Offset: 0x00054B50
		private void OnSlotDoubleClicked(SlotDisplay display)
		{
			Item item = display.GetItem();
			if (item == null)
			{
				return;
			}
			item.Detach();
			ItemUtilities.SendToPlayer(item, false, true);
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0005697C File Offset: 0x00054B7C
		private void OnSubmitButtonClicked()
		{
			if (this.KeySlot != null && this.KeySlot.Content != null && MasterKeysManager.SubmitAndActivate(this.KeySlot.Content))
			{
				this.IndicateSuccess();
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000569B1 File Offset: 0x00054BB1
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000569C0 File Offset: 0x00054BC0
		private UniTask SuccessIndicationTask()
		{
			MasterKeysRegisterView.<SuccessIndicationTask>d__24 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<MasterKeysRegisterView.<SuccessIndicationTask>d__24>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00056A03 File Offset: 0x00054C03
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00056A10 File Offset: 0x00054C10
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.KeySlot.CanPlug(e) && !MasterKeysManager.IsActive(e.TypeID);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00056A3D File Offset: 0x00054C3D
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.KeySlot.CanPlug(e);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00056A58 File Offset: 0x00054C58
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
			this.inventoryDisplay.ShowOperationButtons = false;
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.KeySlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00056B64 File Offset: 0x00054D64
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.KeySlot != null && this.KeySlot.Content != null)
			{
				Item content = this.KeySlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00056BC6 File Offset: 0x00054DC6
		private void RegisterEvents()
		{
			this.KeySlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00056BF0 File Offset: 0x00054DF0
		private void UnregisterEvents()
		{
			this.KeySlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00056C1A File Offset: 0x00054E1A
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00056C48 File Offset: 0x00054E48
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.KeySlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = MasterKeysManager.IsActive(content.TypeID);
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00056C8F File Offset: 0x00054E8F
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00056CC5 File Offset: 0x00054EC5
		public static void Show()
		{
			if (MasterKeysRegisterView.Instance == null)
			{
				return;
			}
			MasterKeysRegisterView.Instance.Open(null);
		}

		// Token: 0x0400113A RID: 4410
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400113B RID: 4411
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400113C RID: 4412
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x0400113D RID: 4413
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400113E RID: 4414
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x0400113F RID: 4415
		[SerializeField]
		private Button submitButton;

		// Token: 0x04001140 RID: 4416
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x04001141 RID: 4417
		[SerializeField]
		private string keySlotKey = "Key";

		// Token: 0x04001142 RID: 4418
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x04001143 RID: 4419
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x04001144 RID: 4420
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04001145 RID: 4421
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x04001146 RID: 4422
		private string sfx_Register = "UI/register";
	}
}
