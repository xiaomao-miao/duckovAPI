using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class CameraShaker : MonoBehaviour
{
	// Token: 0x06000B62 RID: 2914 RVA: 0x000303D6 File Offset: 0x0002E5D6
	private void Awake()
	{
		CameraShaker._instance = this;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x000303E0 File Offset: 0x0002E5E0
	public static void Shake(Vector3 velocity, CameraShaker.CameraShakeTypes shakeType)
	{
		if (CameraShaker._instance == null)
		{
			return;
		}
		switch (shakeType)
		{
		case CameraShaker.CameraShakeTypes.recoil:
			CameraShaker._instance.recoilSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.explosion:
			CameraShaker._instance.explosionSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.meleeAttackHit:
			CameraShaker._instance.meleeAttackSource.GenerateImpulseWithVelocity(velocity);
			return;
		default:
			return;
		}
	}

	// Token: 0x040009B1 RID: 2481
	private static CameraShaker _instance;

	// Token: 0x040009B2 RID: 2482
	public CinemachineImpulseSource recoilSource;

	// Token: 0x040009B3 RID: 2483
	public CinemachineImpulseSource meleeAttackSource;

	// Token: 0x040009B4 RID: 2484
	public CinemachineImpulseSource explosionSource;

	// Token: 0x020004B6 RID: 1206
	public enum CameraShakeTypes
	{
		// Token: 0x04001C7A RID: 7290
		recoil,
		// Token: 0x04001C7B RID: 7291
		explosion,
		// Token: 0x04001C7C RID: 7292
		meleeAttackHit
	}
}
