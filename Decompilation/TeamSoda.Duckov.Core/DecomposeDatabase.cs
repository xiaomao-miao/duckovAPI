using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001A7 RID: 423
[CreateAssetMenu]
public class DecomposeDatabase : ScriptableObject
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00034AD5 File Offset: 0x00032CD5
	public static DecomposeDatabase Instance
	{
		get
		{
			return GameplayDataSettings.DecomposeDatabase;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00034ADC File Offset: 0x00032CDC
	private Dictionary<int, DecomposeFormula> Dic
	{
		get
		{
			if (this._dic == null)
			{
				this.RebuildDictionary();
			}
			return this._dic;
		}
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00034AF4 File Offset: 0x00032CF4
	public void RebuildDictionary()
	{
		this._dic = new Dictionary<int, DecomposeFormula>();
		foreach (DecomposeFormula decomposeFormula in this.entries)
		{
			this._dic[decomposeFormula.item] = decomposeFormula;
		}
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00034B3C File Offset: 0x00032D3C
	public DecomposeFormula GetFormula(int itemTypeID)
	{
		DecomposeFormula result;
		if (!this.Dic.TryGetValue(itemTypeID, out result))
		{
			return default(DecomposeFormula);
		}
		return result;
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00034B64 File Offset: 0x00032D64
	public static UniTask<bool> Decompose(Item item, int count)
	{
		DecomposeDatabase.<Decompose>d__8 <Decompose>d__;
		<Decompose>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Decompose>d__.item = item;
		<Decompose>d__.count = count;
		<Decompose>d__.<>1__state = -1;
		<Decompose>d__.<>t__builder.Start<DecomposeDatabase.<Decompose>d__8>(ref <Decompose>d__);
		return <Decompose>d__.<>t__builder.Task;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x00034BAF File Offset: 0x00032DAF
	public static bool CanDecompose(int itemTypeID)
	{
		return !(DecomposeDatabase.Instance == null) && DecomposeDatabase.Instance.GetFormula(itemTypeID).valid;
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x00034BD0 File Offset: 0x00032DD0
	public static bool CanDecompose(Item item)
	{
		return !(item == null) && DecomposeDatabase.CanDecompose(item.TypeID);
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00034BE8 File Offset: 0x00032DE8
	public static DecomposeFormula GetDecomposeFormula(int itemTypeID)
	{
		if (DecomposeDatabase.Instance == null)
		{
			return default(DecomposeFormula);
		}
		return DecomposeDatabase.Instance.GetFormula(itemTypeID);
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00034C17 File Offset: 0x00032E17
	public void SetData(List<DecomposeFormula> formulas)
	{
		this.entries = formulas.ToArray();
	}

	// Token: 0x04000AD9 RID: 2777
	[SerializeField]
	private DecomposeFormula[] entries;

	// Token: 0x04000ADA RID: 2778
	private Dictionary<int, DecomposeFormula> _dic;
}
