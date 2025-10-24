using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DA RID: 986
	public class LooperClock : MonoBehaviour
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0007D149 File Offset: 0x0007B349
		public float t
		{
			get
			{
				if (this.duration > 0f)
				{
					return this.time / this.duration;
				}
				return 1f;
			}
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x060023DF RID: 9183 RVA: 0x0007D16C File Offset: 0x0007B36C
		// (remove) Token: 0x060023E0 RID: 9184 RVA: 0x0007D1A4 File Offset: 0x0007B3A4
		public event Action<LooperClock, float> onTick;

		// Token: 0x060023E1 RID: 9185 RVA: 0x0007D1D9 File Offset: 0x0007B3D9
		private void Update()
		{
			if (this.duration > 0f)
			{
				this.time += Time.unscaledDeltaTime;
				this.time %= this.duration;
				this.Tick();
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0007D213 File Offset: 0x0007B413
		private void Tick()
		{
			Action<LooperClock, float> action = this.onTick;
			if (action == null)
			{
				return;
			}
			action(this, this.t);
		}

		// Token: 0x04001853 RID: 6227
		[SerializeField]
		private float duration = 1f;

		// Token: 0x04001854 RID: 6228
		private float time;
	}
}
