using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000EB RID: 235
public abstract class ItemSettingBase : MonoBehaviour
{
	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00022F44 File Offset: 0x00021144
	public Item Item
	{
		get
		{
			if (this._item == null)
			{
				this._item = base.GetComponent<Item>();
			}
			return this._item;
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00022F66 File Offset: 0x00021166
	public void Awake()
	{
		if (this.Item)
		{
			this.SetMarkerParam(this.Item);
			this.OnInit();
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00022F87 File Offset: 0x00021187
	public virtual void OnInit()
	{
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00022F89 File Offset: 0x00021189
	public virtual void Start()
	{
	}

	// Token: 0x060007CC RID: 1996
	public abstract void SetMarkerParam(Item selfItem);

	// Token: 0x04000745 RID: 1861
	protected Item _item;
}
