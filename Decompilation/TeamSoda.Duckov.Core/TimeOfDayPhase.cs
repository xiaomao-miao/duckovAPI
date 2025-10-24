using System;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x02000193 RID: 403
[Serializable]
public struct TimeOfDayPhase
{
	// Token: 0x04000A50 RID: 2640
	[FormerlySerializedAs("phaseTag")]
	public TimePhaseTags timePhaseTag;

	// Token: 0x04000A51 RID: 2641
	public VolumeProfile volumeProfile;
}
