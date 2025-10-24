using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000392 RID: 914
	public class ItemEffectEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x0600202D RID: 8237 RVA: 0x00070955 File Offset: 0x0006EB55
		public void NotifyPooled()
		{
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00070957 File Offset: 0x0006EB57
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00070966 File Offset: 0x0006EB66
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0007096E File Offset: 0x0006EB6E
		public void Setup(Effect target)
		{
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00070983 File Offset: 0x0006EB83
		private void Refresh()
		{
			this.text.text = this.target.GetDisplayString();
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0007099B File Offset: 0x0006EB9B
		private void RegisterEvents()
		{
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0007099D File Offset: 0x0006EB9D
		private void UnregisterEvents()
		{
		}

		// Token: 0x040015F6 RID: 5622
		private Effect target;

		// Token: 0x040015F7 RID: 5623
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
