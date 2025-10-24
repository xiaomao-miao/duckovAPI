using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine.EventSystems;

// Token: 0x0200015F RID: 351
public interface IItemDragSource : IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06000AB4 RID: 2740 RVA: 0x0002E590 File Offset: 0x0002C790
	// (remove) Token: 0x06000AB5 RID: 2741 RVA: 0x0002E5C4 File Offset: 0x0002C7C4
	public static event Action<Item> OnStartDragItem;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06000AB6 RID: 2742 RVA: 0x0002E5F8 File Offset: 0x0002C7F8
	// (remove) Token: 0x06000AB7 RID: 2743 RVA: 0x0002E62C File Offset: 0x0002C82C
	public static event Action<Item> OnEndDragItem;

	// Token: 0x06000AB8 RID: 2744
	bool IsEditable();

	// Token: 0x06000AB9 RID: 2745
	Item GetItem();

	// Token: 0x06000ABA RID: 2746 RVA: 0x0002E660 File Offset: 0x0002C860
	void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.IsEditable())
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onStartDragItem = IItemDragSource.OnStartDragItem;
		if (onStartDragItem != null)
		{
			onStartDragItem(item);
		}
		if (item == null)
		{
			return;
		}
		ItemUIUtilities.NotifyPutItem(item, true);
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0002E6A8 File Offset: 0x0002C8A8
	void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onEndDragItem = IItemDragSource.OnEndDragItem;
		if (onEndDragItem == null)
		{
			return;
		}
		onEndDragItem(item);
	}
}
