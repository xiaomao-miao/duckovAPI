using System;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class ItemAmountDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06000A97 RID: 2711 RVA: 0x0002E258 File Offset: 0x0002C458
	// (remove) Token: 0x06000A98 RID: 2712 RVA: 0x0002E28C File Offset: 0x0002C48C
	public static event Action<ItemAmountDisplay> OnMouseEnter;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06000A99 RID: 2713 RVA: 0x0002E2C0 File Offset: 0x0002C4C0
	// (remove) Token: 0x06000A9A RID: 2714 RVA: 0x0002E2F4 File Offset: 0x0002C4F4
	public static event Action<ItemAmountDisplay> OnMouseExit;

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002E327 File Offset: 0x0002C527
	public int TypeID
	{
		get
		{
			return this.typeID;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0002E32F File Offset: 0x0002C52F
	public ItemMetaData MetaData
	{
		get
		{
			return this.metaData;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0002E337 File Offset: 0x0002C537
	public ItemMetaData GetMetaData()
	{
		return this.metaData;
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0002E33F File Offset: 0x0002C53F
	private void Awake()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
		LevelManager.OnLevelInitialized += this.Refresh;
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0002E363 File Offset: 0x0002C563
	private void OnDestroy()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		LevelManager.OnLevelInitialized -= this.Refresh;
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0002E387 File Offset: 0x0002C587
	public void Setup(int itemTypeID, long amount)
	{
		this.typeID = itemTypeID;
		this.amount = amount;
		this.Refresh();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0002E3A0 File Offset: 0x0002C5A0
	private void Refresh()
	{
		int itemCount = ItemUtilities.GetItemCount(this.typeID);
		this.metaData = ItemAssetsCollection.GetMetaData(this.typeID);
		this.icon.sprite = this.metaData.icon;
		this.amountText.text = this.amountFormat.Format(new
		{
			amount = this.amount,
			possess = itemCount
		});
		bool flag = (long)itemCount >= this.amount;
		this.background.color = (flag ? this.enoughColor : this.normalColor);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0002E42C File Offset: 0x0002C62C
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseEnter = ItemAmountDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0002E43E File Offset: 0x0002C63E
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseExit = ItemAmountDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x0400093F RID: 2367
	[SerializeField]
	private Image background;

	// Token: 0x04000940 RID: 2368
	[SerializeField]
	private Image icon;

	// Token: 0x04000941 RID: 2369
	[SerializeField]
	private TextMeshProUGUI amountText;

	// Token: 0x04000942 RID: 2370
	[SerializeField]
	private string amountFormat = "( {possess} / {amount} )";

	// Token: 0x04000943 RID: 2371
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	private Color enoughColor;

	// Token: 0x04000945 RID: 2373
	private int typeID;

	// Token: 0x04000946 RID: 2374
	private long amount;

	// Token: 0x04000947 RID: 2375
	private ItemMetaData metaData;
}
