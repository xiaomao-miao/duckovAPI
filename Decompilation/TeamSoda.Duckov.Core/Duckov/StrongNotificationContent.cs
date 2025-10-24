using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000241 RID: 577
	public class StrongNotificationContent
	{
		// Token: 0x060011F3 RID: 4595 RVA: 0x00044A20 File Offset: 0x00042C20
		public StrongNotificationContent(string mainText, string subText = "", Sprite image = null)
		{
			this.mainText = mainText;
			this.subText = subText;
			this.image = image;
		}

		// Token: 0x04000DCB RID: 3531
		public string mainText;

		// Token: 0x04000DCC RID: 3532
		public string subText;

		// Token: 0x04000DCD RID: 3533
		public Sprite image;
	}
}
