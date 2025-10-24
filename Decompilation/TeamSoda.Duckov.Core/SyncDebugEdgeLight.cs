using System;
using Soda;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class SyncDebugEdgeLight : MonoBehaviour
{
	// Token: 0x0600065C RID: 1628 RVA: 0x0001CCF0 File Offset: 0x0001AEF0
	private void Awake()
	{
		DebugView.OnDebugViewConfigChanged += this.OnDebugConfigChanged;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0001CD03 File Offset: 0x0001AF03
	private void OnDestroy()
	{
		DebugView.OnDebugViewConfigChanged -= this.OnDebugConfigChanged;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0001CD16 File Offset: 0x0001AF16
	private void OnDebugConfigChanged(DebugView debugView)
	{
		if (debugView == null)
		{
			return;
		}
		base.gameObject.SetActive(debugView.EdgeLightActive);
	}
}
