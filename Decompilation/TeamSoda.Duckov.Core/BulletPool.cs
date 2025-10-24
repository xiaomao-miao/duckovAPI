using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x0200010D RID: 269
public class BulletPool : MonoBehaviour
{
	// Token: 0x0600092D RID: 2349 RVA: 0x00028B43 File Offset: 0x00026D43
	private void Awake()
	{
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00028B45 File Offset: 0x00026D45
	public Projectile GetABullet(Projectile bulletPrefab)
	{
		return this.GetAPool(bulletPrefab).Get();
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00028B54 File Offset: 0x00026D54
	private ObjectPool<Projectile> GetAPool(Projectile pfb)
	{
		ObjectPool<Projectile> result;
		if (this.pools.TryGetValue(pfb, out result))
		{
			return result;
		}
		ObjectPool<Projectile> objectPool = new ObjectPool<Projectile>(() => this.CreateABulletInPool(pfb), new Action<Projectile>(this.OnGetABulletInPool), new Action<Projectile>(this.OnBulletRelease), null, true, 10, 10000);
		this.pools.Add(pfb, objectPool);
		return objectPool;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00028BD4 File Offset: 0x00026DD4
	private Projectile CreateABulletInPool(Projectile pfb)
	{
		Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(pfb);
		projectile.transform.SetParent(base.transform);
		ObjectPool<Projectile> apool = this.GetAPool(pfb);
		projectile.SetPool(apool);
		return projectile;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00028C07 File Offset: 0x00026E07
	private void OnGetABulletInPool(Projectile bulletToGet)
	{
		bulletToGet.gameObject.SetActive(true);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00028C15 File Offset: 0x00026E15
	private void OnBulletRelease(Projectile bulletToGet)
	{
		bulletToGet.transform.SetParent(base.transform);
		bulletToGet.gameObject.SetActive(false);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00028C34 File Offset: 0x00026E34
	public bool Release(Projectile instance, Projectile prefab)
	{
		ObjectPool<Projectile> objectPool;
		if (this.pools.TryGetValue(prefab, out objectPool))
		{
			objectPool.Release(prefab);
			return true;
		}
		return false;
	}

	// Token: 0x04000835 RID: 2101
	public Dictionary<Projectile, ObjectPool<Projectile>> pools = new Dictionary<Projectile, ObjectPool<Projectile>>();
}
