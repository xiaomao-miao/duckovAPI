using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class EvacuationCountdownUIProxy : MonoBehaviour
{
	// Token: 0x0600094A RID: 2378 RVA: 0x00029103 File Offset: 0x00027303
	public void Request(CountDownArea target)
	{
		EvacuationCountdownUI.Request(target);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0002910B File Offset: 0x0002730B
	public void Release(CountDownArea target)
	{
		EvacuationCountdownUI.Release(target);
	}
}
