using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000302 RID: 770
	public class DeathLottery : MonoBehaviour
	{
		// Token: 0x1400009F RID: 159
		// (add) Token: 0x060018F3 RID: 6387 RVA: 0x0005ACC8 File Offset: 0x00058EC8
		// (remove) Token: 0x060018F4 RID: 6388 RVA: 0x0005ACFC File Offset: 0x00058EFC
		public static event Action<DeathLottery> OnRequestUI;

		// Token: 0x060018F5 RID: 6389 RVA: 0x0005AD2F File Offset: 0x00058F2F
		public void RequestUI()
		{
			Action<DeathLottery> onRequestUI = DeathLottery.OnRequestUI;
			if (onRequestUI == null)
			{
				return;
			}
			onRequestUI(this);
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x0005AD41 File Offset: 0x00058F41
		public int MaxChances
		{
			get
			{
				return this.costs.Length;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x0005AD4B File Offset: 0x00058F4B
		public static uint CurrentDeadCharacterToken
		{
			get
			{
				return SavesSystem.Load<uint>("DeadCharacterToken");
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x0005AD57 File Offset: 0x00058F57
		private string SelectNotificationFormat
		{
			get
			{
				return this.selectNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0005AD64 File Offset: 0x00058F64
		public DeathLottery.OptionalCosts[] Costs
		{
			get
			{
				return this.costs;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x0005AD6C File Offset: 0x00058F6C
		public List<Item> ItemInstances
		{
			get
			{
				return this.itemInstances;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0005AD74 File Offset: 0x00058F74
		public DeathLottery.Status CurrentStatus
		{
			get
			{
				if (this.loading)
				{
					return default(DeathLottery.Status);
				}
				if (!this.status.valid)
				{
					return default(DeathLottery.Status);
				}
				return this.status;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x0005ADB0 File Offset: 0x00058FB0
		public int RemainingChances
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x0005ADBE File Offset: 0x00058FBE
		public bool Loading
		{
			get
			{
				return this.loading;
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0005ADC8 File Offset: 0x00058FC8
		private UniTask Load()
		{
			DeathLottery.<Load>d__31 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<DeathLottery.<Load>d__31>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0005AE0C File Offset: 0x0005900C
		private UniTask LoadItemInstances()
		{
			DeathLottery.<LoadItemInstances>d__32 <LoadItemInstances>d__;
			<LoadItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadItemInstances>d__.<>4__this = this;
			<LoadItemInstances>d__.<>1__state = -1;
			<LoadItemInstances>d__.<>t__builder.Start<DeathLottery.<LoadItemInstances>d__32>(ref <LoadItemInstances>d__);
			return <LoadItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0005AE50 File Offset: 0x00059050
		private void ClearItemInstances()
		{
			for (int i = 0; i < this.itemInstances.Count; i++)
			{
				Item item = this.itemInstances[i];
				if (!(item.ParentItem != null))
				{
					item.DestroyTree();
				}
			}
			this.itemInstances.Clear();
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0005AE9F File Offset: 0x0005909F
		[ContextMenu("ForceCreateNewStatus")]
		private void ForceCreateNewStatus()
		{
			if (this.Loading)
			{
				return;
			}
			this.ForceCreateNewStatusTask().Forget();
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0005AEB8 File Offset: 0x000590B8
		private UniTask ForceCreateNewStatusTask()
		{
			DeathLottery.<ForceCreateNewStatusTask>d__35 <ForceCreateNewStatusTask>d__;
			<ForceCreateNewStatusTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForceCreateNewStatusTask>d__.<>4__this = this;
			<ForceCreateNewStatusTask>d__.<>1__state = -1;
			<ForceCreateNewStatusTask>d__.<>t__builder.Start<DeathLottery.<ForceCreateNewStatusTask>d__35>(ref <ForceCreateNewStatusTask>d__);
			return <ForceCreateNewStatusTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0005AEFC File Offset: 0x000590FC
		private UniTask CreateNewStatus()
		{
			DeathLottery.<CreateNewStatus>d__36 <CreateNewStatus>d__;
			<CreateNewStatus>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateNewStatus>d__.<>4__this = this;
			<CreateNewStatus>d__.<>1__state = -1;
			<CreateNewStatus>d__.<>t__builder.Start<DeathLottery.<CreateNewStatus>d__36>(ref <CreateNewStatus>d__);
			return <CreateNewStatus>d__.<>t__builder.Task;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0005AF3F File Offset: 0x0005913F
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0005AF52 File Offset: 0x00059152
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0005AF5F File Offset: 0x0005915F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0005AF72 File Offset: 0x00059172
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			SavesSystem.Save<DeathLottery.Status>("DeathLottery/status", this.status);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0005AF90 File Offset: 0x00059190
		private UniTask<List<Item>> SelectCandidates(Item deadCharacter)
		{
			DeathLottery.<SelectCandidates>d__42 <SelectCandidates>d__;
			<SelectCandidates>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
			<SelectCandidates>d__.<>4__this = this;
			<SelectCandidates>d__.deadCharacter = deadCharacter;
			<SelectCandidates>d__.<>1__state = -1;
			<SelectCandidates>d__.<>t__builder.Start<DeathLottery.<SelectCandidates>d__42>(ref <SelectCandidates>d__);
			return <SelectCandidates>d__.<>t__builder.Task;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0005AFDC File Offset: 0x000591DC
		private bool CanBeACandidate(Item item)
		{
			if (item == null)
			{
				return false;
			}
			foreach (Tag item2 in this.excludeTags)
			{
				if (item.Tags.Contains(item2))
				{
					return false;
				}
			}
			foreach (Tag item3 in this.requireTags)
			{
				if (item.Tags.Contains(item3))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0005B048 File Offset: 0x00059248
		public UniTask<bool> Select(int index, Cost payWhenSucceed)
		{
			DeathLottery.<Select>d__44 <Select>d__;
			<Select>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Select>d__.<>4__this = this;
			<Select>d__.index = index;
			<Select>d__.payWhenSucceed = payWhenSucceed;
			<Select>d__.<>1__state = -1;
			<Select>d__.<>t__builder.Start<DeathLottery.<Select>d__44>(ref <Select>d__);
			return <Select>d__.<>t__builder.Task;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0005B09C File Offset: 0x0005929C
		internal DeathLottery.OptionalCosts GetCost()
		{
			if (!this.status.valid)
			{
				return default(DeathLottery.OptionalCosts);
			}
			if (this.status.SelectedCount >= this.Costs.Length)
			{
				return default(DeathLottery.OptionalCosts);
			}
			return this.Costs[this.status.SelectedCount];
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0005B113 File Offset: 0x00059313
		[CompilerGenerated]
		internal static void <Select>g__SendToPlayer|44_0(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacter(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x04001223 RID: 4643
		public const int MaxCandidateCount = 8;

		// Token: 0x04001224 RID: 4644
		[SerializeField]
		[LocalizationKey("Default")]
		private string selectNotificationFormatKey = "DeathLottery_SelectNotification";

		// Token: 0x04001225 RID: 4645
		[SerializeField]
		private Tag[] requireTags;

		// Token: 0x04001226 RID: 4646
		[SerializeField]
		private Tag[] excludeTags;

		// Token: 0x04001227 RID: 4647
		[SerializeField]
		private RandomContainer<DeathLottery.dummyItemEntry> dummyItems;

		// Token: 0x04001228 RID: 4648
		[SerializeField]
		private DeathLottery.OptionalCosts[] costs;

		// Token: 0x04001229 RID: 4649
		private DeathLottery.Status status;

		// Token: 0x0400122A RID: 4650
		private List<Item> itemInstances = new List<Item>();

		// Token: 0x0400122B RID: 4651
		private bool loading;

		// Token: 0x0200058E RID: 1422
		[Serializable]
		private struct dummyItemEntry
		{
			// Token: 0x04001FD7 RID: 8151
			[ItemTypeID]
			public int typeID;
		}

		// Token: 0x0200058F RID: 1423
		[Serializable]
		public struct OptionalCosts
		{
			// Token: 0x04001FD8 RID: 8152
			[SerializeField]
			public Cost costA;

			// Token: 0x04001FD9 RID: 8153
			[SerializeField]
			public bool useCostB;

			// Token: 0x04001FDA RID: 8154
			[SerializeField]
			public Cost costB;
		}

		// Token: 0x02000590 RID: 1424
		[Serializable]
		public struct Status
		{
			// Token: 0x1700076A RID: 1898
			// (get) Token: 0x06002880 RID: 10368 RVA: 0x00095627 File Offset: 0x00093827
			public int SelectedCount
			{
				get
				{
					return this.selectedItems.Count;
				}
			}

			// Token: 0x04001FDB RID: 8155
			public bool valid;

			// Token: 0x04001FDC RID: 8156
			public uint deadCharacterToken;

			// Token: 0x04001FDD RID: 8157
			public List<int> selectedItems;

			// Token: 0x04001FDE RID: 8158
			public List<ItemTreeData> candidates;
		}
	}
}
