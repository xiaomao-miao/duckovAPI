using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000024 RID: 36
	public class LogWhenUsed : UsageBehavior
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00007E32 File Offset: 0x00006032
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007E35 File Offset: 0x00006035
		protected override void OnUse(Item item, object user)
		{
			Debug.Log(item.name);
		}
	}
}
