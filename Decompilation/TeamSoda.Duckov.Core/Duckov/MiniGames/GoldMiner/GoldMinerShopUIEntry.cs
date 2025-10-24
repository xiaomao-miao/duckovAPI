using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A0 RID: 672
	public class GoldMinerShopUIEntry : MonoBehaviour
	{
		// Token: 0x060015E9 RID: 5609 RVA: 0x00051030 File Offset: 0x0004F230
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.OnVCTEnter));
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x000510B2 File Offset: 0x0004F2B2
		private void OnVCTEnter()
		{
			this.master.hoveringEntry = this;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x000510C0 File Offset: 0x0004F2C0
		private void OnInteract(NavEntry entry)
		{
			this.master.target.Buy(this.target);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x000510DC File Offset: 0x0004F2DC
		internal void Setup(GoldMinerShopUI master, ShopEntity target)
		{
			this.master = master;
			this.target = target;
			if (target == null || target.artifact == null)
			{
				this.SetupEmpty();
				return;
			}
			this.mainLayout.SetActive(true);
			this.nameText.text = target.artifact.DisplayName;
			this.icon.sprite = target.artifact.Icon;
			this.Refresh();
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00051150 File Offset: 0x0004F350
		private void Refresh()
		{
			bool flag;
			int num = this.master.target.CalculateDealPrice(this.target, out flag);
			this.priceText.text = num.ToString(this.priceFormat);
			this.priceIndicator.SetActive(num > 0);
			this.freeIndicator.SetActive(num <= 0);
			this.soldIndicator.SetActive(this.target.sold);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000511C5 File Offset: 0x0004F3C5
		private void SetupEmpty()
		{
			this.mainLayout.SetActive(false);
		}

		// Token: 0x0400102F RID: 4143
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x04001030 RID: 4144
		[SerializeField]
		private VirtualCursorTarget VCT;

		// Token: 0x04001031 RID: 4145
		[SerializeField]
		private GameObject mainLayout;

		// Token: 0x04001032 RID: 4146
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001033 RID: 4147
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001034 RID: 4148
		[SerializeField]
		private string priceFormat = "0";

		// Token: 0x04001035 RID: 4149
		[SerializeField]
		private GameObject priceIndicator;

		// Token: 0x04001036 RID: 4150
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x04001037 RID: 4151
		[SerializeField]
		private Image icon;

		// Token: 0x04001038 RID: 4152
		[SerializeField]
		private GameObject soldIndicator;

		// Token: 0x04001039 RID: 4153
		private GoldMinerShopUI master;

		// Token: 0x0400103A RID: 4154
		public ShopEntity target;
	}
}
