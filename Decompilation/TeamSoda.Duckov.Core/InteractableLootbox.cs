using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000DB RID: 219
public class InteractableLootbox : InteractableBase
{
	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001F0A7 File Offset: 0x0001D2A7
	public bool ShowSortButton
	{
		get
		{
			return this.showSortButton;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001F0AF File Offset: 0x0001D2AF
	public bool UsePages
	{
		get
		{
			return this.usePages;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001F0B7 File Offset: 0x0001D2B7
	public static Transform LootBoxInventoriesParent
	{
		get
		{
			return LevelManager.LootBoxInventoriesParent;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001F0BE File Offset: 0x0001D2BE
	public static Dictionary<int, Inventory> Inventories
	{
		get
		{
			return LevelManager.LootBoxInventories;
		}
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0001F0C8 File Offset: 0x0001D2C8
	public static Inventory GetOrCreateInventory(InteractableLootbox lootBox)
	{
		if (lootBox == null)
		{
			if (CharacterMainControl.Main != null)
			{
				CharacterMainControl.Main.PopText("ERROR:尝试创建Inventory, 但lootbox是null", -1f);
			}
			Debug.LogError("尝试创建Inventory, 但lootbox是null");
			return null;
		}
		int key = lootBox.GetKey();
		Inventory inventory;
		if (InteractableLootbox.Inventories.TryGetValue(key, out inventory))
		{
			if (!(inventory == null))
			{
				return inventory;
			}
			CharacterMainControl.Main.PopText(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key), -1f);
			Debug.LogError(string.Format("Inventory缓存字典里有Key: {0}, 但其对应值为null.重新创建Inventory。", key));
		}
		GameObject gameObject = new GameObject(string.Format("Inventory_{0}", key));
		gameObject.transform.SetParent(InteractableLootbox.LootBoxInventoriesParent);
		gameObject.transform.position = lootBox.transform.position;
		inventory = gameObject.AddComponent<Inventory>();
		inventory.NeedInspection = lootBox.needInspect;
		InteractableLootbox.Inventories.Add(key, inventory);
		LootBoxLoader component = lootBox.GetComponent<LootBoxLoader>();
		if (component && component.autoSetup)
		{
			component.Setup().Forget();
		}
		return inventory;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return vector3Int.GetHashCode();
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001F23C File Offset: 0x0001D43C
	public Inventory Inventory
	{
		get
		{
			Inventory orCreateInventory;
			if (this.inventoryReference)
			{
				orCreateInventory = this.inventoryReference;
			}
			else
			{
				orCreateInventory = InteractableLootbox.GetOrCreateInventory(this);
				if (orCreateInventory == null)
				{
					if (LevelManager.Instance == null)
					{
						Debug.Log("LevelManager.Instance 不存在，取消创建i nventory");
						return null;
					}
					LevelManager.Instance.MainCharacter.PopText("空的Inventory", -1f);
					Debug.LogError("未能成功创建Inventory," + base.gameObject.name, this);
				}
				this.inventoryReference = orCreateInventory;
			}
			if (this.inventoryReference && this.inventoryReference.hasBeenInspectedInLootBox)
			{
				base.SetMarkerUsed();
			}
			orCreateInventory.DisplayNameKey = this.displayNameKey;
			return orCreateInventory;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001F2F2 File Offset: 0x0001D4F2
	public bool Looted
	{
		get
		{
			return LootView.HasInventoryEverBeenLooted(this.Inventory);
		}
	}

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060006F0 RID: 1776 RVA: 0x0001F300 File Offset: 0x0001D500
	// (remove) Token: 0x060006F1 RID: 1777 RVA: 0x0001F334 File Offset: 0x0001D534
	public static event Action<InteractableLootbox> OnStartLoot;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060006F2 RID: 1778 RVA: 0x0001F368 File Offset: 0x0001D568
	// (remove) Token: 0x060006F3 RID: 1779 RVA: 0x0001F39C File Offset: 0x0001D59C
	public static event Action<InteractableLootbox> OnStopLoot;

	// Token: 0x060006F4 RID: 1780 RVA: 0x0001F3D0 File Offset: 0x0001D5D0
	protected override void Start()
	{
		base.Start();
		if (this.inventoryReference == null)
		{
			InteractableLootbox.GetOrCreateInventory(this);
		}
		if (this.Inventory && this.Inventory.hasBeenInspectedInLootBox)
		{
			base.SetMarkerUsed();
		}
		this.overrideInteractName = true;
		base.InteractName = this.displayNameKey;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0001F42B File Offset: 0x0001D62B
	protected override bool IsInteractable()
	{
		if (this.Inventory == null)
		{
			if (CharacterMainControl.Main)
			{
				CharacterMainControl.Main.PopText("ERROR :( 存在不包含Inventory的Lootbox。", -1f);
			}
			return false;
		}
		return this.lootState == InteractableLootbox.LootBoxStates.closed;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0001F468 File Offset: 0x0001D668
	protected override void OnUpdate(CharacterMainControl interactCharacter, float deltaTime)
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			if (LootView.Instance && LootView.Instance.open)
			{
				LootView.Instance.Close();
			}
			return;
		}
		switch (this.lootState)
		{
		case InteractableLootbox.LootBoxStates.closed:
			base.StopInteract();
			return;
		case InteractableLootbox.LootBoxStates.openning:
			if (interactCharacter.CurrentAction.ActionTimer >= base.InteractTime && !this.Inventory.Loading)
			{
				if (this.StartLoot())
				{
					this.lootState = InteractableLootbox.LootBoxStates.looting;
					return;
				}
				CharacterMainControl.Main.PopText("ERROR :Start loot失败，终止交互。", -1f);
				base.StopInteract();
				this.lootState = InteractableLootbox.LootBoxStates.closed;
				return;
			}
			break;
		case InteractableLootbox.LootBoxStates.looting:
			if (!LootView.Instance || !LootView.Instance.open)
			{
				CharacterMainControl.Main.PopText("ERROR :打开Loot界面失败，终止交互。", -1f);
				base.StopInteract();
				return;
			}
			if (this.inspectingItem != null)
			{
				this.inspectTimer += deltaTime;
				if (this.inspectTimer >= this.inspectTime)
				{
					this.inspectingItem.Inspected = true;
					this.inspectingItem.Inspecting = false;
				}
				if (!this.inspectingItem.Inspecting)
				{
					this.inspectingItem = null;
					return;
				}
			}
			else
			{
				Item item = this.FindFistUninspectedItem();
				if (!item)
				{
					base.StopInteract();
					return;
				}
				this.StartInspectItem(item);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0001F5CC File Offset: 0x0001D7CC
	private void StartInspectItem(Item item)
	{
		if (item == null)
		{
			return;
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.inspectingItem = item;
		this.inspectingItem.Inspecting = true;
		this.inspectTimer = 0f;
		this.inspectTime = GameplayDataSettings.LootingData.GetInspectingTime(item);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0001F627 File Offset: 0x0001D827
	private void UpdateInspect()
	{
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0001F62C File Offset: 0x0001D82C
	private Item FindFistUninspectedItem()
	{
		if (!this.Inventory)
		{
			return null;
		}
		if (!this.Inventory.NeedInspection)
		{
			return null;
		}
		return this.Inventory.FirstOrDefault((Item e) => !e.Inspected);
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0001F681 File Offset: 0x0001D881
	protected override void OnInteractStart(CharacterMainControl interactCharacter)
	{
		this.lootState = InteractableLootbox.LootBoxStates.openning;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0001F68C File Offset: 0x0001D88C
	protected override void OnInteractStop()
	{
		this.lootState = InteractableLootbox.LootBoxStates.closed;
		Action<InteractableLootbox> onStopLoot = InteractableLootbox.OnStopLoot;
		if (onStopLoot != null)
		{
			onStopLoot(this);
		}
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		if (this.Inventory)
		{
			this.Inventory.hasBeenInspectedInLootBox = true;
		}
		base.SetMarkerUsed();
		this.CheckHideIfEmpty();
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0001F6F0 File Offset: 0x0001D8F0
	protected override void OnInteractFinished()
	{
		base.OnInteractFinished();
		if (this.inspectingItem != null)
		{
			this.inspectingItem.Inspecting = false;
		}
		this.CheckHideIfEmpty();
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0001F718 File Offset: 0x0001D918
	public void CheckHideIfEmpty()
	{
		if (!this.hideIfEmpty)
		{
			return;
		}
		if (this.Inventory.IsEmpty())
		{
			this.hideIfEmpty.gameObject.SetActive(false);
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0001F746 File Offset: 0x0001D946
	private bool StartLoot()
	{
		if (this.Inventory == null)
		{
			base.StopInteract();
			Debug.LogError("开始loot失败，缺少inventory。");
			return false;
		}
		Action<InteractableLootbox> onStartLoot = InteractableLootbox.OnStartLoot;
		if (onStartLoot != null)
		{
			onStartLoot(this);
		}
		return true;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0001F77C File Offset: 0x0001D97C
	private void CreateLocalInventory()
	{
		Inventory inventory = base.gameObject.AddComponent<Inventory>();
		this.inventoryReference = inventory;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001F79C File Offset: 0x0001D99C
	public static InteractableLootbox Prefab
	{
		get
		{
			GameplayDataSettings.PrefabsData prefabs = GameplayDataSettings.Prefabs;
			if (prefabs == null)
			{
				return null;
			}
			return prefabs.LootBoxPrefab;
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0001F7B0 File Offset: 0x0001D9B0
	public static InteractableLootbox CreateFromItem(Item item, Vector3 position, Quaternion rotation, bool moveToMainScene = true, InteractableLootbox prefab = null, bool filterDontDropOnDead = false)
	{
		if (item == null)
		{
			Debug.LogError("正在尝试给一个不存在的Item创建LootBox，已取消。");
			return null;
		}
		if (prefab == null)
		{
			prefab = InteractableLootbox.Prefab;
		}
		if (prefab == null)
		{
			Debug.LogError("未配置LootBox的Prefab");
			return null;
		}
		InteractableLootbox interactableLootbox = UnityEngine.Object.Instantiate<InteractableLootbox>(prefab, position, rotation);
		interactableLootbox.CreateLocalInventory();
		if (moveToMainScene)
		{
			MultiSceneCore.MoveToActiveWithScene(interactableLootbox.gameObject, SceneManager.GetActiveScene().buildIndex);
		}
		Inventory inventory = interactableLootbox.Inventory;
		if (inventory == null)
		{
			Debug.LogError("LootBox未配置Inventory");
			return interactableLootbox;
		}
		inventory.SetCapacity(512);
		List<Item> list = new List<Item>();
		if (item.Slots != null)
		{
			foreach (Slot slot in item.Slots)
			{
				Item content = slot.Content;
				if (!(content == null))
				{
					content.Inspected = true;
					if (content.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
					{
						content.DestroyTree();
					}
					if (!filterDontDropOnDead || (!content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot) && !content.Sticky))
					{
						list.Add(content);
					}
				}
			}
		}
		if (item.Inventory != null)
		{
			foreach (Item item2 in item.Inventory)
			{
				if (!(item2 == null) && !item2.Tags.Contains(GameplayDataSettings.Tags.DestroyOnLootBox))
				{
					list.Add(item2);
				}
			}
		}
		foreach (Item item3 in list)
		{
			item3.Detach();
			inventory.AddAndMerge(item3, 0);
		}
		int capacity = Mathf.Max(8, inventory.GetLastItemPosition() + 1);
		inventory.SetCapacity(capacity);
		inventory.NeedInspection = prefab.needInspect;
		return interactableLootbox;
	}

	// Token: 0x0400069B RID: 1691
	public bool useDefaultInteractName;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private bool showSortButton;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private bool usePages;

	// Token: 0x0400069E RID: 1694
	public bool needInspect = true;

	// Token: 0x0400069F RID: 1695
	public bool showPickAllButton = true;

	// Token: 0x040006A0 RID: 1696
	public Transform hideIfEmpty;

	// Token: 0x040006A1 RID: 1697
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayNameKey;

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	private Inventory inventoryReference;

	// Token: 0x040006A3 RID: 1699
	private Item inspectingItem;

	// Token: 0x040006A4 RID: 1700
	private float inspectTime = 1f;

	// Token: 0x040006A5 RID: 1701
	private float inspectTimer;

	// Token: 0x040006A6 RID: 1702
	private InteractableLootbox.LootBoxStates lootState;

	// Token: 0x02000463 RID: 1123
	public enum LootBoxStates
	{
		// Token: 0x04001B20 RID: 6944
		closed,
		// Token: 0x04001B21 RID: 6945
		openning,
		// Token: 0x04001B22 RID: 6946
		looting
	}
}
