using System;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B9 RID: 953
	public class PerkDetails : MonoBehaviour
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x00078D87 File Offset: 0x00076F87
		[SerializeField]
		private string RequireLevelFormatKey
		{
			get
			{
				return "UI_Perk_RequireLevel";
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x00078D8E File Offset: 0x00076F8E
		[SerializeField]
		private string RequireLevelFormat
		{
			get
			{
				return this.RequireLevelFormatKey.ToPlainText();
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00078D9B File Offset: 0x00076F9B
		private void Awake()
		{
			this.beginButton.onClick.AddListener(new UnityAction(this.OnBeginButtonClicked));
			this.activateButton.onClick.AddListener(new UnityAction(this.OnActivateButtonClicked));
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00078DD5 File Offset: 0x00076FD5
		private void OnActivateButtonClicked()
		{
			this.showingPerk.ConfirmUnlock();
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x00078DE3 File Offset: 0x00076FE3
		private void OnBeginButtonClicked()
		{
			this.showingPerk.SubmitItemsAndBeginUnlocking();
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00078DF1 File Offset: 0x00076FF1
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00078DF9 File Offset: 0x00076FF9
		public void Setup(Perk perk, bool editable = false)
		{
			this.UnregisterEvents();
			this.showingPerk = perk;
			this.editable = editable;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x00078E1B File Offset: 0x0007701B
		private void RegisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00078E41 File Offset: 0x00077041
		private void OnTargetStateChanged(Perk perk, bool arg2)
		{
			this.Refresh();
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00078E49 File Offset: 0x00077049
		private void UnregisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00078E70 File Offset: 0x00077070
		private void Refresh()
		{
			if (this.showingPerk == null)
			{
				this.content.Hide();
				this.placeHolder.Show();
				return;
			}
			this.text_Name.text = this.showingPerk.DisplayName;
			this.text_Description.text = this.showingPerk.Description;
			this.icon.sprite = this.showingPerk.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.showingPerk.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			bool flag = !this.showingPerk.Unlocked && this.editable;
			bool flag2 = this.showingPerk.AreAllParentsUnlocked();
			bool flag3 = false;
			if (flag2)
			{
				flag3 = this.showingPerk.Requirement.AreSatisfied();
			}
			this.activateButton.gameObject.SetActive(false);
			this.beginButton.gameObject.SetActive(false);
			this.buttonUnavaliablePlaceHolder.SetActive(false);
			this.buttonUnsatisfiedPlaceHolder.SetActive(false);
			this.inProgressPlaceHolder.SetActive(false);
			this.unlockedIndicator.SetActive(this.showingPerk.Unlocked);
			if (!this.showingPerk.Unlocked)
			{
				if (this.showingPerk.Unlocking)
				{
					if (this.showingPerk.GetRemainingTime() <= TimeSpan.Zero)
					{
						this.activateButton.gameObject.SetActive(true);
					}
					else
					{
						this.inProgressPlaceHolder.SetActive(true);
					}
				}
				else if (flag2)
				{
					if (flag3)
					{
						this.beginButton.gameObject.SetActive(true);
					}
					else
					{
						this.buttonUnsatisfiedPlaceHolder.SetActive(true);
					}
				}
				else
				{
					this.buttonUnavaliablePlaceHolder.SetActive(true);
				}
			}
			if (flag)
			{
				this.SetupActivationInfo();
			}
			this.activationInfoParent.SetActive(flag);
			this.content.Show();
			this.placeHolder.Hide();
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00079080 File Offset: 0x00077280
		private void SetupActivationInfo()
		{
			if (!this.showingPerk)
			{
				return;
			}
			int level = this.showingPerk.Requirement.level;
			if (level > 0)
			{
				bool flag = EXPManager.Level >= level;
				string text = "#" + (flag ? this.normalTextColor.ToHexString() : this.unsatisfiedTextColor.ToHexString());
				this.text_RequireLevel.gameObject.SetActive(true);
				int level2 = this.showingPerk.Requirement.level;
				string color = text;
				this.text_RequireLevel.text = this.RequireLevelFormat.Format(new
				{
					level = level2,
					color = color
				});
			}
			else
			{
				this.text_RequireLevel.gameObject.SetActive(false);
			}
			this.costDisplay.Setup(this.showingPerk.Requirement.cost, 1);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00079150 File Offset: 0x00077350
		private void Update()
		{
			if (this.showingPerk && this.showingPerk.Unlocking && this.inProgressPlaceHolder.activeSelf)
			{
				this.UpdateCountDown();
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00079180 File Offset: 0x00077380
		private void UpdateCountDown()
		{
			TimeSpan remainingTime = this.showingPerk.GetRemainingTime();
			if (remainingTime <= TimeSpan.Zero)
			{
				this.Refresh();
				return;
			}
			this.progressFillImage.fillAmount = this.showingPerk.GetProgress01();
			this.countDownText.text = string.Format("{0} {1:00}:{2:00}:{3:00}.{4:000}", new object[]
			{
				remainingTime.Days,
				remainingTime.Hours,
				remainingTime.Minutes,
				remainingTime.Seconds,
				remainingTime.Milliseconds
			});
		}

		// Token: 0x0400177A RID: 6010
		[SerializeField]
		private FadeGroup content;

		// Token: 0x0400177B RID: 6011
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x0400177C RID: 6012
		[SerializeField]
		private TextMeshProUGUI text_Name;

		// Token: 0x0400177D RID: 6013
		[SerializeField]
		private TextMeshProUGUI text_Description;

		// Token: 0x0400177E RID: 6014
		[SerializeField]
		private Image icon;

		// Token: 0x0400177F RID: 6015
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x04001780 RID: 6016
		[SerializeField]
		private GameObject unlockedIndicator;

		// Token: 0x04001781 RID: 6017
		[SerializeField]
		private GameObject activationInfoParent;

		// Token: 0x04001782 RID: 6018
		[SerializeField]
		private TextMeshProUGUI text_RequireLevel;

		// Token: 0x04001783 RID: 6019
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001784 RID: 6020
		[SerializeField]
		private Color normalTextColor = Color.white;

		// Token: 0x04001785 RID: 6021
		[SerializeField]
		private Color unsatisfiedTextColor = Color.red;

		// Token: 0x04001786 RID: 6022
		[SerializeField]
		private Button activateButton;

		// Token: 0x04001787 RID: 6023
		[SerializeField]
		private Button beginButton;

		// Token: 0x04001788 RID: 6024
		[SerializeField]
		private GameObject buttonUnsatisfiedPlaceHolder;

		// Token: 0x04001789 RID: 6025
		[SerializeField]
		private GameObject buttonUnavaliablePlaceHolder;

		// Token: 0x0400178A RID: 6026
		[SerializeField]
		private GameObject inProgressPlaceHolder;

		// Token: 0x0400178B RID: 6027
		[SerializeField]
		private Image progressFillImage;

		// Token: 0x0400178C RID: 6028
		[SerializeField]
		private TextMeshProUGUI countDownText;

		// Token: 0x0400178D RID: 6029
		private Perk showingPerk;

		// Token: 0x0400178E RID: 6030
		private bool editable;
	}
}
