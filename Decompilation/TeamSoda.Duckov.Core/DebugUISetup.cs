using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class DebugUISetup : MonoBehaviour
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0003AC69 File Offset: 0x00038E69
	private CharacterMainControl Character
	{
		get
		{
			return LevelManager.Instance.MainCharacter;
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0003AC75 File Offset: 0x00038E75
	private Item CharacterItem
	{
		get
		{
			return this.Character.CharacterItem;
		}
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0003AC82 File Offset: 0x00038E82
	public void Setup()
	{
		this.slotCollectionDisplay.Setup(this.CharacterItem, false);
		this.inventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, false, null);
	}

	// Token: 0x04000C27 RID: 3111
	[SerializeField]
	private ItemSlotCollectionDisplay slotCollectionDisplay;

	// Token: 0x04000C28 RID: 3112
	[SerializeField]
	private InventoryDisplay inventoryDisplay;
}
