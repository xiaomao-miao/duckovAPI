using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class PetProxy : MonoBehaviour
{
	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000916 RID: 2326 RVA: 0x000285B2 File Offset: 0x000267B2
	public static PetProxy Instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.PetProxy;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x000285CD File Offset: 0x000267CD
	public static Inventory PetInventory
	{
		get
		{
			if (PetProxy.Instance == null)
			{
				return null;
			}
			return PetProxy.Instance.Inventory;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x000285E8 File Offset: 0x000267E8
	public Inventory Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000285F0 File Offset: 0x000267F0
	private void Start()
	{
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		ItemSavesUtilities.LoadInventory("Inventory_Safe", this.inventory).Forget();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00028618 File Offset: 0x00026818
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0002862B File Offset: 0x0002682B
	private void OnCollectSaveData()
	{
		this.inventory.Save("Inventory_Safe");
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00028640 File Offset: 0x00026840
	public void DestroyItemInBase()
	{
		if (!this.Inventory)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in this.Inventory)
		{
			list.Add(item);
		}
		foreach (Item item2 in list)
		{
			if (item2.Tags.Contains("DestroyInBase"))
			{
				item2.DestroyTree();
			}
		}
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000286F4 File Offset: 0x000268F4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (LevelManager.Instance.PetCharacter == null)
		{
			return;
		}
		base.transform.position = LevelManager.Instance.PetCharacter.transform.position;
		if (this.checkTimer > 0f)
		{
			this.checkTimer -= Time.unscaledDeltaTime;
			return;
		}
		if (CharacterMainControl.Main.PetCapcity != this.inventory.Capacity)
		{
			this.inventory.SetCapacity(CharacterMainControl.Main.PetCapcity);
		}
		this.checkTimer = 1f;
	}

	// Token: 0x0400082B RID: 2091
	[SerializeField]
	private Inventory inventory;

	// Token: 0x0400082C RID: 2092
	private float checkTimer = 0.02f;
}
