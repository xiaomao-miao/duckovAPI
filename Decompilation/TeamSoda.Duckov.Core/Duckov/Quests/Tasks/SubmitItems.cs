using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000358 RID: 856
	public class SubmitItems : Task
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x0006A05E File Offset: 0x0006825E
		public int ItemTypeID
		{
			get
			{
				return this.itemTypeID;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0006A068 File Offset: 0x00068268
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null || this._cachedMeta.Value.id != this.itemTypeID)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0006A0BB File Offset: 0x000682BB
		private string descriptionFormatKey
		{
			get
			{
				return "Task_SubmitItems";
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x0006A0C2 File Offset: 0x000682C2
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x0006A0CF File Offset: 0x000682CF
		private string havingAmountFormatKey
		{
			get
			{
				return "Task_SubmitItems_HavingAmount";
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x0006A0D6 File Offset: 0x000682D6
		private string HavingAmountFormat
		{
			get
			{
				return this.havingAmountFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x0006A0E4 File Offset: 0x000682E4
		public override string Description
		{
			get
			{
				string text = this.DescriptionFormat.Format(new
				{
					ItemDisplayName = this.CachedMeta.DisplayName,
					submittedAmount = this.submittedAmount,
					requiredAmount = this.requiredAmount
				});
				if (!base.IsFinished())
				{
					text = text + " " + this.HavingAmountFormat.Format(new
					{
						amount = ItemUtilities.GetItemCount(this.itemTypeID)
					});
				}
				return text;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0006A14C File Offset: 0x0006834C
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x0006A159 File Offset: 0x00068359
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0006A15C File Offset: 0x0006835C
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckItemEnough();
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x0006A164 File Offset: 0x00068364
		public override string InteractText
		{
			get
			{
				return "Task_SubmitItems_Interact".ToPlainText();
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0006A170 File Offset: 0x00068370
		public override bool NeedInspection
		{
			get
			{
				return !base.IsFinished() && this.CheckItemEnough();
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06001DF1 RID: 7665 RVA: 0x0006A184 File Offset: 0x00068384
		// (remove) Token: 0x06001DF2 RID: 7666 RVA: 0x0006A1B8 File Offset: 0x000683B8
		public static event Action<SubmitItems> onItemEnough;

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0006A1EB File Offset: 0x000683EB
		protected override void OnInit()
		{
			base.OnInit();
			PlayerStorage.OnPlayerStorageChange += this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Combine(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
			this.CheckItemEnough();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0006A22B File Offset: 0x0006842B
		private void OnDestroy()
		{
			PlayerStorage.OnPlayerStorageChange -= this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Remove(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0006A260 File Offset: 0x00068460
		private void OnPlayerStorageChanged(PlayerStorage storage, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0006A2A4 File Offset: 0x000684A4
		private void OnMainCharacterInventoryChanged(CharacterMainControl control, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0006A2E6 File Offset: 0x000684E6
		private bool CheckItemEnough()
		{
			if (ItemUtilities.GetItemCount(this.itemTypeID) >= this.requiredAmount)
			{
				Action<SubmitItems> action = SubmitItems.onItemEnough;
				if (action != null)
				{
					action(this);
				}
				this.SetMapElementVisable(false);
				return true;
			}
			this.SetMapElementVisable(true);
			return false;
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0006A31D File Offset: 0x0006851D
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0006A354 File Offset: 0x00068554
		public void Submit(Item item)
		{
			if (item.TypeID != this.itemTypeID)
			{
				Debug.LogError("提交的物品类型与需求不一致。");
				return;
			}
			int num = this.requiredAmount - this.submittedAmount;
			if (num <= 0)
			{
				Debug.LogError("目标已达成，不需要继续提交物品");
				return;
			}
			int num2 = this.submittedAmount;
			if (num < item.StackCount)
			{
				item.StackCount -= num;
				this.submittedAmount += num;
			}
			else
			{
				foreach (Item item2 in item.GetAllChildren(false, true))
				{
					item2.Detach();
					if (!ItemUtilities.SendToPlayerCharacter(item2, false))
					{
						item2.Drop(CharacterMainControl.Main, true);
					}
				}
				item.Detach();
				item.DestroyTree();
				this.submittedAmount += item.StackCount;
			}
			Debug.Log("submission done");
			if (num2 != this.submittedAmount)
			{
				base.Master.NotifyTaskFinished(this);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0006A468 File Offset: 0x00068668
		protected override bool CheckFinished()
		{
			return this.submittedAmount >= this.requiredAmount;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0006A47B File Offset: 0x0006867B
		public override object GenerateSaveData()
		{
			return this.submittedAmount;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0006A488 File Offset: 0x00068688
		public override void SetupSaveData(object data)
		{
			this.submittedAmount = (int)data;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0006A498 File Offset: 0x00068698
		public override void Interact()
		{
			if (base.Master == null)
			{
				return;
			}
			List<Item> list = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == this.itemTypeID);
			for (int i = 0; i < list.Count; i++)
			{
				Item item = list[i];
				this.Submit(item);
				if (base.IsFinished())
				{
					break;
				}
			}
		}

		// Token: 0x0400148F RID: 5263
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x04001490 RID: 5264
		[Range(1f, 100f)]
		[SerializeField]
		private int requiredAmount = 1;

		// Token: 0x04001491 RID: 5265
		[SerializeField]
		private int submittedAmount;

		// Token: 0x04001492 RID: 5266
		private ItemMetaData? _cachedMeta;

		// Token: 0x04001493 RID: 5267
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
