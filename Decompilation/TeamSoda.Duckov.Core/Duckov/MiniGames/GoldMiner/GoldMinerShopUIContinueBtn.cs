using System;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029F RID: 671
	public class GoldMinerShopUIContinueBtn : MonoBehaviour
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x00050FCC File Offset: 0x0004F1CC
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00051019 File Offset: 0x0004F219
		private void OnInteract(NavEntry entry)
		{
			this.shop.Continue();
		}

		// Token: 0x0400102D RID: 4141
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x0400102E RID: 4142
		[SerializeField]
		private NavEntry navEntry;
	}
}
