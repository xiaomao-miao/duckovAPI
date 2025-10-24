using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F4 RID: 500
public class ItemMetaDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x06000E9A RID: 3738 RVA: 0x0003A7AD File Offset: 0x000389AD
	public ItemMetaData GetMetaData()
	{
		return this.data;
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06000E9B RID: 3739 RVA: 0x0003A7B8 File Offset: 0x000389B8
	// (remove) Token: 0x06000E9C RID: 3740 RVA: 0x0003A7EC File Offset: 0x000389EC
	public static event Action<ItemMetaDisplay> OnMouseEnter;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06000E9D RID: 3741 RVA: 0x0003A820 File Offset: 0x00038A20
	// (remove) Token: 0x06000E9E RID: 3742 RVA: 0x0003A854 File Offset: 0x00038A54
	public static event Action<ItemMetaDisplay> OnMouseExit;

	// Token: 0x06000E9F RID: 3743 RVA: 0x0003A887 File Offset: 0x00038A87
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseEnter = ItemMetaDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0003A899 File Offset: 0x00038A99
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseExit = ItemMetaDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0003A8AC File Offset: 0x00038AAC
	public void Setup(int typeID)
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(typeID);
		this.Setup(metaData);
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0003A8C7 File Offset: 0x00038AC7
	public void Setup(ItemMetaData data)
	{
		this.data = data;
		this.icon.sprite = data.icon;
		GameplayDataSettings.UIStyle.ApplyDisplayQualityShadow(data.displayQuality, this.displayQualityShadow);
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0003A8F7 File Offset: 0x00038AF7
	internal void Setup(object rootTypeID)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000C16 RID: 3094
	[SerializeField]
	private Image icon;

	// Token: 0x04000C17 RID: 3095
	[SerializeField]
	private TrueShadow displayQualityShadow;

	// Token: 0x04000C18 RID: 3096
	private ItemMetaData data;
}
