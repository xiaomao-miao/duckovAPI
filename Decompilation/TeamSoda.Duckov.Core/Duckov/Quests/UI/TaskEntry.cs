using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034E RID: 846
	public class TaskEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x00068F2E File Offset: 0x0006712E
		// (set) Token: 0x06001D52 RID: 7506 RVA: 0x00068F36 File Offset: 0x00067136
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

		// Token: 0x06001D53 RID: 7507 RVA: 0x00068F3F File Offset: 0x0006713F
		private void Awake()
		{
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00068F5D File Offset: 0x0006715D
		private void OnInteractionButtonClicked()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.Interact();
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00068F79 File Offset: 0x00067179
		public void NotifyPooled()
		{
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00068F7B File Offset: 0x0006717B
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x00068F8A File Offset: 0x0006718A
		internal void Setup(Task target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x00068FA8 File Offset: 0x000671A8
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.description.text = this.target.Description;
			foreach (string str in this.target.ExtraDescriptsions)
			{
				TextMeshProUGUI textMeshProUGUI = this.description;
				textMeshProUGUI.text = textMeshProUGUI.text + "  \n- " + str;
			}
			Sprite icon = this.target.Icon;
			if (icon)
			{
				this.taskIcon.sprite = icon;
				this.taskIcon.gameObject.SetActive(true);
			}
			else
			{
				this.taskIcon.gameObject.SetActive(false);
			}
			bool flag = this.target.IsFinished();
			this.statusIcon.sprite = (flag ? this.satisfiedIcon : this.unsatisfiedIcon);
			if (this.Interactable && !flag && this.target.Interactable)
			{
				bool possibleValidInteraction = this.target.PossibleValidInteraction;
				this.interactionText.text = this.target.InteractText;
				this.interactionPlaceHolderText.text = this.target.InteractText;
				this.interactionButton.gameObject.SetActive(possibleValidInteraction);
				this.targetNotInteractablePlaceHolder.gameObject.SetActive(!possibleValidInteraction);
				return;
			}
			this.interactionButton.gameObject.SetActive(false);
			this.targetNotInteractablePlaceHolder.gameObject.SetActive(false);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00069120 File Offset: 0x00067320
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x00069148 File Offset: 0x00067348
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00069170 File Offset: 0x00067370
		private void OnTargetStatusChanged(Task task)
		{
			if (task != this.target)
			{
				Debug.LogError("目标不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x00069191 File Offset: 0x00067391
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				this.target.ForceFinish();
				eventData.Use();
			}
		}

		// Token: 0x0400145D RID: 5213
		[SerializeField]
		private Image statusIcon;

		// Token: 0x0400145E RID: 5214
		[SerializeField]
		private Image taskIcon;

		// Token: 0x0400145F RID: 5215
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04001460 RID: 5216
		[SerializeField]
		private Button interactionButton;

		// Token: 0x04001461 RID: 5217
		[SerializeField]
		private GameObject targetNotInteractablePlaceHolder;

		// Token: 0x04001462 RID: 5218
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x04001463 RID: 5219
		[SerializeField]
		private TextMeshProUGUI interactionPlaceHolderText;

		// Token: 0x04001464 RID: 5220
		[SerializeField]
		private Sprite unsatisfiedIcon;

		// Token: 0x04001465 RID: 5221
		[SerializeField]
		private Sprite satisfiedIcon;

		// Token: 0x04001466 RID: 5222
		[SerializeField]
		private bool interactable;

		// Token: 0x04001467 RID: 5223
		private Task target;
	}
}
