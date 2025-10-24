using System;
using System.Text;
using ItemStatsSystem;
using Sirenix.OdinInspector;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024A RID: 586
	public class Perk : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x000453FF File Offset: 0x000435FF
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00045407 File Offset: 0x00043607
		public DisplayQuality DisplayQuality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x0004540F File Offset: 0x0004360F
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00045419 File Offset: 0x00043619
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x00045417 File Offset: 0x00043617
		[LocalizationKey("Perks")]
		private string description
		{
			get
			{
				if (!this.hasDescription)
				{
					return string.Empty;
				}
				return this.displayName + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00045439 File Offset: 0x00043639
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00045448 File Offset: 0x00043648
		public string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string value = this.description.ToPlainText();
				if (!string.IsNullOrEmpty(value))
				{
					stringBuilder.AppendLine(value);
				}
				PerkBehaviour[] components = base.GetComponents<PerkBehaviour>();
				for (int i = 0; i < components.Length; i++)
				{
					string description = components[i].Description;
					if (!string.IsNullOrEmpty(description))
					{
						stringBuilder.AppendLine(description);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x000454AE File Offset: 0x000436AE
		public PerkRequirement Requirement
		{
			get
			{
				return this.requirement;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x000454B6 File Offset: 0x000436B6
		public bool DefaultUnlocked
		{
			get
			{
				return this.defaultUnlocked;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x000454C0 File Offset: 0x000436C0
		private DateTime UnlockingBeginTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.unlockingBeginTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x00045505 File Offset: 0x00043705
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x0004550D File Offset: 0x0004370D
		public bool Unlocked
		{
			get
			{
				return this._unlocked;
			}
			internal set
			{
				this._unlocked = value;
				Action<Perk, bool> action = this.onUnlockStateChanged;
				if (action == null)
				{
					return;
				}
				action(this, value);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00045528 File Offset: 0x00043728
		public bool Unlocking
		{
			get
			{
				return this.unlocking;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x00045530 File Offset: 0x00043730
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x00045538 File Offset: 0x00043738
		public PerkTree Master
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

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x00045541 File Offset: 0x00043741
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00045549 File Offset: 0x00043749
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001240 RID: 4672 RVA: 0x00045554 File Offset: 0x00043754
		// (remove) Token: 0x06001241 RID: 4673 RVA: 0x0004558C File Offset: 0x0004378C
		public event Action<Perk, bool> onUnlockStateChanged;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001242 RID: 4674 RVA: 0x000455C4 File Offset: 0x000437C4
		// (remove) Token: 0x06001243 RID: 4675 RVA: 0x000455F8 File Offset: 0x000437F8
		public static event Action<Perk> OnPerkUnlockConfirmed;

		// Token: 0x06001244 RID: 4676 RVA: 0x0004562B File Offset: 0x0004382B
		public bool AreAllParentsUnlocked()
		{
			return this.master.AreAllParentsUnlocked(this);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00045639 File Offset: 0x00043839
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<PerkTree>();
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00045655 File Offset: 0x00043855
		private bool CheckAndPay()
		{
			return this.requirement == null || (EXPManager.Level >= this.requirement.level && this.requirement.cost.Pay(true, true));
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0004568C File Offset: 0x0004388C
		public bool SubmitItemsAndBeginUnlocking()
		{
			if (this.Unlocked)
			{
				Debug.LogError("Perk " + this.displayName + " already unlocked!");
				return false;
			}
			if (!this.CheckAndPay())
			{
				return false;
			}
			this.unlocking = true;
			this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
			this.master.NotifyChildStateChanged(this);
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action != null)
			{
				action(this, this._unlocked);
			}
			return true;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00045708 File Offset: 0x00043908
		public bool ConfirmUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			if (!this.unlocking)
			{
				return false;
			}
			if (DateTime.UtcNow - this.UnlockingBeginTime < this.requirement.RequireTime)
			{
				return false;
			}
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			Action<Perk> onPerkUnlockConfirmed = Perk.OnPerkUnlockConfirmed;
			if (onPerkUnlockConfirmed != null)
			{
				onPerkUnlockConfirmed(this);
			}
			return true;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00045779 File Offset: 0x00043979
		public bool ForceUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			Debug.Log("Unlock default:" + this.displayName);
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			return true;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x000457B8 File Offset: 0x000439B8
		public TimeSpan GetRemainingTime()
		{
			if (this.Unlocked)
			{
				return TimeSpan.Zero;
			}
			if (!this.unlocking)
			{
				return TimeSpan.Zero;
			}
			TimeSpan t = DateTime.UtcNow - this.UnlockingBeginTime;
			TimeSpan timeSpan = this.requirement.RequireTime - t;
			if (timeSpan < TimeSpan.Zero)
			{
				return TimeSpan.Zero;
			}
			return timeSpan;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00045818 File Offset: 0x00043A18
		public float GetProgress01()
		{
			TimeSpan remainingTime = this.GetRemainingTime();
			double totalSeconds = this.requirement.RequireTime.TotalSeconds;
			if (totalSeconds <= 0.0)
			{
				return 1f;
			}
			double totalSeconds2 = remainingTime.TotalSeconds;
			return 1f - (float)(totalSeconds2 / totalSeconds);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00045864 File Offset: 0x00043A64
		public void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddWarning("未指定PerkTree");
			}
			if (this.master)
			{
				if (!this.master.Perks.Contains(this))
				{
					result.AddError("PerkTree未包含此Perk").WithFix(delegate()
					{
						this.master.perks.Add(this);
					}, true);
				}
				PerkTree perkTree = this.master;
				bool flag;
				if (perkTree == null)
				{
					flag = (null != null);
				}
				else
				{
					PerkTreeRelationGraphOwner relationGraphOwner = perkTree.RelationGraphOwner;
					flag = (((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null) != null);
				}
				if (!flag)
				{
					result.AddError("未在Graph中指定技能的关系");
				}
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000458F6 File Offset: 0x00043AF6
		internal Vector2 GetLayoutPosition()
		{
			if (this.master == null)
			{
				return Vector2.zero;
			}
			PerkTreeRelationGraphOwner relationGraphOwner = this.master.RelationGraphOwner;
			return ((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null).cachedPosition;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00045929 File Offset: 0x00043B29
		internal void NotifyParentStateChanged()
		{
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Unlocked);
		}

		// Token: 0x04000DFA RID: 3578
		[SerializeField]
		private PerkTree master;

		// Token: 0x04000DFB RID: 3579
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x04000DFC RID: 3580
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000DFD RID: 3581
		[SerializeField]
		private DisplayQuality quality;

		// Token: 0x04000DFE RID: 3582
		[LocalizationKey("Perks")]
		[SerializeField]
		private string displayName = "未命名技能";

		// Token: 0x04000DFF RID: 3583
		[SerializeField]
		private bool hasDescription;

		// Token: 0x04000E00 RID: 3584
		[SerializeField]
		private PerkRequirement requirement;

		// Token: 0x04000E01 RID: 3585
		[SerializeField]
		private bool defaultUnlocked;

		// Token: 0x04000E02 RID: 3586
		[SerializeField]
		internal bool unlocking;

		// Token: 0x04000E03 RID: 3587
		[DateTime]
		[SerializeField]
		internal long unlockingBeginTimeRaw;

		// Token: 0x04000E04 RID: 3588
		[SerializeField]
		private bool _unlocked;
	}
}
