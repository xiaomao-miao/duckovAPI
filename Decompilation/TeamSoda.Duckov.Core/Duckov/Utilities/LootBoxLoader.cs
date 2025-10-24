using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x020003FB RID: 1019
	public class LootBoxLoader : MonoBehaviour
	{
		// Token: 0x060024F1 RID: 9457 RVA: 0x0007FADA File Offset: 0x0007DCDA
		public void CalculateChances()
		{
			this.randomPool.RefreshPercent();
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x0007FAE7 File Offset: 0x0007DCE7
		public List<int> FixedItems
		{
			get
			{
				return this.fixedItems;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0007FAEF File Offset: 0x0007DCEF
		[SerializeField]
		private Inventory Inventory
		{
			get
			{
				if (this._lootBox == null)
				{
					this._lootBox = base.GetComponent<InteractableLootbox>();
					if (this._lootBox == null)
					{
						return null;
					}
				}
				return this._lootBox.Inventory;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0007FB26 File Offset: 0x0007DD26
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0007FB2E File Offset: 0x0007DD2E
		private void Awake()
		{
			if (this._lootBox == null)
			{
				this._lootBox = base.GetComponent<InteractableLootbox>();
			}
			this.RandomActive();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0007FB50 File Offset: 0x0007DD50
		private int GetKey()
		{
			Vector3 vector = base.transform.position * 10f;
			int x = Mathf.RoundToInt(vector.x);
			int y = Mathf.RoundToInt(vector.y);
			int z = Mathf.RoundToInt(vector.z);
			Vector3Int vector3Int = new Vector3Int(x, y, z);
			return string.Format("LootBoxLoader_{0}", vector3Int).GetHashCode();
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0007FBB4 File Offset: 0x0007DDB4
		private void RandomActive()
		{
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
			{
				if (obj is bool)
				{
					bool flag2 = (bool)obj;
					flag = flag2;
				}
			}
			else
			{
				flag = (UnityEngine.Random.Range(0f, 1f) < this.activeChance);
				MultiSceneCore.Instance.inLevelData.Add(key, flag);
			}
			base.gameObject.SetActive(flag);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0007FC2B File Offset: 0x0007DE2B
		public void StartSetup()
		{
			this.Setup().Forget();
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0007FC38 File Offset: 0x0007DE38
		public UniTask Setup()
		{
			LootBoxLoader.<Setup>d__26 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootBoxLoader.<Setup>d__26>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x0007FC7C File Offset: 0x0007DE7C
		private UniTask CreateCash()
		{
			LootBoxLoader.<CreateCash>d__27 <CreateCash>d__;
			<CreateCash>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCash>d__.<>4__this = this;
			<CreateCash>d__.<>1__state = -1;
			<CreateCash>d__.<>t__builder.Start<LootBoxLoader.<CreateCash>d__27>(ref <CreateCash>d__);
			return <CreateCash>d__.<>t__builder.Task;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x0007FCBF File Offset: 0x0007DEBF
		private void OnValidate()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x04001927 RID: 6439
		public bool autoSetup = true;

		// Token: 0x04001928 RID: 6440
		public bool dropOnSpawnItem;

		// Token: 0x04001929 RID: 6441
		[SerializeField]
		[Range(0f, 1f)]
		private float activeChance = 1f;

		// Token: 0x0400192A RID: 6442
		[SerializeField]
		private int inventorySize = 8;

		// Token: 0x0400192B RID: 6443
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x0400192C RID: 6444
		public bool randomFromPool;

		// Token: 0x0400192D RID: 6445
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x0400192E RID: 6446
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x0400192F RID: 6447
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001930 RID: 6448
		[SerializeField]
		private RandomContainer<LootBoxLoader.Entry> randomPool;

		// Token: 0x04001931 RID: 6449
		[Range(0f, 1f)]
		public float GenrateCashChance;

		// Token: 0x04001932 RID: 6450
		public int maxRandomCash;

		// Token: 0x04001933 RID: 6451
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x04001934 RID: 6452
		[Range(0f, 1f)]
		[SerializeField]
		private float fixedItemSpawnChance = 1f;

		// Token: 0x04001935 RID: 6453
		[SerializeField]
		private InteractableLootbox _lootBox;

		// Token: 0x02000664 RID: 1636
		[Serializable]
		private struct Entry
		{
			// Token: 0x04002305 RID: 8965
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
