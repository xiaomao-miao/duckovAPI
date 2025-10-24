using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000385 RID: 901
	public class FormulasRegisterView : View
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x0006D4B7 File Offset: 0x0006B6B7
		public static FormulasRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasRegisterView>();
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0006D4BE File Offset: 0x0006B6BE
		private string FormulaUnlockedNotificationFormat
		{
			get
			{
				return this.formulaUnlockedFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0006D4CB File Offset: 0x0006B6CB
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

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x0006D4E8 File Offset: 0x0006B6E8
		private Slot SubmitItemSlot
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
				return this.keySlotItem.Slots[this.slotKey];
			}
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0006D528 File Offset: 0x0006B728
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

		// Token: 0x06001F3B RID: 7995 RVA: 0x0006D5B4 File Offset: 0x0006B7B4
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
			if (!this.SubmitItemSlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.SubmitItemSlot.Plug(item, out item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0006D610 File Offset: 0x0006B810
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

		// Token: 0x06001F3D RID: 7997 RVA: 0x0006D63C File Offset: 0x0006B83C
		private void OnSubmitButtonClicked()
		{
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				string formulaID = FormulasRegisterView.GetFormulaID(content);
				if (string.IsNullOrEmpty(formulaID))
				{
					return;
				}
				if (CraftingManager.IsFormulaUnlocked(formulaID))
				{
					return;
				}
				CraftingManager.UnlockFormula(formulaID);
				CraftingFormula formula = CraftingManager.GetFormula(formulaID);
				if (formula.IDValid)
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
					string mainText = this.FormulaUnlockedNotificationFormat.Format(new
					{
						itemDisplayName = metaData.DisplayName
					});
					Sprite icon = metaData.icon;
					StrongNotification.Push(new StrongNotificationContent(mainText, "", icon));
				}
				content.Detach();
				content.DestroyTreeImmediate();
				this.IndicateSuccess();
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0006D6F8 File Offset: 0x0006B8F8
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006D708 File Offset: 0x0006B908
		private UniTask SuccessIndicationTask()
		{
			FormulasRegisterView.<SuccessIndicationTask>d__28 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<FormulasRegisterView.<SuccessIndicationTask>d__28>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0006D74B File Offset: 0x0006B94B
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0006D758 File Offset: 0x0006B958
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.SubmitItemSlot.CanPlug(e) && !CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(e));
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0006D785 File Offset: 0x0006B985
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.SubmitItemSlot.CanPlug(e);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0006D7A0 File Offset: 0x0006B9A0
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
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.SubmitItemSlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0006D8AC File Offset: 0x0006BAAC
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0006D90E File Offset: 0x0006BB0E
		private void RegisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0006D938 File Offset: 0x0006BB38
		private void UnregisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0006D962 File Offset: 0x0006BB62
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0006D990 File Offset: 0x0006BB90
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.SubmitItemSlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(content));
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0006D9D7 File Offset: 0x0006BBD7
		private bool IsFormulaItem(Item item)
		{
			return !(item == null) && item.GetComponent<ItemSetting_Formula>() != null;
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0006D9F0 File Offset: 0x0006BBF0
		public static string GetFormulaID(Item item)
		{
			if (item == null)
			{
				return null;
			}
			ItemSetting_Formula component = item.GetComponent<ItemSetting_Formula>();
			if (component == null)
			{
				return null;
			}
			return component.formulaID;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0006DA20 File Offset: 0x0006BC20
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

		// Token: 0x06001F4C RID: 8012 RVA: 0x0006DA56 File Offset: 0x0006BC56
		public static void Show(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			FormulasRegisterView.SetupTags(requireTags);
			FormulasRegisterView.Instance.Open(null);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0006DA78 File Offset: 0x0006BC78
		private static void SetupTags(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			Slot submitItemSlot = FormulasRegisterView.Instance.SubmitItemSlot;
			if (submitItemSlot == null)
			{
				return;
			}
			submitItemSlot.requireTags.Clear();
			submitItemSlot.requireTags.Add(FormulasRegisterView.Instance.formulaTag);
			if (requireTags != null)
			{
				submitItemSlot.requireTags.AddRange(requireTags);
			}
		}

		// Token: 0x04001551 RID: 5457
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001552 RID: 5458
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x04001553 RID: 5459
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x04001554 RID: 5460
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001555 RID: 5461
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001556 RID: 5462
		[SerializeField]
		private Button submitButton;

		// Token: 0x04001557 RID: 5463
		[SerializeField]
		private Tag formulaTag;

		// Token: 0x04001558 RID: 5464
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x04001559 RID: 5465
		[SerializeField]
		private string slotKey = "SubmitItem";

		// Token: 0x0400155A RID: 5466
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x0400155B RID: 5467
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x0400155C RID: 5468
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x0400155D RID: 5469
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x0400155E RID: 5470
		private string sfx_Register = "UI/register";

		// Token: 0x0400155F RID: 5471
		[LocalizationKey("Default")]
		[SerializeField]
		private string formulaUnlockedFormatKey = "UI_Formulas_RegisterSucceedFormat";
	}
}
