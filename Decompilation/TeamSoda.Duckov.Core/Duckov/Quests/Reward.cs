using System;
using Saves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000338 RID: 824
	public abstract class Reward : MonoBehaviour, ISelfValidator, ISaveDataProvider
	{
		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06001C42 RID: 7234 RVA: 0x000665BC File Offset: 0x000647BC
		// (remove) Token: 0x06001C43 RID: 7235 RVA: 0x000665F0 File Offset: 0x000647F0
		public static event Action<Reward> OnRewardClaimed;

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x00066623 File Offset: 0x00064823
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x0006662B File Offset: 0x0006482B
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06001C46 RID: 7238 RVA: 0x00066634 File Offset: 0x00064834
		// (remove) Token: 0x06001C47 RID: 7239 RVA: 0x0006666C File Offset: 0x0006486C
		internal event Action onStatusChanged;

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000666A1 File Offset: 0x000648A1
		public bool Claimable
		{
			get
			{
				return this.master.Complete;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x000666AE File Offset: 0x000648AE
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000666B1 File Offset: 0x000648B1
		public virtual string Description
		{
			get
			{
				return "未定义奖励描述";
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001C4B RID: 7243
		public abstract bool Claimed { get; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x000666B8 File Offset: 0x000648B8
		public virtual bool Claiming { get; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x000666C0 File Offset: 0x000648C0
		public virtual bool AutoClaim
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x000666C3 File Offset: 0x000648C3
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x000666CB File Offset: 0x000648CB
		public Quest Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000666D4 File Offset: 0x000648D4
		public void Claim()
		{
			if (!this.Claimable || this.Claimed)
			{
				return;
			}
			this.OnClaim();
			this.Master.NotifyRewardClaimed(this);
			Action<Reward> onRewardClaimed = Reward.OnRewardClaimed;
			if (onRewardClaimed == null)
			{
				return;
			}
			onRewardClaimed(this);
		}

		// Token: 0x06001C51 RID: 7249
		public abstract void OnClaim();

		// Token: 0x06001C52 RID: 7250 RVA: 0x0006670C File Offset: 0x0006490C
		public virtual void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddWarning("Reward需要master(Quest)。").WithFix("设为父物体中的Quest。", delegate()
				{
					this.master = base.GetComponent<Quest>();
					if (this.master == null)
					{
						this.master = base.GetComponentInParent<Quest>();
					}
				}, true);
			}
			if (this.master != null)
			{
				if (base.transform != this.master.transform && !base.transform.IsChildOf(this.master.transform))
				{
					result.AddError("Task需要存在于master子物体中。").WithFix("设为master子物体", delegate()
					{
						base.transform.SetParent(this.master.transform);
					}, true);
				}
				if (!this.master.rewards.Contains(this))
				{
					result.AddError("Master的Task列表中不包含本物体。").WithFix("将本物体添加至master的Task列表中", delegate()
					{
						this.master.rewards.Add(this);
					}, true);
				}
			}
		}

		// Token: 0x06001C53 RID: 7251
		public abstract object GenerateSaveData();

		// Token: 0x06001C54 RID: 7252
		public abstract void SetupSaveData(object data);

		// Token: 0x06001C55 RID: 7253 RVA: 0x000667E4 File Offset: 0x000649E4
		private void Awake()
		{
			this.Master.onStatusChanged += this.OnMasterStatusChanged;
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000667FD File Offset: 0x000649FD
		private void OnDestroy()
		{
			this.Master.onStatusChanged -= this.OnMasterStatusChanged;
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00066816 File Offset: 0x00064A16
		public void OnMasterStatusChanged(Quest quest)
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00066828 File Offset: 0x00064A28
		protected void ReportStatusChanged()
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x0006683A File Offset: 0x00064A3A
		public virtual void NotifyReload(Quest questInstance)
		{
		}

		// Token: 0x040013CA RID: 5066
		[SerializeField]
		private int id;

		// Token: 0x040013CB RID: 5067
		[SerializeField]
		private Quest master;
	}
}
