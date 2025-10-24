using System;
using Duckov.Quests;

// Token: 0x02000117 RID: 279
public class Condition_RaidDead : Condition
{
	// Token: 0x0600096D RID: 2413 RVA: 0x00029392 File Offset: 0x00027592
	public override bool Evaluate()
	{
		return RaidUtilities.CurrentRaid.dead;
	}
}
