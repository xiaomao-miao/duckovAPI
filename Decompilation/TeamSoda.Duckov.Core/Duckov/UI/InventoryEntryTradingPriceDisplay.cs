using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A1 RID: 929
	public class InventoryEntryTradingPriceDisplay : MonoBehaviour
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x00073852 File Offset: 0x00071A52
		// (set) Token: 0x0600211F RID: 8479 RVA: 0x0007385A File Offset: 0x00071A5A
		public bool Selling
		{
			get
			{
				return this.selling;
			}
			set
			{
				this.selling = value;
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00073863 File Offset: 0x00071A63
		private void Awake()
		{
			this.master.onRefresh += this.OnRefresh;
			TradingUIUtilities.OnActiveMerchantChanged += this.OnActiveMerchantChanged;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0007388D File Offset: 0x00071A8D
		private void OnActiveMerchantChanged(IMerchant merchant)
		{
			this.Refresh();
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00073895 File Offset: 0x00071A95
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0007389D File Offset: 0x00071A9D
		private void OnDestroy()
		{
			if (this.master != null)
			{
				this.master.onRefresh -= this.OnRefresh;
			}
			TradingUIUtilities.OnActiveMerchantChanged -= this.OnActiveMerchantChanged;
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000738D5 File Offset: 0x00071AD5
		private void OnRefresh(InventoryEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000738E0 File Offset: 0x00071AE0
		private void Refresh()
		{
			InventoryEntry inventoryEntry = this.master;
			Item item = (inventoryEntry != null) ? inventoryEntry.Content : null;
			if (item != null)
			{
				this.canvasGroup.alpha = 1f;
				string text = this.GetPrice(item).ToString(this.moneyFormat);
				this.priceText.text = text;
				return;
			}
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0007394C File Offset: 0x00071B4C
		private int GetPrice(Item content)
		{
			if (content == null)
			{
				return 0;
			}
			int value = content.Value;
			if (TradingUIUtilities.ActiveMerchant == null)
			{
				return value;
			}
			return TradingUIUtilities.ActiveMerchant.ConvertPrice(content, this.selling);
		}

		// Token: 0x04001676 RID: 5750
		[SerializeField]
		private InventoryEntry master;

		// Token: 0x04001677 RID: 5751
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001678 RID: 5752
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001679 RID: 5753
		[SerializeField]
		private bool selling = true;

		// Token: 0x0400167A RID: 5754
		[SerializeField]
		private string moneyFormat = "n0";
	}
}
