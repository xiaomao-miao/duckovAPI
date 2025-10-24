using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class ItemStarterkit : InteractableBase
{
	// Token: 0x0600070D RID: 1805 RVA: 0x0001FC3B File Offset: 0x0001DE3B
	protected override bool IsInteractable()
	{
		return !this.picked && this.cached;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0001FC54 File Offset: 0x0001DE54
	private UniTask CacheItems()
	{
		ItemStarterkit.<CacheItems>d__10 <CacheItems>d__;
		<CacheItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CacheItems>d__.<>4__this = this;
		<CacheItems>d__.<>1__state = -1;
		<CacheItems>d__.<>t__builder.Start<ItemStarterkit.<CacheItems>d__10>(ref <CacheItems>d__);
		return <CacheItems>d__.<>t__builder.Task;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0001FC97 File Offset: 0x0001DE97
	protected override void Awake()
	{
		base.Awake();
		SavesSystem.OnCollectSaveData += this.Save;
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0001FCC1 File Offset: 0x0001DEC1
	protected override void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		base.OnDestroy();
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0001FCEB File Offset: 0x0001DEEB
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		this.picked = false;
		this.Save();
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0001FCFA File Offset: 0x0001DEFA
	private void Save()
	{
		SavesSystem.Save<bool>(this.saveKey, this.picked);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0001FD10 File Offset: 0x0001DF10
	private void Load()
	{
		this.picked = SavesSystem.Load<bool>(this.saveKey);
		base.MarkerActive = !this.picked;
		if (this.notPickedItem)
		{
			GameObject gameObject = this.notPickedItem;
			if (gameObject != null)
			{
				gameObject.SetActive(!this.picked);
			}
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(this.picked);
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0001FD82 File Offset: 0x0001DF82
	protected override void Start()
	{
		base.Start();
		this.Load();
		if (!this.picked)
		{
			this.CacheItems().Forget();
		}
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
	protected override void OnInteractFinished()
	{
		foreach (Item item in this.itemsCache)
		{
			ItemUtilities.SendToPlayerCharacter(item, false);
		}
		this.picked = true;
		base.MarkerActive = !this.picked;
		this.itemsCache.Clear();
		this.OnPicked();
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0001FE20 File Offset: 0x0001E020
	private void OnPicked()
	{
		if (this.notPickedItem)
		{
			this.notPickedItem.SetActive(false);
		}
		if (this.pickedItem)
		{
			this.pickedItem.SetActive(true);
		}
		if (this.pickFX)
		{
			this.pickFX.SetActive(true);
		}
		NotificationText.Push(this.notificationTextKey.ToPlainText());
	}

	// Token: 0x040006B1 RID: 1713
	[ItemTypeID]
	[SerializeField]
	private List<int> items;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private GameObject notPickedItem;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	private GameObject pickedItem;

	// Token: 0x040006B4 RID: 1716
	[SerializeField]
	private GameObject pickFX;

	// Token: 0x040006B5 RID: 1717
	private List<Item> itemsCache;

	// Token: 0x040006B6 RID: 1718
	[SerializeField]
	private string notificationTextKey;

	// Token: 0x040006B7 RID: 1719
	private bool caching;

	// Token: 0x040006B8 RID: 1720
	private bool cached;

	// Token: 0x040006B9 RID: 1721
	private bool picked;

	// Token: 0x040006BA RID: 1722
	private string saveKey = "StarterKit_Picked";
}
