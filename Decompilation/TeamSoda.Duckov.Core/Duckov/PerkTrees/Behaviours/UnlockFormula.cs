using System;
using System.Collections.Generic;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025B RID: 603
	public class UnlockFormula : PerkBehaviour
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x00046813 File Offset: 0x00044A13
		private IEnumerable<string> FormulasToUnlock
		{
			get
			{
				if (!CraftingFormulaCollection.Instance)
				{
					yield break;
				}
				string matchKey = base.Master.Master.ID + "/" + base.Master.name;
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (craftingFormula.requirePerk == matchKey)
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00046824 File Offset: 0x00044A24
		protected override void OnUnlocked()
		{
			foreach (string formulaID in this.FormulasToUnlock)
			{
				CraftingManager.UnlockFormula(formulaID);
			}
		}
	}
}
