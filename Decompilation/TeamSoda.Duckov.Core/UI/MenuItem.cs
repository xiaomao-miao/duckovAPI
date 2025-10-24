using System;
using UnityEngine;

namespace UI
{
	// Token: 0x02000211 RID: 529
	public class MenuItem : MonoBehaviour
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0003DFD7 File Offset: 0x0003C1D7
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x0003E00A File Offset: 0x0003C20A
		public Menu Master
		{
			get
			{
				if (this._master == null)
				{
					Transform parent = base.transform.parent;
					this._master = ((parent != null) ? parent.GetComponent<Menu>() : null);
				}
				return this._master;
			}
			set
			{
				this._master = value;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0003E013 File Offset: 0x0003C213
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0003E02A File Offset: 0x0003C22A
		public bool Selectable
		{
			get
			{
				return base.gameObject.activeSelf && this.selectable;
			}
			set
			{
				this.selectable = value;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0003E033 File Offset: 0x0003C233
		public bool IsSelected
		{
			get
			{
				return this.cacheSelected;
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0003E03B File Offset: 0x0003C23B
		private void OnTransformParentChanged()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003E058 File Offset: 0x0003C258
		private void OnEnable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0003E075 File Offset: 0x0003C275
		private void OnDisable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Unegister(this);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003E092 File Offset: 0x0003C292
		public void Select()
		{
			if (this.Master == null)
			{
				Debug.LogError("Menu Item " + base.name + " 没有Master。");
				return;
			}
			this.Master.Select(this);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0003E0C9 File Offset: 0x0003C2C9
		internal void NotifySelected()
		{
			this.cacheSelected = true;
			Action<MenuItem> action = this.onSelected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0003E0E3 File Offset: 0x0003C2E3
		internal void NotifyDeselected()
		{
			this.cacheSelected = false;
			Action<MenuItem> action = this.onDeselected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0003E0FD File Offset: 0x0003C2FD
		internal void NotifyConfirmed()
		{
			Action<MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0003E110 File Offset: 0x0003C310
		internal void NotifyCanceled()
		{
			Action<MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0003E123 File Offset: 0x0003C323
		internal void NotifyMasterFocusStatusChanged()
		{
			Action<MenuItem, bool> action = this.onFocusStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Master.Focused);
		}

		// Token: 0x04000CAE RID: 3246
		private Menu _master;

		// Token: 0x04000CAF RID: 3247
		[SerializeField]
		private bool selectable = true;

		// Token: 0x04000CB0 RID: 3248
		private bool cacheSelected;

		// Token: 0x04000CB1 RID: 3249
		public Action<MenuItem> onSelected;

		// Token: 0x04000CB2 RID: 3250
		public Action<MenuItem> onDeselected;

		// Token: 0x04000CB3 RID: 3251
		public Action<MenuItem> onConfirmed;

		// Token: 0x04000CB4 RID: 3252
		public Action<MenuItem> onCanceled;

		// Token: 0x04000CB5 RID: 3253
		public Action<MenuItem, bool> onFocusStatusChanged;
	}
}
