using System;
using DG.Tweening;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000399 RID: 921
	public class ItemDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x00070F1A File Offset: 0x0006F11A
		private Sprite FallbackIcon
		{
			get
			{
				return GameplayDataSettings.UIStyle.FallbackItemIcon;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x00070F26 File Offset: 0x0006F126
		// (set) Token: 0x06002060 RID: 8288 RVA: 0x00070F2E File Offset: 0x0006F12E
		public Item Target { get; private set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x00070F37 File Offset: 0x0006F137
		// (set) Token: 0x06002062 RID: 8290 RVA: 0x00070F3F File Offset: 0x0006F13F
		internal Action releaseAction { get; set; }

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06002063 RID: 8291 RVA: 0x00070F48 File Offset: 0x0006F148
		// (remove) Token: 0x06002064 RID: 8292 RVA: 0x00070F80 File Offset: 0x0006F180
		internal event Action<ItemDisplay, PointerEventData> onDoubleClicked;

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06002065 RID: 8293 RVA: 0x00070FB8 File Offset: 0x0006F1B8
		// (remove) Token: 0x06002066 RID: 8294 RVA: 0x00070FF0 File Offset: 0x0006F1F0
		public event Action<PointerEventData> onReceiveDrop;

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x00071025 File Offset: 0x0006F225
		public bool Selected
		{
			get
			{
				return ItemUIUtilities.SelectedItemDisplay == this;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x00071034 File Offset: 0x0006F234
		private PrefabPool<SlotIndicator> SlotIndicatorPool
		{
			get
			{
				if (this._slotIndicatorPool == null)
				{
					if (this.slotIndicatorTemplate == null)
					{
						Debug.LogError("SI is null", base.gameObject);
					}
					this._slotIndicatorPool = new PrefabPool<SlotIndicator>(this.slotIndicatorTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._slotIndicatorPool;
			}
		}

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06002069 RID: 8297 RVA: 0x0007108C File Offset: 0x0006F28C
		// (remove) Token: 0x0600206A RID: 8298 RVA: 0x000710C0 File Offset: 0x0006F2C0
		public static event Action<ItemDisplay> OnPointerEnterItemDisplay;

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x0600206B RID: 8299 RVA: 0x000710F4 File Offset: 0x0006F2F4
		// (remove) Token: 0x0600206C RID: 8300 RVA: 0x00071128 File Offset: 0x0006F328
		public static event Action<ItemDisplay> OnPointerExitItemDisplay;

		// Token: 0x0600206D RID: 8301 RVA: 0x0007115C File Offset: 0x0006F35C
		public void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			this.slotIndicatorTemplate.gameObject.SetActive(false);
			if (target == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.icon.color = Color.white;
				this.icon.sprite = target.Icon;
				if (this.icon.sprite == null)
				{
					this.icon.sprite = this.FallbackIcon;
				}
				this.icon.gameObject.SetActive(true);
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
				this.displayQualityShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.displayQualityShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.displayQualityShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				bool stackable = this.Target.Stackable;
				this.countGameObject.SetActive(stackable);
				this.nameText.text = this.Target.DisplayName;
				if (target.Slots != null)
				{
					foreach (Slot target2 in target.Slots)
					{
						this.SlotIndicatorPool.Get(null).Setup(target2);
					}
				}
			}
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x0600206E RID: 8302 RVA: 0x000712D8 File Offset: 0x0006F4D8
		// (remove) Token: 0x0600206F RID: 8303 RVA: 0x00071310 File Offset: 0x0006F510
		public event Action<ItemDisplay, PointerEventData> onPointerClick;

		// Token: 0x06002070 RID: 8304 RVA: 0x00071348 File Offset: 0x0006F548
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged += this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy += this.OnTargetDestroy;
			this.Target.onSetStackCount += this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged += this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000713E8 File Offset: 0x0006F5E8
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged -= this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy -= this.OnTargetDestroy;
			this.Target.onSetStackCount -= this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged -= this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00071482 File Offset: 0x0006F682
		private void OnWishlistChanged(int type)
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.TypeID == type)
			{
				this.RefreshWishlistInfo();
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000714A7 File Offset: 0x0006F6A7
		private void OnTargetDurabilityChanged(Item item)
		{
			this.Refresh();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000714AF File Offset: 0x0006F6AF
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000714B1 File Offset: 0x0006F6B1
		private void OnTargetSetStackCount(Item item)
		{
			if (item != this.Target)
			{
				Debug.LogError("触发事件的Item不匹配!");
			}
			this.Refresh();
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000714D1 File Offset: 0x0006F6D1
		private void OnItemUtilitiesSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000714D9 File Offset: 0x0006F6D9
		private void OnTargetInspectionStateChanged(Item item)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000714E7 File Offset: 0x0006F6E7
		private void Clear()
		{
			this.SlotIndicatorPool.ReleaseAll();
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000714F4 File Offset: 0x0006F6F4
		private void SetupEmpty()
		{
			this.icon.sprite = EmptySprite.Get();
			this.icon.color = Color.clear;
			this.countText.text = string.Empty;
			this.nameText.text = string.Empty;
			this.durabilityFill.fillAmount = 0f;
			this.durabilityLoss.fillAmount = 0f;
			this.durabilityZeroIndicator.gameObject.SetActive(false);
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00071574 File Offset: 0x0006F774
		private void Refresh()
		{
			if (this == null)
			{
				Debug.Log("NULL");
				return;
			}
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.Target == null)
			{
				this.HideMainContentAndDisableControl();
				this.HideInspectionElements();
				if (ItemUIUtilities.SelectedItemDisplayRaw == this)
				{
					ItemUIUtilities.Select(null);
				}
			}
			else if (this.Target.NeedInspection)
			{
				this.HideMainContentAndDisableControl();
				this.ShowInspectionElements();
			}
			else
			{
				this.HideInspectionElements();
				this.ShowMainContentAndEnableControl();
			}
			this.selectionIndicator.gameObject.SetActive(this.Selected);
			this.RefreshWishlistInfo();
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00071610 File Offset: 0x0006F810
		private void RefreshWishlistInfo()
		{
			if (this.Target == null || this.Target.NeedInspection)
			{
				this.wishlistedIndicator.SetActive(false);
				this.questRequiredIndicator.SetActive(false);
				this.buildingRequiredIndicator.SetActive(false);
				return;
			}
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(this.Target.TypeID);
			this.wishlistedIndicator.SetActive(wishlistInfo.isManuallyWishlisted);
			this.questRequiredIndicator.SetActive(wishlistInfo.isQuestRequired);
			this.buildingRequiredIndicator.SetActive(wishlistInfo.isBuildingRequired);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000716A4 File Offset: 0x0006F8A4
		private void HideMainContentAndDisableControl()
		{
			this.mainContentShown = false;
			if (this.mainContentShown && ItemUIUtilities.SelectedItemDisplay == this)
			{
				ItemUIUtilities.Select(null);
			}
			this.interactionEventReceiver.raycastTarget = false;
			this.icon.gameObject.SetActive(false);
			this.countGameObject.SetActive(false);
			this.durabilityGameObject.SetActive(false);
			this.durabilityZeroIndicator.gameObject.SetActive(false);
			this.nameContainer.SetActive(false);
			this.slotIndicatorContainer.SetActive(false);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00071734 File Offset: 0x0006F934
		private void ShowMainContentAndEnableControl()
		{
			this.mainContentShown = true;
			this.interactionEventReceiver.raycastTarget = true;
			this.icon.gameObject.SetActive(true);
			this.nameContainer.SetActive(true);
			this.countText.text = (this.Target.Stackable ? this.Target.StackCount.ToString() : string.Empty);
			bool useDurability = this.Target.UseDurability;
			if (useDurability)
			{
				float num = this.Target.Durability / this.Target.MaxDurability;
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityFillColorOverT.Evaluate(num);
				this.durabilityZeroIndicator.SetActive(this.Target.Durability <= 0f);
				this.durabilityLoss.fillAmount = this.Target.DurabilityLoss;
			}
			else
			{
				this.durabilityZeroIndicator.gameObject.SetActive(false);
			}
			this.countGameObject.SetActive(this.Target.Stackable);
			this.durabilityGameObject.SetActive(useDurability);
			this.slotIndicatorContainer.SetActive(true);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00071864 File Offset: 0x0006FA64
		private void ShowInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(true);
			bool inspecting = this.Target.Inspecting;
			if (this.inspectingElement)
			{
				this.inspectingElement.SetActive(inspecting);
			}
			if (this.notInspectingElement)
			{
				this.notInspectingElement.SetActive(!inspecting);
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000718C3 File Offset: 0x0006FAC3
		private void HideInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(false);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000718D6 File Offset: 0x0006FAD6
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000718DE File Offset: 0x0006FADE
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			if (this.Selected)
			{
				ItemUIUtilities.Select(null);
			}
			this.UnregisterEvents();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00071905 File Offset: 0x0006FB05
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			this.isBeingDestroyed = true;
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x00071925 File Offset: 0x0006FB25
		public static PrefabPool<ItemDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.ItemDisplayPool;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x00071931 File Offset: 0x0006FB31
		// (set) Token: 0x06002085 RID: 8325 RVA: 0x00071939 File Offset: 0x0006FB39
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x00071942 File Offset: 0x0006FB42
		// (set) Token: 0x06002087 RID: 8327 RVA: 0x0007194A File Offset: 0x0006FB4A
		public bool Editable { get; set; }

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x00071953 File Offset: 0x0006FB53
		// (set) Token: 0x06002089 RID: 8329 RVA: 0x0007195B File Offset: 0x0006FB5B
		public bool Movable { get; set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x00071964 File Offset: 0x0006FB64
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x0007196C File Offset: 0x0006FB6C
		public bool CanDrop { get; set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x00071975 File Offset: 0x0006FB75
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x0007197D File Offset: 0x0006FB7D
		public bool IsStockshopSample { get; set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x00071986 File Offset: 0x0006FB86
		public bool CanUse
		{
			get
			{
				return !(this.Target == null) && this.Editable && this.Target.IsUsable(CharacterMainControl.Main);
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600208F RID: 8335 RVA: 0x000719B7 File Offset: 0x0006FBB7
		public bool CanSplit
		{
			get
			{
				return !(this.Target == null) && this.Editable && (this.Movable && this.Target.StackCount > 1);
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x000719EC File Offset: 0x0006FBEC
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x000719F4 File Offset: 0x0006FBF4
		public bool CanLockSort { get; internal set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000719FD File Offset: 0x0006FBFD
		public bool CanSetShortcut
		{
			get
			{
				return !(this.Target == null) && this.showOperationButtons && ItemShortcut.IsItemValid(this.Target);
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00071A29 File Offset: 0x0006FC29
		public static ItemDisplay Get()
		{
			return ItemDisplay.Pool.Get(null);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00071A36 File Offset: 0x0006FC36
		public static void Release(ItemDisplay item)
		{
			ItemDisplay.Pool.Release(item);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00071A43 File Offset: 0x0006FC43
		public void NotifyPooled()
		{
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00071A45 File Offset: 0x0006FC45
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.SetupEmpty();
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00071A5A File Offset: 0x0006FC5A
		[ContextMenu("Select")]
		private void Select()
		{
			ItemUIUtilities.Select(this);
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00071A62 File Offset: 0x0006FC62
		public void NotifySelected()
		{
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00071A64 File Offset: 0x0006FC64
		public void NotifyUnselected()
		{
			KontextMenu.Hide(this);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00071A6C File Offset: 0x0006FC6C
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<ItemDisplay, PointerEventData> action = this.onPointerClick;
			if (action != null)
			{
				action(this, eventData);
			}
			if (!eventData.used && eventData.button == PointerEventData.InputButton.Left)
			{
				if (eventData.clickTime - this.lastClickTime <= 0.3f && !this.doubleClickInvoked)
				{
					this.doubleClickInvoked = true;
					Action<ItemDisplay, PointerEventData> action2 = this.onDoubleClicked;
					if (action2 != null)
					{
						action2(this, eventData);
					}
				}
				if (!eventData.used && (!this.Target || !this.Target.NeedInspection))
				{
					if (ItemUIUtilities.SelectedItemDisplay != this)
					{
						this.Select();
						eventData.Use();
					}
					else
					{
						ItemUIUtilities.Select(null);
						eventData.Use();
					}
				}
			}
			if (eventData.clickTime - this.lastClickTime > 0.3f)
			{
				this.doubleClickInvoked = false;
			}
			this.lastClickTime = eventData.clickTime;
			this.Punch();
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00071B4C File Offset: 0x0006FD4C
		public void Punch()
		{
			this.selectionIndicator.transform.DOKill(false);
			this.icon.transform.DOKill(false);
			this.backgroundRing.transform.DOKill(false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			this.icon.transform.localScale = Vector3.one;
			this.backgroundRing.transform.localScale = Vector3.one;
			this.selectionIndicator.transform.DOPunchScale(Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
			this.icon.transform.DOPunchScale(Vector3.one * this.iconPunchScale, this.punchDuration, 10, 1f);
			this.backgroundRing.transform.DOPunchScale(Vector3.one * this.backgroundRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00071C58 File Offset: 0x0006FE58
		public void OnPointerDown(PointerEventData eventData)
		{
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00071C5A File Offset: 0x0006FE5A
		public void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00071C5C File Offset: 0x0006FE5C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerExitItemDisplay = ItemDisplay.OnPointerExitItemDisplay;
			if (onPointerExitItemDisplay == null)
			{
				return;
			}
			onPointerExitItemDisplay(this);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00071C7D File Offset: 0x0006FE7D
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerEnterItemDisplay = ItemDisplay.OnPointerEnterItemDisplay;
			if (onPointerEnterItemDisplay == null)
			{
				return;
			}
			onPointerEnterItemDisplay(this);
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00071C9E File Offset: 0x0006FE9E
		public void OnDrop(PointerEventData eventData)
		{
			this.HandleDirectDrop(eventData);
			if (eventData.used)
			{
				return;
			}
			Action<PointerEventData> action = this.onReceiveDrop;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00071CC4 File Offset: 0x0006FEC4
		private void HandleDirectDrop(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.IsStockshopSample)
			{
				return;
			}
			IItemDragSource component = eventData.pointerDrag.gameObject.GetComponent<IItemDragSource>();
			if (component == null)
			{
				return;
			}
			if (!component.IsEditable())
			{
				return;
			}
			Item item = component.GetItem();
			if (!this.Target.TryPlug(item, false, null, 0))
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			eventData.Use();
		}

		// Token: 0x0400160B RID: 5643
		[SerializeField]
		private Image icon;

		// Token: 0x0400160C RID: 5644
		[SerializeField]
		private TrueShadow displayQualityShadow;

		// Token: 0x0400160D RID: 5645
		[SerializeField]
		private GameObject countGameObject;

		// Token: 0x0400160E RID: 5646
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x0400160F RID: 5647
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001610 RID: 5648
		[SerializeField]
		private Graphic interactionEventReceiver;

		// Token: 0x04001611 RID: 5649
		[SerializeField]
		private GameObject backgroundRing;

		// Token: 0x04001612 RID: 5650
		[SerializeField]
		private GameObject inspectionElementRoot;

		// Token: 0x04001613 RID: 5651
		[SerializeField]
		private GameObject inspectingElement;

		// Token: 0x04001614 RID: 5652
		[SerializeField]
		private GameObject notInspectingElement;

		// Token: 0x04001615 RID: 5653
		[SerializeField]
		private GameObject nameContainer;

		// Token: 0x04001616 RID: 5654
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001617 RID: 5655
		[SerializeField]
		private GameObject durabilityGameObject;

		// Token: 0x04001618 RID: 5656
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private Gradient durabilityFillColorOverT;

		// Token: 0x0400161A RID: 5658
		[SerializeField]
		private GameObject durabilityZeroIndicator;

		// Token: 0x0400161B RID: 5659
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x0400161C RID: 5660
		[SerializeField]
		private GameObject slotIndicatorContainer;

		// Token: 0x0400161D RID: 5661
		[SerializeField]
		private SlotIndicator slotIndicatorTemplate;

		// Token: 0x0400161E RID: 5662
		[SerializeField]
		private GameObject wishlistedIndicator;

		// Token: 0x0400161F RID: 5663
		[SerializeField]
		private GameObject questRequiredIndicator;

		// Token: 0x04001620 RID: 5664
		[SerializeField]
		private GameObject buildingRequiredIndicator;

		// Token: 0x04001621 RID: 5665
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x04001622 RID: 5666
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x04001623 RID: 5667
		[SerializeField]
		[Range(-1f, 1f)]
		private float backgroundRingPunchScale = 0.2f;

		// Token: 0x04001624 RID: 5668
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x04001629 RID: 5673
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x0400162A RID: 5674
		private PrefabPool<SlotIndicator> _slotIndicatorPool;

		// Token: 0x0400162E RID: 5678
		private bool mainContentShown = true;

		// Token: 0x0400162F RID: 5679
		private bool isBeingDestroyed;

		// Token: 0x04001630 RID: 5680
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x04001636 RID: 5686
		private float lastClickTime;

		// Token: 0x04001637 RID: 5687
		private bool doubleClickInvoked;
	}
}
