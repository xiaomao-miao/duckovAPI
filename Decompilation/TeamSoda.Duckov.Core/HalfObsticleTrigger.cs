using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class HalfObsticleTrigger : MonoBehaviour
{
	// Token: 0x0600039E RID: 926 RVA: 0x0000FE5D File Offset: 0x0000E05D
	private void OnTriggerEnter(Collider other)
	{
		this.parent.OnTriggerEnter(other);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000FE6B File Offset: 0x0000E06B
	private void OnTriggerExit(Collider other)
	{
		this.parent.OnTriggerExit(other);
	}

	// Token: 0x040002C0 RID: 704
	public HalfObsticle parent;
}
