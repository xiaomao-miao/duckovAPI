using System;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025A RID: 602
	public class UnlockAchievement : PerkBehaviour
	{
		// Token: 0x060012BA RID: 4794 RVA: 0x000467E6 File Offset: 0x000449E6
		protected override void OnUnlocked()
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(this.achievementKey.Trim());
		}

		// Token: 0x04000E24 RID: 3620
		[SerializeField]
		private string achievementKey;
	}
}
