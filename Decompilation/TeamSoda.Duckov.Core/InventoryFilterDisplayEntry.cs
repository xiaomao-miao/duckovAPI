using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F1 RID: 497
public class InventoryFilterDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0003A6BF File Offset: 0x000388BF
	// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0003A6C7 File Offset: 0x000388C7
	public InventoryFilterProvider.FilterEntry Filter { get; private set; }

	// Token: 0x06000E92 RID: 3730 RVA: 0x0003A6D0 File Offset: 0x000388D0
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<InventoryFilterDisplayEntry, PointerEventData> action = this.onPointerClick;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0003A6E4 File Offset: 0x000388E4
	internal void Setup(Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick, InventoryFilterProvider.FilterEntry filter)
	{
		this.onPointerClick = onPointerClick;
		this.Filter = filter;
		if (this.icon)
		{
			this.icon.sprite = filter.icon;
		}
		if (this.nameDisplay)
		{
			this.nameDisplay.text = filter.DisplayName;
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0003A73C File Offset: 0x0003893C
	internal void NotifySelectionChanged(bool isThisSelected)
	{
		this.selectedIndicator.SetActive(isThisSelected);
	}

	// Token: 0x04000C0F RID: 3087
	[SerializeField]
	private Image icon;

	// Token: 0x04000C10 RID: 3088
	[SerializeField]
	private TextMeshProUGUI nameDisplay;

	// Token: 0x04000C11 RID: 3089
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000C13 RID: 3091
	private Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick;
}
