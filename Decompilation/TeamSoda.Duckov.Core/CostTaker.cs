using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Economy;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D4 RID: 212
public class CostTaker : InteractableBase
{
	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001D767 File Offset: 0x0001B967
	public Cost Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000684 RID: 1668 RVA: 0x0001D770 File Offset: 0x0001B970
	// (remove) Token: 0x06000685 RID: 1669 RVA: 0x0001D7A8 File Offset: 0x0001B9A8
	public event Action<CostTaker> onPayed;

	// Token: 0x06000686 RID: 1670 RVA: 0x0001D7DD File Offset: 0x0001B9DD
	protected override bool IsInteractable()
	{
		return this.cost.Enough;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0001D7EC File Offset: 0x0001B9EC
	protected override void OnInteractFinished()
	{
		if (!this.cost.Enough)
		{
			return;
		}
		if (this.cost.Pay(true, true))
		{
			Action<CostTaker> action = this.onPayed;
			if (action != null)
			{
				action(this);
			}
			UnityEvent<CostTaker> unityEvent = this.onPayedUnityEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0001D839 File Offset: 0x0001BA39
	private void OnEnable()
	{
		CostTaker.Register(this);
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x0001D841 File Offset: 0x0001BA41
	private void OnDisable()
	{
		CostTaker.Unregister(this);
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001D849 File Offset: 0x0001BA49
	public static ReadOnlyCollection<CostTaker> ActiveCostTakers
	{
		get
		{
			if (CostTaker._activeCostTakers_ReadOnly == null)
			{
				CostTaker._activeCostTakers_ReadOnly = new ReadOnlyCollection<CostTaker>(CostTaker.activeCostTakers);
			}
			return CostTaker._activeCostTakers_ReadOnly;
		}
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x0600068B RID: 1675 RVA: 0x0001D868 File Offset: 0x0001BA68
	// (remove) Token: 0x0600068C RID: 1676 RVA: 0x0001D89C File Offset: 0x0001BA9C
	public static event Action<CostTaker> OnCostTakerRegistered;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600068D RID: 1677 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
	// (remove) Token: 0x0600068E RID: 1678 RVA: 0x0001D904 File Offset: 0x0001BB04
	public static event Action<CostTaker> OnCostTakerUnregistered;

	// Token: 0x0600068F RID: 1679 RVA: 0x0001D937 File Offset: 0x0001BB37
	public static void Register(CostTaker costTaker)
	{
		CostTaker.activeCostTakers.Add(costTaker);
		Action<CostTaker> onCostTakerRegistered = CostTaker.OnCostTakerRegistered;
		if (onCostTakerRegistered == null)
		{
			return;
		}
		onCostTakerRegistered(costTaker);
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0001D954 File Offset: 0x0001BB54
	public static void Unregister(CostTaker costTaker)
	{
		if (CostTaker.activeCostTakers.Remove(costTaker))
		{
			Action<CostTaker> onCostTakerUnregistered = CostTaker.OnCostTakerUnregistered;
			if (onCostTakerUnregistered == null)
			{
				return;
			}
			onCostTakerUnregistered(costTaker);
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0001D973 File Offset: 0x0001BB73
	public void SetCost(Cost cost)
	{
		CostTaker.Unregister(this);
		this.cost = cost;
		if (base.isActiveAndEnabled)
		{
			CostTaker.Register(this);
		}
	}

	// Token: 0x04000654 RID: 1620
	[SerializeField]
	private Cost cost;

	// Token: 0x04000656 RID: 1622
	public UnityEvent<CostTaker> onPayedUnityEvent;

	// Token: 0x04000657 RID: 1623
	private static List<CostTaker> activeCostTakers = new List<CostTaker>();

	// Token: 0x04000658 RID: 1624
	private static ReadOnlyCollection<CostTaker> _activeCostTakers_ReadOnly;
}
