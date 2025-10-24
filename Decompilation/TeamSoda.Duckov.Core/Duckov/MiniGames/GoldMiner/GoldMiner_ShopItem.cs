using System;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000295 RID: 661
	public class GoldMiner_ShopItem : MonoBehaviour
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x0005081E File Offset: 0x0004EA1E
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00050826 File Offset: 0x0004EA26
		public string DisplayNameKey
		{
			get
			{
				return this.displayNameKey;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x0005082E File Offset: 0x0004EA2E
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x0005083B File Offset: 0x0004EA3B
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00050843 File Offset: 0x0004EA43
		public void OnBought(GoldMiner target)
		{
			UnityEvent<GoldMiner> unityEvent = this.onBought;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(target);
		}

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400100D RID: 4109
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayNameKey;

		// Token: 0x0400100E RID: 4110
		[SerializeField]
		private int basePrice;

		// Token: 0x0400100F RID: 4111
		public UnityEvent<GoldMiner> onBought;
	}
}
