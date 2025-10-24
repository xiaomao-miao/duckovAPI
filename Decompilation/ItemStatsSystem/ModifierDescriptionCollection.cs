using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000020 RID: 32
	public class ModifierDescriptionCollection : ItemComponent, ICollection<ModifierDescription>, IEnumerable<ModifierDescription>, IEnumerable
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000759C File Offset: 0x0000579C
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000075B8 File Offset: 0x000057B8
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000075A9 File Offset: 0x000057A9
		public bool ModifierEnable
		{
			get
			{
				return this._modifierEnableCache;
			}
			set
			{
				this._modifierEnableCache = value;
				this.ReapplyModifiers();
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000075C0 File Offset: 0x000057C0
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000075C3 File Offset: 0x000057C3
		internal override void OnInitialize()
		{
			base.Master.onItemTreeChanged += this.OnItemTreeChange;
			base.Master.onDurabilityChanged += this.OnDurabilityChange;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000075F3 File Offset: 0x000057F3
		private void OnDurabilityChange(Item item)
		{
			this.ReapplyModifiers();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000075FB File Offset: 0x000057FB
		private void OnDestroy()
		{
			if (base.Master)
			{
				base.Master.onItemTreeChanged -= this.OnItemTreeChange;
				base.Master.onDurabilityChanged -= this.OnDurabilityChange;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007638 File Offset: 0x00005838
		private void OnItemTreeChange(Item item)
		{
			this.ReapplyModifiers();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007640 File Offset: 0x00005840
		public void ReapplyModifiers()
		{
			if (base.Master == null)
			{
				return;
			}
			bool flag = this.ModifierEnable;
			if (base.Master.UseDurability && base.Master.Durability <= 0f)
			{
				flag = false;
			}
			if (!flag)
			{
				foreach (ModifierDescription modifierDescription in this.list)
				{
					modifierDescription.Release();
				}
				return;
			}
			foreach (ModifierDescription modifierDescription2 in this.list)
			{
				modifierDescription2.ReapplyModifier(this);
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000770C File Offset: 0x0000590C
		public void Add(ModifierDescription item)
		{
			this.list.Add(item);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000771C File Offset: 0x0000591C
		public void Clear()
		{
			if (this.list == null)
			{
				this.list = new List<ModifierDescription>();
			}
			foreach (ModifierDescription modifierDescription in this.list)
			{
				modifierDescription.Release();
			}
			this.list.Clear();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000778C File Offset: 0x0000598C
		public bool Contains(ModifierDescription item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000779A File Offset: 0x0000599A
		public void CopyTo(ModifierDescription[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000077A9 File Offset: 0x000059A9
		public bool Remove(ModifierDescription item)
		{
			if (item != null && this.list.Contains(item))
			{
				item.Release();
			}
			return this.list.Remove(item);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000077CE File Offset: 0x000059CE
		public IEnumerator<ModifierDescription> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000077E0 File Offset: 0x000059E0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000077F2 File Offset: 0x000059F2
		public ModifierDescription Find(Predicate<ModifierDescription> predicate)
		{
			return this.list.Find(predicate);
		}

		// Token: 0x0400009B RID: 155
		private bool _modifierEnableCache = true;

		// Token: 0x0400009C RID: 156
		[SerializeField]
		private List<ModifierDescription> list;
	}
}
