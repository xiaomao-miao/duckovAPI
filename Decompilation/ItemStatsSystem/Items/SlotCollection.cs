using System;
using System.Collections;
using System.Collections.Generic;

namespace ItemStatsSystem.Items
{
	// Token: 0x0200002A RID: 42
	public class SlotCollection : ItemComponent, ICollection<Slot>, IEnumerable<Slot>, IEnumerable
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000220 RID: 544 RVA: 0x000085F2 File Offset: 0x000067F2
		private Dictionary<int, Slot> slotsDictionary
		{
			get
			{
				if (this._cachedSlotsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedSlotsDictionary;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00008608 File Offset: 0x00006808
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000861F File Offset: 0x0000681F
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008622 File Offset: 0x00006822
		public Slot GetSlotByIndex(int index)
		{
			return this.list[index];
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008630 File Offset: 0x00006830
		public Slot GetSlot(int hash)
		{
			Slot result;
			if (this.slotsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008650 File Offset: 0x00006850
		public Slot GetSlot(string key)
		{
			int hashCode = key.GetHashCode();
			Slot slot = this.GetSlot(hashCode);
			if (slot == null)
			{
				slot = this.list.Find((Slot e) => e.Key == key);
			}
			return slot;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000869C File Offset: 0x0000689C
		private void BuildDictionary()
		{
			if (this._cachedSlotsDictionary == null)
			{
				this._cachedSlotsDictionary = new Dictionary<int, Slot>();
			}
			this._cachedSlotsDictionary.Clear();
			foreach (Slot slot in this.list)
			{
				int hashCode = slot.Key.GetHashCode();
				this._cachedSlotsDictionary[hashCode] = slot;
			}
		}

		// Token: 0x1700009A RID: 154
		public Slot this[string key]
		{
			get
			{
				return this.GetSlot(key);
			}
		}

		// Token: 0x1700009B RID: 155
		public Slot this[int index]
		{
			get
			{
				return this.GetSlotByIndex(index);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008734 File Offset: 0x00006934
		internal override void OnInitialize()
		{
			foreach (Slot slot in this.list)
			{
				slot.Initialize(this);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008788 File Offset: 0x00006988
		public IEnumerator<Slot> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000879A File Offset: 0x0000699A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000087AC File Offset: 0x000069AC
		public void Add(Slot item)
		{
			this.list.Add(item);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000087BA File Offset: 0x000069BA
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000087C7 File Offset: 0x000069C7
		public bool Contains(Slot item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000087D5 File Offset: 0x000069D5
		public void CopyTo(Slot[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000087E4 File Offset: 0x000069E4
		public bool Remove(Slot item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000C7 RID: 199
		public Action<Slot> OnSlotContentChanged;

		// Token: 0x040000C8 RID: 200
		public List<Slot> list;

		// Token: 0x040000C9 RID: 201
		private Dictionary<int, Slot> _cachedSlotsDictionary;
	}
}
