using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C4 RID: 196
public class EvacuationCountdownUI : MonoBehaviour
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001BE46 File Offset: 0x0001A046
	public static EvacuationCountdownUI Instance
	{
		get
		{
			return EvacuationCountdownUI._instance;
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0001BE4D File Offset: 0x0001A04D
	private void Awake()
	{
		if (EvacuationCountdownUI._instance == null)
		{
			EvacuationCountdownUI._instance = this;
		}
		if (EvacuationCountdownUI._instance != this)
		{
			Debug.LogWarning("Multiple Evacuation Countdown UI detected");
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0001BE7C File Offset: 0x0001A07C
	private string ToDigitString(float number)
	{
		int num = (int)number;
		int num2 = Mathf.Min(999, Mathf.RoundToInt((number - (float)num) * 1000f));
		int num3 = num / 60;
		num -= num3 * 60;
		return string.Format(this.digitFormat, num3, num, num2);
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001BECF File Offset: 0x0001A0CF
	private void Update()
	{
		if (this.target == null && this.fadeGroup.IsShown)
		{
			this.Hide().Forget();
		}
		this.Refresh();
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0001BF00 File Offset: 0x0001A100
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.progressFill.fillAmount = this.target.Progress;
		this.countdownDigit.text = this.ToDigitString(this.target.RemainingTime);
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0001BF50 File Offset: 0x0001A150
	private UniTask Hide()
	{
		EvacuationCountdownUI.<Hide>d__12 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<EvacuationCountdownUI.<Hide>d__12>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0001BF94 File Offset: 0x0001A194
	private UniTask Show(CountDownArea target)
	{
		EvacuationCountdownUI.<Show>d__13 <Show>d__;
		<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Show>d__.<>4__this = this;
		<Show>d__.target = target;
		<Show>d__.<>1__state = -1;
		<Show>d__.<>t__builder.Start<EvacuationCountdownUI.<Show>d__13>(ref <Show>d__);
		return <Show>d__.<>t__builder.Task;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0001BFDF File Offset: 0x0001A1DF
	public static void Request(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		EvacuationCountdownUI.Instance.Show(target).Forget();
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0001BFFF File Offset: 0x0001A1FF
	public static void Release(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		if (EvacuationCountdownUI.Instance.target == target)
		{
			EvacuationCountdownUI.Instance.Hide().Forget();
		}
	}

	// Token: 0x040005EC RID: 1516
	private static EvacuationCountdownUI _instance;

	// Token: 0x040005ED RID: 1517
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x040005EE RID: 1518
	[SerializeField]
	private Image progressFill;

	// Token: 0x040005EF RID: 1519
	[SerializeField]
	private TextMeshProUGUI countdownDigit;

	// Token: 0x040005F0 RID: 1520
	[SerializeField]
	private string digitFormat = "{0:00}:{1:00}<sub>.{2:000}</sub>";

	// Token: 0x040005F1 RID: 1521
	private CountDownArea target;
}
