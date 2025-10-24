using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001AC RID: 428
public class CraftView_ListEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00035256 File Offset: 0x00033456
	// (set) Token: 0x06000CAB RID: 3243 RVA: 0x0003525E File Offset: 0x0003345E
	public CraftView Master { get; private set; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00035267 File Offset: 0x00033467
	// (set) Token: 0x06000CAD RID: 3245 RVA: 0x0003526F File Offset: 0x0003346F
	public CraftingFormula Formula { get; private set; }

	// Token: 0x06000CAE RID: 3246 RVA: 0x00035278 File Offset: 0x00033478
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0003528B File Offset: 0x0003348B
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x000352A0 File Offset: 0x000334A0
	public void Setup(CraftView master, CraftingFormula formula)
	{
		this.Master = master;
		this.Formula = formula;
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.Formula.result.id);
		this.icon.sprite = metaData.icon;
		this.nameText.text = string.Format("{0} x{1}", metaData.DisplayName, formula.result.amount);
		this.Refresh();
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00035314 File Offset: 0x00033514
	public void OnPointerClick(PointerEventData eventData)
	{
		CraftView master = this.Master;
		if (master == null)
		{
			return;
		}
		master.SetSelection(this);
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00035328 File Offset: 0x00033528
	internal void NotifyUnselected()
	{
		this.Refresh();
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00035330 File Offset: 0x00033530
	internal void NotifySelected()
	{
		this.Refresh();
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00035338 File Offset: 0x00033538
	private void Refresh()
	{
		if (this.Master == null)
		{
			return;
		}
		bool active = this.Master.GetSelection() == this;
		Color color = this.normalColor;
		if (this.selectedIndicator != null)
		{
			this.selectedIndicator.SetActive(active);
		}
		if (this.Formula.cost.Enough)
		{
			color = this.normalColor;
		}
		else
		{
			color = this.normalInsufficientColor;
		}
		this.background.color = color;
	}

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000AFC RID: 2812
	[SerializeField]
	private Color normalInsufficientColor;

	// Token: 0x04000AFD RID: 2813
	[SerializeField]
	private Image icon;

	// Token: 0x04000AFE RID: 2814
	[SerializeField]
	private Image background;

	// Token: 0x04000AFF RID: 2815
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B00 RID: 2816
	[SerializeField]
	private GameObject selectedIndicator;
}
