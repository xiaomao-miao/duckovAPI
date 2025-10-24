using System;
using Duckov.Weathers;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D1 RID: 209
public class TimeOfDayDisplay : MonoBehaviour
{
	// Token: 0x0600066C RID: 1644 RVA: 0x0001CF95 File Offset: 0x0001B195
	private void Start()
	{
		this.RefreshPhase(TimeOfDayController.Instance.CurrentPhase.timePhaseTag);
		this.RefreshWeather(TimeOfDayController.Instance.CurrentWeather);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0001CFBC File Offset: 0x0001B1BC
	private void Update()
	{
		this.refreshTimer -= Time.unscaledDeltaTime;
		if (this.refreshTimer > 0f)
		{
			return;
		}
		this.refreshTimer = this.refreshTimeSpace;
		TimePhaseTags timePhaseTag = TimeOfDayController.Instance.CurrentPhase.timePhaseTag;
		if (this.currentPhaseTag != timePhaseTag)
		{
			this.RefreshPhase(timePhaseTag);
		}
		Weather weather = TimeOfDayController.Instance.CurrentWeather;
		if (this.currentWeather != weather)
		{
			this.RefreshWeather(weather);
		}
		this.RefreshStormText(weather);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0001D038 File Offset: 0x0001B238
	private void RefreshStormText(Weather _weather)
	{
		TimeSpan timeSpan = default(TimeSpan);
		float fillAmount;
		if (_weather == Weather.Stormy_I)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = this.StormPhaseIIETAKey.ToPlainText();
			timeSpan = WeatherManager.Instance.Storm.GetStormIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else if (_weather == Weather.Stormy_II)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = this.StormOverETAKey.ToPlainText();
			timeSpan = WeatherManager.Instance.Storm.GetStormIIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else
		{
			this.stormIndicatorAnimator.SetBool("Grow", true);
			fillAmount = WeatherManager.Instance.Storm.GetSleepPercent(GameClock.Now);
			timeSpan = WeatherManager.Instance.Storm.GetStormETA(GameClock.Now);
			if (timeSpan.TotalHours < 24.0)
			{
				this.stormTitleText.text = this.StormComingOneDayKey.ToPlainText();
				this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
			}
			else
			{
				this.stormTitleText.text = this.StormComingETAKey.ToPlainText();
				this.stormDescObject.SetActive(false);
			}
		}
		this.stormFillImage.fillAmount = fillAmount;
		this.stormText.text = string.Format("{0:000}:{1:00}", Mathf.FloorToInt((float)timeSpan.TotalHours), timeSpan.Minutes);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0001D20C File Offset: 0x0001B40C
	private void RefreshPhase(TimePhaseTags _phase)
	{
		this.currentPhaseTag = _phase;
		this.phaseText.text = TimeOfDayController.GetTimePhaseNameByPhaseTag(_phase);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0001D226 File Offset: 0x0001B426
	private void RefreshWeather(Weather _weather)
	{
		this.currentWeather = _weather;
		this.weatherText.text = TimeOfDayController.GetWeatherNameByWeather(_weather);
	}

	// Token: 0x0400062D RID: 1581
	private TimePhaseTags currentPhaseTag;

	// Token: 0x0400062E RID: 1582
	private Weather currentWeather;

	// Token: 0x0400062F RID: 1583
	public TextMeshProUGUI phaseText;

	// Token: 0x04000630 RID: 1584
	public TextMeshProUGUI weatherText;

	// Token: 0x04000631 RID: 1585
	public TextMeshProUGUI stormTitleText;

	// Token: 0x04000632 RID: 1586
	public TextMeshProUGUI stormText;

	// Token: 0x04000633 RID: 1587
	[LocalizationKey("Default")]
	public string StormComingETAKey = "StormETA";

	// Token: 0x04000634 RID: 1588
	[LocalizationKey("Default")]
	public string StormComingOneDayKey = "StormOneDayETA";

	// Token: 0x04000635 RID: 1589
	[LocalizationKey("Default")]
	public string StormPhaseIIETAKey = "StormPhaseIIETA";

	// Token: 0x04000636 RID: 1590
	[LocalizationKey("Default")]
	public string StormOverETAKey = "StormOverETA";

	// Token: 0x04000637 RID: 1591
	public GameObject stormDescObject;

	// Token: 0x04000638 RID: 1592
	private float refreshTimeSpace = 0.5f;

	// Token: 0x04000639 RID: 1593
	private float refreshTimer;

	// Token: 0x0400063A RID: 1594
	public Animator stormIndicatorAnimator;

	// Token: 0x0400063B RID: 1595
	public ProceduralImage stormFillImage;
}
