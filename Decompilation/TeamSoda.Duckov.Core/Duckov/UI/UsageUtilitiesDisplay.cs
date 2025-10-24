using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000397 RID: 919
	public class UsageUtilitiesDisplay : MonoBehaviour
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x00070D93 File Offset: 0x0006EF93
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x00070D9B File Offset: 0x0006EF9B
		public UsageUtilities Target { get; private set; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00070DA4 File Offset: 0x0006EFA4
		private PrefabPool<UsageUtilitiesDisplay_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<UsageUtilitiesDisplay_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x00070DE0 File Offset: 0x0006EFE0
		public void Setup(Item item)
		{
			if (!(item == null))
			{
				UsageUtilities component = item.GetComponent<UsageUtilities>();
				if (!(component == null))
				{
					this.Target = component;
					base.gameObject.SetActive(true);
					this.Refresh();
					return;
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00070E2C File Offset: 0x0006F02C
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			foreach (UsageBehavior usageBehavior in this.Target.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.DisplaySettings.display)
				{
					this.EntryPool.Get(null).Setup(usageBehavior);
				}
			}
			if (this.EntryPool.ActiveEntries.Count <= 0)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04001607 RID: 5639
		[SerializeField]
		private UsageUtilitiesDisplay_Entry entryTemplate;

		// Token: 0x04001608 RID: 5640
		private PrefabPool<UsageUtilitiesDisplay_Entry> _entryPool;
	}
}
