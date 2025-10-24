using System;
using Duckov;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class LoopSoundWithObject : MonoBehaviour
{
	// Token: 0x060001BE RID: 446 RVA: 0x00008893 File Offset: 0x00006A93
	private void Start()
	{
		this.eventInstance = AudioManager.Post(this.sfx, base.gameObject);
		this.stoped = false;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000088B4 File Offset: 0x00006AB4
	public void Stop()
	{
		if (this.stoped)
		{
			return;
		}
		this.stoped = true;
		if (this.eventInstance != null)
		{
			this.eventInstance.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x000088F3 File Offset: 0x00006AF3
	private void OnDestroy()
	{
		this.Stop();
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x000088FB File Offset: 0x00006AFB
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0400016C RID: 364
	public string sfx;

	// Token: 0x0400016D RID: 365
	private EventInstance? eventInstance;

	// Token: 0x0400016E RID: 366
	private bool stoped = true;
}
