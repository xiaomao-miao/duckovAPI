using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using Saves;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class SavedInventory : MonoBehaviour
{
	// Token: 0x060007B5 RID: 1973 RVA: 0x000228AB File Offset: 0x00020AAB
	private void Awake()
	{
		if (this.inventory == null)
		{
			this.inventory = base.GetComponent<Inventory>();
		}
		this.Register();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000228CD File Offset: 0x00020ACD
	private void Start()
	{
		if (this.registered)
		{
			this.Load();
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x000228DD File Offset: 0x00020ADD
	private void OnDestroy()
	{
		this.Unregsister();
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000228E8 File Offset: 0x00020AE8
	private void Register()
	{
		SavedInventory savedInventory;
		if (SavedInventory.activeInventories.TryGetValue(this.key, out savedInventory))
		{
			Debug.LogError("存在多个带有相同Key的Saved Inventory: " + this.key, base.gameObject);
			return;
		}
		SavesSystem.OnCollectSaveData += this.Save;
		this.registered = true;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002293D File Offset: 0x00020B3D
	private void Unregsister()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00022950 File Offset: 0x00020B50
	private void Save()
	{
		this.inventory.Save(this.key);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00022963 File Offset: 0x00020B63
	private void Load()
	{
		if (this.inventory.Loading)
		{
			Debug.LogError("Inventory is already loading.", base.gameObject);
			return;
		}
		ItemSavesUtilities.LoadInventory(this.key, this.inventory).Forget();
	}

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000740 RID: 1856
	[SerializeField]
	private string key = "DefaultSavedInventory";

	// Token: 0x04000741 RID: 1857
	private static Dictionary<string, SavedInventory> activeInventories = new Dictionary<string, SavedInventory>();

	// Token: 0x04000742 RID: 1858
	private bool registered;
}
