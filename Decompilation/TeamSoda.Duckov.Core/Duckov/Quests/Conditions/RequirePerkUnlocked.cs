using System;
using Duckov.PerkTrees;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000365 RID: 869
	public class RequirePerkUnlocked : Condition
	{
		// Token: 0x06001E4B RID: 7755 RVA: 0x0006AA76 File Offset: 0x00068C76
		public override bool Evaluate()
		{
			return this.GetUnlocked();
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0006AA80 File Offset: 0x00068C80
		private bool GetUnlocked()
		{
			if (this.perk)
			{
				return this.perk.Unlocked;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				foreach (Perk perk in perkTree.perks)
				{
					if (perk.gameObject.name == this.perkObjectName)
					{
						this.perk = perk;
						return this.perk.Unlocked;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x040014A6 RID: 5286
		[SerializeField]
		private string perkTreeID;

		// Token: 0x040014A7 RID: 5287
		[SerializeField]
		private string perkObjectName;

		// Token: 0x040014A8 RID: 5288
		private Perk perk;
	}
}
