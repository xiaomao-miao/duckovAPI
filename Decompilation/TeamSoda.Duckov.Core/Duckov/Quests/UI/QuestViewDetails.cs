using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034B RID: 843
	public class QuestViewDetails : MonoBehaviour
	{
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x0006892F File Offset: 0x00066B2F
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x00068937 File Offset: 0x00066B37
		// (set) Token: 0x06001D31 RID: 7473 RVA: 0x0006893F File Offset: 0x00066B3F
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00068948 File Offset: 0x00066B48
		private PrefabPool<TaskEntry> TaskEntryPool
		{
			get
			{
				if (this._taskEntryPool == null)
				{
					this._taskEntryPool = new PrefabPool<TaskEntry>(this.taskEntryPrefab, this.tasksParent, null, null, null, true, 10, 10000, null);
				}
				return this._taskEntryPool;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x00068988 File Offset: 0x00066B88
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntry, this.rewardsParent, null, null, null, true, 10, 10000, null);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000689C6 File Offset: 0x00066BC6
		private void Awake()
		{
			this.rewardEntry.gameObject.SetActive(false);
			this.taskEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000689EA File Offset: 0x00066BEA
		internal void Refresh()
		{
			this.RefreshAsync().Forget();
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000689F8 File Offset: 0x00066BF8
		private int GetNewToken()
		{
			int num;
			for (num = this.activeTaskToken; num == this.activeTaskToken; num = UnityEngine.Random.Range(1, int.MaxValue))
			{
			}
			this.activeTaskToken = num;
			return this.activeTaskToken;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x00068A30 File Offset: 0x00066C30
		private UniTask RefreshAsync()
		{
			QuestViewDetails.<RefreshAsync>d__28 <RefreshAsync>d__;
			<RefreshAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RefreshAsync>d__.<>4__this = this;
			<RefreshAsync>d__.<>1__state = -1;
			<RefreshAsync>d__.<>t__builder.Start<QuestViewDetails.<RefreshAsync>d__28>(ref <RefreshAsync>d__);
			return <RefreshAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00068A74 File Offset: 0x00066C74
		private void SetupTasks()
		{
			this.TaskEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Task task in this.target.tasks)
			{
				TaskEntry taskEntry = this.TaskEntryPool.Get(this.tasksParent);
				taskEntry.Interactable = this.Interactable;
				taskEntry.Setup(task);
				taskEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00068B10 File Offset: 0x00066D10
		private void SetupRewards()
		{
			this.RewardEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Reward x in this.target.rewards)
			{
				if (x == null)
				{
					Debug.LogError(string.Format("任务 {0} - {1} 中包含值为 null 的奖励。", this.target.ID, this.target.DisplayName));
				}
				else
				{
					RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardsParent);
					rewardEntry.Interactable = this.Interactable;
					rewardEntry.Setup(x);
					rewardEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x00068BE0 File Offset: 0x00066DE0
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00068C08 File Offset: 0x00066E08
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00068C30 File Offset: 0x00066E30
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00068C38 File Offset: 0x00066E38
		internal void Setup(Quest quest)
		{
			this.target = quest;
			this.Refresh();
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00068C47 File Offset: 0x00066E47
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0400143C RID: 5180
		private Quest target;

		// Token: 0x0400143D RID: 5181
		[SerializeField]
		private TaskEntry taskEntryPrefab;

		// Token: 0x0400143E RID: 5182
		[SerializeField]
		private RewardEntry rewardEntry;

		// Token: 0x0400143F RID: 5183
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x04001440 RID: 5184
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04001441 RID: 5185
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001442 RID: 5186
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04001443 RID: 5187
		[SerializeField]
		private TextMeshProUGUI questGiverDisplayName;

		// Token: 0x04001444 RID: 5188
		[SerializeField]
		private Transform tasksParent;

		// Token: 0x04001445 RID: 5189
		[SerializeField]
		private Transform rewardsParent;

		// Token: 0x04001446 RID: 5190
		[SerializeField]
		private QuestRequiredItem requiredItem;

		// Token: 0x04001447 RID: 5191
		[SerializeField]
		private bool interactable;

		// Token: 0x04001448 RID: 5192
		private PrefabPool<TaskEntry> _taskEntryPool;

		// Token: 0x04001449 RID: 5193
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x0400144A RID: 5194
		private Quest showingQuest;

		// Token: 0x0400144B RID: 5195
		private int activeTaskToken;
	}
}
