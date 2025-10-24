using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public static class ColorExtensions
{
	// Token: 0x0600052A RID: 1322 RVA: 0x00017500 File Offset: 0x00015700
	public static string ToHexString(this Color color)
	{
		return ((byte)(color.r * 255f)).ToString("X2") + ((byte)(color.g * 255f)).ToString("X2") + ((byte)(color.b * 255f)).ToString("X2") + ((byte)(color.a * 255f)).ToString("X2");
	}
}
