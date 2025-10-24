using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class LogOnEnableAndDisable : MonoBehaviour
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x0002AF64 File Offset: 0x00029164
	private void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0002AF70 File Offset: 0x00029170
	private void OnDisable()
	{
		Debug.Log("OnDisable");
	}
}
