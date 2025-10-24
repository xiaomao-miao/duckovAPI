using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000386 RID: 902
	public class SleepView : View
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0006DB05 File Offset: 0x0006BD05
		public static SleepView Instance
		{
			get
			{
				return View.GetViewInstance<SleepView>();
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x0006DB0C File Offset: 0x0006BD0C
		private TimeSpan SleepTimeSpan
		{
			get
			{
				return TimeSpan.FromMinutes((double)this.sleepForMinuts);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0006DB1A File Offset: 0x0006BD1A
		private TimeSpan WillWakeUpAt
		{
			get
			{
				return GameClock.TimeOfDay + this.SleepTimeSpan;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0006DB2C File Offset: 0x0006BD2C
		private bool WillWakeUpNextDay
		{
			get
			{
				return this.WillWakeUpAt.Days > 0;
			}
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0006DB4A File Offset: 0x0006BD4A
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0006DB5D File Offset: 0x0006BD5D
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0006DB70 File Offset: 0x0006BD70
		protected override void Awake()
		{
			base.Awake();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0006DBB0 File Offset: 0x0006BDB0
		private void OnConfirmButtonClicked()
		{
			this.Sleep((float)this.sleepForMinuts).Forget();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0006DBC4 File Offset: 0x0006BDC4
		private UniTask Sleep(float minuts)
		{
			SleepView.<Sleep>d__21 <Sleep>d__;
			<Sleep>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sleep>d__.<>4__this = this;
			<Sleep>d__.minuts = minuts;
			<Sleep>d__.<>1__state = -1;
			<Sleep>d__.<>t__builder.Start<SleepView.<Sleep>d__21>(ref <Sleep>d__);
			return <Sleep>d__.<>t__builder.Task;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0006DC0F File Offset: 0x0006BE0F
		private void OnGameClockStep()
		{
			this.Refresh();
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0006DC17 File Offset: 0x0006BE17
		private void OnEnable()
		{
			this.InitializeUI();
			GameClock.OnGameClockStep += this.OnGameClockStep;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0006DC30 File Offset: 0x0006BE30
		private void OnDisable()
		{
			GameClock.OnGameClockStep -= this.OnGameClockStep;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0006DC43 File Offset: 0x0006BE43
		private void OnSliderValueChanged(float newValue)
		{
			this.sleepForMinuts = Mathf.RoundToInt(newValue);
			this.Refresh();
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0006DC57 File Offset: 0x0006BE57
		private void InitializeUI()
		{
			this.slider.SetValueWithoutNotify((float)this.sleepForMinuts);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0006DC6B File Offset: 0x0006BE6B
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0006DC74 File Offset: 0x0006BE74
		private void Refresh()
		{
			TimeSpan willWakeUpAt = this.WillWakeUpAt;
			this.willWakeUpAtText.text = string.Format("{0:00}:{1:00}", willWakeUpAt.Hours, willWakeUpAt.Minutes);
			TimeSpan sleepTimeSpan = this.SleepTimeSpan;
			this.sleepTimeSpanText.text = string.Format("{0:00} h {1:00} min", (int)sleepTimeSpan.TotalHours, sleepTimeSpan.Minutes);
			this.nextDayIndicator.gameObject.SetActive(willWakeUpAt.Days > 0);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0006DD04 File Offset: 0x0006BF04
		public static void Show()
		{
			if (SleepView.Instance == null)
			{
				return;
			}
			SleepView.Instance.Open(null);
		}

		// Token: 0x04001560 RID: 5472
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001561 RID: 5473
		[SerializeField]
		private Slider slider;

		// Token: 0x04001562 RID: 5474
		[SerializeField]
		private TextMeshProUGUI willWakeUpAtText;

		// Token: 0x04001563 RID: 5475
		[SerializeField]
		private TextMeshProUGUI sleepTimeSpanText;

		// Token: 0x04001564 RID: 5476
		[SerializeField]
		private GameObject nextDayIndicator;

		// Token: 0x04001565 RID: 5477
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04001566 RID: 5478
		private int sleepForMinuts;

		// Token: 0x04001567 RID: 5479
		public static Action OnAfterSleep;

		// Token: 0x04001568 RID: 5480
		private bool sleeping;
	}
}
