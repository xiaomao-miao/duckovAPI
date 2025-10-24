using System;
using Duckov.Buffs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000376 RID: 886
	public class BuffDetailsOverlay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001E9E RID: 7838 RVA: 0x0006B9A9 File Offset: 0x00069BA9
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0006B9B1 File Offset: 0x00069BB1
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked += this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0006B9D5 File Offset: 0x00069BD5
		private void OnDestroy()
		{
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked -= this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0006B9E8 File Offset: 0x00069BE8
		private void OnBuffsDisplayEntryClicked(BuffsDisplayEntry entry, PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown && this.target == entry.Target)
			{
				this.fadeGroup.Hide();
				this.punchReceiver.Punch();
				return;
			}
			this.Setup(entry);
			this.Show();
			this.punchReceiver.Punch();
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0006BA44 File Offset: 0x00069C44
		public void Setup(Buff target)
		{
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.text_BuffName.text = target.DisplayName;
			this.text_BuffDescription.text = target.Description;
			this.RefreshCountDown();
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0006BA80 File Offset: 0x00069C80
		private void Update()
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				if (this.target != null)
				{
					this.RefreshCountDown();
				}
				else
				{
					this.fadeGroup.Hide();
				}
				if (this.TimeSinceShowStarted > this.disappearAfterSeconds)
				{
					this.fadeGroup.Hide();
				}
			}
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0006BAE4 File Offset: 0x00069CE4
		public void Setup(BuffsDisplayEntry target)
		{
			if (target == null)
			{
				return;
			}
			this.Setup((target != null) ? target.Target : null);
			RectTransform rectTransform = target.Icon.rectTransform;
			Vector3 position = rectTransform.TransformPoint(rectTransform.rect.max);
			this.rectTransform.pivot = Vector2.up;
			this.rectTransform.position = position;
			this.rectTransform.SetAsLastSibling();
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0006BB58 File Offset: 0x00069D58
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				float remainingTime = this.target.RemainingTime;
				this.text_CountDown.text = string.Format("{0:0.0}s", remainingTime);
				return;
			}
			this.text_CountDown.text = "";
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x0006BBB9 File Offset: 0x00069DB9
		private float TimeSinceShowStarted
		{
			get
			{
				return Time.unscaledTime - this.timeWhenShowStarted;
			}
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0006BBC7 File Offset: 0x00069DC7
		public void Show()
		{
			this.fadeGroup.Show();
			this.timeWhenShowStarted = Time.unscaledTime;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0006BBDF File Offset: 0x00069DDF
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				this.punchReceiver.Punch();
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x040014EA RID: 5354
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040014EB RID: 5355
		[SerializeField]
		private TextMeshProUGUI text_BuffName;

		// Token: 0x040014EC RID: 5356
		[SerializeField]
		private TextMeshProUGUI text_BuffDescription;

		// Token: 0x040014ED RID: 5357
		[SerializeField]
		private TextMeshProUGUI text_CountDown;

		// Token: 0x040014EE RID: 5358
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x040014EF RID: 5359
		[SerializeField]
		private float disappearAfterSeconds = 5f;

		// Token: 0x040014F0 RID: 5360
		private RectTransform rectTransform;

		// Token: 0x040014F1 RID: 5361
		private Buff target;

		// Token: 0x040014F2 RID: 5362
		private float timeWhenShowStarted;
	}
}
