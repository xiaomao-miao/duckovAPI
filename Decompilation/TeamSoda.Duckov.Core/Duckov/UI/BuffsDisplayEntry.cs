using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000378 RID: 888
	public class BuffsDisplayEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06001EB6 RID: 7862 RVA: 0x0006BE4C File Offset: 0x0006A04C
		// (remove) Token: 0x06001EB7 RID: 7863 RVA: 0x0006BE80 File Offset: 0x0006A080
		public static event Action<BuffsDisplayEntry, PointerEventData> OnBuffsDisplayEntryClicked;

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0006BEB3 File Offset: 0x0006A0B3
		public Image Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0006BEBC File Offset: 0x0006A0BC
		public void Setup(BuffsDisplay master, Buff target)
		{
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			if (this.displayName)
			{
				this.displayName.text = target.DisplayName;
			}
			this.fadeGroup.Show();
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0006BF11 File Offset: 0x0006A111
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0006BF1C File Offset: 0x0006A11C
		private void Refresh()
		{
			if (this.target == null)
			{
				this.Release();
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				this.remainingTimeText.text = string.Format(this.timeFormat, this.target.RemainingTime);
			}
			else
			{
				this.remainingTimeText.text = "";
			}
			if (this.target.MaxLayers > 1)
			{
				this.layersText.text = this.target.CurrentLayers.ToString();
				return;
			}
			this.layersText.text = "";
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0006BFC0 File Offset: 0x0006A1C0
		public void Release()
		{
			if (this.releasing)
			{
				return;
			}
			this.releasing = true;
			this.ReleaseTask().Forget();
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0006BFE0 File Offset: 0x0006A1E0
		private UniTask ReleaseTask()
		{
			BuffsDisplayEntry.<ReleaseTask>d__19 <ReleaseTask>d__;
			<ReleaseTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ReleaseTask>d__.<>4__this = this;
			<ReleaseTask>d__.<>1__state = -1;
			<ReleaseTask>d__.<>t__builder.Start<BuffsDisplayEntry.<ReleaseTask>d__19>(ref <ReleaseTask>d__);
			return <ReleaseTask>d__.<>t__builder.Task;
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0006C023 File Offset: 0x0006A223
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0006C02B File Offset: 0x0006A22B
		public void NotifyPooled()
		{
			this.pooled = true;
			this.releasing = false;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0006C03B File Offset: 0x0006A23B
		public void NotifyReleased()
		{
			this.pooled = false;
			this.target = null;
			this.releasing = false;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0006C052 File Offset: 0x0006A252
		public void OnPointerClick(PointerEventData eventData)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			Action<BuffsDisplayEntry, PointerEventData> onBuffsDisplayEntryClicked = BuffsDisplayEntry.OnBuffsDisplayEntryClicked;
			if (onBuffsDisplayEntryClicked == null)
			{
				return;
			}
			onBuffsDisplayEntryClicked(this, eventData);
		}

		// Token: 0x040014F8 RID: 5368
		[SerializeField]
		private Image icon;

		// Token: 0x040014F9 RID: 5369
		[SerializeField]
		private TextMeshProUGUI remainingTimeText;

		// Token: 0x040014FA RID: 5370
		[SerializeField]
		private TextMeshProUGUI layersText;

		// Token: 0x040014FB RID: 5371
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040014FC RID: 5372
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040014FD RID: 5373
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x040014FE RID: 5374
		[SerializeField]
		private string timeFormat = "{0:0}s";

		// Token: 0x040014FF RID: 5375
		private BuffsDisplay master;

		// Token: 0x04001500 RID: 5376
		private Buff target;

		// Token: 0x04001501 RID: 5377
		private bool releasing;

		// Token: 0x04001502 RID: 5378
		private bool pooled;
	}
}
