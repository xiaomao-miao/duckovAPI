using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class PlayerStorage : MonoBehaviour, IInitializedQueryHandler
{
	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x0002487A File Offset: 0x00022A7A
	// (set) Token: 0x06000826 RID: 2086 RVA: 0x00024881 File Offset: 0x00022A81
	public static PlayerStorage Instance { get; private set; }

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06000827 RID: 2087 RVA: 0x0002488C File Offset: 0x00022A8C
	// (remove) Token: 0x06000828 RID: 2088 RVA: 0x000248C0 File Offset: 0x00022AC0
	public static event Action<PlayerStorage, Inventory, int> OnPlayerStorageChange;

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000829 RID: 2089 RVA: 0x000248F3 File Offset: 0x00022AF3
	public static Inventory Inventory
	{
		get
		{
			if (PlayerStorage.Instance == null)
			{
				return null;
			}
			return PlayerStorage.Instance.inventory;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600082A RID: 2090 RVA: 0x0002490E File Offset: 0x00022B0E
	public static List<ItemTreeData> IncomingItemBuffer
	{
		get
		{
			return PlayerStorageBuffer.Buffer;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x00024915 File Offset: 0x00022B15
	public InteractableLootbox InteractableLootBox
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x0600082C RID: 2092 RVA: 0x00024920 File Offset: 0x00022B20
	// (remove) Token: 0x0600082D RID: 2093 RVA: 0x00024954 File Offset: 0x00022B54
	public static event Action<PlayerStorage.StorageCapacityCalculationHolder> OnRecalculateStorageCapacity;

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x0600082E RID: 2094 RVA: 0x00024988 File Offset: 0x00022B88
	// (remove) Token: 0x0600082F RID: 2095 RVA: 0x000249BC File Offset: 0x00022BBC
	public static event Action OnTakeBufferItem;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000830 RID: 2096 RVA: 0x000249F0 File Offset: 0x00022BF0
	// (remove) Token: 0x06000831 RID: 2097 RVA: 0x00024A24 File Offset: 0x00022C24
	public static event Action<Item> OnItemAddedToBuffer;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06000832 RID: 2098 RVA: 0x00024A58 File Offset: 0x00022C58
	// (remove) Token: 0x06000833 RID: 2099 RVA: 0x00024A8C File Offset: 0x00022C8C
	public static event Action OnLoadingFinished;

	// Token: 0x06000834 RID: 2100 RVA: 0x00024ABF File Offset: 0x00022CBF
	public static bool IsAccessableAndNotFull()
	{
		return !(PlayerStorage.Instance == null) && !(PlayerStorage.Inventory == null) && PlayerStorage.Inventory.GetFirstEmptyPosition(0) >= 0;
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000835 RID: 2101 RVA: 0x00024AF0 File Offset: 0x00022CF0
	public int DefaultCapacity
	{
		get
		{
			return this.defaultCapacity;
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00024AF8 File Offset: 0x00022CF8
	public static void NotifyCapacityDirty()
	{
		PlayerStorage.needRecalculateCapacity = true;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00024B00 File Offset: 0x00022D00
	private void Awake()
	{
		if (PlayerStorage.Instance == null)
		{
			PlayerStorage.Instance = this;
		}
		if (PlayerStorage.Instance != this)
		{
			Debug.LogError("发现了多个Player Storage!");
			return;
		}
		if (this.interactable == null)
		{
			this.interactable = base.GetComponent<InteractableLootbox>();
		}
		this.inventory.onContentChanged += this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData += this.SavesSystem_OnCollectSaveData;
		LevelManager.RegisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00024B80 File Offset: 0x00022D80
	private void Start()
	{
		this.Load().Forget();
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00024B8D File Offset: 0x00022D8D
	private void OnDestroy()
	{
		this.inventory.onContentChanged -= this.OnInventoryContentChanged;
		SavesSystem.OnCollectSaveData -= this.SavesSystem_OnCollectSaveData;
		LevelManager.UnregisterWaitForInitialization<PlayerStorage>(this);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00024BBE File Offset: 0x00022DBE
	private void SavesSystem_OnSetFile()
	{
		this.Load().Forget();
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00024BCB File Offset: 0x00022DCB
	private void SavesSystem_OnCollectSaveData()
	{
		this.Save();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00024BD3 File Offset: 0x00022DD3
	private void OnInventoryContentChanged(Inventory inventory, int index)
	{
		Action<PlayerStorage, Inventory, int> onPlayerStorageChange = PlayerStorage.OnPlayerStorageChange;
		if (onPlayerStorageChange == null)
		{
			return;
		}
		onPlayerStorageChange(this, inventory, index);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00024BE8 File Offset: 0x00022DE8
	public static void Push(Item item, bool toBufferDirectly = false)
	{
		if (item == null)
		{
			return;
		}
		if (!toBufferDirectly && PlayerStorage.Inventory != null)
		{
			if (item.Stackable)
			{
				Func<Item, bool> <>9__0;
				while (item.StackCount > 0)
				{
					IEnumerable<Item> source = PlayerStorage.Inventory;
					Func<Item, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Item e) => e.TypeID == item.TypeID && e.MaxStackCount > e.StackCount));
					}
					Item item2 = source.FirstOrDefault(predicate);
					if (item2 == null)
					{
						break;
					}
					item2.Combine(item);
				}
			}
			if (item != null && item.StackCount > 0)
			{
				int firstEmptyPosition = PlayerStorage.Inventory.GetFirstEmptyPosition(0);
				if (firstEmptyPosition >= 0)
				{
					PlayerStorage.Inventory.AddAt(item, firstEmptyPosition);
					return;
				}
			}
		}
		NotificationText.Push("PlayerStorage_Notification_ItemAddedToBuffer".ToPlainText().Format(new
		{
			displayName = item.DisplayName
		}));
		PlayerStorage.IncomingItemBuffer.Add(ItemTreeData.FromItem(item));
		item.Detach();
		item.DestroyTree();
		Action<Item> onItemAddedToBuffer = PlayerStorage.OnItemAddedToBuffer;
		if (onItemAddedToBuffer == null)
		{
			return;
		}
		onItemAddedToBuffer(item);
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00024D26 File Offset: 0x00022F26
	private void Save()
	{
		if (PlayerStorage.Loading)
		{
			return;
		}
		this.inventory.Save("PlayerStorage");
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x00024D40 File Offset: 0x00022F40
	// (set) Token: 0x06000840 RID: 2112 RVA: 0x00024D47 File Offset: 0x00022F47
	public static bool Loading { get; private set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000841 RID: 2113 RVA: 0x00024D4F File Offset: 0x00022F4F
	// (set) Token: 0x06000842 RID: 2114 RVA: 0x00024D56 File Offset: 0x00022F56
	public static bool TakingItem { get; private set; }

	// Token: 0x06000843 RID: 2115 RVA: 0x00024D60 File Offset: 0x00022F60
	private UniTask Load()
	{
		PlayerStorage.<Load>d__52 <Load>d__;
		<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Load>d__.<>4__this = this;
		<Load>d__.<>1__state = -1;
		<Load>d__.<>t__builder.Start<PlayerStorage.<Load>d__52>(ref <Load>d__);
		return <Load>d__.<>t__builder.Task;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00024DA3 File Offset: 0x00022FA3
	private void Update()
	{
		if (PlayerStorage.needRecalculateCapacity)
		{
			PlayerStorage.RecalculateStorageCapacity();
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00024DB4 File Offset: 0x00022FB4
	public static int RecalculateStorageCapacity()
	{
		if (PlayerStorage.Instance == null)
		{
			return 0;
		}
		PlayerStorage.StorageCapacityCalculationHolder storageCapacityCalculationHolder = new PlayerStorage.StorageCapacityCalculationHolder();
		storageCapacityCalculationHolder.capacity = PlayerStorage.Instance.DefaultCapacity;
		Action<PlayerStorage.StorageCapacityCalculationHolder> onRecalculateStorageCapacity = PlayerStorage.OnRecalculateStorageCapacity;
		if (onRecalculateStorageCapacity != null)
		{
			onRecalculateStorageCapacity(storageCapacityCalculationHolder);
		}
		int capacity = storageCapacityCalculationHolder.capacity;
		PlayerStorage.Instance.SetCapacity(capacity);
		PlayerStorage.needRecalculateCapacity = false;
		return capacity;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00024E10 File Offset: 0x00023010
	private void SetCapacity(int capacity)
	{
		this.inventory.SetCapacity(capacity);
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00024E20 File Offset: 0x00023020
	public static UniTask TakeBufferItem(int index)
	{
		PlayerStorage.<TakeBufferItem>d__56 <TakeBufferItem>d__;
		<TakeBufferItem>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<TakeBufferItem>d__.index = index;
		<TakeBufferItem>d__.<>1__state = -1;
		<TakeBufferItem>d__.<>t__builder.Start<PlayerStorage.<TakeBufferItem>d__56>(ref <TakeBufferItem>d__);
		return <TakeBufferItem>d__.<>t__builder.Task;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00024E63 File Offset: 0x00023063
	public bool HasInitialized()
	{
		return this.initialized;
	}

	// Token: 0x04000771 RID: 1905
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04000772 RID: 1906
	[SerializeField]
	private InteractableLootbox interactable;

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	private int defaultCapacity = 32;

	// Token: 0x04000778 RID: 1912
	private static bool needRecalculateCapacity;

	// Token: 0x04000779 RID: 1913
	private const string inventorySaveKey = "PlayerStorage";

	// Token: 0x0400077C RID: 1916
	private bool initialized;

	// Token: 0x0200047B RID: 1147
	public class StorageCapacityCalculationHolder
	{
		// Token: 0x04001B6D RID: 7021
		public int capacity;
	}
}
