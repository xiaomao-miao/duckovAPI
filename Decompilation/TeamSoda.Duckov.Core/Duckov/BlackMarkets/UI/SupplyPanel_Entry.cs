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
	// Token: 0x0200030C RID: 780
	public class SupplyPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x0005CA66 File Offset: 0x0005AC66
		// (set) Token: 0x0600199B RID: 6555 RVA: 0x0005CA6E File Offset: 0x0005AC6E
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x0600199C RID: 6556 RVA: 0x0005CA78 File Offset: 0x0005AC78
		// (remove) Token: 0x0600199D RID: 6557 RVA: 0x0005CAB0 File Offset: 0x0005ACB0
		public event Action<SupplyPanel_Entry> onDealButtonClicked;

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x0005CAE5 File Offset: 0x0005ACE5
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor <= 0.9f)
				{
					return this.titleFormatKey_Low;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x0005CB14 File Offset: 0x0005AD14
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

		// Token: 0x060019A0 RID: 6560 RVA: 0x0005CB44 File Offset: 0x0005AD44
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.BuyCost.Enough;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0005CB7E File Offset: 0x0005AD7E
		public void NotifyPooled()
		{
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0005CB80 File Offset: 0x0005AD80
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0005CBA1 File Offset: 0x0005ADA1
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0005CBAC File Offset: 0x0005ADAC
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.BuyCost, 1);
			this.resultDisplay.Setup(target.ItemID, (long)target.ItemMetaData.defaultStackCount);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0005CC2C File Offset: 0x0005AE2C
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0005CC3F File Offset: 0x0005AE3F
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0005CC52 File Offset: 0x0005AE52
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0005CC70 File Offset: 0x0005AE70
		private void OnDealButtonClicked()
		{
			Action<SupplyPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0005CC84 File Offset: 0x0005AE84
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

		// Token: 0x04001282 RID: 4738
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x04001283 RID: 4739
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001284 RID: 4740
		[SerializeField]
		private ItemAmountDisplay resultDisplay;

		// Token: 0x04001285 RID: 4741
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x04001286 RID: 4742
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x04001287 RID: 4743
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x04001288 RID: 4744
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x04001289 RID: 4745
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Supply_Title_Normal";

		// Token: 0x0400128A RID: 4746
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Low = "BlackMarket_Supply_Title_Low";

		// Token: 0x0400128B RID: 4747
		[SerializeField]
		private Button dealButton;
	}
}
