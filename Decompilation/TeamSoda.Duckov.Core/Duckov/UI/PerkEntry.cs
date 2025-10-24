using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BA RID: 954
	public class PerkEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPoolable
	{
		// Token: 0x060022A1 RID: 8865 RVA: 0x00079247 File Offset: 0x00077447
		private void SwitchToActiveLook()
		{
			this.ApplyLook(this.activeLook);
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00079255 File Offset: 0x00077455
		private void SwitchToAvaliableLook()
		{
			this.ApplyLook(this.avaliableLook);
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00079263 File Offset: 0x00077463
		private void SwitchToUnavaliableLook()
		{
			this.ApplyLook(this.unavaliableLook);
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x00079271 File Offset: 0x00077471
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x00079293 File Offset: 0x00077493
		public Perk Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0007929C File Offset: 0x0007749C
		public void Setup(PerkTreeView master, Perk target)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayNameText.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x0007933C File Offset: 0x0007753C
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			bool unlocked = this.target.Unlocked;
			bool flag = this.target.AreAllParentsUnlocked();
			if (unlocked)
			{
				this.SwitchToActiveLook();
			}
			else if (flag)
			{
				this.SwitchToAvaliableLook();
			}
			else
			{
				this.SwitchToUnavaliableLook();
			}
			bool unlocking = this.target.Unlocking;
			bool flag2 = this.target.GetRemainingTime() <= TimeSpan.Zero;
			this.avaliableForResearchIndicator.SetActive(!unlocked && !unlocking && this.target.AreAllParentsUnlocked() && this.target.Requirement.AreSatisfied());
			this.inProgressIndicator.SetActive(!unlocked && unlocking && !flag2);
			this.timeUpIndicator.SetActive(!unlocked && unlocking && flag2);
			if (this.master == null)
			{
				return;
			}
			this.selectionIndicator.SetActive(this.master.GetSelection() == this);
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00079437 File Offset: 0x00077637
		private void OnMasterSelectionChanged(PerkEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x00079440 File Offset: 0x00077640
		private void RegisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged += this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00079495 File Offset: 0x00077695
		private void OnTargetStateChanged(Perk perk, bool state)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.Refresh();
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000794B0 File Offset: 0x000776B0
		private void UnregisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged -= this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x00079505 File Offset: 0x00077705
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0007950D File Offset: 0x0007770D
		public void NotifyPooled()
		{
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x0007950F File Offset: 0x0007770F
		public void NotifyReleased()
		{
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00079511 File Offset: 0x00077711
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.master.SetSelection(this);
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x00079540 File Offset: 0x00077740
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.GetLayoutPosition();
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00079564 File Offset: 0x00077764
		private void ApplyLook(PerkEntry.Look look)
		{
			this.icon.material = look.material;
			this.icon.color = look.iconColor;
			this.frame.color = look.frameColor;
			this.frameGlow.enabled = (look.frameGlowColor.a > 0f);
			this.frameGlow.Color = look.frameGlowColor;
			this.background.color = look.backgroundColor;
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000795E3 File Offset: 0x000777E3
		private void FixedUpdate()
		{
			if (this.inProgressIndicator.activeSelf && this.target.GetRemainingTime() <= TimeSpan.Zero)
			{
				this.Refresh();
			}
		}

		// Token: 0x0400178F RID: 6031
		[SerializeField]
		private Image icon;

		// Token: 0x04001790 RID: 6032
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x04001791 RID: 6033
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001792 RID: 6034
		[SerializeField]
		private Image frame;

		// Token: 0x04001793 RID: 6035
		[SerializeField]
		private TrueShadow frameGlow;

		// Token: 0x04001794 RID: 6036
		[SerializeField]
		private Image background;

		// Token: 0x04001795 RID: 6037
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001796 RID: 6038
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x04001797 RID: 6039
		[SerializeField]
		private GameObject inProgressIndicator;

		// Token: 0x04001798 RID: 6040
		[SerializeField]
		private GameObject timeUpIndicator;

		// Token: 0x04001799 RID: 6041
		[SerializeField]
		private GameObject avaliableForResearchIndicator;

		// Token: 0x0400179A RID: 6042
		[SerializeField]
		private PerkEntry.Look activeLook;

		// Token: 0x0400179B RID: 6043
		[SerializeField]
		private PerkEntry.Look avaliableLook;

		// Token: 0x0400179C RID: 6044
		[SerializeField]
		private PerkEntry.Look unavaliableLook;

		// Token: 0x0400179D RID: 6045
		private RectTransform _rectTransform;

		// Token: 0x0400179E RID: 6046
		private PerkTreeView master;

		// Token: 0x0400179F RID: 6047
		private Perk target;

		// Token: 0x0200062D RID: 1581
		[Serializable]
		public struct Look
		{
			// Token: 0x040021EF RID: 8687
			public Color iconColor;

			// Token: 0x040021F0 RID: 8688
			public Material material;

			// Token: 0x040021F1 RID: 8689
			public Color frameColor;

			// Token: 0x040021F2 RID: 8690
			public Color frameGlowColor;

			// Token: 0x040021F3 RID: 8691
			public Color backgroundColor;
		}
	}
}
