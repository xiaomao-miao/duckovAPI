using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public interface IMiniMapEntry
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000D5C RID: 3420
	Sprite Sprite { get; }

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000D5D RID: 3421
	float PixelSize { get; }

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000D5E RID: 3422
	Vector2 Offset { get; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000D5F RID: 3423
	string SceneID { get; }

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000D60 RID: 3424
	bool Hide { get; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000D61 RID: 3425
	bool NoSignal { get; }
}
