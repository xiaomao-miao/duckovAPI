using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[Serializable]
public struct CharacterRandomPresetInfo
{
	// Token: 0x040004A5 RID: 1189
	public CharacterRandomPreset randomPreset;

	// Token: 0x040004A6 RID: 1190
	[Range(0f, 1f)]
	public float weight;
}
