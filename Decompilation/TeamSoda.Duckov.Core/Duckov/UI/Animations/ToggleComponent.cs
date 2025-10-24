using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E4 RID: 996
	public class ToggleComponent : MonoBehaviour
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x0007D8DF File Offset: 0x0007BADF
		private bool Status
		{
			get
			{
				return this.master && this.master.Status;
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x0007D8FB File Offset: 0x0007BAFB
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<ToggleAnimation>();
			}
			this.master.onSetToggle += this.OnSetToggle;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0007D92F File Offset: 0x0007BB2F
		private void OnDestroy()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetToggle -= this.OnSetToggle;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x0007D958 File Offset: 0x0007BB58
		protected virtual void OnSetToggle(ToggleAnimation master, bool value)
		{
		}

		// Token: 0x04001876 RID: 6262
		[SerializeField]
		private ToggleAnimation master;
	}
}
