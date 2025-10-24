using System;
using Duckov;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class AudioEventProxy : MonoBehaviour
{
	// Token: 0x060001AD RID: 429 RVA: 0x00008629 File Offset: 0x00006829
	private void Awake()
	{
		if (this.playOnAwake)
		{
			this.Post();
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00008639 File Offset: 0x00006839
	public void Post()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		AudioManager.Post(this.eventName, base.gameObject);
	}

	// Token: 0x04000164 RID: 356
	public bool playOnAwake;

	// Token: 0x04000165 RID: 357
	[SerializeField]
	private string eventName;
}
