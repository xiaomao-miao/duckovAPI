using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B0 RID: 944
	public class ClosureView : View
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x00076093 File Offset: 0x00074293
		public static ClosureView Instance
		{
			get
			{
				return View.GetViewInstance<ClosureView>();
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0007609A File Offset: 0x0007429A
		private string EvacuatedTitleText
		{
			get
			{
				return this.evacuatedTitleTextKey.ToPlainText();
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000760A7 File Offset: 0x000742A7
		private string FailedTitleText
		{
			get
			{
				return this.failedTitleTextKey.ToPlainText();
			}
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000760B4 File Offset: 0x000742B4
		protected override void Awake()
		{
			base.Awake();
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000760D8 File Offset: 0x000742D8
		private void OnContinueButtonClicked()
		{
			if (!this.canContinue)
			{
				return;
			}
			this.continueButtonClicked = true;
			this.contentFadeGroup.Hide();
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000760F5 File Offset: 0x000742F5
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.contentFadeGroup.Show();
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00076113 File Offset: 0x00074313
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00076128 File Offset: 0x00074328
		public static UniTask ShowAndReturnTask(float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__36 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__36>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0007616C File Offset: 0x0007436C
		public static UniTask ShowAndReturnTask(DamageInfo dmgInfo, float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__37 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.dmgInfo = dmgInfo;
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__37>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000761B7 File Offset: 0x000743B7
		private void SetupDamageInfo(DamageInfo dmgInfo)
		{
			this.damageSourceText.text = dmgInfo.GenerateDescription();
			this.damageInfoContainer.gameObject.SetActive(true);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000761DC File Offset: 0x000743DC
		private UniTask ClosureTask()
		{
			ClosureView.<ClosureTask>d__39 <ClosureTask>d__;
			<ClosureTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ClosureTask>d__.<>4__this = this;
			<ClosureTask>d__.<>1__state = -1;
			<ClosureTask>d__.<>t__builder.Start<ClosureView.<ClosureTask>d__39>(ref <ClosureTask>d__);
			return <ClosureTask>d__.<>t__builder.Task;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00076220 File Offset: 0x00074420
		private void SetupBeginning()
		{
			long cachedExp = EXPManager.CachedExp;
			long exp = EXPManager.EXP;
			this.Refresh(0f, cachedExp, exp);
			this.continueButton.gameObject.SetActive(false);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00076258 File Offset: 0x00074458
		private void SetupTitle(bool dead)
		{
			if (dead)
			{
				this.titleText.color = this.failedTitleTextColor;
				this.titleText.text = this.FailedTitleText;
				return;
			}
			this.titleText.color = this.evacuatedTitleTextColor;
			this.titleText.text = this.EvacuatedTitleText;
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000762B0 File Offset: 0x000744B0
		private UniTask AnimateExpBar(long fromExp, long toExp)
		{
			ClosureView.<AnimateExpBar>d__42 <AnimateExpBar>d__;
			<AnimateExpBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateExpBar>d__.<>4__this = this;
			<AnimateExpBar>d__.fromExp = fromExp;
			<AnimateExpBar>d__.toExp = toExp;
			<AnimateExpBar>d__.<>1__state = -1;
			<AnimateExpBar>d__.<>t__builder.Start<ClosureView.<AnimateExpBar>d__42>(ref <AnimateExpBar>d__);
			return <AnimateExpBar>d__.<>t__builder.Task;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00076304 File Offset: 0x00074504
		private void SpitExpUpSfx(float expDelta)
		{
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime - this.lastTimeExpUpSfxPlayed < 0.05f)
			{
				return;
			}
			this.lastTimeExpUpSfxPlayed = unscaledTime;
			AudioManager.SetRTPC("ExpDelta", expDelta, null);
			AudioManager.Post(this.sfx_ExpUp);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00076348 File Offset: 0x00074548
		private long Refresh(float t, long fromExp, long toExp)
		{
			long num = this.LongLerp(fromExp, toExp, t);
			this.SetExpDisplay(num, fromExp);
			this.SetLevelDisplay(this.cachedLevel);
			return num;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00076374 File Offset: 0x00074574
		private long LongLerp(long from, long to, float t)
		{
			return (long)((float)(to - from) * t) + from;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00076380 File Offset: 0x00074580
		private void CacheLevelInfo(int level)
		{
			if (level == this.cachedLevel)
			{
				return;
			}
			this.cachedLevel = level;
			this.cachedLevelRange = EXPManager.Instance.GetLevelExpRange(level);
			this.cachedLevelLength = this.cachedLevelRange.Item2 - this.cachedLevelRange.Item1;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000763CC File Offset: 0x000745CC
		private void SetExpDisplay(long currentExp, long oldExp)
		{
			int level = EXPManager.Instance.LevelFromExp(currentExp);
			this.CacheLevelInfo(level);
			float fillAmount = 0f;
			if (oldExp >= this.cachedLevelRange.Item1 && oldExp <= this.cachedLevelRange.Item2)
			{
				fillAmount = (float)(oldExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			}
			float fillAmount2 = (float)(currentExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			this.expBar_OldFill.fillAmount = fillAmount;
			this.expBar_CurrentFill.fillAmount = fillAmount2;
			this.expDisplay.text = string.Format(this.expFormat, currentExp, this.cachedLevelRange.Item2);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00076480 File Offset: 0x00074680
		private void SetLevelDisplay(int level)
		{
			if (this.displayingLevel > 0 && level != this.displayingLevel)
			{
				this.LevelUpPunch();
			}
			this.displayingLevel = level;
			this.levelDisplay.text = string.Format(this.levelFormat, level);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000764BD File Offset: 0x000746BD
		private void LevelUpPunch()
		{
			PunchReceiver punchReceiver = this.levelDisplayPunchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			PunchReceiver punchReceiver2 = this.barPunchReceiver;
			if (punchReceiver2 != null)
			{
				punchReceiver2.Punch();
			}
			AudioManager.Post(this.sfx_LvUp);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000764ED File Offset: 0x000746ED
		internal override void TryQuit()
		{
		}

		// Token: 0x040016E7 RID: 5863
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016E8 RID: 5864
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x040016E9 RID: 5865
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x040016EA RID: 5866
		[SerializeField]
		[LocalizationKey("Default")]
		private string evacuatedTitleTextKey = "UI_Closure_Escaped";

		// Token: 0x040016EB RID: 5867
		[SerializeField]
		private Color evacuatedTitleTextColor = Color.white;

		// Token: 0x040016EC RID: 5868
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTitleTextKey = "UI_Closure_Dead";

		// Token: 0x040016ED RID: 5869
		[SerializeField]
		private Color failedTitleTextColor = Color.red;

		// Token: 0x040016EE RID: 5870
		[SerializeField]
		private GameObject damageInfoContainer;

		// Token: 0x040016EF RID: 5871
		[SerializeField]
		private TextMeshProUGUI damageSourceText;

		// Token: 0x040016F0 RID: 5872
		[SerializeField]
		private Image expBar_OldFill;

		// Token: 0x040016F1 RID: 5873
		[SerializeField]
		private Image expBar_CurrentFill;

		// Token: 0x040016F2 RID: 5874
		[SerializeField]
		private AnimationCurve expBarAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040016F3 RID: 5875
		[SerializeField]
		private float expBarAnimationTime = 3f;

		// Token: 0x040016F4 RID: 5876
		[SerializeField]
		private TextMeshProUGUI expDisplay;

		// Token: 0x040016F5 RID: 5877
		[SerializeField]
		private string expFormat = "{0}/<sub>{1}</sub>";

		// Token: 0x040016F6 RID: 5878
		[SerializeField]
		private TextMeshProUGUI levelDisplay;

		// Token: 0x040016F7 RID: 5879
		[SerializeField]
		private string levelFormat = "Lv.{0}";

		// Token: 0x040016F8 RID: 5880
		[SerializeField]
		private PunchReceiver levelDisplayPunchReceiver;

		// Token: 0x040016F9 RID: 5881
		[SerializeField]
		private PunchReceiver barPunchReceiver;

		// Token: 0x040016FA RID: 5882
		[SerializeField]
		private Button continueButton;

		// Token: 0x040016FB RID: 5883
		[SerializeField]
		private PunchReceiver continueButtonPunchReceiver;

		// Token: 0x040016FC RID: 5884
		private string sfx_Pop = "UI/pop";

		// Token: 0x040016FD RID: 5885
		private string sfx_ExpUp = "UI/exp_up";

		// Token: 0x040016FE RID: 5886
		private string sfx_LvUp = "UI/level_up";

		// Token: 0x040016FF RID: 5887
		private bool continueButtonClicked;

		// Token: 0x04001700 RID: 5888
		private bool canContinue;

		// Token: 0x04001701 RID: 5889
		private float lastTimeExpUpSfxPlayed = float.MinValue;

		// Token: 0x04001702 RID: 5890
		private const float minIntervalForExpUpSfx = 0.05f;

		// Token: 0x04001703 RID: 5891
		private int cachedLevel = -1;

		// Token: 0x04001704 RID: 5892
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		private ValueTuple<long, long> cachedLevelRange;

		// Token: 0x04001705 RID: 5893
		private long cachedLevelLength;

		// Token: 0x04001706 RID: 5894
		private int displayingLevel = -1;
	}
}
