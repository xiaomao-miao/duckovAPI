using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000361 RID: 865
	public class RequireDemo : Condition
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x0006A9CB File Offset: 0x00068BCB
		public override bool Evaluate()
		{
			if (this.inverse)
			{
				return !GameMetaData.Instance.IsDemo;
			}
			return GameMetaData.Instance.IsDemo;
		}

		// Token: 0x040014A1 RID: 5281
		[SerializeField]
		private bool inverse;
	}
}
