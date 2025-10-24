using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024C RID: 588
	[RequireComponent(typeof(Perk))]
	public abstract class PerkBehaviour : MonoBehaviour
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x0004599E File Offset: 0x00043B9E
		protected Perk Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x000459A6 File Offset: 0x00043BA6
		private bool Unlocked
		{
			get
			{
				return !(this.master == null) && this.master.Unlocked;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x000459C3 File Offset: 0x00043BC3
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000459CC File Offset: 0x00043BCC
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
			this.master.onUnlockStateChanged += this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
			this.OnAwake();
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00045A2E File Offset: 0x00043C2E
		private void OnLevelInitialized()
		{
			this.NotifyUnlockStateChanged(this.Unlocked);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00045A3C File Offset: 0x00043C3C
		private void OnDestroy()
		{
			this.OnOnDestroy();
			if (this.master == null)
			{
				return;
			}
			this.master.onUnlockStateChanged -= this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00045A7B File Offset: 0x00043C7B
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00045A97 File Offset: 0x00043C97
		private void OnMasterUnlockStateChanged(Perk perk, bool unlocked)
		{
			if (perk != this.master)
			{
				Debug.LogError("Perk对象不匹配");
			}
			this.NotifyUnlockStateChanged(unlocked);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00045AB8 File Offset: 0x00043CB8
		private void NotifyUnlockStateChanged(bool unlocked)
		{
			this.OnUnlockStateChanged(unlocked);
			if (unlocked)
			{
				this.OnUnlocked();
				return;
			}
			this.OnLocked();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00045AD1 File Offset: 0x00043CD1
		protected virtual void OnUnlockStateChanged(bool unlocked)
		{
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00045AD3 File Offset: 0x00043CD3
		protected virtual void OnUnlocked()
		{
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00045AD5 File Offset: 0x00043CD5
		protected virtual void OnLocked()
		{
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00045AD7 File Offset: 0x00043CD7
		protected virtual void OnAwake()
		{
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00045AD9 File Offset: 0x00043CD9
		protected virtual void OnOnDestroy()
		{
		}

		// Token: 0x04000E0A RID: 3594
		private Perk master;
	}
}
