using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x020001B8 RID: 440
public class FXPool : MonoBehaviour
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00036480 File Offset: 0x00034680
	// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00036487 File Offset: 0x00034687
	public static FXPool Instance { get; private set; }

	// Token: 0x06000D00 RID: 3328 RVA: 0x0003648F File Offset: 0x0003468F
	private void Awake()
	{
		FXPool.Instance = this;
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00036498 File Offset: 0x00034698
	private void FixedUpdate()
	{
		if (this.poolsDic == null)
		{
			return;
		}
		foreach (FXPool.Pool pool in this.poolsDic.Values)
		{
			pool.Tick();
		}
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x000364F8 File Offset: 0x000346F8
	private FXPool.Pool GetOrCreatePool(ParticleSystem prefab)
	{
		if (this.poolsDic == null)
		{
			this.poolsDic = new Dictionary<ParticleSystem, FXPool.Pool>();
		}
		FXPool.Pool result;
		if (this.poolsDic.TryGetValue(prefab, out result))
		{
			return result;
		}
		FXPool.Pool pool = new FXPool.Pool(prefab, base.transform, null, null, null, null, true, 10, 100);
		this.poolsDic[prefab] = pool;
		return pool;
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0003654E File Offset: 0x0003474E
	private static ParticleSystem Get(ParticleSystem prefab)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		return FXPool.Instance.GetOrCreatePool(prefab).Get();
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x00036570 File Offset: 0x00034770
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x000365C8 File Offset: 0x000347C8
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation, Color color)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.main.startColor = color;
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x04000B40 RID: 2880
	private Dictionary<ParticleSystem, FXPool.Pool> poolsDic;

	// Token: 0x020004CD RID: 1229
	private class Pool
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x0008D5D8 File Offset: 0x0008B7D8
		public Pool(ParticleSystem prefab, Transform parent, Action<ParticleSystem> onCreate = null, Action<ParticleSystem> onGet = null, Action<ParticleSystem> onRelease = null, Action<ParticleSystem> onDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 100)
		{
			this.prefab = prefab;
			this.parent = parent;
			this.pool = new ObjectPool<ParticleSystem>(new Func<ParticleSystem>(this.Create), new Action<ParticleSystem>(this.OnEntryGet), new Action<ParticleSystem>(this.OnEntryRelease), new Action<ParticleSystem>(this.OnEntryDestroy), collectionCheck, defaultCapacity, maxSize);
			this.onCreate = onCreate;
			this.onGet = onGet;
			this.onRelease = onRelease;
			this.onDestroy = onDestroy;
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x0008D664 File Offset: 0x0008B864
		private ParticleSystem Create()
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.prefab, this.parent);
			Action<ParticleSystem> action = this.onCreate;
			if (action != null)
			{
				action(particleSystem);
			}
			return particleSystem;
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0008D696 File Offset: 0x0008B896
		public void OnEntryGet(ParticleSystem obj)
		{
			this.activeEntries.Add(obj);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x0008D6A4 File Offset: 0x0008B8A4
		public void OnEntryRelease(ParticleSystem obj)
		{
			this.activeEntries.Remove(obj);
			obj.gameObject.SetActive(false);
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x0008D6BF File Offset: 0x0008B8BF
		public void OnEntryDestroy(ParticleSystem obj)
		{
			Action<ParticleSystem> action = this.onDestroy;
			if (action == null)
			{
				return;
			}
			action(obj);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x0008D6D2 File Offset: 0x0008B8D2
		public ParticleSystem Get()
		{
			return this.pool.Get();
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x0008D6DF File Offset: 0x0008B8DF
		public void Release(ParticleSystem obj)
		{
			this.pool.Release(obj);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x0008D6F0 File Offset: 0x0008B8F0
		public void Tick()
		{
			List<ParticleSystem> list = new List<ParticleSystem>();
			foreach (ParticleSystem particleSystem in this.activeEntries)
			{
				if (!particleSystem.isPlaying)
				{
					list.Add(particleSystem);
				}
			}
			foreach (ParticleSystem obj in list)
			{
				this.Release(obj);
			}
		}

		// Token: 0x04001CCB RID: 7371
		private ParticleSystem prefab;

		// Token: 0x04001CCC RID: 7372
		private Transform parent;

		// Token: 0x04001CCD RID: 7373
		private ObjectPool<ParticleSystem> pool;

		// Token: 0x04001CCE RID: 7374
		private Action<ParticleSystem> onCreate;

		// Token: 0x04001CCF RID: 7375
		private Action<ParticleSystem> onGet;

		// Token: 0x04001CD0 RID: 7376
		private Action<ParticleSystem> onRelease;

		// Token: 0x04001CD1 RID: 7377
		private Action<ParticleSystem> onDestroy;

		// Token: 0x04001CD2 RID: 7378
		private List<ParticleSystem> activeEntries = new List<ParticleSystem>();
	}
}
