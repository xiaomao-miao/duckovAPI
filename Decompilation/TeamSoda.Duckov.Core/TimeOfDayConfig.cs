using System;
using Duckov.Weathers;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200018F RID: 399
public class TimeOfDayConfig : MonoBehaviour
{
	// Token: 0x06000BD0 RID: 3024 RVA: 0x0003217C File Offset: 0x0003037C
	public TimeOfDayEntry GetCurrentEntry(Weather weather)
	{
		switch (weather)
		{
		case Weather.Sunny:
			return this.defaultEntry;
		case Weather.Cloudy:
			return this.cloudyEntry;
		case Weather.Rainy:
			return this.rainyEntry;
		case Weather.Stormy_I:
			return this.stormIEntry;
		case Weather.Stormy_II:
			return this.stormIIEntry;
		default:
			return this.defaultEntry;
		}
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x000321D0 File Offset: 0x000303D0
	public void InvokeDebug()
	{
		TimeOfDayEntry currentEntry = this.GetCurrentEntry(this.debugWeather);
		if (!currentEntry)
		{
			Debug.Log("No entry found");
			return;
		}
		TimeOfDayPhase phase = currentEntry.GetPhase(this.debugPhase);
		if (!Application.isPlaying)
		{
			if (this.lookDevVolume && this.lookDevVolume.profile != phase.volumeProfile)
			{
				this.lookDevVolume.profile = phase.volumeProfile;
				return;
			}
		}
		else
		{
			int num;
			switch (this.debugPhase)
			{
			case TimePhaseTags.day:
				num = 9;
				break;
			case TimePhaseTags.dawn:
				num = 17;
				break;
			case TimePhaseTags.night:
				num = 22;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			WeatherManager.SetForceWeather(true, this.debugWeather);
			TimeSpan time = new TimeSpan(num, 10, 0);
			GameClock.Instance.StepTimeTil(time);
			Debug.Log(string.Format("Set Weather to {0},and time to {1}", this.debugWeather, num));
		}
	}

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private TimeOfDayEntry defaultEntry;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	private TimeOfDayEntry cloudyEntry;

	// Token: 0x04000A29 RID: 2601
	[SerializeField]
	private TimeOfDayEntry rainyEntry;

	// Token: 0x04000A2A RID: 2602
	[SerializeField]
	private TimeOfDayEntry stormIEntry;

	// Token: 0x04000A2B RID: 2603
	[SerializeField]
	private TimeOfDayEntry stormIIEntry;

	// Token: 0x04000A2C RID: 2604
	public bool forceSetTime;

	// Token: 0x04000A2D RID: 2605
	[Range(0f, 24f)]
	public int forceSetTimeTo = 8;

	// Token: 0x04000A2E RID: 2606
	public bool forceSetWeather;

	// Token: 0x04000A2F RID: 2607
	public Weather forceSetWeatherTo;

	// Token: 0x04000A30 RID: 2608
	[SerializeField]
	private Volume lookDevVolume;

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private TimePhaseTags debugPhase;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private Weather debugWeather;
}
