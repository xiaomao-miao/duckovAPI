using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class CraftingManager : MonoBehaviour
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000347FD File Offset: 0x000329FD
	private static CraftingFormulaCollection FormulaCollection
	{
		get
		{
			return CraftingFormulaCollection.Instance;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00034804 File Offset: 0x00032A04
	// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0003480B File Offset: 0x00032A0B
	public static CraftingManager Instance { get; private set; }

	// Token: 0x06000C75 RID: 3189 RVA: 0x00034813 File Offset: 0x00032A13
	private void Awake()
	{
		CraftingManager.Instance = this;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00034832 File Offset: 0x00032A32
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00034845 File Offset: 0x00032A45
	private void Save()
	{
		SavesSystem.Save<List<string>>("Crafting/UnlockedFormulaIDs", this.unlockedFormulaIDs);
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00034858 File Offset: 0x00032A58
	private void Load()
	{
		this.unlockedFormulaIDs = SavesSystem.Load<List<string>>("Crafting/UnlockedFormulaIDs");
		if (this.unlockedFormulaIDs == null)
		{
			this.unlockedFormulaIDs = new List<string>();
		}
		foreach (CraftingFormula craftingFormula in CraftingManager.FormulaCollection.Entries)
		{
			if (craftingFormula.unlockByDefault && !this.unlockedFormulaIDs.Contains(craftingFormula.id))
			{
				this.unlockedFormulaIDs.Add(craftingFormula.id);
			}
		}
		this.unlockedFormulaIDs.Sort();
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000348FC File Offset: 0x00032AFC
	public static IEnumerable<string> UnlockedFormulaIDs
	{
		get
		{
			if (!(CraftingManager.Instance == null))
			{
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (CraftingManager.IsFormulaUnlocked(craftingFormula.id))
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
			}
			yield break;
			yield break;
		}
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00034908 File Offset: 0x00032B08
	public static void UnlockFormula(string formulaID)
	{
		if (CraftingManager.Instance == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(formulaID))
		{
			Debug.LogError("Invalid formula ID");
			return;
		}
		CraftingFormula craftingFormula = CraftingManager.FormulaCollection.Entries.FirstOrDefault((CraftingFormula e) => e.id == formulaID);
		if (!craftingFormula.IDValid)
		{
			Debug.LogError("Invalid formula ID: " + formulaID);
			return;
		}
		if (craftingFormula.unlockByDefault)
		{
			Debug.LogError("Formula is unlocked by default: " + formulaID);
			return;
		}
		if (CraftingManager.Instance.unlockedFormulaIDs.Contains(formulaID))
		{
			return;
		}
		CraftingManager.Instance.unlockedFormulaIDs.Add(formulaID);
		Action<string> onFormulaUnlocked = CraftingManager.OnFormulaUnlocked;
		if (onFormulaUnlocked == null)
		{
			return;
		}
		onFormulaUnlocked(formulaID);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x000349E4 File Offset: 0x00032BE4
	private UniTask<List<Item>> Craft(CraftingFormula formula)
	{
		CraftingManager.<Craft>d__17 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.formula = formula;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__17>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00034A28 File Offset: 0x00032C28
	public UniTask<List<Item>> Craft(string id)
	{
		CraftingManager.<Craft>d__18 <Craft>d__;
		<Craft>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Craft>d__.<>4__this = this;
		<Craft>d__.id = id;
		<Craft>d__.<>1__state = -1;
		<Craft>d__.<>t__builder.Start<CraftingManager.<Craft>d__18>(ref <Craft>d__);
		return <Craft>d__.<>t__builder.Task;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00034A73 File Offset: 0x00032C73
	internal static bool IsFormulaUnlocked(string value)
	{
		return !(CraftingManager.Instance == null) && !string.IsNullOrEmpty(value) && CraftingManager.Instance.unlockedFormulaIDs.Contains(value);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00034AA0 File Offset: 0x00032CA0
	internal static CraftingFormula GetFormula(string id)
	{
		CraftingFormula result;
		if (CraftingFormulaCollection.TryGetFormula(id, out result))
		{
			return result;
		}
		return default(CraftingFormula);
	}

	// Token: 0x04000AD4 RID: 2772
	public static Action<CraftingFormula, Item> OnItemCrafted;

	// Token: 0x04000AD5 RID: 2773
	public static Action<string> OnFormulaUnlocked;

	// Token: 0x04000AD7 RID: 2775
	private const string SaveKey = "Crafting/UnlockedFormulaIDs";

	// Token: 0x04000AD8 RID: 2776
	private List<string> unlockedFormulaIDs = new List<string>();
}
