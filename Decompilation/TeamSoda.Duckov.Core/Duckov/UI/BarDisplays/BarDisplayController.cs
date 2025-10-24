using System;
using UnityEngine;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003CC RID: 972
	public class BarDisplayController : MonoBehaviour
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x0007BBB7 File Offset: 0x00079DB7
		protected virtual float Current
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x0007BBBE File Offset: 0x00079DBE
		protected virtual float Max
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0007BBC8 File Offset: 0x00079DC8
		protected void Refresh()
		{
			float current = this.Current;
			float max = this.Max;
			this.bar.SetValue(current, max, "0.#", 0f);
		}

		// Token: 0x04001809 RID: 6153
		[SerializeField]
		private BarDisplay bar;
	}
}
