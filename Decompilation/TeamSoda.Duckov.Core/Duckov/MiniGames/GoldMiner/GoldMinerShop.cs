using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000291 RID: 657
	public class GoldMinerShop : MiniGameBehaviour
	{
		// Token: 0x0600157B RID: 5499 RVA: 0x0004F754 File Offset: 0x0004D954
		private void Clear()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.stock.Count; i++)
			{
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity != null && (shopEntity.sold || !shopEntity.locked))
				{
					this.stock[i] = null;
				}
			}
			for (int j = this.capacity; j < this.stock.Count; j++)
			{
				if (this.stock[j] == null)
				{
					this.stock.RemoveAt(j);
				}
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0004F7F0 File Offset: 0x0004D9F0
		private void Refill()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.capacity; i++)
			{
				if (this.stock.Count <= i)
				{
					this.stock.Add(null);
				}
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity == null || shopEntity.sold)
				{
					this.stock[i] = this.GenerateNewShopItem();
				}
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0004F868 File Offset: 0x0004DA68
		private void RefreshStock()
		{
			this.Clear();
			this.CacheValidCandiateLists();
			this.Refill();
			Action action = this.onAfterOperation;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0004F88C File Offset: 0x0004DA8C
		private void CacheValidCandiateLists()
		{
			for (int i = 0; i < 5; i++)
			{
				int quality = i + 1;
				List<GoldMinerArtifact> list = this.SearchValidCandidateArtifactIDs(quality).ToList<GoldMinerArtifact>();
				this.validCandidateLists[i] = list;
			}
			foreach (ShopEntity shopEntity in this.stock)
			{
				if (shopEntity != null && !(shopEntity.artifact == null) && !shopEntity.artifact.AllowMultiple)
				{
					foreach (List<GoldMinerArtifact> list2 in this.validCandidateLists)
					{
						if (list2 != null)
						{
							list2.Remove(shopEntity.artifact);
						}
					}
				}
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0004F958 File Offset: 0x0004DB58
		private IEnumerable<GoldMinerArtifact> SearchValidCandidateArtifactIDs(int quality)
		{
			using (IEnumerator<GoldMinerArtifact> enumerator = this.master.ArtifactPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GoldMinerArtifact artifact = enumerator.Current;
					if (artifact.Quality == quality && (artifact.AllowMultiple || (this.master.run.GetArtifactCount(artifact.ID) <= 0 && !this.stock.Any((ShopEntity e) => e != null && !e.sold && e.ID == artifact.ID))))
					{
						yield return artifact;
					}
				}
			}
			IEnumerator<GoldMinerArtifact> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0004F96F File Offset: 0x0004DB6F
		private List<GoldMinerArtifact> GetValidCandidateArtifactIDs(int q)
		{
			return this.validCandidateLists[q - 1];
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0004F97C File Offset: 0x0004DB7C
		private ShopEntity GenerateNewShopItem()
		{
			int num = this.qualityDistribute.GetRandom(0f);
			List<GoldMinerArtifact> list = null;
			for (int i = num; i >= 1; i--)
			{
				list = this.GetValidCandidateArtifactIDs(i);
				if (list.Count > 0)
				{
					num = i;
					break;
				}
			}
			GoldMinerArtifact random = list.GetRandom(this.master.run.shopRandom);
			if (random != null && !random.AllowMultiple)
			{
				List<GoldMinerArtifact> validCandidateArtifactIDs = this.GetValidCandidateArtifactIDs(num);
				if (validCandidateArtifactIDs != null)
				{
					validCandidateArtifactIDs.Remove(random);
				}
			}
			if (random == null)
			{
				Debug.Log(string.Format("{0} failed to generate", num));
			}
			return new ShopEntity
			{
				artifact = random
			};
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0004FA28 File Offset: 0x0004DC28
		public bool Buy(ShopEntity entity)
		{
			if (!this.stock.Contains(entity))
			{
				Debug.LogError("Buying entity that doesn't exist in shop stock");
				return false;
			}
			if (entity.sold)
			{
				return false;
			}
			bool flag;
			int price = this.CalculateDealPrice(entity, out flag);
			if (this.master.run.shopTicket > 0)
			{
				this.master.run.shopTicket--;
			}
			else if (!this.master.PayMoney(price))
			{
				return false;
			}
			this.master.run.AttachArtifactFromPrefab(entity.artifact);
			entity.sold = true;
			Action action = this.onAfterOperation;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0004FAD0 File Offset: 0x0004DCD0
		public int CalculateDealPrice(ShopEntity entity, out bool useTicket)
		{
			useTicket = false;
			if (entity == null)
			{
				return 0;
			}
			if (this.master.run.shopTicket > 0)
			{
				useTicket = true;
				return 0;
			}
			GoldMinerArtifact artifact = entity.artifact;
			if (artifact == null)
			{
				return 0;
			}
			return Mathf.CeilToInt((float)artifact.BasePrice * entity.priceFactor * this.master.GlobalPriceFactor);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x0004FB2E File Offset: 0x0004DD2E
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x0004FB36 File Offset: 0x0004DD36
		public int refreshChance { get; private set; }

		// Token: 0x06001586 RID: 5510 RVA: 0x0004FB40 File Offset: 0x0004DD40
		public UniTask Execute()
		{
			GoldMinerShop.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<GoldMinerShop.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0004FB83 File Offset: 0x0004DD83
		internal void Continue()
		{
			this.complete = true;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0004FB8C File Offset: 0x0004DD8C
		internal bool TryRefresh()
		{
			if (this.refreshChance <= 0)
			{
				return false;
			}
			int refreshCost = this.GetRefreshCost();
			if (!this.master.PayMoney(refreshCost))
			{
				return false;
			}
			int refreshChance = this.refreshChance;
			this.refreshChance = refreshChance - 1;
			this.refreshPrice += Mathf.RoundToInt(this.master.run.shopRefreshPriceIncrement.Value);
			this.RefreshStock();
			return true;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0004FBF9 File Offset: 0x0004DDF9
		internal int GetRefreshCost()
		{
			return this.refreshPrice;
		}

		// Token: 0x04000FDC RID: 4060
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		private GoldMinerShopUI ui;

		// Token: 0x04000FDE RID: 4062
		[SerializeField]
		private RandomContainer<int> qualityDistribute;

		// Token: 0x04000FDF RID: 4063
		public List<ShopEntity> stock = new List<ShopEntity>();

		// Token: 0x04000FE0 RID: 4064
		public Action onAfterOperation;

		// Token: 0x04000FE1 RID: 4065
		private int capacity;

		// Token: 0x04000FE2 RID: 4066
		private List<GoldMinerArtifact>[] validCandidateLists = new List<GoldMinerArtifact>[5];

		// Token: 0x04000FE3 RID: 4067
		private bool complete;

		// Token: 0x04000FE4 RID: 4068
		private int refreshPrice = 100;
	}
}
