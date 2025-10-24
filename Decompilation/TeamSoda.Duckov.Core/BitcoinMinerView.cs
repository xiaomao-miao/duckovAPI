using System;
using Duckov.Bitcoins;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001A1 RID: 417
public class BitcoinMinerView : View
{
	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00033A5C File Offset: 0x00031C5C
	public static BitcoinMinerView Instance
	{
		get
		{
			return View.GetViewInstance<BitcoinMinerView>();
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00033A63 File Offset: 0x00031C63
	// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00033A6A File Offset: 0x00031C6A
	[LocalizationKey("Default")]
	private string ActiveCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Active";
		}
		set
		{
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00033A6C File Offset: 0x00031C6C
	// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00033A73 File Offset: 0x00031C73
	[LocalizationKey("Default")]
	private string StoppedCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Stopped";
		}
		set
		{
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00033A78 File Offset: 0x00031C78
	protected override void Awake()
	{
		base.Awake();
		this.minerInventoryDisplay.onDisplayDoubleClicked += this.OnMinerInventoryEntryDoubleClicked;
		this.inventoryDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.storageDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.minerSlotsDisplay.onElementDoubleClicked += this.OnMinerSlotEntryDoubleClicked;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00033AE8 File Offset: 0x00031CE8
	private void OnMinerSlotEntryDoubleClicked(ItemSlotCollectionDisplay display1, SlotDisplay slotDisplay)
	{
		Slot target = slotDisplay.Target;
		if (target == null)
		{
			return;
		}
		Item content = target.Content;
		if (content == null)
		{
			return;
		}
		ItemUtilities.SendToPlayer(content, false, PlayerStorage.Instance != null);
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00033B24 File Offset: 0x00031D24
	private void OnPlayerItemsDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		Item item = BitcoinMiner.Instance.Item;
		if (item == null)
		{
			return;
		}
		item.TryPlug(content, true, content.InInventory, 0);
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00033B68 File Offset: 0x00031D68
	private void OnMinerInventoryEntryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		if (data.button == PointerEventData.InputButton.Left)
		{
			ItemUtilities.SendToPlayer(content, false, true);
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00033B96 File Offset: 0x00031D96
	public static void Show()
	{
		if (BitcoinMinerView.Instance == null)
		{
			return;
		}
		if (BitcoinMiner.Instance == null)
		{
			return;
		}
		BitcoinMinerView.Instance.Open(null);
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x00033BC0 File Offset: 0x00031DC0
	protected override void OnOpen()
	{
		base.OnOpen();
		CharacterMainControl main = CharacterMainControl.Main;
		if (!(main == null))
		{
			Item characterItem = main.CharacterItem;
			if (!(characterItem == null))
			{
				BitcoinMiner instance = BitcoinMiner.Instance;
				if (!instance.Loading)
				{
					Item item = instance.Item;
					if (!(item == null))
					{
						this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
						if (PlayerStorage.Inventory != null)
						{
							this.storageDisplay.gameObject.SetActive(true);
							this.storageDisplay.Setup(PlayerStorage.Inventory, null, null, false, null);
						}
						else
						{
							this.storageDisplay.gameObject.SetActive(false);
						}
						this.minerSlotsDisplay.Setup(item, false);
						this.minerInventoryDisplay.Setup(item.Inventory, null, null, false, null);
						this.fadeGroup.Show();
						return;
					}
				}
			}
		}
		Debug.Log("Failed");
		base.Close();
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00033CB4 File Offset: 0x00031EB4
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00033CC7 File Offset: 0x00031EC7
	private void FixedUpdate()
	{
		this.RefreshStatus();
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00033CD0 File Offset: 0x00031ED0
	private void RefreshStatus()
	{
		if (BitcoinMiner.Instance.WorkPerSecond > 0.0)
		{
			TimeSpan remainingTime = BitcoinMiner.Instance.RemainingTime;
			TimeSpan timePerCoin = BitcoinMiner.Instance.TimePerCoin;
			this.remainingTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTime.TotalHours), remainingTime.Minutes, remainingTime.Seconds);
			this.timeEachCoinText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)timePerCoin.TotalHours), timePerCoin.Minutes, timePerCoin.Seconds);
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
			this.commentText.text = this.ActiveCommentKey.ToPlainText();
		}
		else
		{
			this.remainingTimeText.text = "--:--:--";
			this.timeEachCoinText.text = "--:--:--";
			this.commentText.text = this.StoppedCommentKey.ToPlainText();
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
		}
		this.fill.fillAmount = BitcoinMiner.Instance.NormalizedProgress;
	}

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private InventoryDisplay inventoryDisplay;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private InventoryDisplay storageDisplay;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private ItemSlotCollectionDisplay minerSlotsDisplay;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private InventoryDisplay minerInventoryDisplay;

	// Token: 0x04000AA2 RID: 2722
	[SerializeField]
	private TextMeshProUGUI commentText;

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	private TextMeshProUGUI remainingTimeText;

	// Token: 0x04000AA4 RID: 2724
	[SerializeField]
	private TextMeshProUGUI timeEachCoinText;

	// Token: 0x04000AA5 RID: 2725
	[SerializeField]
	private TextMeshProUGUI performanceText;

	// Token: 0x04000AA6 RID: 2726
	[SerializeField]
	private Image fill;
}
