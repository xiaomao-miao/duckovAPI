using System;
using Duckov;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class PlayEventOnAwake : MonoBehaviour
{
	// Token: 0x060001C3 RID: 451 RVA: 0x00008912 File Offset: 0x00006B12
	private void Awake()
	{
		AudioManager.Post(this.sfx, base.gameObject);
	}

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private string sfx;
}
