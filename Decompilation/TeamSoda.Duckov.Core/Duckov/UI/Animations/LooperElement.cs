using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DB RID: 987
	public abstract class LooperElement : MonoBehaviour
	{
		// Token: 0x060023E4 RID: 9188 RVA: 0x0007D23F File Offset: 0x0007B43F
		protected virtual void OnEnable()
		{
			this.clock.onTick += this.OnTick;
			this.OnTick(this.clock, this.clock.t);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x0007D270 File Offset: 0x0007B470
		protected virtual void OnDisable()
		{
			if (this.clock != null)
			{
				this.clock.onTick -= this.OnTick;
			}
		}

		// Token: 0x060023E6 RID: 9190
		protected abstract void OnTick(LooperClock clock, float t);

		// Token: 0x04001856 RID: 6230
		[SerializeField]
		private LooperClock clock;
	}
}
