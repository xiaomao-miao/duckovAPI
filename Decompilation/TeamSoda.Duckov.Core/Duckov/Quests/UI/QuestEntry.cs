using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Quests.UI
{
	// Token: 0x02000345 RID: 837
	public class QuestEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x00067864 File Offset: 0x00065A64
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06001CD3 RID: 7379 RVA: 0x0006786C File Offset: 0x00065A6C
		// (remove) Token: 0x06001CD4 RID: 7380 RVA: 0x000678A4 File Offset: 0x00065AA4
		public event Action<QuestEntry, PointerEventData> onClick;

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000678D9 File Offset: 0x00065AD9
		public bool Selected
		{
			get
			{
				return this.menu.GetSelection() == this;
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000678EC File Offset: 0x00065AEC
		public void NotifyPooled()
		{
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000678EE File Offset: 0x00065AEE
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000678FD File Offset: 0x00065AFD
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00067905 File Offset: 0x00065B05
		internal void Setup(Quest quest)
		{
			this.UnregisterEvents();
			this.target = quest;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x00067920 File Offset: 0x00065B20
		internal void SetMenu(ISingleSelectionMenu<QuestEntry> menu)
		{
			this.menu = menu;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00067929 File Offset: 0x00065B29
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged += this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged += this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00067967 File Offset: 0x00065B67
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged -= this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged -= this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000679A5 File Offset: 0x00065BA5
		private void OnNeedInspectionChanged(Quest obj)
		{
			this.Refresh();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000679AD File Offset: 0x00065BAD
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000679B5 File Offset: 0x00065BB5
		private void OnMasterSelectionChanged(QuestView view, Quest oldSelection, Quest newSelection)
		{
			this.Refresh();
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000679C0 File Offset: 0x00065BC0
		private void Refresh()
		{
			this.selectionIndicator.SetActive(this.Selected);
			this.displayName.text = this.target.DisplayName;
			this.questIDDisplay.text = string.Format("{0:0000}", this.target.ID);
			SceneInfoEntry requireSceneInfo = this.target.RequireSceneInfo;
			if (requireSceneInfo == null)
			{
				this.locationName.text = this.anyLocationKey.ToPlainText();
			}
			else
			{
				this.locationName.text = requireSceneInfo.DisplayName;
			}
			this.redDot.SetActive(this.target.NeedInspection);
			this.claimableIndicator.SetActive(this.target.Complete || this.target.AreTasksFinished());
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00067A8D File Offset: 0x00065C8D
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<QuestEntry, PointerEventData> action = this.onClick;
			if (action != null)
			{
				action(this, eventData);
			}
			this.menu.SetSelection(this);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00067AAF File Offset: 0x00065CAF
		public void NotifyRefresh()
		{
			this.Refresh();
		}

		// Token: 0x04001400 RID: 5120
		private ISingleSelectionMenu<QuestEntry> menu;

		// Token: 0x04001401 RID: 5121
		private Quest target;

		// Token: 0x04001402 RID: 5122
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001403 RID: 5123
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001404 RID: 5124
		[SerializeField]
		private TextMeshProUGUI locationName;

		// Token: 0x04001405 RID: 5125
		[SerializeField]
		[LocalizationKey("Default")]
		private string anyLocationKey;

		// Token: 0x04001406 RID: 5126
		[SerializeField]
		private GameObject redDot;

		// Token: 0x04001407 RID: 5127
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x04001408 RID: 5128
		[SerializeField]
		private TextMeshProUGUI questIDDisplay;
	}
}
