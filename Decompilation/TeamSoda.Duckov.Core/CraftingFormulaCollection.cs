using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001A5 RID: 421
[CreateAssetMenu]
public class CraftingFormulaCollection : ScriptableObject
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0003476B File Offset: 0x0003296B
	public static CraftingFormulaCollection Instance
	{
		get
		{
			return GameplayDataSettings.CraftingFormulas;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00034772 File Offset: 0x00032972
	public ReadOnlyCollection<CraftingFormula> Entries
	{
		get
		{
			if (this._entries_ReadOnly == null)
			{
				this._entries_ReadOnly = new ReadOnlyCollection<CraftingFormula>(this.list);
			}
			return this._entries_ReadOnly;
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00034794 File Offset: 0x00032994
	public static bool TryGetFormula(string id, out CraftingFormula formula)
	{
		if (!(CraftingFormulaCollection.Instance == null))
		{
			CraftingFormula craftingFormula = CraftingFormulaCollection.Instance.list.FirstOrDefault((CraftingFormula e) => e.id == id);
			if (!string.IsNullOrEmpty(craftingFormula.id))
			{
				formula = craftingFormula;
				return true;
			}
		}
		formula = default(CraftingFormula);
		return false;
	}

	// Token: 0x04000AD2 RID: 2770
	[SerializeField]
	private List<CraftingFormula> list;

	// Token: 0x04000AD3 RID: 2771
	private ReadOnlyCollection<CraftingFormula> _entries_ReadOnly;
}
