using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class StorageDockCountTMP : MonoBehaviour
{
	// Token: 0x06000EF9 RID: 3833 RVA: 0x0003B6BE File Offset: 0x000398BE
	private void Awake()
	{
		PlayerStorage.OnItemAddedToBuffer += this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished += this.OnLoadingFinished;
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0003B6F3 File Offset: 0x000398F3
	private void OnDestroy()
	{
		PlayerStorage.OnItemAddedToBuffer -= this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished -= this.OnLoadingFinished;
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0003B728 File Offset: 0x00039928
	private void OnLoadingFinished()
	{
		this.Refresh();
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0003B730 File Offset: 0x00039930
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0003B738 File Offset: 0x00039938
	private void OnTakeBufferItem()
	{
		this.Refresh();
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0003B740 File Offset: 0x00039940
	private void OnItemAddedToBuffer(Item item)
	{
		this.Refresh();
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0003B748 File Offset: 0x00039948
	private void Refresh()
	{
		int count = PlayerStorage.IncomingItemBuffer.Count;
		this.tmp.text = string.Format("{0}", count);
		if (this.setActiveFalseWhenCountIsZero)
		{
			base.gameObject.SetActive(count > 0);
		}
	}

	// Token: 0x04000C4E RID: 3150
	[SerializeField]
	private TextMeshPro tmp;

	// Token: 0x04000C4F RID: 3151
	[SerializeField]
	private bool setActiveFalseWhenCountIsZero;
}
