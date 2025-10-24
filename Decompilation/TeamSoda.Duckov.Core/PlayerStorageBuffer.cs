using System;
using System.Collections.Generic;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class PlayerStorageBuffer : MonoBehaviour
{
	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00024E7B File Offset: 0x0002307B
	// (set) Token: 0x0600084B RID: 2123 RVA: 0x00024E82 File Offset: 0x00023082
	public static PlayerStorageBuffer Instance { get; private set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x00024E8A File Offset: 0x0002308A
	public static List<ItemTreeData> Buffer
	{
		get
		{
			return PlayerStorageBuffer.incomingItemBuffer;
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00024E91 File Offset: 0x00023091
	private void Awake()
	{
		PlayerStorageBuffer.Instance = this;
		PlayerStorageBuffer.LoadBuffer();
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00024EAF File Offset: 0x000230AF
	private void OnCollectSaveData()
	{
		PlayerStorageBuffer.SaveBuffer();
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00024EB8 File Offset: 0x000230B8
	public static void SaveBuffer()
	{
		List<ItemTreeData> list = new List<ItemTreeData>();
		foreach (ItemTreeData itemTreeData in PlayerStorageBuffer.incomingItemBuffer)
		{
			if (itemTreeData != null)
			{
				list.Add(itemTreeData);
			}
		}
		SavesSystem.Save<List<ItemTreeData>>("PlayerStorage_Buffer", list);
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00024F20 File Offset: 0x00023120
	public static void LoadBuffer()
	{
		PlayerStorageBuffer.incomingItemBuffer.Clear();
		List<ItemTreeData> list = SavesSystem.Load<List<ItemTreeData>>("PlayerStorage_Buffer");
		if (list != null)
		{
			if (list.Count <= 0)
			{
				Debug.Log("tree data is empty");
			}
			using (List<ItemTreeData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemTreeData item = enumerator.Current;
					PlayerStorageBuffer.incomingItemBuffer.Add(item);
				}
				return;
			}
		}
		Debug.Log("Tree Data is null");
	}

	// Token: 0x0400077E RID: 1918
	private const string bufferSaveKey = "PlayerStorage_Buffer";

	// Token: 0x0400077F RID: 1919
	private static List<ItemTreeData> incomingItemBuffer = new List<ItemTreeData>();
}
