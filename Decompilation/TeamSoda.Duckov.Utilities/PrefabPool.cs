using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Pool;

namespace Duckov.Utilities
{
	// Token: 0x0200000B RID: 11
	public class PrefabPool<T> where T : Component
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002EB0 File Offset: 0x000010B0
		public ReadOnlyCollection<T> ActiveEntries
		{
			get
			{
				return this.activeObjects.AsReadOnly();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002EC0 File Offset: 0x000010C0
		public PrefabPool(T prefab, Transform poolParent = null, Action<T> onGet = null, Action<T> onRelease = null, Action<T> onDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000, Action<T> onCreate = null)
		{
			this.Prefab = prefab;
			prefab.gameObject.SetActive(false);
			if (poolParent == null)
			{
				poolParent = prefab.transform.parent;
			}
			this.poolParent = poolParent;
			this.onGet = onGet;
			this.onRelease = onRelease;
			this.onDestroy = onDestroy;
			this.CollectionCheck = collectionCheck;
			this.DefaultCapacity = defaultCapacity;
			this.MaxSize = maxSize;
			this.onCreate = onCreate;
			this.pool = new ObjectPool<T>(new Func<T>(this.CreateInstance), new Action<T>(this.OnGet), new Action<T>(this.OnRelease), new Action<T>(this.OnDestroy), collectionCheck, defaultCapacity, maxSize);
			this.activeObjects = new List<T>();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F90 File Offset: 0x00001190
		public T Get(Transform setParent = null)
		{
			if (setParent == null)
			{
				setParent = this.poolParent;
			}
			T t = this.pool.Get();
			if (setParent)
			{
				t.transform.SetParent(setParent, false);
				t.transform.SetAsLastSibling();
			}
			return t;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002FE8 File Offset: 0x000011E8
		public void Release(T item)
		{
			this.pool.Release(item);
			IPoolable poolable = item as IPoolable;
			if (poolable != null)
			{
				poolable.NotifyReleased();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003018 File Offset: 0x00001218
		private T CreateInstance()
		{
			T t = UnityEngine.Object.Instantiate<T>(this.Prefab);
			Action<T> action = this.onCreate;
			if (action != null)
			{
				action(t);
			}
			return t;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003044 File Offset: 0x00001244
		private void OnGet(T item)
		{
			this.activeObjects.Add(item);
			item.gameObject.SetActive(true);
			IPoolable poolable = item as IPoolable;
			if (poolable != null)
			{
				poolable.NotifyPooled();
			}
			Action<T> action = this.onGet;
			if (action == null)
			{
				return;
			}
			action(item);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003094 File Offset: 0x00001294
		private void OnRelease(T item)
		{
			this.activeObjects.Remove(item);
			Action<T> action = this.onRelease;
			if (action != null)
			{
				action(item);
			}
			if (item != null)
			{
				item.gameObject.SetActive(false);
				item.transform.SetParent(this.poolParent);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000030F5 File Offset: 0x000012F5
		private void OnDestroy(T item)
		{
			Action<T> action = this.onDestroy;
			if (action != null)
			{
				action(item);
			}
			UnityEngine.Object.Destroy(item.gameObject);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000311C File Offset: 0x0000131C
		public void ReleaseAll()
		{
			this.activeObjects.RemoveAll((T e) => e == null);
			foreach (T item in this.activeObjects.ToArray())
			{
				this.Release(item);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003180 File Offset: 0x00001380
		public T Find(Predicate<T> predicate)
		{
			foreach (T t in this.activeObjects)
			{
				if (predicate(t))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000031E4 File Offset: 0x000013E4
		public int ReleaseAll(Predicate<T> predicate)
		{
			List<T> list = new List<T>();
			foreach (T t in this.activeObjects)
			{
				if (predicate(t))
				{
					list.Add(t);
				}
			}
			foreach (T item in list)
			{
				this.Release(item);
			}
			return list.Count;
		}

		// Token: 0x0400001C RID: 28
		public readonly T Prefab;

		// Token: 0x0400001D RID: 29
		public Transform poolParent;

		// Token: 0x0400001E RID: 30
		private Action<T> onGet;

		// Token: 0x0400001F RID: 31
		private Action<T> onRelease;

		// Token: 0x04000020 RID: 32
		private Action<T> onDestroy;

		// Token: 0x04000021 RID: 33
		private Action<T> onCreate;

		// Token: 0x04000022 RID: 34
		public readonly bool CollectionCheck;

		// Token: 0x04000023 RID: 35
		public readonly int DefaultCapacity;

		// Token: 0x04000024 RID: 36
		public readonly int MaxSize;

		// Token: 0x04000025 RID: 37
		private readonly ObjectPool<T> pool;

		// Token: 0x04000026 RID: 38
		private List<T> activeObjects;
	}
}
