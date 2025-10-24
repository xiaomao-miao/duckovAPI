using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000362 RID: 866
	public class RequireFormulaUnlocked : Condition
	{
		// Token: 0x06001E43 RID: 7747 RVA: 0x0006A9F5 File Offset: 0x00068BF5
		public override bool Evaluate()
		{
			return CraftingManager.IsFormulaUnlocked(this.formulaID);
		}

		// Token: 0x040014A2 RID: 5282
		[ItemTypeID]
		[SerializeField]
		private int itemID;

		// Token: 0x040014A3 RID: 5283
		[SerializeField]
		private string formulaID;

		// Token: 0x040014A4 RID: 5284
		public Item setFromItem;
	}
}
