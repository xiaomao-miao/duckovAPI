using System;
using Duckov;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class FmodEventTester : MonoBehaviour
{
	// Token: 0x06000C3C RID: 3132 RVA: 0x000339F7 File Offset: 0x00031BF7
	public void PlayEvent()
	{
		AudioManager.Post(this.e, base.gameObject);
	}

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private string e;
}
