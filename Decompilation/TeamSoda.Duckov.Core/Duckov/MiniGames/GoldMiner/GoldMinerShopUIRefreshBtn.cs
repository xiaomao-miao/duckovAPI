using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A1 RID: 673
	public class GoldMinerShopUIRefreshBtn : MonoBehaviour
	{
		// Token: 0x060015F0 RID: 5616 RVA: 0x000511E8 File Offset: 0x0004F3E8
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMinerShop goldMinerShop = this.shop;
			goldMinerShop.onAfterOperation = (Action)Delegate.Combine(goldMinerShop.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0005125C File Offset: 0x0004F45C
		private void OnEnable()
		{
			this.RefreshCostText();
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00051264 File Offset: 0x0004F464
		private void OnAfterOperation()
		{
			this.RefreshCostText();
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0005126C File Offset: 0x0004F46C
		private void RefreshCostText()
		{
			this.costText.text = string.Format("${0}", this.shop.GetRefreshCost());
			this.refreshChanceText.text = string.Format("{0}", this.shop.refreshChance);
			this.noChanceIndicator.SetActive(this.shop.refreshChance < 1);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000512DC File Offset: 0x0004F4DC
		private void OnInteract(NavEntry entry)
		{
			this.shop.TryRefresh();
		}

		// Token: 0x0400103B RID: 4155
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x0400103C RID: 4156
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x0400103D RID: 4157
		[SerializeField]
		private TextMeshProUGUI costText;

		// Token: 0x0400103E RID: 4158
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x0400103F RID: 4159
		[SerializeField]
		private GameObject noChanceIndicator;
	}
}
