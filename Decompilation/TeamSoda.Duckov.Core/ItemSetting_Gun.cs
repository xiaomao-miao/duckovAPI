using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class ItemSetting_Gun : ItemSettingBase
{
	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060007DD RID: 2013 RVA: 0x00023179 File Offset: 0x00021379
	public int TargetBulletID
	{
		get
		{
			return this.targetBulletID;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060007DE RID: 2014 RVA: 0x00023184 File Offset: 0x00021384
	public string CurrentBulletName
	{
		get
		{
			if (this.TargetBulletID < 0)
			{
				return "UI_Bullet_NotAssigned".ToPlainText();
			}
			return ItemAssetsCollection.GetMetaData(this.TargetBulletID).DisplayName;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x000231B8 File Offset: 0x000213B8
	public int BulletCount
	{
		get
		{
			if (this.loadingBullets)
			{
				return -1;
			}
			if (this.bulletCount < 0)
			{
				this.bulletCount = this.GetBulletCount();
			}
			return this.bulletCount;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00023204 File Offset: 0x00021404
	// (set) Token: 0x060007E0 RID: 2016 RVA: 0x000231DF File Offset: 0x000213DF
	private int bulletCount
	{
		get
		{
			return this._bulletCountCache;
		}
		set
		{
			this._bulletCountCache = value;
			base.Item.Variables.SetInt(this.bulletCountHash, this._bulletCountCache);
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0002320C File Offset: 0x0002140C
	public int Capacity
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemSetting_Gun.CapacityHash));
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00023223 File Offset: 0x00021423
	public bool LoadingBullets
	{
		get
		{
			return this.loadingBullets;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0002322B File Offset: 0x0002142B
	public bool LoadBulletsSuccess
	{
		get
		{
			return this.loadBulletsSuccess;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002323C File Offset: 0x0002143C
	// (set) Token: 0x060007E5 RID: 2021 RVA: 0x00023233 File Offset: 0x00021433
	public Item PreferdBulletsToLoad
	{
		get
		{
			return this.preferedBulletsToLoad;
		}
		set
		{
			this.preferedBulletsToLoad = value;
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00023244 File Offset: 0x00021444
	public void SetTargetBulletType(Item bulletItem)
	{
		if (bulletItem != null)
		{
			this.SetTargetBulletType(bulletItem.TypeID);
			return;
		}
		this.SetTargetBulletType(-1);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00023264 File Offset: 0x00021464
	public void SetTargetBulletType(int typeID)
	{
		bool flag = false;
		if (this.TargetBulletID != typeID && this.TargetBulletID != -1)
		{
			flag = true;
		}
		this.targetBulletID = typeID;
		if (flag)
		{
			this.TakeOutAllBullets();
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00023297 File Offset: 0x00021497
	public override void Start()
	{
		base.Start();
		this.AutoSetTypeInInventory(null);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x000232A8 File Offset: 0x000214A8
	public void UseABullet()
	{
		if (LevelManager.Instance.IsBaseLevel)
		{
			return;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null) && item.StackCount >= 1)
			{
				item.StackCount--;
				break;
			}
		}
		this.bulletCount--;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00023330 File Offset: 0x00021530
	public bool IsFull()
	{
		return this.bulletCount >= this.Capacity;
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00023344 File Offset: 0x00021544
	public bool IsValidBullet(Item newBulletItem)
	{
		if (newBulletItem == null)
		{
			return false;
		}
		if (!newBulletItem.Tags.Contains(GameplayDataSettings.Tags.Bullet))
		{
			return false;
		}
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null && currentLoadedBullet.TypeID == newBulletItem.TypeID && this.bulletCount >= this.Capacity)
		{
			return false;
		}
		string @string = newBulletItem.Constants.GetString(this.caliberHash, null);
		string string2 = base.Item.Constants.GetString(this.caliberHash, null);
		return !(@string != string2);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x000233D8 File Offset: 0x000215D8
	public bool LoadSpecificBullet(Item newBulletItem)
	{
		Debug.Log("尝试安装指定弹药");
		if (!this.IsValidBullet(newBulletItem))
		{
			return false;
		}
		Debug.Log("指定弹药判定通过");
		ItemAgent_Gun itemAgent_Gun = base.Item.ActiveAgent as ItemAgent_Gun;
		if (!(itemAgent_Gun != null))
		{
			Inventory inventory = base.Item.InInventory;
			if (inventory != null && inventory != CharacterMainControl.Main.CharacterItem.Inventory)
			{
				inventory = null;
			}
			this.preferedBulletsToLoad = newBulletItem;
			this.LoadBulletsFromInventory(inventory).Forget();
			return true;
		}
		if (itemAgent_Gun.Holder != null)
		{
			bool flag = itemAgent_Gun.CharacterReload(newBulletItem);
			Debug.Log(string.Format("角色reload:{0}", flag));
			return true;
		}
		return false;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00023494 File Offset: 0x00021694
	public UniTaskVoid LoadBulletsFromInventory(Inventory inventory)
	{
		ItemSetting_Gun.<LoadBulletsFromInventory>d__45 <LoadBulletsFromInventory>d__;
		<LoadBulletsFromInventory>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<LoadBulletsFromInventory>d__.<>4__this = this;
		<LoadBulletsFromInventory>d__.inventory = inventory;
		<LoadBulletsFromInventory>d__.<>1__state = -1;
		<LoadBulletsFromInventory>d__.<>t__builder.Start<ItemSetting_Gun.<LoadBulletsFromInventory>d__45>(ref <LoadBulletsFromInventory>d__);
		return <LoadBulletsFromInventory>d__.<>t__builder.Task;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x000234E0 File Offset: 0x000216E0
	public bool AutoSetTypeInInventory(Inventory inventory)
	{
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		Item currentLoadedBullet = this.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null)
		{
			this.SetTargetBulletType(currentLoadedBullet);
			return false;
		}
		if (inventory == null)
		{
			return false;
		}
		foreach (Item item in inventory)
		{
			if (item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				this.SetTargetBulletType(item);
				break;
			}
		}
		return this.targetBulletID != -1;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002359C File Offset: 0x0002179C
	public int GetBulletCount()
	{
		int num = 0;
		if (base.Item == null)
		{
			return 0;
		}
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0002360C File Offset: 0x0002180C
	public Item GetCurrentLoadedBullet()
	{
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				return item;
			}
		}
		return null;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00023668 File Offset: 0x00021868
	public int GetBulletCountofTypeInInventory(int bulletItemTypeID, Inventory inventory)
	{
		if (this.targetBulletID == -1)
		{
			return 0;
		}
		int num = 0;
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.TypeID == bulletItemTypeID)
			{
				num += item.StackCount;
			}
		}
		return num;
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000236D4 File Offset: 0x000218D4
	public void TakeOutAllBullets()
	{
		if (base.Item == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		foreach (Item item in base.Item.Inventory)
		{
			if (!(item == null))
			{
				list.Add(item);
			}
		}
		CharacterMainControl characterMainControl = base.Item.GetCharacterMainControl();
		if (base.Item.InInventory && LevelManager.Instance && base.Item.InInventory == LevelManager.Instance.PetProxy.Inventory)
		{
			characterMainControl = LevelManager.Instance.MainCharacter;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Item item2 = list[i];
			if (!(item2 == null))
			{
				if (characterMainControl)
				{
					item2.Drop(characterMainControl, true);
					characterMainControl.PickupItem(item2);
				}
				else
				{
					bool flag = false;
					Inventory inInventory = base.Item.InInventory;
					if (inInventory)
					{
						flag = inInventory.AddAndMerge(item2, 0);
					}
					if (!flag)
					{
						item2.Detach();
						item2.DestroyTree();
					}
				}
			}
		}
		this.bulletCount = 0;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00023820 File Offset: 0x00021A20
	public Dictionary<int, BulletTypeInfo> GetBulletTypesInInventory(Inventory inventory)
	{
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		string @string = base.Item.Constants.GetString(this.caliberHash, null);
		foreach (Item item in inventory)
		{
			if (!(item == null) && item.GetBool("IsBullet", false) && !(item.Constants.GetString(this.caliberHash, null) != @string))
			{
				if (!dictionary.ContainsKey(item.TypeID))
				{
					BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
					bulletTypeInfo.bulletTypeID = item.TypeID;
					bulletTypeInfo.count = item.StackCount;
					dictionary.Add(bulletTypeInfo.bulletTypeID, bulletTypeInfo);
				}
				else
				{
					dictionary[item.TypeID].count += item.StackCount;
				}
			}
		}
		return dictionary;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00023918 File Offset: 0x00021B18
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsGun", true, true);
	}

	// Token: 0x04000754 RID: 1876
	private int targetBulletID = -1;

	// Token: 0x04000755 RID: 1877
	public ADSAimMarker adsAimMarker;

	// Token: 0x04000756 RID: 1878
	public GameObject muzzleFxPfb;

	// Token: 0x04000757 RID: 1879
	public Projectile bulletPfb;

	// Token: 0x04000758 RID: 1880
	public string shootKey = "Default";

	// Token: 0x04000759 RID: 1881
	public string reloadKey = "Default";

	// Token: 0x0400075A RID: 1882
	private int bulletCountHash = "BulletCount".GetHashCode();

	// Token: 0x0400075B RID: 1883
	private int _bulletCountCache = -1;

	// Token: 0x0400075C RID: 1884
	private static int CapacityHash = "Capacity".GetHashCode();

	// Token: 0x0400075D RID: 1885
	private bool loadingBullets;

	// Token: 0x0400075E RID: 1886
	private bool loadBulletsSuccess;

	// Token: 0x0400075F RID: 1887
	private int caliberHash = "Caliber".GetHashCode();

	// Token: 0x04000760 RID: 1888
	public ItemSetting_Gun.TriggerModes triggerMode;

	// Token: 0x04000761 RID: 1889
	public ItemSetting_Gun.ReloadModes reloadMode;

	// Token: 0x04000762 RID: 1890
	public bool autoReload;

	// Token: 0x04000763 RID: 1891
	public ElementTypes element;

	// Token: 0x04000764 RID: 1892
	public Buff buff;

	// Token: 0x04000765 RID: 1893
	private Item preferedBulletsToLoad;

	// Token: 0x0200046B RID: 1131
	public enum TriggerModes
	{
		// Token: 0x04001B43 RID: 6979
		auto,
		// Token: 0x04001B44 RID: 6980
		semi,
		// Token: 0x04001B45 RID: 6981
		bolt
	}

	// Token: 0x0200046C RID: 1132
	public enum ReloadModes
	{
		// Token: 0x04001B47 RID: 6983
		fullMag,
		// Token: 0x04001B48 RID: 6984
		singleBullet
	}
}
