using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public abstract class OptionsProviderBase : MonoBehaviour
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000D8D RID: 3469
	public abstract string Key { get; }

	// Token: 0x06000D8E RID: 3470
	public abstract string[] GetOptions();

	// Token: 0x06000D8F RID: 3471
	public abstract string GetCurrentOption();

	// Token: 0x06000D90 RID: 3472
	public abstract void Set(int index);
}
