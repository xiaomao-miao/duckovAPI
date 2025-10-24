using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200022F RID: 559
	public struct Progress
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x000434B9 File Offset: 0x000416B9
		public float progress
		{
			get
			{
				if (this.total > 0f)
				{
					return Mathf.Clamp01(this.current / this.total);
				}
				return 1f;
			}
		}

		// Token: 0x04000D77 RID: 3447
		public bool inProgress;

		// Token: 0x04000D78 RID: 3448
		public float total;

		// Token: 0x04000D79 RID: 3449
		public float current;

		// Token: 0x04000D7A RID: 3450
		public string progressName;
	}
}
