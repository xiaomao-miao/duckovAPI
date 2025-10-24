using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AF RID: 943
	public static class TradingUIUtilities
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x00076002 File Offset: 0x00074202
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x0007600E File Offset: 0x0007420E
		public static IMerchant ActiveMerchant
		{
			get
			{
				return TradingUIUtilities.activeMerchant as IMerchant;
			}
			set
			{
				TradingUIUtilities.activeMerchant = (value as UnityEngine.Object);
				Action<IMerchant> onActiveMerchantChanged = TradingUIUtilities.OnActiveMerchantChanged;
				if (onActiveMerchantChanged == null)
				{
					return;
				}
				onActiveMerchantChanged(value);
			}
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060021E4 RID: 8676 RVA: 0x0007602C File Offset: 0x0007422C
		// (remove) Token: 0x060021E5 RID: 8677 RVA: 0x00076060 File Offset: 0x00074260
		public static event Action<IMerchant> OnActiveMerchantChanged;

		// Token: 0x040016E5 RID: 5861
		private static UnityEngine.Object activeMerchant;
	}
}
