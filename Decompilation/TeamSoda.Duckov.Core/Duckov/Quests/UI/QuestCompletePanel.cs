using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000344 RID: 836
	public class QuestCompletePanel : MonoBehaviour
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x0006760C File Offset: 0x0006580C
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntryTemplate, this.rewardEntryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.rewardEntryTemplate.gameObject.SetActive(false);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x00067665 File Offset: 0x00065865
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0006766D File Offset: 0x0006586D
		private void Awake()
		{
			this.skipButton.onClick.AddListener(new UnityAction(this.Skip));
			this.takeAllButton.onClick.AddListener(new UnityAction(this.TakeAll));
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000676A8 File Offset: 0x000658A8
		private void TakeAll()
		{
			if (this.target == null)
			{
				return;
			}
			foreach (Reward reward in this.target.rewards)
			{
				if (!reward.Claimed)
				{
					reward.Claim();
				}
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00067718 File Offset: 0x00065918
		public void Skip()
		{
			this.skipClicked = true;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00067724 File Offset: 0x00065924
		public UniTask Show(Quest quest)
		{
			QuestCompletePanel.<Show>d__14 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.quest = quest;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<QuestCompletePanel.<Show>d__14>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00067770 File Offset: 0x00065970
		private UniTask WaitForEndOfInteraction()
		{
			QuestCompletePanel.<WaitForEndOfInteraction>d__16 <WaitForEndOfInteraction>d__;
			<WaitForEndOfInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForEndOfInteraction>d__.<>4__this = this;
			<WaitForEndOfInteraction>d__.<>1__state = -1;
			<WaitForEndOfInteraction>d__.<>t__builder.Start<QuestCompletePanel.<WaitForEndOfInteraction>d__16>(ref <WaitForEndOfInteraction>d__);
			return <WaitForEndOfInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000677B4 File Offset: 0x000659B4
		private void SetupContent(Quest quest)
		{
			this.target = quest;
			if (quest == null)
			{
				return;
			}
			this.questNameText.text = quest.DisplayName;
			this.RewardEntryPool.ReleaseAll();
			foreach (Reward reward in quest.rewards)
			{
				RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardEntryTemplate.transform.parent);
				rewardEntry.Setup(reward);
				rewardEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x040013F8 RID: 5112
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x040013F9 RID: 5113
		[SerializeField]
		private TextMeshProUGUI questNameText;

		// Token: 0x040013FA RID: 5114
		[SerializeField]
		private RewardEntry rewardEntryTemplate;

		// Token: 0x040013FB RID: 5115
		[SerializeField]
		private Button skipButton;

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private Button takeAllButton;

		// Token: 0x040013FD RID: 5117
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x040013FE RID: 5118
		private Quest target;

		// Token: 0x040013FF RID: 5119
		private bool skipClicked;
	}
}
