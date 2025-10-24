using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
[Serializable]
public struct CustomFacePartInfo
{
	// Token: 0x040000A1 RID: 161
	public float radius;

	// Token: 0x040000A2 RID: 162
	public Color color;

	// Token: 0x040000A3 RID: 163
	public float height;

	// Token: 0x040000A4 RID: 164
	public float heightOffset;

	// Token: 0x040000A5 RID: 165
	public float scale;

	// Token: 0x040000A6 RID: 166
	public float twist;

	// Token: 0x040000A7 RID: 167
	[Range(0f, 90f)]
	public float distanceAngle;

	// Token: 0x040000A8 RID: 168
	[Range(-90f, 90f)]
	public float leftRightAngle;
}
