using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034D RID: 845
	public class RewardEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00068CE5 File Offset: 0x00066EE5
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x00068CED File Offset: 0x00066EED
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

		// Token: 0x06001D47 RID: 7495 RVA: 0x00068CF6 File Offset: 0x00066EF6
		private void Awake()
		{
			this.claimButton.onClick.AddListener(new UnityAction(this.OnClaimButtonClicked));
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x00068D14 File Offset: 0x00066F14
		private void OnClaimButtonClicked()
		{
			Reward reward = this.target;
			if (reward == null)
			{
				return;
			}
			reward.Claim();
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x00068D26 File Offset: 0x00066F26
		public void NotifyPooled()
		{
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00068D28 File Offset: 0x00066F28
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00068D30 File Offset: 0x00066F30
		internal void Setup(Reward target)
		{
			this.UnregisterEvents();
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00068D55 File Offset: 0x00066F55
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00068D7D File Offset: 0x00066F7D
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00068DA5 File Offset: 0x00066FA5
		private void OnTargetStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00068DB0 File Offset: 0x00066FB0
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.rewardText.text = this.target.Description;
			Sprite icon = this.target.Icon;
			this.rewardIcon.gameObject.SetActive(icon);
			this.rewardIcon.sprite = icon;
			bool claimed = this.target.Claimed;
			bool claimable = this.target.Claimable;
			bool flag = this.Interactable && claimable;
			bool active = !this.Interactable && claimable && !claimed;
			this.claimButton.gameObject.SetActive(flag);
			if (this.claimableIndicator != null)
			{
				this.claimableIndicator.SetActive(active);
			}
			if (flag)
			{
				if (this.buttonText)
				{
					this.buttonText.text = (claimed ? this.claimedTextKey.ToPlainText() : this.claimTextKey.ToPlainText());
				}
				this.statusIcon.sprite = (claimed ? this.claimedIcon : this.claimIcon);
				this.claimButton.interactable = !claimed;
				this.statusIcon.gameObject.SetActive(!this.target.Claiming);
				this.claimingIcon.gameObject.SetActive(this.target.Claiming);
			}
		}

		// Token: 0x04001450 RID: 5200
		[SerializeField]
		private Image rewardIcon;

		// Token: 0x04001451 RID: 5201
		[SerializeField]
		private TextMeshProUGUI rewardText;

		// Token: 0x04001452 RID: 5202
		[SerializeField]
		private Button claimButton;

		// Token: 0x04001453 RID: 5203
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x04001454 RID: 5204
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04001455 RID: 5205
		[SerializeField]
		private TextMeshProUGUI buttonText;

		// Token: 0x04001456 RID: 5206
		[SerializeField]
		private GameObject claimingIcon;

		// Token: 0x04001457 RID: 5207
		[SerializeField]
		private Sprite claimIcon;

		// Token: 0x04001458 RID: 5208
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimTextKey = "UI_Quest_RewardClaim";

		// Token: 0x04001459 RID: 5209
		[SerializeField]
		private Sprite claimedIcon;

		// Token: 0x0400145A RID: 5210
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimedTextKey = "UI_Quest_RewardClaimed";

		// Token: 0x0400145B RID: 5211
		[SerializeField]
		private bool interactable;

		// Token: 0x0400145C RID: 5212
		private Reward target;
	}
}
