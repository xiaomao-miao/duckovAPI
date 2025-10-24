using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x0200037B RID: 891
	public class ItemHoveringUI : MonoBehaviour
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x0006C2E8 File Offset: 0x0006A4E8
		// (set) Token: 0x06001ED4 RID: 7892 RVA: 0x0006C2EF File Offset: 0x0006A4EF
		public static ItemHoveringUI Instance { get; private set; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x0006C2F7 File Offset: 0x0006A4F7
		public RectTransform LayoutParent
		{
			get
			{
				return this.layoutParent;
			}
		}

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06001ED6 RID: 7894 RVA: 0x0006C300 File Offset: 0x0006A500
		// (remove) Token: 0x06001ED7 RID: 7895 RVA: 0x0006C334 File Offset: 0x0006A534
		public static event Action<ItemHoveringUI, ItemMetaData> onSetupMeta;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06001ED8 RID: 7896 RVA: 0x0006C368 File Offset: 0x0006A568
		// (remove) Token: 0x06001ED9 RID: 7897 RVA: 0x0006C39C File Offset: 0x0006A59C
		public static event Action<ItemHoveringUI, Item> onSetupItem;

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0006C3CF File Offset: 0x0006A5CF
		// (set) Token: 0x06001EDB RID: 7899 RVA: 0x0006C3D6 File Offset: 0x0006A5D6
		public static int DisplayingItemID { get; private set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x0006C3DE File Offset: 0x0006A5DE
		public static bool Shown
		{
			get
			{
				return !(ItemHoveringUI.Instance == null) && ItemHoveringUI.Instance.fadeGroup.IsShown;
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0006C400 File Offset: 0x0006A600
		private void Awake()
		{
			ItemHoveringUI.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			ItemDisplay.OnPointerEnterItemDisplay += this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay += this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter += this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit += this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter += this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit += this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0006C494 File Offset: 0x0006A694
		private void OnDestroy()
		{
			ItemDisplay.OnPointerEnterItemDisplay -= this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay -= this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter -= this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit -= this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter -= this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit -= this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0006C507 File Offset: 0x0006A707
		private void OnMouseExitMetaDisplay(ItemMetaDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0006C51D File Offset: 0x0006A71D
		private void OnMouseEnterMetaDisplay(ItemMetaDisplay display)
		{
			this.SetupAndShowMeta<ItemMetaDisplay>(display);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0006C526 File Offset: 0x0006A726
		private void OnMouseExitItemAmountDisplay(ItemAmountDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0006C53C File Offset: 0x0006A73C
		private void OnMouseEnterItemAmountDisplay(ItemAmountDisplay display)
		{
			this.SetupAndShowMeta<ItemAmountDisplay>(display);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0006C545 File Offset: 0x0006A745
		private void OnPointerExitItemDisplay(ItemDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0006C55B File Offset: 0x0006A75B
		private void OnPointerEnterItemDisplay(ItemDisplay display)
		{
			this.SetupAndShow(display);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0006C564 File Offset: 0x0006A764
		private void SetupAndShow(ItemDisplay display)
		{
			if (display == null)
			{
				return;
			}
			Item item = display.Target;
			if (item == null)
			{
				return;
			}
			if (item.NeedInspection)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = display;
			this.itemName.text = (item.DisplayName ?? "");
			this.itemDescription.text = (item.Description ?? "");
			this.weightDisplay.gameObject.SetActive(true);
			this.weightDisplay.text = string.Format("{0:0.#} kg", item.TotalWeight);
			this.itemID.text = string.Format("#{0}", item.TypeID);
			ItemHoveringUI.DisplayingItemID = item.TypeID;
			this.itemProperties.gameObject.SetActive(true);
			this.itemProperties.Setup(item);
			this.interactionIndicatorsContainer.SetActive(true);
			this.interactionIndicator_Menu.SetActive(display.ShowOperationButtons);
			this.interactionIndicator_Move.SetActive(display.Movable);
			this.interactionIndicator_Drop.SetActive(display.CanDrop);
			this.interactionIndicator_Use.SetActive(display.CanUse);
			this.interactionIndicator_Split.SetActive(display.CanSplit);
			this.interactionIndicator_LockSort.SetActive(display.CanLockSort);
			this.interactionIndicator_Shortcut.SetActive(display.CanSetShortcut);
			this.usageUtilitiesDisplay.Setup(item);
			this.SetupWishlistInfos(item.TypeID);
			this.SetupBulletDisplay();
			try
			{
				Action<ItemHoveringUI, Item> action = ItemHoveringUI.onSetupItem;
				if (action != null)
				{
					action(this, item);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.RefreshPosition();
			this.SetupRegisteredInfo(item);
			this.fadeGroup.Show();
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0006C73C File Offset: 0x0006A93C
		private void SetupRegisteredInfo(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (item.IsRegistered())
			{
				this.registeredIndicator.SetActive(true);
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0006C75C File Offset: 0x0006A95C
		private void SetupAndShowMeta<T>(T dataProvider) where T : MonoBehaviour, IItemMetaDataProvider
		{
			if (dataProvider == null)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = dataProvider;
			ItemMetaData metaData = dataProvider.GetMetaData();
			this.itemName.text = metaData.DisplayName;
			this.itemID.text = string.Format("{0}", metaData.id);
			ItemHoveringUI.DisplayingItemID = metaData.id;
			this.itemDescription.text = metaData.Description;
			this.interactionIndicatorsContainer.SetActive(true);
			this.weightDisplay.gameObject.SetActive(false);
			this.bulletTypeDisplay.gameObject.SetActive(false);
			this.itemProperties.gameObject.SetActive(false);
			this.interactionIndicator_Menu.gameObject.SetActive(false);
			this.interactionIndicator_Move.gameObject.SetActive(false);
			this.interactionIndicator_Drop.gameObject.SetActive(false);
			this.interactionIndicator_Use.gameObject.SetActive(false);
			this.usageUtilitiesDisplay.gameObject.SetActive(false);
			this.interactionIndicator_Split.SetActive(false);
			this.interactionIndicator_Shortcut.SetActive(false);
			this.SetupWishlistInfos(metaData.id);
			try
			{
				Action<ItemHoveringUI, ItemMetaData> action = ItemHoveringUI.onSetupMeta;
				if (action != null)
				{
					action(this, metaData);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.RefreshPosition();
			this.fadeGroup.Show();
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0006C8E0 File Offset: 0x0006AAE0
		private void SetupBulletDisplay()
		{
			ItemDisplay itemDisplay = this.target as ItemDisplay;
			if (itemDisplay == null)
			{
				return;
			}
			ItemSetting_Gun component = itemDisplay.Target.GetComponent<ItemSetting_Gun>();
			if (component == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(component.TargetBulletID);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0006C94C File Offset: 0x0006AB4C
		private unsafe void RefreshPosition()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, null, out vector);
			float xMax = this.contents.rect.xMax;
			float yMin = this.contents.rect.yMin;
			float b = this.rectTransform.rect.xMax - xMax;
			float b2 = this.rectTransform.rect.yMin - yMin;
			vector.x = Mathf.Min(vector.x, b);
			vector.y = Mathf.Max(vector.y, b2);
			this.contents.anchoredPosition = vector;
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0006CA0C File Offset: 0x0006AC0C
		private void Hide()
		{
			this.fadeGroup.Hide();
			ItemHoveringUI.DisplayingItemID = -1;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0006CA20 File Offset: 0x0006AC20
		private void Update()
		{
			if (this.fadeGroup.IsShown)
			{
				if (this.target == null || !this.target.isActiveAndEnabled)
				{
					this.Hide();
				}
				ItemDisplay itemDisplay = this.target as ItemDisplay;
				if (itemDisplay != null && itemDisplay.Target == null)
				{
					this.Hide();
				}
			}
			this.RefreshPosition();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0006CA84 File Offset: 0x0006AC84
		private void SetupWishlistInfos(int itemTypeID)
		{
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(itemTypeID);
			bool isManuallyWishlisted = wishlistInfo.isManuallyWishlisted;
			bool isBuildingRequired = wishlistInfo.isBuildingRequired;
			bool isQuestRequired = wishlistInfo.isQuestRequired;
			bool active = isManuallyWishlisted || isBuildingRequired || isQuestRequired;
			this.wishlistIndicator.SetActive(isManuallyWishlisted);
			this.buildingIndicator.SetActive(isBuildingRequired);
			this.questIndicator.SetActive(isQuestRequired);
			this.wishlistInfoParent.SetActive(active);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x0006CAE1 File Offset: 0x0006ACE1
		internal static void NotifyRefreshWishlistInfo()
		{
			if (ItemHoveringUI.Instance == null)
			{
				return;
			}
			ItemHoveringUI.Instance.SetupWishlistInfos(ItemHoveringUI.DisplayingItemID);
		}

		// Token: 0x04001511 RID: 5393
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001512 RID: 5394
		[SerializeField]
		private RectTransform layoutParent;

		// Token: 0x04001513 RID: 5395
		[SerializeField]
		private RectTransform contents;

		// Token: 0x04001514 RID: 5396
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001515 RID: 5397
		[SerializeField]
		private TextMeshProUGUI itemName;

		// Token: 0x04001516 RID: 5398
		[SerializeField]
		private TextMeshProUGUI weightDisplay;

		// Token: 0x04001517 RID: 5399
		[SerializeField]
		private TextMeshProUGUI itemDescription;

		// Token: 0x04001518 RID: 5400
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x04001519 RID: 5401
		[SerializeField]
		private ItemPropertiesDisplay itemProperties;

		// Token: 0x0400151A RID: 5402
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x0400151B RID: 5403
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x0400151C RID: 5404
		[SerializeField]
		private GameObject interactionIndicatorsContainer;

		// Token: 0x0400151D RID: 5405
		[SerializeField]
		private GameObject interactionIndicator_Move;

		// Token: 0x0400151E RID: 5406
		[SerializeField]
		private GameObject interactionIndicator_Menu;

		// Token: 0x0400151F RID: 5407
		[SerializeField]
		private GameObject interactionIndicator_Drop;

		// Token: 0x04001520 RID: 5408
		[SerializeField]
		private GameObject interactionIndicator_Use;

		// Token: 0x04001521 RID: 5409
		[SerializeField]
		private GameObject interactionIndicator_Split;

		// Token: 0x04001522 RID: 5410
		[SerializeField]
		private GameObject interactionIndicator_LockSort;

		// Token: 0x04001523 RID: 5411
		[SerializeField]
		private GameObject interactionIndicator_Shortcut;

		// Token: 0x04001524 RID: 5412
		[SerializeField]
		private GameObject wishlistInfoParent;

		// Token: 0x04001525 RID: 5413
		[SerializeField]
		private GameObject wishlistIndicator;

		// Token: 0x04001526 RID: 5414
		[SerializeField]
		private GameObject buildingIndicator;

		// Token: 0x04001527 RID: 5415
		[SerializeField]
		private GameObject questIndicator;

		// Token: 0x04001528 RID: 5416
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x0400152C RID: 5420
		private MonoBehaviour target;
	}
}
