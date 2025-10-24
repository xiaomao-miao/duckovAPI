using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000023 RID: 35
	public class StatCollection : ItemComponent, ICollection<Stat>, IEnumerable<Stat>, IEnumerable
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00007C41 File Offset: 0x00005E41
		private Dictionary<int, Stat> statsDictionary
		{
			get
			{
				if (this._cachedStatsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedStatsDictionary;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007C57 File Offset: 0x00005E57
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007C64 File Offset: 0x00005E64
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007C68 File Offset: 0x00005E68
		public Stat GetStat(int hash)
		{
			Stat result;
			if (this.statsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007C88 File Offset: 0x00005E88
		public Stat GetStat(string key)
		{
			int hashCode = key.GetHashCode();
			Stat stat = this.GetStat(hashCode);
			if (stat == null)
			{
				stat = this.list.Find((Stat e) => e.Key == key);
			}
			return stat;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007CD4 File Offset: 0x00005ED4
		private void BuildDictionary()
		{
			if (this._cachedStatsDictionary == null)
			{
				this._cachedStatsDictionary = new Dictionary<int, Stat>();
			}
			this._cachedStatsDictionary.Clear();
			foreach (Stat stat in this.list)
			{
				int hashCode = stat.Key.GetHashCode();
				this._cachedStatsDictionary[hashCode] = stat;
			}
		}

		// Token: 0x17000088 RID: 136
		public Stat this[int hash]
		{
			get
			{
				return this.GetStat(hash);
			}
		}

		// Token: 0x17000089 RID: 137
		public Stat this[string key]
		{
			get
			{
				return this.GetStat(key);
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007D6C File Offset: 0x00005F6C
		internal override void OnInitialize()
		{
			foreach (Stat stat in this.list)
			{
				stat.Initialize(this);
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public IEnumerator<Stat> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007DD2 File Offset: 0x00005FD2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007DE4 File Offset: 0x00005FE4
		public void Add(Stat item)
		{
			this.list.Add(item);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007DF2 File Offset: 0x00005FF2
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007DFF File Offset: 0x00005FFF
		public bool Contains(Stat item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007E0D File Offset: 0x0000600D
		public void CopyTo(Stat[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007E1C File Offset: 0x0000601C
		public bool Remove(Stat item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000AA RID: 170
		[SerializeField]
		private List<Stat> list;

		// Token: 0x040000AB RID: 171
		private Dictionary<int, Stat> _cachedStatsDictionary;
	}
}
