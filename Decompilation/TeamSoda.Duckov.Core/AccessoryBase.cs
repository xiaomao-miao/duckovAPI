using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class AccessoryBase : MonoBehaviour
{
	// Token: 0x06000177 RID: 375 RVA: 0x00007290 File Offset: 0x00005490
	public void Init(DuckovItemAgent _parentAgent, Item _selfItem)
	{
		if (_parentAgent == null || _selfItem == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.parentAgent = _parentAgent;
		this.selfItem = _selfItem;
		Transform socket = this.parentAgent.GetSocket(this.socketName, true);
		base.transform.SetParent(socket);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		this.OnInit();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000730E File Offset: 0x0000550E
	protected virtual void OnInit()
	{
	}

	// Token: 0x0400010A RID: 266
	public string socketName;

	// Token: 0x0400010B RID: 267
	protected Item selfItem;

	// Token: 0x0400010C RID: 268
	protected DuckovItemAgent parentAgent;
}
