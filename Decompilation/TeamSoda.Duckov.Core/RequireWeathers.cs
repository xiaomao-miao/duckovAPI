using System;
using System.Collections.Generic;
using Duckov.Quests;
using Duckov.Weathers;

// Token: 0x0200011F RID: 287
public class RequireWeathers : Condition
{
	// Token: 0x0600097F RID: 2431 RVA: 0x00029628 File Offset: 0x00027828
	public override bool Evaluate()
	{
		if (!LevelManager.LevelInited)
		{
			return false;
		}
		Weather currentWeather = LevelManager.Instance.TimeOfDayController.CurrentWeather;
		return this.weathers.Contains(currentWeather);
	}

	// Token: 0x04000862 RID: 2146
	public List<Weather> weathers;
}
