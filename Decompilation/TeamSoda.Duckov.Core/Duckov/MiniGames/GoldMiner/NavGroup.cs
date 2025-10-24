using System;
using System.Collections.Generic;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A5 RID: 677
	public class NavGroup : MiniGameBehaviour
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x000515EB File Offset: 0x0004F7EB
		// (set) Token: 0x06001608 RID: 5640 RVA: 0x000515F2 File Offset: 0x0004F7F2
		public static NavGroup ActiveNavGroup { get; private set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x000515FA File Offset: 0x0004F7FA
		public bool active
		{
			get
			{
				return NavGroup.ActiveNavGroup == this;
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00051608 File Offset: 0x0004F808
		public void SetAsActiveNavGroup()
		{
			NavGroup activeNavGroup = NavGroup.ActiveNavGroup;
			NavGroup.ActiveNavGroup = this;
			this.RefreshAll();
			if (activeNavGroup != null)
			{
				activeNavGroup.RefreshAll();
			}
			Action onNavGroupChanged = NavGroup.OnNavGroupChanged;
			if (onNavGroupChanged == null)
			{
				return;
			}
			onNavGroupChanged();
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x00051645 File Offset: 0x0004F845
		// (set) Token: 0x0600160C RID: 5644 RVA: 0x00051650 File Offset: 0x0004F850
		public int NavIndex
		{
			get
			{
				return this._navIndex;
			}
			set
			{
				int navIndex = this._navIndex;
				this._navIndex = value;
				this.CleanupIndex();
				int navIndex2 = this._navIndex;
				this.RefreshEntry(navIndex);
				this.RefreshEntry(navIndex2);
			}
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00051686 File Offset: 0x0004F886
		protected override void OnEnable()
		{
			base.OnEnable();
			this.RefreshAll();
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00051694 File Offset: 0x0004F894
		private void CleanupIndex()
		{
			if (this._navIndex < 0)
			{
				this._navIndex = this.entries.Count - 1;
			}
			if (this._navIndex >= this.entries.Count)
			{
				this._navIndex = 0;
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000516CC File Offset: 0x0004F8CC
		private void RefreshAll()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.RefreshEntry(i);
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x000516F6 File Offset: 0x0004F8F6
		private void RefreshEntry(int index)
		{
			if (index < 0 || index >= this.entries.Count)
			{
				return;
			}
			this.entries[index].NotifySelectionState(this.active && this.NavIndex == index);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00051730 File Offset: 0x0004F930
		public NavEntry GetSelectedEntry()
		{
			if (this.NavIndex < 0 || this.NavIndex >= this.entries.Count)
			{
				return null;
			}
			return this.entries[this.NavIndex];
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00051764 File Offset: 0x0004F964
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<GoldMiner>();
			}
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000517B2 File Offset: 0x0004F9B2
		private void OnLevelBegin(GoldMiner miner)
		{
			this.RefreshAll();
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000517BA File Offset: 0x0004F9BA
		internal void Remove(NavEntry navEntry)
		{
			this.entries.Remove(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x000517D5 File Offset: 0x0004F9D5
		internal void Add(NavEntry navEntry)
		{
			this.entries.Add(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x000517F0 File Offset: 0x0004F9F0
		internal void TrySelect(NavEntry navEntry)
		{
			if (!this.entries.Contains(navEntry))
			{
				return;
			}
			int navIndex = this.entries.IndexOf(navEntry);
			this.SetAsActiveNavGroup();
			this.NavIndex = navIndex;
		}

		// Token: 0x04001058 RID: 4184
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001059 RID: 4185
		[SerializeField]
		public List<NavEntry> entries;

		// Token: 0x0400105B RID: 4187
		public static Action OnNavGroupChanged;

		// Token: 0x0400105C RID: 4188
		private int _navIndex;
	}
}
