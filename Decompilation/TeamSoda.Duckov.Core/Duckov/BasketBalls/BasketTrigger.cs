using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x02000310 RID: 784
	public class BasketTrigger : MonoBehaviour
	{
		// Token: 0x060019CF RID: 6607 RVA: 0x0005D29C File Offset: 0x0005B49C
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("ONTRIGGERENTER:" + other.name);
			BasketBall component = other.GetComponent<BasketBall>();
			if (component == null)
			{
				return;
			}
			this.onGoal.Invoke(component);
		}

		// Token: 0x040012A2 RID: 4770
		public UnityEvent<BasketBall> onGoal;
	}
}
