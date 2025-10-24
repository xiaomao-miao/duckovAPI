using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Economy.UI
{
	// Token: 0x02000328 RID: 808
	public class StockShopView : View, ISingleSelectionMenu<StockShopItemEntry>
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x00062513 File Offset: 0x00060713
		public static StockShopView Instance
		{
			get
			{
				return View.GetViewInstance<StockShopView>();
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x0006251A File Offset: 0x0006071A
		private string TextBuy
		{
			get
			{
				return this.textBuy.ToPlainText();
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x00062527 File Offset: 0x00060727
		private string TextSoldOut
		{
			get
			{
				return this.textSoldOut.ToPlainText();
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x00062534 File Offset: 0x00060734
		private string TextSell
		{
			get
			{
				return this.textSell.ToPlainText();
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x00062541 File Offset: 0x00060741
		private string TextUnlock
		{
			get
			{
				return this.textUnlock.ToPlainText();
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0006254E File Offset: 0x0006074E
		private string TextLocked
		{
			get
			{
				return this.textLocked.ToPlainText();
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x0006255C File Offset: 0x0006075C
		private PrefabPool<StockShopItemEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StockShopItemEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000625B5 File Offset: 0x000607B5
		private UnityEngine.Object Selection
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay != null)
				{
					return ItemUIUtilities.SelectedItemDisplay;
				}
				if (this.selectedItem != null)
				{
					return this.selectedItem;
				}
				return null;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000625E0 File Offset: 0x000607E0
		public StockShop Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000625E8 File Offset: 0x000607E8
		protected override void Awake()
		{
			base.Awake();
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0006261D File Offset: 0x0006081D
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00062636 File Offset: 0x00060836
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.OnInteractionButtonClicked();
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00062647 File Offset: 0x00060847
		private void FixedUpdate()
		{
			this.RefreshCountDown();
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00062650 File Offset: 0x00060850
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				this.refreshCountDown.text = "-";
			}
			TimeSpan nextRefreshETA = this.target.NextRefreshETA;
			int days = nextRefreshETA.Days;
			int hours = nextRefreshETA.Hours;
			int minutes = nextRefreshETA.Minutes;
			int seconds = nextRefreshETA.Seconds;
			this.refreshCountDown.text = string.Format("{0}{1:00}:{2:00}:{3:00}", new object[]
			{
				(days > 0) ? (days.ToString() + " - ") : "",
				hours,
				minutes,
				seconds
			});
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00062700 File Offset: 0x00060900
		private void OnInteractionButtonClicked()
		{
			if (this.Selection == null)
			{
				return;
			}
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				this.Target.Sell(itemDisplay.Target).Forget();
				AudioManager.Post(this.sfx_Sell);
				ItemUIUtilities.Select(null);
				this.OnSelectionChanged();
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				int itemTypeID = stockShopItemEntry.Target.ItemTypeID;
				if (stockShopItemEntry.IsUnlocked())
				{
					this.BuyTask(itemTypeID).Forget();
					return;
				}
				if (EconomyManager.IsWaitingForUnlockConfirm(itemTypeID))
				{
					EconomyManager.ConfirmUnlock(itemTypeID);
				}
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00062798 File Offset: 0x00060998
		private UniTask BuyTask(int itemTypeID)
		{
			StockShopView.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShopView.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000627E4 File Offset: 0x000609E4
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold += this.OnAfterItemSold;
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00062848 File Offset: 0x00060A48
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold -= this.OnAfterItemSold;
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000628AA File Offset: 0x00060AAA
		private void OnNextPage(UIInputEventData data)
		{
			this.playerStorageDisplay.NextPage();
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000628B7 File Offset: 0x00060AB7
		private void OnPreviousPage(UIInputEventData data)
		{
			this.playerStorageDisplay.PreviousPage();
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000628C4 File Offset: 0x00060AC4
		private void OnAfterItemSold(StockShop shop)
		{
			this.RefreshInteractionButton();
			this.RefreshStockText();
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000628D2 File Offset: 0x00060AD2
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.details.Target == null)
			{
				return;
			}
			if (itemTypeID == this.details.Target.TypeID)
			{
				this.RefreshInteractionButton();
				this.RefreshStockText();
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00062907 File Offset: 0x00060B07
		private void OnItemUIUtilitiesSelectionChanged()
		{
			if (this.selectedItem != null && ItemUIUtilities.SelectedItemDisplay != null)
			{
				this.selectedItem = null;
			}
			this.OnSelectionChanged();
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00062934 File Offset: 0x00060B34
		private void OnSelectionChanged()
		{
			Action action = this.onSelectionChanged;
			if (action != null)
			{
				action();
			}
			if (this.Selection == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			Item x = null;
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				x = stockShopItemEntry.GetItem();
			}
			else
			{
				ItemDisplay itemDisplay = this.Selection as ItemDisplay;
				if (itemDisplay != null)
				{
					x = itemDisplay.Target;
				}
			}
			if (x == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			this.details.Setup(x);
			this.RefreshStockText();
			this.RefreshInteractionButton();
			this.RefreshCountDown();
			this.detailsFadeGroup.Show();
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000629DC File Offset: 0x00060BDC
		private void RefreshStockText()
		{
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				this.stockText.gameObject.SetActive(true);
				this.stockText.text = this.stockTextFormat.Format(new
				{
					text = this.stockTextKey.ToPlainText(),
					current = stockShopItemEntry.Target.CurrentStock,
					max = stockShopItemEntry.Target.MaxStock
				});
				return;
			}
			if (this.Selection is ItemDisplay)
			{
				this.stockText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x00062A64 File Offset: 0x00060C64
		public StockShopItemEntry GetSelection()
		{
			return this.Selection as StockShopItemEntry;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00062A71 File Offset: 0x00060C71
		public bool SetSelection(StockShopItemEntry selection)
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				ItemUIUtilities.Select(null);
			}
			this.selectedItem = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x00062A94 File Offset: 0x00060C94
		internal void Setup(StockShop target)
		{
			this.target = target;
			this.detailsFadeGroup.SkipHide();
			this.merchantNameText.text = target.DisplayName;
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
			this.playerInventoryDisplay.Setup(inventory2, null, (Item e) => e == null || e.CanBeSold, false, null);
			if (PetProxy.PetInventory != null)
			{
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, (Item e) => e == null || e.CanBeSold, false, null);
				this.petInventoryDisplay.gameObject.SetActive(true);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.playerStorageDisplay.gameObject.SetActive(true);
				this.playerStorageDisplay.Setup(inventory3, null, (Item e) => e == null || e.CanBeSold, false, null);
			}
			else
			{
				this.playerStorageDisplay.gameObject.SetActive(false);
			}
			this.EntryPool.ReleaseAll();
			Transform parent = this.entryTemplate.transform.parent;
			foreach (StockShop.Entry entry in target.entries)
			{
				if (entry.Show)
				{
					StockShopItemEntry stockShopItemEntry = this.EntryPool.Get(parent);
					stockShopItemEntry.Setup(this, entry);
					stockShopItemEntry.transform.SetAsLastSibling();
				}
			}
			TradingUIUtilities.ActiveMerchant = target;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00062C6C File Offset: 0x00060E6C
		private void RefreshInteractionButton()
		{
			this.cannotSellIndicator.SetActive(false);
			this.cashOnlyIndicator.SetActive(!this.Target.AccountAvaliable);
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				bool canBeSold = itemDisplay.Target.CanBeSold;
				this.interactionButton.interactable = canBeSold;
				this.priceDisplay.gameObject.SetActive(true);
				this.lockDisplay.gameObject.SetActive(false);
				this.interactionText.text = this.TextSell;
				this.interactionButtonImage.color = this.buttonColor_Interactable;
				this.priceText.text = this.<RefreshInteractionButton>g__GetPriceText|71_1(itemDisplay.Target, true);
				this.cannotSellIndicator.SetActive(!itemDisplay.Target.CanBeSold);
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				bool flag = stockShopItemEntry.IsUnlocked();
				bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(stockShopItemEntry.Target.ItemTypeID);
				this.interactionButton.interactable = (flag || flag2);
				this.priceDisplay.gameObject.SetActive(flag);
				this.lockDisplay.gameObject.SetActive(!flag);
				this.cannotSellIndicator.SetActive(false);
				if (flag)
				{
					Item item = stockShopItemEntry.GetItem();
					int num = this.<RefreshInteractionButton>g__GetPrice|71_0(item, false);
					bool enough = new Cost((long)num).Enough;
					this.priceText.text = num.ToString("n0");
					if (stockShopItemEntry.Target.CurrentStock > 0)
					{
						this.interactionText.text = this.TextBuy;
						this.interactionButtonImage.color = (enough ? this.buttonColor_Interactable : this.buttonColor_NotInteractable);
						return;
					}
					this.interactionButton.interactable = false;
					this.interactionText.text = this.TextSoldOut;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
					return;
				}
				else
				{
					if (flag2)
					{
						this.interactionText.text = this.TextUnlock;
						this.interactionButtonImage.color = this.buttonColor_Interactable;
						return;
					}
					this.interactionText.text = this.TextLocked;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
				}
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00062EA1 File Offset: 0x000610A1
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00062EB4 File Offset: 0x000610B4
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00062EC7 File Offset: 0x000610C7
		internal void SetupAndShow(StockShop stockShop)
		{
			ItemUIUtilities.Select(null);
			this.SetSelection(null);
			this.Setup(stockShop);
			base.Open(null);
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00062F5E File Offset: 0x0006115E
		[CompilerGenerated]
		private int <RefreshInteractionButton>g__GetPrice|71_0(Item item, bool selling)
		{
			return this.Target.ConvertPrice(item, selling);
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00062F70 File Offset: 0x00061170
		[CompilerGenerated]
		private string <RefreshInteractionButton>g__GetPriceText|71_1(Item item, bool selling)
		{
			return this.<RefreshInteractionButton>g__GetPrice|71_0(item, selling).ToString("n0");
		}

		// Token: 0x04001349 RID: 4937
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400134A RID: 4938
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x0400134B RID: 4939
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x0400134C RID: 4940
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x0400134D RID: 4941
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x0400134E RID: 4942
		[SerializeField]
		private InventoryDisplay playerStorageDisplay;

		// Token: 0x0400134F RID: 4943
		[SerializeField]
		private StockShopItemEntry entryTemplate;

		// Token: 0x04001350 RID: 4944
		[SerializeField]
		private TextMeshProUGUI stockText;

		// Token: 0x04001351 RID: 4945
		[SerializeField]
		[LocalizationKey("Default")]
		private string stockTextKey = "UI_Stock";

		// Token: 0x04001352 RID: 4946
		[SerializeField]
		private string stockTextFormat = "{text} {current}/{max}";

		// Token: 0x04001353 RID: 4947
		[SerializeField]
		private TextMeshProUGUI merchantNameText;

		// Token: 0x04001354 RID: 4948
		[SerializeField]
		private Button interactionButton;

		// Token: 0x04001355 RID: 4949
		[SerializeField]
		private Image interactionButtonImage;

		// Token: 0x04001356 RID: 4950
		[SerializeField]
		private Color buttonColor_Interactable;

		// Token: 0x04001357 RID: 4951
		[SerializeField]
		private Color buttonColor_NotInteractable;

		// Token: 0x04001358 RID: 4952
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x04001359 RID: 4953
		[SerializeField]
		private GameObject cashOnlyIndicator;

		// Token: 0x0400135A RID: 4954
		[SerializeField]
		private GameObject cannotSellIndicator;

		// Token: 0x0400135B RID: 4955
		[LocalizationKey("Default")]
		[SerializeField]
		private string textBuy = "购买";

		// Token: 0x0400135C RID: 4956
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSoldOut = "已售罄";

		// Token: 0x0400135D RID: 4957
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSell = "出售";

		// Token: 0x0400135E RID: 4958
		[LocalizationKey("Default")]
		[SerializeField]
		private string textUnlock = "解锁";

		// Token: 0x0400135F RID: 4959
		[LocalizationKey("Default")]
		[SerializeField]
		private string textLocked = "已锁定";

		// Token: 0x04001360 RID: 4960
		[SerializeField]
		private GameObject priceDisplay;

		// Token: 0x04001361 RID: 4961
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001362 RID: 4962
		[SerializeField]
		private GameObject lockDisplay;

		// Token: 0x04001363 RID: 4963
		[SerializeField]
		private FadeGroup clickBlockerFadeGroup;

		// Token: 0x04001364 RID: 4964
		[SerializeField]
		private TextMeshProUGUI refreshCountDown;

		// Token: 0x04001365 RID: 4965
		private string sfx_Buy = "UI/buy";

		// Token: 0x04001366 RID: 4966
		private string sfx_Sell = "UI/sell";

		// Token: 0x04001367 RID: 4967
		private PrefabPool<StockShopItemEntry> _entryPool;

		// Token: 0x04001368 RID: 4968
		private StockShop target;

		// Token: 0x04001369 RID: 4969
		private StockShopItemEntry selectedItem;

		// Token: 0x0400136A RID: 4970
		public Action onSelectionChanged;
	}
}
