using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D0 RID: 976
	public class BarDisplayController_Thurst : BarDisplayController
	{
		// Token: 0x06002370 RID: 9072 RVA: 0x0007BE84 File Offset: 0x0007A084
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingCurrent != num || this.displayingMax != max)
			{
				base.Refresh();
				this.displayingCurrent = num;
				this.displayingMax = max;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x0007BEC5 File Offset: 0x0007A0C5
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

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x0007BEE6 File Offset: 0x0007A0E6
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentWater;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x0007BF08 File Offset: 0x0007A108
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxWater;
			}
		}

		// Token: 0x04001811 RID: 6161
		private CharacterMainControl _target;

		// Token: 0x04001812 RID: 6162
		private float displayingCurrent = -1f;

		// Token: 0x04001813 RID: 6163
		private float displayingMax = -1f;
	}
}
