using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000357 RID: 855
	public class QuestTask_UseItem : Task
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00069EC2 File Offset: 0x000680C2
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

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x00069EF2 File Offset: 0x000680F2
		private string descriptionFormatKey
		{
			get
			{
				return "Task_UseItem";
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x00069EF9 File Offset: 0x000680F9
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x00069F08 File Offset: 0x00068108
		private string ItemDisplayName
		{
			get
			{
				return this.CachedMeta.DisplayName;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x00069F23 File Offset: 0x00068123
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.ItemDisplayName,
					this.amount,
					this.requireAmount
				});
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x00069F47 File Offset: 0x00068147
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00069F54 File Offset: 0x00068154
		private void OnEnable()
		{
			Item.onUseStatic += this.OnItemUsed;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00069F78 File Offset: 0x00068178
		private void OnDisable()
		{
			Item.onUseStatic -= this.OnItemUsed;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00069F9C File Offset: 0x0006819C
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x00069FAD File Offset: 0x000681AD
		private void OnItemUsed(Item item, object user)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (user as CharacterMainControl == LevelManager.Instance.MainCharacter && item.TypeID == this.itemTypeID)
			{
				this.AddCount();
			}
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00069FE7 File Offset: 0x000681E7
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0006A00B File Offset: 0x0006820B
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0006A018 File Offset: 0x00068218
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0006A02C File Offset: 0x0006822C
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x0400148A RID: 5258
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x0400148B RID: 5259
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x0400148C RID: 5260
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x0400148D RID: 5261
		[SerializeField]
		private int amount;

		// Token: 0x0400148E RID: 5262
		private ItemMetaData? _cachedMeta;
	}
}
