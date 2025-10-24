using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class Condition_TimeOfDay : Condition
{
	// Token: 0x0600096F RID: 2415 RVA: 0x000293A8 File Offset: 0x000275A8
	public override bool Evaluate()
	{
		float num = (float)GameClock.TimeOfDay.TotalHours % 24f;
		return (num >= this.from && num <= this.to) || (this.to < this.from && (num >= this.from || num <= this.to));
	}

	// Token: 0x04000852 RID: 2130
	[Range(0f, 24f)]
	public float from;

	// Token: 0x04000853 RID: 2131
	[Range(0f, 24f)]
	public float to;
}
