using System;
using Duckov.Achievements;
using Duckov.Rules.UI;
using Saves;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class EndingControl : MonoBehaviour
{
	// Token: 0x06000552 RID: 1362 RVA: 0x00017C90 File Offset: 0x00015E90
	public void SetEndingIndex()
	{
		Ending.endingIndex = this.endingIndex;
		AchievementManager instance = AchievementManager.Instance;
		bool flag = SavesSystem.Load<bool>(this.MissleLuncherClosedKey);
		DifficultySelection.UnlockRage();
		if (instance)
		{
			if (this.endingIndex == 0)
			{
				if (!flag)
				{
					instance.Unlock("Ending_0");
					return;
				}
				instance.Unlock("Ending_3");
				return;
			}
			else
			{
				if (!flag)
				{
					instance.Unlock("Ending_1");
					return;
				}
				instance.Unlock("Ending_2");
			}
		}
	}

	// Token: 0x040004C5 RID: 1221
	public int endingIndex;

	// Token: 0x040004C6 RID: 1222
	public string MissleLuncherClosedKey = "MissleLuncherClosed";
}
