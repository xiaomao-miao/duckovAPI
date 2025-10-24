using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003CF RID: 975
	public class BarDisplayController_Stemina : BarDisplayController
	{
		// Token: 0x0600236B RID: 9067 RVA: 0x0007BDC0 File Offset: 0x00079FC0
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingStemina != num || this.displayingMaxStemina != max)
			{
				base.Refresh();
				this.displayingStemina = num;
				this.displayingMaxStemina = max;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x0007BE01 File Offset: 0x0007A001
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

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x0007BE22 File Offset: 0x0007A022
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentStamina;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x0007BE44 File Offset: 0x0007A044
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxStamina;
			}
		}

		// Token: 0x0400180E RID: 6158
		private CharacterMainControl _target;

		// Token: 0x0400180F RID: 6159
		private float displayingStemina = -1f;

		// Token: 0x04001810 RID: 6160
		private float displayingMaxStemina = -1f;
	}
}
