using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x0200030A RID: 778
	public class DemandPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x0005C5E6 File Offset: 0x0005A7E6
		// (set) Token: 0x0600197D RID: 6525 RVA: 0x0005C5EE File Offset: 0x0005A7EE
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x0600197E RID: 6526 RVA: 0x0005C5F8 File Offset: 0x0005A7F8
		// (remove) Token: 0x0600197F RID: 6527 RVA: 0x0005C630 File Offset: 0x0005A830
		public event Action<DemandPanel_Entry> onDealButtonClicked;

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0005C665 File Offset: 0x0005A865
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor >= 1.9f)
				{
					return this.titleFormatKey_High;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0005C694 File Offset: 0x0005A894
		private string TitleText
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				return this.TitleFormatKey.ToPlainText().Format(new
				{
					itemName = this.Target.ItemDisplayName
				});
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0005C6C4 File Offset: 0x0005A8C4
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.SellCost.Enough;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0005C6FE File Offset: 0x0005A8FE
		public void NotifyPooled()
		{
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0005C700 File Offset: 0x0005A900
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0005C721 File Offset: 0x0005A921
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0005C729 File Offset: 0x0005A929
		public void OnDealButtonClicked()
		{
			Action<DemandPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0005C73C File Offset: 0x0005A93C
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.SellCost, 1);
			this.moneyDisplay.text = string.Format("{0}", target.TotalPrice);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0005C7BF File Offset: 0x0005A9BF
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0005C7D2 File Offset: 0x0005A9D2
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0005C7E5 File Offset: 0x0005A9E5
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0005C804 File Offset: 0x0005AA04
		private void Refresh()
		{
			if (this.Target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.remainingAmountDisplay.text = string.Format("{0}", this.Target.Remaining);
			bool active = this.CanInteract();
			this.canInteractIndicator.SetActive(active);
			bool active2 = this.Target.Remaining <= 0;
			this.outOfStockIndicator.SetActive(active2);
			this.remainingInfoContainer.SetActive(this.Target.remaining > 1);
		}

		// Token: 0x04001273 RID: 4723
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x04001274 RID: 4724
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001275 RID: 4725
		[SerializeField]
		private TextMeshProUGUI moneyDisplay;

		// Token: 0x04001276 RID: 4726
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x04001277 RID: 4727
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x04001278 RID: 4728
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x04001279 RID: 4729
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x0400127A RID: 4730
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Demand_Title_Normal";

		// Token: 0x0400127B RID: 4731
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_High = "BlackMarket_Demand_Title_High";

		// Token: 0x0400127C RID: 4732
		[SerializeField]
		private Button dealButton;
	}
}
