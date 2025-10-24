using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000367 RID: 871
	public class RequireQuestsFinished : Condition
	{
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0006AB51 File Offset: 0x00068D51
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x0006AB59 File Offset: 0x00068D59
		public override bool Evaluate()
		{
			return QuestManager.AreQuestFinished(this.requiredQuestIDs);
		}

		// Token: 0x040014AA RID: 5290
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
