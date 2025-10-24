using System;
using UnityEngine.Events;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003CD RID: 973
	public class BarDisplayController_HP : BarDisplayController
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x0007BC02 File Offset: 0x00079E02
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.CurrentHealth;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x0007BC28 File Offset: 0x00079E28
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.MaxHealth;
			}
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007BC4E File Offset: 0x00079E4E
		private void OnEnable()
		{
			base.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0007BC5C File Offset: 0x00079E5C
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0007BC64 File Offset: 0x00079E64
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.AddListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0007BC96 File Offset: 0x00079E96
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x0007BCC8 File Offset: 0x00079EC8
		private CharacterMainControl Target
		{
			get
			{
				if (this._target == null)
				{
					this._target = CharacterMainControl.Main;
				}
				return this._target;
			}
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0007BCE9 File Offset: 0x00079EE9
		private void OnHealthChange(Health health)
		{
			base.Refresh();
		}

		// Token: 0x0400180A RID: 6154
		private CharacterMainControl _target;
	}
}
