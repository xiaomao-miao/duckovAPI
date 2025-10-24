using System;
using System.Collections.Generic;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000377 RID: 887
	public class BuffsDisplay : MonoBehaviour
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0006BC24 File Offset: 0x00069E24
		private PrefabPool<BuffsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BuffsDisplayEntry>(this.prefab, base.transform, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0006BC78 File Offset: 0x00069E78
		public void ReleaseEntry(BuffsDisplayEntry entry)
		{
			this.EntryPool.Release(entry);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0006BC86 File Offset: 0x00069E86
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0006BCA6 File Offset: 0x00069EA6
		private void OnDestroy()
		{
			this.UnregisterEvents();
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0006BCC0 File Offset: 0x00069EC0
		private void OnLevelInitialized()
		{
			this.UnregisterEvents();
			this.buffManager = LevelManager.Instance.MainCharacter.GetBuffManager();
			foreach (Buff buff in this.buffManager.Buffs)
			{
				this.OnAddBuff(this.buffManager, buff);
			}
			this.RegisterEvents();
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0006BD3C File Offset: 0x00069F3C
		private void RegisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff += this.OnAddBuff;
			this.buffManager.onRemoveBuff += this.OnRemoveBuff;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0006BD7B File Offset: 0x00069F7B
		private void UnregisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff -= this.OnAddBuff;
			this.buffManager.onRemoveBuff -= this.OnRemoveBuff;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0006BDBA File Offset: 0x00069FBA
		private void OnAddBuff(CharacterBuffManager manager, Buff buff)
		{
			if (buff.Hide)
			{
				return;
			}
			this.EntryPool.Get(null).Setup(this, buff);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0006BDD8 File Offset: 0x00069FD8
		private void OnRemoveBuff(CharacterBuffManager manager, Buff buff)
		{
			BuffsDisplayEntry buffsDisplayEntry = this.activeEntries.Find((BuffsDisplayEntry e) => e.Target == buff);
			if (buffsDisplayEntry == null)
			{
				return;
			}
			buffsDisplayEntry.Release();
		}

		// Token: 0x040014F3 RID: 5363
		[SerializeField]
		private BuffsDisplayEntry prefab;

		// Token: 0x040014F4 RID: 5364
		private PrefabPool<BuffsDisplayEntry> _entryPool;

		// Token: 0x040014F5 RID: 5365
		private List<BuffsDisplayEntry> activeEntries = new List<BuffsDisplayEntry>();

		// Token: 0x040014F6 RID: 5366
		private CharacterBuffManager buffManager;
	}
}
