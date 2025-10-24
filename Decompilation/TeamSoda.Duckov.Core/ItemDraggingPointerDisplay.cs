using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000160 RID: 352
public class ItemDraggingPointerDisplay : MonoBehaviour
{
	// Token: 0x06000ABC RID: 2748 RVA: 0x0002E6D8 File Offset: 0x0002C8D8
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
		this.parentRectTransform = (base.transform.parent as RectTransform);
		IItemDragSource.OnStartDragItem += this.OnStartDragItem;
		IItemDragSource.OnEndDragItem += this.OnEndDragItem;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0002E73A File Offset: 0x0002C93A
	private void OnDestroy()
	{
		IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
		IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0002E75E File Offset: 0x0002C95E
	private void Update()
	{
		this.RefreshPosition();
		if (Mouse.current.leftButton.wasReleasedThisFrame)
		{
			this.OnEndDragItem(null);
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0002E780 File Offset: 0x0002C980
	private unsafe void RefreshPosition()
	{
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform.parent as RectTransform, *Pointer.current.position.value, null, out v);
		this.rectTransform.localPosition = v;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0002E7CB File Offset: 0x0002C9CB
	private void OnStartDragItem(Item item)
	{
		this.target = item;
		if (this.target == null)
		{
			return;
		}
		this.display.Setup(this.target);
		this.RefreshPosition();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0002E806 File Offset: 0x0002CA06
	private void OnEndDragItem(Item item)
	{
		this.target = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400094F RID: 2383
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04000950 RID: 2384
	[SerializeField]
	private RectTransform parentRectTransform;

	// Token: 0x04000951 RID: 2385
	[SerializeField]
	private ItemDisplay display;

	// Token: 0x04000952 RID: 2386
	private Item target;
}
