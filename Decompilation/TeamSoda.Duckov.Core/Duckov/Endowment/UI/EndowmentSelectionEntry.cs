using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002F7 RID: 759
	public class EndowmentSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x00059C4B File Offset: 0x00057E4B
		public string DisplayName
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DisplayName;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x00059C6C File Offset: 0x00057E6C
		public string Description
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.Description;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00059C8D File Offset: 0x00057E8D
		public string DescriptionAndEffects
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DescriptionAndEffects;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00059CAE File Offset: 0x00057EAE
		public EndowmentIndex Index
		{
			get
			{
				if (this.Target == null)
				{
					return EndowmentIndex.None;
				}
				return this.Target.Index;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x00059CCB File Offset: 0x00057ECB
		// (set) Token: 0x060018AE RID: 6318 RVA: 0x00059CD3 File Offset: 0x00057ED3
		public EndowmentEntry Target { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x00059CDC File Offset: 0x00057EDC
		// (set) Token: 0x060018B0 RID: 6320 RVA: 0x00059CE4 File Offset: 0x00057EE4
		public bool Selected { get; private set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x00059CED File Offset: 0x00057EED
		public bool Unlocked
		{
			get
			{
				return EndowmentManager.GetEndowmentUnlocked(this.Index);
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x00059CFA File Offset: 0x00057EFA
		public bool Locked
		{
			get
			{
				return !this.Unlocked;
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00059D08 File Offset: 0x00057F08
		public void Setup(EndowmentEntry target)
		{
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			this.displayNameText.text = this.Target.DisplayName;
			this.icon.sprite = this.Target.Icon;
			this.requirementText.text = "- " + this.Target.RequirementText + " -";
			this.Refresh();
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00059D82 File Offset: 0x00057F82
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Selected);
			this.lockedIndcator.SetActive(this.Locked);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00059DB5 File Offset: 0x00057FB5
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Locked)
			{
				return;
			}
			Action<EndowmentSelectionEntry, PointerEventData> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this, eventData);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00059DD2 File Offset: 0x00057FD2
		internal void SetSelection(bool value)
		{
			this.Selected = value;
			this.Refresh();
		}

		// Token: 0x040011F0 RID: 4592
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040011F1 RID: 4593
		[SerializeField]
		private Image icon;

		// Token: 0x040011F2 RID: 4594
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040011F3 RID: 4595
		[SerializeField]
		private GameObject lockedIndcator;

		// Token: 0x040011F4 RID: 4596
		[SerializeField]
		private TextMeshProUGUI requirementText;

		// Token: 0x040011F5 RID: 4597
		public Action<EndowmentSelectionEntry, PointerEventData> onClicked;
	}
}
