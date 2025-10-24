using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003CE RID: 974
	public class BarDisplayController_Hunger : BarDisplayController
	{
		// Token: 0x06002366 RID: 9062 RVA: 0x0007BCFC File Offset: 0x00079EFC
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

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x0007BD3D File Offset: 0x00079F3D
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

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x0007BD5E File Offset: 0x00079F5E
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentEnergy;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x0007BD80 File Offset: 0x00079F80
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxEnergy;
			}
		}

		// Token: 0x0400180B RID: 6155
		private CharacterMainControl _target;

		// Token: 0x0400180C RID: 6156
		private float displayingCurrent = -1f;

		// Token: 0x0400180D RID: 6157
		private float displayingMax = -1f;
	}
}
