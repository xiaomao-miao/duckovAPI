using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class BowAnimation : MonoBehaviour
{
	// Token: 0x06000B53 RID: 2899 RVA: 0x0003008C File Offset: 0x0002E28C
	private void Start()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent += this.OnShoot;
			this.gunAgent.OnLoadedEvent += this.OnLoaded;
			if (this.gunAgent.BulletCount > 0)
			{
				this.OnLoaded();
			}
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x000300E9 File Offset: 0x0002E2E9
	private void OnDestroy()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent -= this.OnShoot;
			this.gunAgent.OnLoadedEvent -= this.OnLoaded;
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00030127 File Offset: 0x0002E327
	private void OnShoot()
	{
		this.animator.SetTrigger("Shoot");
		if (this.gunAgent.BulletCount <= 0)
		{
			this.animator.SetBool("Loaded", false);
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00030158 File Offset: 0x0002E358
	private void OnLoaded()
	{
		this.animator.SetBool("Loaded", true);
	}

	// Token: 0x040009A3 RID: 2467
	public ItemAgent_Gun gunAgent;

	// Token: 0x040009A4 RID: 2468
	public Animator animator;

	// Token: 0x040009A5 RID: 2469
	private int hash_Loaded = "Loaded".GetHashCode();

	// Token: 0x040009A6 RID: 2470
	private int hash_Aiming = "Aiming".GetHashCode();

	// Token: 0x040009A7 RID: 2471
	private int hash_Shoot = "Shoot".GetHashCode();
}
