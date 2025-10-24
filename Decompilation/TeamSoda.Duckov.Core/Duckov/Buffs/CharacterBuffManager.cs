using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.Buffs
{
	// Token: 0x020003FD RID: 1021
	public class CharacterBuffManager : MonoBehaviour
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x0008006B File Offset: 0x0007E26B
		public CharacterMainControl Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00080073 File Offset: 0x0007E273
		public ReadOnlyCollection<Buff> Buffs
		{
			get
			{
				if (this._readOnlyBuffsCollection == null)
				{
					this._readOnlyBuffsCollection = new ReadOnlyCollection<Buff>(this.buffs);
				}
				return this._readOnlyBuffsCollection;
			}
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x0600251D RID: 9501 RVA: 0x00080094 File Offset: 0x0007E294
		// (remove) Token: 0x0600251E RID: 9502 RVA: 0x000800CC File Offset: 0x0007E2CC
		public event Action<CharacterBuffManager, Buff> onAddBuff;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x0600251F RID: 9503 RVA: 0x00080104 File Offset: 0x0007E304
		// (remove) Token: 0x06002520 RID: 9504 RVA: 0x0008013C File Offset: 0x0007E33C
		public event Action<CharacterBuffManager, Buff> onRemoveBuff;

		// Token: 0x06002521 RID: 9505 RVA: 0x00080171 File Offset: 0x0007E371
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<CharacterMainControl>();
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x00080190 File Offset: 0x0007E390
		public void AddBuff(Buff buffPrefab, CharacterMainControl fromWho, int overrideWeaponID = 0)
		{
			if (buffPrefab == null)
			{
				return;
			}
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffPrefab.ID);
			if (buff)
			{
				buff.NotifyIncomingBuffWithSameID(buffPrefab);
				return;
			}
			Buff buff2 = UnityEngine.Object.Instantiate<Buff>(buffPrefab);
			buff2.Setup(this);
			buff2.fromWho = fromWho;
			if (overrideWeaponID > 0)
			{
				buff2.fromWeaponID = overrideWeaponID;
			}
			this.buffs.Add(buff2);
			Action<CharacterBuffManager, Buff> action = this.onAddBuff;
			if (action == null)
			{
				return;
			}
			action(this, buff2);
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x0008022C File Offset: 0x0007E42C
		public void RemoveBuff(int buffID, bool removeOneLayer)
		{
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffID);
			if (buff != null)
			{
				this.RemoveBuff(buff, removeOneLayer);
			}
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x00080270 File Offset: 0x0007E470
		public void RemoveBuffsByTag(Buff.BuffExclusiveTags buffTag, bool removeOneLayer)
		{
			if (buffTag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return;
			}
			foreach (Buff buff in this.buffs.FindAll((Buff e) => e.ExclusiveTag == buffTag))
			{
				if (buff != null)
				{
					this.RemoveBuff(buff, removeOneLayer);
				}
			}
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000802F4 File Offset: 0x0007E4F4
		public bool HasBuff(int buffID)
		{
			return this.buffs.Find((Buff e) => e.ID == buffID) != null;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x0008032C File Offset: 0x0007E52C
		public Buff GetBuffByTag(Buff.BuffExclusiveTags tag)
		{
			if (tag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return null;
			}
			return this.buffs.Find((Buff e) => e.ExclusiveTag == tag);
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x00080368 File Offset: 0x0007E568
		public void RemoveBuff(Buff toRemove, bool oneLayer)
		{
			if (oneLayer && toRemove.CurrentLayers > 1)
			{
				toRemove.CurrentLayers--;
				if (toRemove.CurrentLayers >= 1)
				{
					return;
				}
			}
			if (this.buffs.Remove(toRemove))
			{
				Action<CharacterBuffManager, Buff> action = this.onRemoveBuff;
				if (action != null)
				{
					action(this, toRemove);
				}
				UnityEngine.Object.Destroy(toRemove.gameObject);
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000803C8 File Offset: 0x0007E5C8
		private void Update()
		{
			bool flag = false;
			foreach (Buff buff in this.buffs)
			{
				if (buff == null)
				{
					flag = true;
				}
				else if (buff.IsOutOfTime)
				{
					buff.NotifyOutOfTime();
					this.outOfTimeBuffsBuffer.Add(buff);
				}
				else
				{
					buff.NotifyUpdate();
				}
			}
			if (this.outOfTimeBuffsBuffer.Count > 0)
			{
				foreach (Buff buff2 in this.outOfTimeBuffsBuffer)
				{
					if (buff2 != null)
					{
						this.RemoveBuff(buff2, false);
					}
				}
				this.outOfTimeBuffsBuffer.Clear();
			}
			if (flag)
			{
				this.buffs.RemoveAll((Buff e) => e == null);
			}
		}

		// Token: 0x0400194A RID: 6474
		[SerializeField]
		private CharacterMainControl master;

		// Token: 0x0400194B RID: 6475
		private List<Buff> buffs = new List<Buff>();

		// Token: 0x0400194C RID: 6476
		private ReadOnlyCollection<Buff> _readOnlyBuffsCollection;

		// Token: 0x0400194F RID: 6479
		private List<Buff> outOfTimeBuffsBuffer = new List<Buff>();
	}
}
