using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000366 RID: 870
	public class RequireQuestsActive : Condition
	{
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x0006AB34 File Offset: 0x00068D34
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0006AB3C File Offset: 0x00068D3C
		public override bool Evaluate()
		{
			return QuestManager.AreQuestsActive(this.requiredQuestIDs);
		}

		// Token: 0x040014A9 RID: 5289
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
