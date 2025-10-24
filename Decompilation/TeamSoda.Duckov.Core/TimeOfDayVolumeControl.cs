using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000194 RID: 404
public class TimeOfDayVolumeControl : MonoBehaviour
{
	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x000327AB File Offset: 0x000309AB
	public VolumeProfile CurrentProfile
	{
		get
		{
			return this.currentProfile;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x000327B3 File Offset: 0x000309B3
	public VolumeProfile BufferTargetProfile
	{
		get
		{
			return this.bufferTargetProfile;
		}
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x000327BC File Offset: 0x000309BC
	private void Update()
	{
		if (!this.blending && this.bufferTargetProfile != null)
		{
			this.StartBlendToBufferdTarget();
		}
		if (this.blending)
		{
			this.UpdateBlending(Time.deltaTime);
		}
		if (!this.blending && this.fromVolume.gameObject.activeSelf)
		{
			this.fromVolume.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00032824 File Offset: 0x00030A24
	private void UpdateBlending(float deltaTime)
	{
		this.blendTimer += deltaTime;
		float num = this.blendTimer / this.blendTime;
		if (num > 1f)
		{
			num = 1f;
			this.blending = false;
		}
		this.toVolume.weight = this.blendCurve.Evaluate(num);
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00032879 File Offset: 0x00030A79
	public void SetTargetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00032884 File Offset: 0x00030A84
	private void StartBlendToBufferdTarget()
	{
		this.blending = true;
		this.blendingTargetProfile = this.bufferTargetProfile;
		this.bufferTargetProfile = null;
		this.currentProfile = this.blendingTargetProfile;
		this.fromVolume.gameObject.SetActive(true);
		this.fromVolume.profile = this.toVolume.profile;
		this.fromVolume.weight = 1f;
		this.toVolume.profile = this.blendingTargetProfile;
		this.toVolume.weight = 0f;
		this.blendTimer = 0f;
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0003291A File Offset: 0x00030B1A
	public void ForceSetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
		this.StartBlendToBufferdTarget();
		this.UpdateBlending(999f);
	}

	// Token: 0x04000A52 RID: 2642
	private VolumeProfile currentProfile;

	// Token: 0x04000A53 RID: 2643
	private VolumeProfile blendingTargetProfile;

	// Token: 0x04000A54 RID: 2644
	private VolumeProfile bufferTargetProfile;

	// Token: 0x04000A55 RID: 2645
	public Volume fromVolume;

	// Token: 0x04000A56 RID: 2646
	public Volume toVolume;

	// Token: 0x04000A57 RID: 2647
	private bool blending;

	// Token: 0x04000A58 RID: 2648
	private float blendTimer;

	// Token: 0x04000A59 RID: 2649
	public float blendTime = 2f;

	// Token: 0x04000A5A RID: 2650
	public AnimationCurve blendCurve;
}
