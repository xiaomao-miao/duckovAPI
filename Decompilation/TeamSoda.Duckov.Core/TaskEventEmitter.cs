using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class TaskEventEmitter : MonoBehaviour
{
	// Token: 0x06000985 RID: 2437 RVA: 0x000296E5 File Offset: 0x000278E5
	public void SetKey(string key)
	{
		this.eventKey = key;
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x000296EE File Offset: 0x000278EE
	private void Awake()
	{
		if (this.emitOnAwake)
		{
			this.EmitEvent();
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x000296FE File Offset: 0x000278FE
	public void EmitEvent()
	{
		Debug.Log("TaskEvent:" + this.eventKey);
		TaskEvent.EmitTaskEvent(this.eventKey);
	}

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private string eventKey;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private bool emitOnAwake;
}
