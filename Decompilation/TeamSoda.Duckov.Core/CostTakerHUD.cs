using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class CostTakerHUD : MonoBehaviour
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001D9A4 File Offset: 0x0001BBA4
	private PrefabPool<CostTakerHUD_Entry> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<CostTakerHUD_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0001D9DD File Offset: 0x0001BBDD
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
		this.ShowAll();
		CostTaker.OnCostTakerRegistered += this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered += this.OnCostTakerUnregistered;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0001DA18 File Offset: 0x0001BC18
	private void OnDestroy()
	{
		CostTaker.OnCostTakerRegistered -= this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered -= this.OnCostTakerUnregistered;
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0001DA3C File Offset: 0x0001BC3C
	private void OnCostTakerRegistered(CostTaker taker)
	{
		this.ShowHUD(taker);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0001DA45 File Offset: 0x0001BC45
	private void OnCostTakerUnregistered(CostTaker taker)
	{
		this.HideHUD(taker);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0001DA4E File Offset: 0x0001BC4E
	private void Start()
	{
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0001DA50 File Offset: 0x0001BC50
	private void ShowAll()
	{
		this.EntryPool.ReleaseAll();
		foreach (CostTaker costTaker in CostTaker.ActiveCostTakers)
		{
			this.ShowHUD(costTaker);
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
	private void ShowHUD(CostTaker costTaker)
	{
		this.EntryPool.Get(null).Setup(costTaker);
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0001DABC File Offset: 0x0001BCBC
	private void HideHUD(CostTaker costTaker)
	{
		CostTakerHUD_Entry costTakerHUD_Entry = this.EntryPool.Find((CostTakerHUD_Entry e) => e.gameObject.activeSelf && e.Target == costTaker);
		if (costTakerHUD_Entry == null)
		{
			return;
		}
		this.EntryPool.Release(costTakerHUD_Entry);
	}

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private CostTakerHUD_Entry entryTemplate;

	// Token: 0x0400065C RID: 1628
	private PrefabPool<CostTakerHUD_Entry> _entryPool;
}
