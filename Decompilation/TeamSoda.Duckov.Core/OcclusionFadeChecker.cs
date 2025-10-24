using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class OcclusionFadeChecker : MonoBehaviour
{
	// Token: 0x06000BA2 RID: 2978 RVA: 0x00031490 File Offset: 0x0002F690
	private void OnTriggerEnter(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Enter();
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x000314B4 File Offset: 0x0002F6B4
	private void OnTriggerExit(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Leave();
	}
}
