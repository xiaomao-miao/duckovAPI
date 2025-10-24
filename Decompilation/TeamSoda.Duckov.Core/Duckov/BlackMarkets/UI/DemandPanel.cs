using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000309 RID: 777
	public class DemandPanel : MonoBehaviour
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0005C441 File Offset: 0x0005A641
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x0005C449 File Offset: 0x0005A649
		public BlackMarket Target { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0005C454 File Offset: 0x0005A654
		private PrefabPool<DemandPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DemandPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<DemandPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0005C498 File Offset: 0x0005A698
		private void OnCreateEntry(DemandPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0005C4AC File Offset: 0x0005A6AC
		private void OnEntryClicked(DemandPanel_Entry entry)
		{
			this.Target.Sell(entry.Target);
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0005C4C0 File Offset: 0x0005A6C0
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 DemandPanel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0005C4F1 File Offset: 0x0005A6F1
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0005C4FF File Offset: 0x0005A6FF
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0005C508 File Offset: 0x0005A708
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Demands)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0005C580 File Offset: 0x0005A780
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0005C5A8 File Offset: 0x0005A7A8
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0005C5D6 File Offset: 0x0005A7D6
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x04001271 RID: 4721
		[SerializeField]
		private DemandPanel_Entry entryTemplate;

		// Token: 0x04001272 RID: 4722
		private PrefabPool<DemandPanel_Entry> _entryPool;
	}
}
