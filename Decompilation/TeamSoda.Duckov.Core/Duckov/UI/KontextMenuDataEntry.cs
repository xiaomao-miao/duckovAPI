using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C7 RID: 967
	public class KontextMenuDataEntry
	{
		// Token: 0x0600233D RID: 9021 RVA: 0x0007B6C5 File Offset: 0x000798C5
		public void Invoke()
		{
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040017F1 RID: 6129
		public Sprite icon;

		// Token: 0x040017F2 RID: 6130
		public string text;

		// Token: 0x040017F3 RID: 6131
		public Action action;
	}
}
