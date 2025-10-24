using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class RequireEnemyKilled : Condition
{
	// Token: 0x06000978 RID: 2424 RVA: 0x000294F7 File Offset: 0x000276F7
	public override bool Evaluate()
	{
		return !(this.enemyPreset == null) && SavesCounter.GetKillCount(this.enemyPreset.nameKey) >= this.threshold;
	}

	// Token: 0x0400085A RID: 2138
	[SerializeField]
	private CharacterRandomPreset enemyPreset;

	// Token: 0x0400085B RID: 2139
	[SerializeField]
	private int threshold = 1;
}
