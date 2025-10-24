using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000306 RID: 774
	public class DeathLotteryVIew : View
	{
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x0005B6E1 File Offset: 0x000598E1
		private string RemainingTextFormat
		{
			get
			{
				return this.remainingTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x0005B6EE File Offset: 0x000598EE
		public DeathLottery Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x0005B6F6 File Offset: 0x000598F6
		public int RemainingChances
		{
			get
			{
				if (this.Target == null)
				{
					return 0;
				}
				return this.Target.RemainingChances;
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0005B713 File Offset: 0x00059913
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.selectionBusyIndicator.SkipHide();
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0005B731 File Offset: 0x00059931
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0005B744 File Offset: 0x00059944
		protected override void Awake()
		{
			base.Awake();
			DeathLottery.OnRequestUI += this.Show;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0005B75D File Offset: 0x0005995D
		protected override void OnDestroy()
		{
			base.OnDestroy();
			DeathLottery.OnRequestUI -= this.Show;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0005B776 File Offset: 0x00059976
		private void Show(DeathLottery target)
		{
			this.target = target;
			this.Setup();
			base.Open(null);
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0005B78C File Offset: 0x0005998C
		private void RefreshTexts()
		{
			this.remainingCountText.text = ((this.RemainingChances > 0) ? this.RemainingTextFormat.Format(new
			{
				amount = this.RemainingChances
			}) : this.noRemainingChances.ToPlainText());
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0005B7C8 File Offset: 0x000599C8
		private void Setup()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.Loading)
			{
				return;
			}
			DeathLottery.Status currentStatus = this.target.CurrentStatus;
			if (!currentStatus.valid)
			{
				return;
			}
			for (int i = 0; i < currentStatus.candidates.Count; i++)
			{
				this.cards[i].Setup(this, i);
			}
			this.RefreshTexts();
			this.HandleRemaining();
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0005B838 File Offset: 0x00059A38
		internal void NotifyEntryClicked(DeathLotteryCard deathLotteryCard, Cost cost)
		{
			if (deathLotteryCard == null)
			{
				return;
			}
			if (this.ProcessingSelection)
			{
				return;
			}
			if (this.RemainingChances <= 0)
			{
				return;
			}
			int index = deathLotteryCard.Index;
			if (this.target.CurrentStatus.selectedItems.Contains(index))
			{
				return;
			}
			this.selectTask = this.SelectTask(index, cost);
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x0005B890 File Offset: 0x00059A90
		private bool ProcessingSelection
		{
			get
			{
				return this.selectTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0005B8A0 File Offset: 0x00059AA0
		private UniTask SelectTask(int index, Cost cost)
		{
			DeathLotteryVIew.<SelectTask>d__24 <SelectTask>d__;
			<SelectTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectTask>d__.<>4__this = this;
			<SelectTask>d__.index = index;
			<SelectTask>d__.cost = cost;
			<SelectTask>d__.<>1__state = -1;
			<SelectTask>d__.<>t__builder.Start<DeathLotteryVIew.<SelectTask>d__24>(ref <SelectTask>d__);
			return <SelectTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0005B8F4 File Offset: 0x00059AF4
		private void HandleRemaining()
		{
			if (this.RemainingChances > 0)
			{
				return;
			}
			DeathLotteryCard[] array = this.cards;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].NotifyFacing(true);
			}
		}

		// Token: 0x04001243 RID: 4675
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001244 RID: 4676
		[LocalizationKey("Default")]
		[SerializeField]
		private string remainingTextFormatKey = "DeathLottery_Remaining";

		// Token: 0x04001245 RID: 4677
		[LocalizationKey("Default")]
		[SerializeField]
		private string noRemainingChances = "DeathLottery_NoRemainingChances";

		// Token: 0x04001246 RID: 4678
		[SerializeField]
		private TextMeshProUGUI remainingCountText;

		// Token: 0x04001247 RID: 4679
		[SerializeField]
		private DeathLotteryCard[] cards;

		// Token: 0x04001248 RID: 4680
		[SerializeField]
		private FadeGroup selectionBusyIndicator;

		// Token: 0x04001249 RID: 4681
		private DeathLottery target;

		// Token: 0x0400124A RID: 4682
		private UniTask selectTask;
	}
}
