using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000398 RID: 920
	public class UsageUtilitiesDisplay_Entry : MonoBehaviour
	{
		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x00070ED8 File Offset: 0x0006F0D8
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x00070EE0 File Offset: 0x0006F0E0
		public UsageBehavior Target { get; private set; }

		// Token: 0x0600205C RID: 8284 RVA: 0x00070EEC File Offset: 0x0006F0EC
		internal void Setup(UsageBehavior cur)
		{
			this.text.text = cur.DisplaySettings.Description;
		}

		// Token: 0x0400160A RID: 5642
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
