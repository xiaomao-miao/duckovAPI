using System;
using Duckov.Achievements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Duckov
{
	// Token: 0x0200023C RID: 572
	public class SocialManager : MonoBehaviour
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x000441F2 File Offset: 0x000423F2
		private void Awake()
		{
			AchievementManager.OnAchievementUnlocked += this.UnlockAchievement;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00044205 File Offset: 0x00042405
		private void Start()
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004421D File Offset: 0x0004241D
		private void ProcessAuthentication(bool success)
		{
			if (success)
			{
				this.initialized = true;
				Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004423A File Offset: 0x0004243A
		private void ProcessLoadedAchievements(IAchievement[] loadedAchievements)
		{
			this._achievement_cache = loadedAchievements;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00044243 File Offset: 0x00042443
		private void UnlockAchievement(string id)
		{
			if (this.initialized)
			{
				return;
			}
			Social.ReportProgress(id, 100.0, new Action<bool>(this.OnReportProgressResult));
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00044269 File Offset: 0x00042469
		private void OnReportProgressResult(bool success)
		{
			Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
		}

		// Token: 0x04000DAE RID: 3502
		private bool initialized;

		// Token: 0x04000DAF RID: 3503
		private IAchievement[] _achievement_cache;
	}
}
