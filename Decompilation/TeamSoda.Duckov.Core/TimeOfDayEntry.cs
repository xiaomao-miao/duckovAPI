using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class TimeOfDayEntry : MonoBehaviour
{
	// Token: 0x06000BDF RID: 3039 RVA: 0x00032718 File Offset: 0x00030918
	private void Start()
	{
		if (this.phases.Count > 0)
		{
			TimeOfDayPhase value = this.phases[0];
			this.phases[0] = value;
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00032750 File Offset: 0x00030950
	public TimeOfDayPhase GetPhase(TimePhaseTags timePhaseTags)
	{
		for (int i = 0; i < this.phases.Count; i++)
		{
			TimeOfDayPhase timeOfDayPhase = this.phases[i];
			if (timeOfDayPhase.timePhaseTag == timePhaseTags)
			{
				return timeOfDayPhase;
			}
		}
		if (timePhaseTags == TimePhaseTags.dawn)
		{
			return this.GetPhase(TimePhaseTags.day);
		}
		return this.phases[0];
	}

	// Token: 0x04000A4B RID: 2635
	[SerializeField]
	private List<TimeOfDayPhase> phases;
}
