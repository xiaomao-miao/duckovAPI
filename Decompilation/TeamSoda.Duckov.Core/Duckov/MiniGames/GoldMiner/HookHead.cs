using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000294 RID: 660
	public class HookHead : MonoBehaviour
	{
		// Token: 0x060015B7 RID: 5559 RVA: 0x000507DA File Offset: 0x0004E9DA
		private void OnCollisionEnter2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionEnter;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000507EE File Offset: 0x0004E9EE
		private void OnCollisionExit2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionExit;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00050802 File Offset: 0x0004EA02
		private void OnCollisionStay2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionStay;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x04001009 RID: 4105
		public Action<HookHead, Collision2D> onCollisionEnter;

		// Token: 0x0400100A RID: 4106
		public Action<HookHead, Collision2D> onCollisionExit;

		// Token: 0x0400100B RID: 4107
		public Action<HookHead, Collision2D> onCollisionStay;
	}
}
