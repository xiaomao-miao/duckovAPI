using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A6 RID: 678
	public class NavGroupLinearController : MiniGameBehaviour
	{
		// Token: 0x06001618 RID: 5656 RVA: 0x00051830 File Offset: 0x0004FA30
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00051884 File Offset: 0x0004FA84
		private void OnLevelBegin(GoldMiner miner)
		{
			if (this.setActiveWhenLevelBegin)
			{
				this.navGroup.SetAsActiveNavGroup();
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00051899 File Offset: 0x0004FA99
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
		}

		// Token: 0x0400105D RID: 4189
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400105E RID: 4190
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x0400105F RID: 4191
		[SerializeField]
		private NavGroup otherNavGroup;

		// Token: 0x04001060 RID: 4192
		[SerializeField]
		private bool setActiveWhenLevelBegin;

		// Token: 0x04001061 RID: 4193
		private bool changeLock;
	}
}
