using System;
using Duckov.Utilities;
using Duckov.Utilities.Updatables;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000017 RID: 23
	public class TickTrigger : EffectTrigger, IUpdatable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003F11 File Offset: 0x00002111
		public override string DisplayName
		{
			get
			{
				return string.Format("每{0}秒", this.period);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003F28 File Offset: 0x00002128
		private float Factor
		{
			get
			{
				if (this.period <= 0f)
				{
					return 0f;
				}
				if (this._currentPeriod != this.period)
				{
					this._factor = 1f / this.period;
					this._currentPeriod = this.period;
				}
				return this._factor;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003F7A File Offset: 0x0000217A
		private void OnEnable()
		{
			UpdatableInvoker.Register(this);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003F82 File Offset: 0x00002182
		private new void OnDisable()
		{
			UpdatableInvoker.Unregister(this);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003F8C File Offset: 0x0000218C
		private void UpdateBuffer()
		{
			this.buffer += Time.deltaTime * this.Factor;
			while (this.buffer > 1f)
			{
				this.buffer -= 1f;
				base.Trigger(true);
				if (!this.allowMultipleTrigger)
				{
					break;
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003FE2 File Offset: 0x000021E2
		public void OnUpdate()
		{
			this.UpdateBuffer();
		}

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private float period = 1f;

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private bool allowMultipleTrigger = true;

		// Token: 0x0400003D RID: 61
		private float buffer;

		// Token: 0x0400003E RID: 62
		private float _currentPeriod;

		// Token: 0x0400003F RID: 63
		private float _factor = 1f;
	}
}
