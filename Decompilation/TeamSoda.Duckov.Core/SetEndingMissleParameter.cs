using System;
using Duckov;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class SetEndingMissleParameter : MonoBehaviour
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x00015858 File Offset: 0x00013A58
	private void Start()
	{
		bool flag = this.launcherClosedCondition.Evaluate();
		AudioManager.SetRTPC("Ending_Missile", (float)(flag ? 1 : 0), null);
	}

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private Condition launcherClosedCondition;
}
