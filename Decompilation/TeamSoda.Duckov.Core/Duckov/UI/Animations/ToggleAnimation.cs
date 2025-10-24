using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E3 RID: 995
	public class ToggleAnimation : MonoBehaviour
	{
		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06002403 RID: 9219 RVA: 0x0007D828 File Offset: 0x0007BA28
		// (remove) Token: 0x06002404 RID: 9220 RVA: 0x0007D860 File Offset: 0x0007BA60
		public event Action<ToggleAnimation, bool> onSetToggle;

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x0007D895 File Offset: 0x0007BA95
		// (set) Token: 0x06002406 RID: 9222 RVA: 0x0007D89D File Offset: 0x0007BA9D
		public bool Status
		{
			get
			{
				return this.status;
			}
			protected set
			{
				this.SetToggle(value);
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0007D8A6 File Offset: 0x0007BAA6
		public void SetToggle(bool value)
		{
			this.status = value;
			if (!Application.isPlaying)
			{
				return;
			}
			this.OnSetToggle(this.Status);
			Action<ToggleAnimation, bool> action = this.onSetToggle;
			if (action == null)
			{
				return;
			}
			action(this, value);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0007D8D5 File Offset: 0x0007BAD5
		protected virtual void OnSetToggle(bool value)
		{
		}

		// Token: 0x04001875 RID: 6261
		[SerializeField]
		[HideInInspector]
		private bool status;
	}
}
