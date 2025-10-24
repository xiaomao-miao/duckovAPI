using System;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class ItemPropertiesDisplay : MonoBehaviour
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002DDF0 File Offset: 0x0002BFF0
	private PrefabPool<LabelAndValue> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<LabelAndValue>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0002DE29 File Offset: 0x0002C029
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0002DE3C File Offset: 0x0002C03C
	internal void Setup(Item targetItem)
	{
		this.EntryPool.ReleaseAll();
		if (targetItem == null)
		{
			return;
		}
		foreach (ValueTuple<string, string, Polarity> valueTuple in targetItem.GetPropertyValueTextPair())
		{
			this.EntryPool.Get(null).Setup(valueTuple.Item1, valueTuple.Item2, valueTuple.Item3);
		}
	}

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	private LabelAndValue entryTemplate;

	// Token: 0x04000924 RID: 2340
	private PrefabPool<LabelAndValue> _entryPool;
}
