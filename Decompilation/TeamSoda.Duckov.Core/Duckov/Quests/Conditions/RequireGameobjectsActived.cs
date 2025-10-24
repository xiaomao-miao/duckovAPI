using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000363 RID: 867
	public class RequireGameobjectsActived : Condition
	{
		// Token: 0x06001E45 RID: 7749 RVA: 0x0006AA0C File Offset: 0x00068C0C
		public override bool Evaluate()
		{
			foreach (GameObject gameObject in this.targets)
			{
				if (gameObject == null || !gameObject.activeInHierarchy)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040014A5 RID: 5285
		[SerializeField]
		private GameObject[] targets;
	}
}
