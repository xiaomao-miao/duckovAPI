using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[Serializable]
public struct CustomFaceHeadSetting
{
	// Token: 0x04000083 RID: 131
	public Color mainColor;

	// Token: 0x04000084 RID: 132
	[Range(-0.4f, 0.4f)]
	public float headScaleOffset;

	// Token: 0x04000085 RID: 133
	[Range(0f, 4f)]
	public float foreheadHeight;

	// Token: 0x04000086 RID: 134
	[Range(0.4f, 4f)]
	public float foreheadRound;
}
