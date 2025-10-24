using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000D RID: 13
	[MenuPath("Debug/Log Item Name")]
	public class LogItemNameAction : EffectAction
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000030A6 File Offset: 0x000012A6
		public override string DisplayName
		{
			get
			{
				return "Log 物品名称";
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000030AD File Offset: 0x000012AD
		protected override void OnTriggeredPositive()
		{
			if (base.Master.Item == null)
			{
				Debug.Log("物品不存在");
				return;
			}
			Debug.Log(base.Master.Item.DisplayName);
		}
	}
}
