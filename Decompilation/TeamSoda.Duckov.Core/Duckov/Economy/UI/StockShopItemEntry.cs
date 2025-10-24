using System;
using DG.Tweening;
using Duckov.UI;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Economy.UI
{
	// Token: 0x02000327 RID: 807
	public class StockShopItemEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x00062115 File Offset: 0x00060315
		private StockShop stockShop
		{
			get
			{
				StockShopView stockShopView = this.master;
				if (stockShopView == null)
				{
					return null;
				}
				return stockShopView.Target;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00062128 File Offset: 0x00060328
		public StockShop.Entry Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00062130 File Offset: 0x00060330
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClick;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00062149 File Offset: 0x00060349
		private void OnItemDisplayPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00062152 File Offset: 0x00060352
		public Item GetItem()
		{
			return this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0006216C File Offset: 0x0006036C
		internal void Setup(StockShopView master, StockShop.Entry entry)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = entry;
			Item itemInstanceDirect = this.stockShop.GetItemInstanceDirect(this.target.ItemTypeID);
			this.itemDisplay.Setup(itemInstanceDirect);
			this.itemDisplay.ShowOperationButtons = false;
			this.itemDisplay.IsStockshopSample = true;
			int stackCount = itemInstanceDirect.StackCount;
			int num = this.stockShop.ConvertPrice(itemInstanceDirect, false);
			this.priceText.text = num.ToString(this.moneyFormat);
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00062204 File Offset: 0x00060404
		private void RegisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Combine(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged += this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00062268 File Offset: 0x00060468
		private void UnregisterEvents()
		{
			if (this.master != null)
			{
				StockShopView stockShopView = this.master;
				stockShopView.onSelectionChanged = (Action)Delegate.Remove(stockShopView.onSelectionChanged, new Action(this.OnMasterSelectionChanged));
			}
			if (this.target != null)
			{
				this.target.onStockChanged -= this.OnTargetStockChanged;
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000622C9 File Offset: 0x000604C9
		private void OnMasterSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000622D1 File Offset: 0x000604D1
		private void OnTargetStockChanged(StockShop.Entry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000622D9 File Offset: 0x000604D9
		public bool IsUnlocked()
		{
			return this.target != null && (this.target.ForceUnlock || EconomyManager.IsUnlocked(this.target.ItemTypeID));
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00062304 File Offset: 0x00060504
		private void Refresh()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			bool active = this.master.GetSelection() == this;
			this.selectionIndicator.SetActive(active);
			bool flag = EconomyManager.IsUnlocked(this.target.ItemTypeID);
			bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID);
			if (this.target.ForceUnlock)
			{
				flag = true;
				flag2 = false;
			}
			this.lockedIndicator.SetActive(!flag && !flag2);
			this.waitingForUnlockIndicator.SetActive(!flag && flag2);
			base.gameObject.SetActive(flag || flag2);
			this.outOfStockIndicator.SetActive(this.Target.CurrentStock <= 0);
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000623C0 File Offset: 0x000605C0
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (this.master == null)
			{
				return;
			}
			eventData.Use();
			if (EconomyManager.IsWaitingForUnlockConfirm(this.target.ItemTypeID))
			{
				EconomyManager.ConfirmUnlock(this.target.ItemTypeID);
			}
			if (this.master.GetSelection() == this)
			{
				this.master.SetSelection(null);
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00062438 File Offset: 0x00060638
		public void Punch()
		{
			this.selectionIndicator.transform.DOKill(false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			this.selectionIndicator.transform.DOPunchScale(Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0006249A File Offset: 0x0006069A
		private void OnEnable()
		{
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000624AD File Offset: 0x000606AD
		private void OnDisable()
		{
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000624C0 File Offset: 0x000606C0
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.target == null)
			{
				return;
			}
			if (itemTypeID == this.target.ItemTypeID)
			{
				this.Refresh();
			}
		}

		// Token: 0x0400133D RID: 4925
		[SerializeField]
		private string moneyFormat = "n0";

		// Token: 0x0400133E RID: 4926
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400133F RID: 4927
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001340 RID: 4928
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001341 RID: 4929
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x04001342 RID: 4930
		[SerializeField]
		private GameObject waitingForUnlockIndicator;

		// Token: 0x04001343 RID: 4931
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x04001344 RID: 4932
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x04001345 RID: 4933
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x04001346 RID: 4934
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x04001347 RID: 4935
		private StockShopView master;

		// Token: 0x04001348 RID: 4936
		private StockShop.Entry target;
	}
}
