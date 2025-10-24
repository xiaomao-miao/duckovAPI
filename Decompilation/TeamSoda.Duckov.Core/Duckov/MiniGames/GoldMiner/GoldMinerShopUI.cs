using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029E RID: 670
	public class GoldMinerShopUI : MiniGameBehaviour
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x00050E34 File Offset: 0x0004F034
		// (set) Token: 0x060015DD RID: 5597 RVA: 0x00050E3C File Offset: 0x0004F03C
		public GoldMinerShop target { get; private set; }

		// Token: 0x060015DE RID: 5598 RVA: 0x00050E45 File Offset: 0x0004F045
		private void UnregisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Remove(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00050E7D File Offset: 0x0004F07D
		private void RegisterEvent()
		{
			if (this.target == null)
			{
				return;
			}
			GoldMinerShop target = this.target;
			target.onAfterOperation = (Action)Delegate.Combine(target.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00050EB5 File Offset: 0x0004F0B5
		private void OnAfterOperation()
		{
			this.RefreshEntries();
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x00050EC0 File Offset: 0x0004F0C0
		private void RefreshEntries()
		{
			for (int i = 0; i < this.entries.Length; i++)
			{
				GoldMinerShopUIEntry goldMinerShopUIEntry = this.entries[i];
				if (i >= this.target.stock.Count)
				{
					goldMinerShopUIEntry.gameObject.SetActive(false);
				}
				else
				{
					goldMinerShopUIEntry.gameObject.SetActive(true);
					ShopEntity target = this.target.stock[i];
					goldMinerShopUIEntry.Setup(this, target);
				}
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00050F30 File Offset: 0x0004F130
		public void Setup(GoldMinerShop shop)
		{
			this.UnregisterEvent();
			this.target = shop;
			this.RegisterEvent();
			this.RefreshEntries();
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00050F4B File Offset: 0x0004F14B
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.RefreshDescriptionText();
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00050F5C File Offset: 0x0004F15C
		private void RefreshDescriptionText()
		{
			string text = "";
			if (this.hoveringEntry != null && this.hoveringEntry.target != null && this.hoveringEntry.target.artifact != null)
			{
				text = this.hoveringEntry.target.artifact.Description;
			}
			this.descriptionText.text = text;
		}

		// Token: 0x04001026 RID: 4134
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001027 RID: 4135
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04001028 RID: 4136
		[SerializeField]
		private GoldMinerShopUIEntry[] entries;

		// Token: 0x04001029 RID: 4137
		public int navIndex;

		// Token: 0x0400102B RID: 4139
		public bool enableInput;

		// Token: 0x0400102C RID: 4140
		public GoldMinerShopUIEntry hoveringEntry;
	}
}
