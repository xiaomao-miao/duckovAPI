using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BF RID: 447
public interface IMiniMapDataProvider
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000D58 RID: 3416
	Sprite CombinedSprite { get; }

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000D59 RID: 3417
	List<IMiniMapEntry> Maps { get; }

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000D5A RID: 3418
	float PixelSize { get; }

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000D5B RID: 3419
	Vector3 CombinedCenter { get; }
}
