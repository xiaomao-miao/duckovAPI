using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class TimeOfDayAlert : MonoBehaviour
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000660 RID: 1632 RVA: 0x0001CD3C File Offset: 0x0001AF3C
	// (remove) Token: 0x06000661 RID: 1633 RVA: 0x0001CD70 File Offset: 0x0001AF70
	public static event Action OnAlertTriggeredEvent;

	// Token: 0x06000662 RID: 1634 RVA: 0x0001CDA3 File Offset: 0x0001AFA3
	private void Awake()
	{
		this.canvasGroup.alpha = 0f;
		TimeOfDayAlert.OnAlertTriggeredEvent += this.OnAlertTriggered;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0001CDC6 File Offset: 0x0001AFC6
	private void OnDestroy()
	{
		TimeOfDayAlert.OnAlertTriggeredEvent -= this.OnAlertTriggered;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0001CDDC File Offset: 0x0001AFDC
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!LevelManager.Instance.IsBaseLevel)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		if (this.timer <= 0f && this.canvasGroup.alpha > 0f)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, 0f, 0.4f * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0001CE74 File Offset: 0x0001B074
	private void OnAlertTriggered()
	{
		bool flag = false;
		float time = TimeOfDayController.Instance.Time;
		if (TimeOfDayController.Instance.AtNight)
		{
			flag = true;
			Debug.Log(string.Format("At Night,time:{0}", time));
			this.text.text = this.inNightKey.ToPlainText();
		}
		else if (TimeOfDayController.Instance.nightStart - time < 4f)
		{
			flag = true;
			Debug.Log(string.Format("Near Night,time:{0},night start:{1}", time, TimeOfDayController.Instance.nightStart));
			this.text.text = this.nearNightKey.ToPlainText();
		}
		if (!flag)
		{
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.timer = this.stayTime;
		this.blinkPunch.Punch();
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0001CF43 File Offset: 0x0001B143
	public static void EnterAlertTrigger()
	{
		Action onAlertTriggeredEvent = TimeOfDayAlert.OnAlertTriggeredEvent;
		if (onAlertTriggeredEvent == null)
		{
			return;
		}
		onAlertTriggeredEvent();
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0001CF54 File Offset: 0x0001B154
	public static void LeaveAlertTrigger()
	{
	}

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000626 RID: 1574
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x04000627 RID: 1575
	[SerializeField]
	private ColorPunch blinkPunch;

	// Token: 0x04000629 RID: 1577
	[LocalizationKey("Default")]
	public string nearNightKey = "TODAlert_NearNight";

	// Token: 0x0400062A RID: 1578
	[LocalizationKey("Default")]
	public string inNightKey = "TODAlert_InNight";

	// Token: 0x0400062B RID: 1579
	private float stayTime = 5f;

	// Token: 0x0400062C RID: 1580
	private float timer;
}
