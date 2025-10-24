using System;
using Duckov.MiniGames;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class GamingConsoleView : View
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00037246 File Offset: 0x00035446
	public static GamingConsoleView Instance
	{
		get
		{
			return View.GetViewInstance<GamingConsoleView>();
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00037250 File Offset: 0x00035450
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.Setup(this.target);
		if (CharacterMainControl.Main)
		{
			this.characterInventory.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, null);
		}
		if (PetProxy.PetInventory)
		{
			this.petInventory.Setup(PetProxy.PetInventory, null, null, false, null);
		}
		if (PlayerStorage.Inventory)
		{
			this.storageInventory.Setup(PlayerStorage.Inventory, null, null, false, null);
		}
		this.RefreshConsole();
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x000372EA File Offset: 0x000354EA
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x00037300 File Offset: 0x00035500
	private void SetTarget(GamingConsole target)
	{
		if (this.target != null)
		{
			this.target.onContentChanged -= this.OnTargetContentChanged;
		}
		if (target != null)
		{
			this.target = target;
			return;
		}
		this.target = UnityEngine.Object.FindObjectOfType<GamingConsole>();
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x00037350 File Offset: 0x00035550
	private void Setup(GamingConsole target)
	{
		this.SetTarget(target);
		if (this.target == null)
		{
			return;
		}
		this.target.onContentChanged += this.OnTargetContentChanged;
		this.consoleSlotDisplay.Setup(this.target.ConsoleSlot);
		this.monitorSlotDisplay.Setup(this.target.MonitorSlot);
		this.RefreshConsole();
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x000373BC File Offset: 0x000355BC
	private void OnTargetContentChanged(GamingConsole console)
	{
		this.RefreshConsole();
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x000373C4 File Offset: 0x000355C4
	private void RefreshConsole()
	{
		if (this.isBeingDestroyed)
		{
			return;
		}
		Slot consoleSlot = this.target.ConsoleSlot;
		if (consoleSlot == null)
		{
			return;
		}
		Item content = consoleSlot.Content;
		this.consoleSlotCollectionDisplay.gameObject.SetActive(content);
		if (content)
		{
			this.consoleSlotCollectionDisplay.Setup(content, false);
		}
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0003741C File Offset: 0x0003561C
	internal static void Show(GamingConsole console)
	{
		GamingConsoleView.Instance.target = console;
		GamingConsoleView.Instance.Open(null);
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x00037434 File Offset: 0x00035634
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.isBeingDestroyed = true;
	}

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private InventoryDisplay characterInventory;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private InventoryDisplay petInventory;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private InventoryDisplay storageInventory;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private SlotDisplay monitorSlotDisplay;

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private SlotDisplay consoleSlotDisplay;

	// Token: 0x04000B5E RID: 2910
	[SerializeField]
	private ItemSlotCollectionDisplay consoleSlotCollectionDisplay;

	// Token: 0x04000B5F RID: 2911
	private GamingConsole target;

	// Token: 0x04000B60 RID: 2912
	private bool isBeingDestroyed;
}
