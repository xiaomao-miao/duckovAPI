using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class ItemWishlist : MonoBehaviour
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00036EA2 File Offset: 0x000350A2
	// (set) Token: 0x06000D39 RID: 3385 RVA: 0x00036EA9 File Offset: 0x000350A9
	public static ItemWishlist Instance { get; private set; }

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06000D3A RID: 3386 RVA: 0x00036EB4 File Offset: 0x000350B4
	// (remove) Token: 0x06000D3B RID: 3387 RVA: 0x00036EE8 File Offset: 0x000350E8
	public static event Action<int> OnWishlistChanged;

	// Token: 0x06000D3C RID: 3388 RVA: 0x00036F1C File Offset: 0x0003511C
	private void Awake()
	{
		ItemWishlist.Instance = this;
		QuestManager.onQuestListsChanged += this.OnQuestListChanged;
		BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		SavesSystem.OnCollectSaveData += this.Save;
		UIInputManager.OnWishlistHoveringItem += this.OnWishlistHoveringItem;
		this.Load();
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00036F79 File Offset: 0x00035179
	private void OnDestroy()
	{
		QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		SavesSystem.OnCollectSaveData -= this.Save;
		UIInputManager.OnWishlistHoveringItem -= this.OnWishlistHoveringItem;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x00036FB0 File Offset: 0x000351B0
	private void OnWishlistHoveringItem(UIInputEventData data)
	{
		if (!ItemHoveringUI.Shown)
		{
			return;
		}
		int displayingItemID = ItemHoveringUI.DisplayingItemID;
		if (this.IsManuallyWishlisted(displayingItemID))
		{
			ItemWishlist.RemoveFromWishlist(displayingItemID);
		}
		else
		{
			ItemWishlist.AddToWishList(displayingItemID);
		}
		ItemHoveringUI.NotifyRefreshWishlistInfo();
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00036FE8 File Offset: 0x000351E8
	private void Load()
	{
		this.manualWishList.Clear();
		List<int> list = SavesSystem.Load<List<int>>("ItemWishlist_Manual");
		if (list != null)
		{
			this.manualWishList.AddRange(list);
		}
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0003701A File Offset: 0x0003521A
	private void Save()
	{
		SavesSystem.Save<List<int>>("ItemWishlist_Manual", this.manualWishList);
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0003702C File Offset: 0x0003522C
	private void Start()
	{
		this.CacheQuestItems();
		this.CacheBuildingItems();
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0003703A File Offset: 0x0003523A
	private void OnQuestListChanged(QuestManager obj)
	{
		this.CacheQuestItems();
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00037042 File Offset: 0x00035242
	private void OnBuildingListChanged()
	{
		this.CacheBuildingItems();
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0003704A File Offset: 0x0003524A
	private void CacheQuestItems()
	{
		this._questRequiredItems = QuestManager.GetAllRequiredItems().ToHashSet<int>();
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0003705C File Offset: 0x0003525C
	private void CacheBuildingItems()
	{
		this._buildingRequiredItems.Clear();
		foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
		{
			if (buildingInfo.RequirementsSatisfied() && buildingInfo.TokenAmount + buildingInfo.CurrentAmount < buildingInfo.maxAmount)
			{
				foreach (Cost.ItemEntry itemEntry in buildingInfo.cost.items)
				{
					this._buildingRequiredItems.Add(itemEntry.id);
				}
			}
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x000370EB File Offset: 0x000352EB
	private IEnumerable<int> IterateAll()
	{
		foreach (int num in this.manualWishList)
		{
			yield return num;
		}
		List<int>.Enumerator enumerator = default(List<int>.Enumerator);
		IEnumerable<int> allRequiredItems = QuestManager.GetAllRequiredItems();
		foreach (int num2 in allRequiredItems)
		{
			yield return num2;
		}
		IEnumerator<int> enumerator2 = null;
		yield break;
		yield break;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x000370FB File Offset: 0x000352FB
	public bool IsQuestRequired(int itemTypeID)
	{
		return this._questRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00037109 File Offset: 0x00035309
	public bool IsManuallyWishlisted(int itemTypeID)
	{
		return this.manualWishList.Contains(itemTypeID);
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00037117 File Offset: 0x00035317
	public bool IsBuildingRequired(int itemTypeID)
	{
		return this._buildingRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00037128 File Offset: 0x00035328
	public static void AddToWishList(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return;
		}
		if (ItemWishlist.Instance.manualWishList.Contains(itemTypeID))
		{
			return;
		}
		ItemWishlist.Instance.manualWishList.Add(itemTypeID);
		Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
		if (onWishlistChanged == null)
		{
			return;
		}
		onWishlistChanged(itemTypeID);
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x00037176 File Offset: 0x00035376
	public static bool RemoveFromWishlist(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return false;
		}
		bool flag = ItemWishlist.Instance.manualWishList.Remove(itemTypeID);
		if (flag)
		{
			Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
			if (onWishlistChanged == null)
			{
				return flag;
			}
			onWishlistChanged(itemTypeID);
		}
		return flag;
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x000371AC File Offset: 0x000353AC
	public static ItemWishlist.WishlistInfo GetWishlistInfo(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return default(ItemWishlist.WishlistInfo);
		}
		bool isManuallyWishlisted = ItemWishlist.Instance.IsManuallyWishlisted(itemTypeID);
		bool isQuestRequired = ItemWishlist.Instance.IsQuestRequired(itemTypeID);
		bool isBuildingRequired = ItemWishlist.Instance.IsBuildingRequired(itemTypeID);
		return new ItemWishlist.WishlistInfo
		{
			itemTypeID = itemTypeID,
			isManuallyWishlisted = isManuallyWishlisted,
			isQuestRequired = isQuestRequired,
			isBuildingRequired = isBuildingRequired
		};
	}

	// Token: 0x04000B53 RID: 2899
	private List<int> manualWishList = new List<int>();

	// Token: 0x04000B54 RID: 2900
	private HashSet<int> _questRequiredItems = new HashSet<int>();

	// Token: 0x04000B55 RID: 2901
	private HashSet<int> _buildingRequiredItems = new HashSet<int>();

	// Token: 0x04000B57 RID: 2903
	private const string SaveKey = "ItemWishlist_Manual";

	// Token: 0x020004D0 RID: 1232
	public struct WishlistInfo
	{
		// Token: 0x04001CDD RID: 7389
		public int itemTypeID;

		// Token: 0x04001CDE RID: 7390
		public bool isManuallyWishlisted;

		// Token: 0x04001CDF RID: 7391
		public bool isQuestRequired;

		// Token: 0x04001CE0 RID: 7392
		public bool isBuildingRequired;
	}
}
