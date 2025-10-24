using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x0200030B RID: 779
	public class SupplyPanel : MonoBehaviour
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x0005C8B7 File Offset: 0x0005AAB7
		// (set) Token: 0x0600198E RID: 6542 RVA: 0x0005C8BF File Offset: 0x0005AABF
		public BlackMarket Target { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0005C8C8 File Offset: 0x0005AAC8
		private PrefabPool<SupplyPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<SupplyPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<SupplyPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0005C90C File Offset: 0x0005AB0C
		private void OnCreateEntry(SupplyPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0005C920 File Offset: 0x0005AB20
		private void OnEntryClicked(SupplyPanel_Entry entry)
		{
			Debug.Log("Supply entry clicked");
			this.Target.Buy(entry.Target);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0005C93E File Offset: 0x0005AB3E
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 Supply Panel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0005C96F File Offset: 0x0005AB6F
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0005C97D File Offset: 0x0005AB7D
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0005C988 File Offset: 0x0005AB88
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Supplies)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0005CA00 File Offset: 0x0005AC00
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0005CA28 File Offset: 0x0005AC28
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0005CA56 File Offset: 0x0005AC56
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x04001280 RID: 4736
		[SerializeField]
		private SupplyPanel_Entry entryTemplate;

		// Token: 0x04001281 RID: 4737
		private PrefabPool<SupplyPanel_Entry> _entryPool;
	}
}
