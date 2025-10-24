using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000339 RID: 825
	public class RewardItem : Reward
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x00066897 File Offset: 0x00064A97
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x0006689F File Offset: 0x00064A9F
		public override bool Claiming
		{
			get
			{
				return this.claiming;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x000668A7 File Offset: 0x00064AA7
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x000668D7 File Offset: 0x00064AD7
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x000668E4 File Offset: 0x00064AE4
		public override string Description
		{
			get
			{
				return string.Format("{0} x{1}", this.CachedMeta.DisplayName, this.amount);
			}
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00066914 File Offset: 0x00064B14
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00066921 File Offset: 0x00064B21
		public override void SetupSaveData(object data)
		{
			this.claimed = (bool)data;
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x0006692F File Offset: 0x00064B2F
		public override void OnClaim()
		{
			if (this.claimed)
			{
				return;
			}
			if (this.claiming)
			{
				return;
			}
			this.claiming = true;
			this.GenerateAndGiveItems().Forget();
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00066958 File Offset: 0x00064B58
		private UniTask GenerateAndGiveItems()
		{
			RewardItem.<GenerateAndGiveItems>d__18 <GenerateAndGiveItems>d__;
			<GenerateAndGiveItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<GenerateAndGiveItems>d__.<>4__this = this;
			<GenerateAndGiveItems>d__.<>1__state = -1;
			<GenerateAndGiveItems>d__.<>t__builder.Start<RewardItem.<GenerateAndGiveItems>d__18>(ref <GenerateAndGiveItems>d__);
			return <GenerateAndGiveItems>d__.<>t__builder.Task;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0006699B File Offset: 0x00064B9B
		private void SendItemToPlayerStorage(Item item)
		{
			PlayerStorage.Push(item, true);
		}

		// Token: 0x040013CE RID: 5070
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x040013CF RID: 5071
		public int amount = 1;

		// Token: 0x040013D0 RID: 5072
		private bool claimed;

		// Token: 0x040013D1 RID: 5073
		private bool claiming;

		// Token: 0x040013D2 RID: 5074
		private ItemMetaData? _cachedMeta;
	}
}
