using System;
using System.Collections.Generic;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class ItemGraphicInfo : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public Item ItemRefrence
	{
		get
		{
			return this.itemRefrence;
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public static ItemGraphicInfo CreateAGraphic(Item item, Transform parent)
	{
		if (item == null || item.ItemGraphic == null)
		{
			return null;
		}
		ItemGraphicInfo itemGraphicInfo = UnityEngine.Object.Instantiate<ItemGraphicInfo>(item.ItemGraphic);
		if (parent != null)
		{
			itemGraphicInfo.transform.SetParent(parent);
		}
		itemGraphicInfo.transform.localPosition = Vector3.zero;
		itemGraphicInfo.transform.localRotation = Quaternion.identity;
		itemGraphicInfo.transform.localScale = Vector3.one;
		itemGraphicInfo.Setup(item);
		return itemGraphicInfo;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020D8 File Offset: 0x000002D8
	public void Setup(Item item)
	{
		this.itemRefrence = item;
		this.subGraphics = new List<ItemGraphicInfo>();
		this.socketsDictionary = new Dictionary<string, ItemGraphicInfo.ItemGraphicSocket>();
		foreach (ItemGraphicInfo.ItemGraphicSocket itemGraphicSocket in this.sockets)
		{
			this.socketsDictionary.Add(itemGraphicSocket.socketPoint.name, itemGraphicSocket);
		}
		if (item.Slots != null && item.Slots.Count > 0)
		{
			foreach (Slot slot in item.Slots)
			{
				Item content = slot.Content;
				if (!(content == null))
				{
					string key = slot.Key;
					ItemGraphicInfo.ItemGraphicSocket itemGraphicSocket2;
					if (this.socketsDictionary.TryGetValue(key, out itemGraphicSocket2))
					{
						ItemGraphicInfo itemGraphicInfo = ItemGraphicInfo.CreateAGraphic(content, itemGraphicSocket2.socketPoint);
						if (itemGraphicInfo)
						{
							if (itemGraphicSocket2.showIfPluged)
							{
								itemGraphicSocket2.showIfPluged.SetActive(true);
							}
							if (itemGraphicSocket2.hideIfPluged)
							{
								itemGraphicSocket2.hideIfPluged.SetActive(false);
							}
							this.subGraphics.Add(itemGraphicInfo);
							itemGraphicInfo.Setup(content);
						}
					}
				}
			}
		}
	}

	// Token: 0x04000001 RID: 1
	[SerializeField]
	private List<ItemGraphicInfo.ItemGraphicSocket> sockets;

	// Token: 0x04000002 RID: 2
	public Dictionary<string, ItemGraphicInfo.ItemGraphicSocket> socketsDictionary;

	// Token: 0x04000003 RID: 3
	private Item itemRefrence;

	// Token: 0x04000004 RID: 4
	private List<ItemGraphicInfo> subGraphics;

	// Token: 0x0200002E RID: 46
	[Serializable]
	public struct ItemGraphicSocket
	{
		// Token: 0x040000D2 RID: 210
		public Transform socketPoint;

		// Token: 0x040000D3 RID: 211
		public GameObject showIfPluged;

		// Token: 0x040000D4 RID: 212
		public GameObject hideIfPluged;
	}
}
