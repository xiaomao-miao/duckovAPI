using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x020003F4 RID: 1012
	public class DifficultySelection_Entry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x0007EDAF File Offset: 0x0007CFAF
		// (set) Token: 0x06002493 RID: 9363 RVA: 0x0007EDB7 File Offset: 0x0007CFB7
		public DifficultySelection Master { get; private set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x0007EDC0 File Offset: 0x0007CFC0
		// (set) Token: 0x06002495 RID: 9365 RVA: 0x0007EDC8 File Offset: 0x0007CFC8
		public DifficultySelection.SettingEntry Setting { get; private set; }

		// Token: 0x06002496 RID: 9366 RVA: 0x0007EDD1 File Offset: 0x0007CFD1
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.locked)
			{
				return;
			}
			this.Master.NotifySelected(this);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0007EDE8 File Offset: 0x0007CFE8
		public void OnPointerEnter(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerEnter(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerEnter;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0007EE0D File Offset: 0x0007D00D
		public void OnPointerExit(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerExit(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerExit;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0007EE32 File Offset: 0x0007D032
		internal void Refresh()
		{
			if (this.Master == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Master.SelectedRuleIndex == this.Setting.ruleIndex);
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0007EE68 File Offset: 0x0007D068
		internal void Setup(DifficultySelection master, DifficultySelection.SettingEntry setting, bool locked)
		{
			this.Master = master;
			this.Setting = setting;
			this.title.text = setting.Title;
			this.icon.sprite = setting.icon;
			this.recommendationIndicator.SetActive(setting.recommended);
			this.locked = locked;
			this.lockedIndicator.SetActive(locked);
			this.Refresh();
		}

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		private Image icon;

		// Token: 0x040018E4 RID: 6372
		[SerializeField]
		private GameObject recommendationIndicator;

		// Token: 0x040018E5 RID: 6373
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040018E6 RID: 6374
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x040018E7 RID: 6375
		internal Action<DifficultySelection_Entry> onPointerEnter;

		// Token: 0x040018E8 RID: 6376
		internal Action<DifficultySelection_Entry> onPointerExit;

		// Token: 0x040018EB RID: 6379
		private bool locked;
	}
}
