using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x020003F9 RID: 1017
	[RequireComponent(typeof(Points))]
	public class LootSpawner : MonoBehaviour
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x0007F5EE File Offset: 0x0007D7EE
		public bool RandomFromPool
		{
			get
			{
				return this.randomGenrate && this.randomFromPool;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0007F600 File Offset: 0x0007D800
		public bool RandomButNotFromPool
		{
			get
			{
				return this.randomGenrate && !this.randomFromPool;
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0007F615 File Offset: 0x0007D815
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
			this.randomPool.RefreshPercent();
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0007F638 File Offset: 0x0007D838
		private void Start()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				flag = flag2;
			}
			if (!flag)
			{
				if (UnityEngine.Random.Range(0f, 1f) <= this.spawnChance)
				{
					this.Setup().Forget();
				}
				MultiSceneCore.Instance.inLevelData.Add(key, true);
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0007F6CC File Offset: 0x0007D8CC
		private int GetKey()
		{
			Transform parent = base.transform.parent;
			string text = base.transform.GetSiblingIndex().ToString();
			while (parent != null)
			{
				text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
				parent = parent.parent;
			}
			text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
			return text.GetHashCode();
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0007F74C File Offset: 0x0007D94C
		public UniTask Setup()
		{
			LootSpawner.<Setup>d__20 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootSpawner.<Setup>d__20>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x0007F78F File Offset: 0x0007D98F
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x0007F797 File Offset: 0x0007D997
		private void OnValidate()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
		}

		// Token: 0x04001915 RID: 6421
		[Range(0f, 1f)]
		public float spawnChance = 1f;

		// Token: 0x04001916 RID: 6422
		public bool randomGenrate = true;

		// Token: 0x04001917 RID: 6423
		public bool randomFromPool;

		// Token: 0x04001918 RID: 6424
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x04001919 RID: 6425
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x0400191B RID: 6427
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		private RandomContainer<LootSpawner.Entry> randomPool;

		// Token: 0x0400191D RID: 6429
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		private Points points;

		// Token: 0x0400191F RID: 6431
		private bool loading;

		// Token: 0x04001920 RID: 6432
		[SerializeField]
		[ItemTypeID]
		private List<int> typeIds;

		// Token: 0x02000661 RID: 1633
		[Serializable]
		private struct Entry
		{
			// Token: 0x040022FB RID: 8955
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
