using System;
using System.Collections.Generic;
using Duckov.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x0200030E RID: 782
	public class Basket : MonoBehaviour
	{
		// Token: 0x060019CB RID: 6603 RVA: 0x0005D236 File Offset: 0x0005B436
		private void Awake()
		{
			this.trigger.onGoal.AddListener(new UnityAction<BasketBall>(this.OnGoal));
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0005D254 File Offset: 0x0005B454
		private void OnGoal(BasketBall ball)
		{
			if (!this.conditions.Satisfied())
			{
				return;
			}
			this.onGoal.Invoke(ball);
			this.netAnimator.SetTrigger("Goal");
		}

		// Token: 0x0400129E RID: 4766
		[SerializeField]
		private Animator netAnimator;

		// Token: 0x0400129F RID: 4767
		[SerializeField]
		private List<Condition> conditions = new List<Condition>();

		// Token: 0x040012A0 RID: 4768
		[SerializeField]
		private BasketTrigger trigger;

		// Token: 0x040012A1 RID: 4769
		public UnityEvent<BasketBall> onGoal;
	}
}
