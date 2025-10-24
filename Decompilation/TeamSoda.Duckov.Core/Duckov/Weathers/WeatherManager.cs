using System;
using Saves;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000242 RID: 578
	public class WeatherManager : MonoBehaviour
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00044A3D File Offset: 0x00042C3D
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00044A44 File Offset: 0x00042C44
		public static WeatherManager Instance { get; private set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00044A4C File Offset: 0x00042C4C
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00044A54 File Offset: 0x00042C54
		public bool ForceWeather { get; set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00044A5D File Offset: 0x00042C5D
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00044A65 File Offset: 0x00042C65
		public Weather ForceWeatherValue { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00044A6E File Offset: 0x00042C6E
		public Storm Storm
		{
			get
			{
				return this.storm;
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00044A76 File Offset: 0x00042C76
		private void Awake()
		{
			WeatherManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
			this.Load();
			this._weatherDirty = true;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00044A9C File Offset: 0x00042C9C
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00044AAF File Offset: 0x00042CAF
		private void Save()
		{
			SavesSystem.Save<WeatherManager.SaveData>("WeatherManagerData", new WeatherManager.SaveData(this));
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00044AC4 File Offset: 0x00042CC4
		private void Load()
		{
			WeatherManager.SaveData saveData = SavesSystem.Load<WeatherManager.SaveData>("WeatherManagerData");
			if (!saveData.valid)
			{
				this.SetRandomKey();
			}
			else
			{
				saveData.Setup(this);
			}
			this.SetupModules();
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00044AFA File Offset: 0x00042CFA
		private void SetRandomKey()
		{
			this.seed = UnityEngine.Random.Range(0, 100000);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00044B0D File Offset: 0x00042D0D
		private void SetupModules()
		{
			this.precipitation.SetSeed(this.seed);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00044B20 File Offset: 0x00042D20
		private Weather M_GetWeather(TimeSpan dayAndTime)
		{
			if (this.ForceWeather)
			{
				return this.ForceWeatherValue;
			}
			if (!this._weatherDirty && dayAndTime == this._cachedDayAndTime)
			{
				return this._cachedWeather;
			}
			int stormLevel = this.storm.GetStormLevel(dayAndTime);
			Weather weather;
			if (stormLevel > 0)
			{
				if (stormLevel == 1)
				{
					weather = Weather.Stormy_I;
				}
				else
				{
					weather = Weather.Stormy_II;
				}
			}
			else
			{
				float num = this.precipitation.Get(dayAndTime);
				if (num > this.precipitation.RainyThreshold)
				{
					weather = Weather.Rainy;
				}
				else if (num > this.precipitation.CloudyThreshold)
				{
					weather = Weather.Cloudy;
				}
				else
				{
					weather = Weather.Sunny;
				}
			}
			this._cachedDayAndTime = dayAndTime;
			this._cachedWeather = weather;
			this._weatherDirty = false;
			return weather;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00044BBF File Offset: 0x00042DBF
		private void M_SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			this.ForceWeather = forceWeather;
			this.ForceWeatherValue = value;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00044BCF File Offset: 0x00042DCF
		public static Weather GetWeather(TimeSpan dayAndTime)
		{
			if (WeatherManager.Instance == null)
			{
				return Weather.Sunny;
			}
			return WeatherManager.Instance.M_GetWeather(dayAndTime);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00044BEC File Offset: 0x00042DEC
		public static Weather GetWeather()
		{
			TimeSpan now = GameClock.Now;
			if (WeatherManager.Instance && WeatherManager.Instance.ForceWeather)
			{
				return WeatherManager.Instance.ForceWeatherValue;
			}
			return WeatherManager.GetWeather(now);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00044C28 File Offset: 0x00042E28
		public static void SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			if (WeatherManager.Instance == null)
			{
				return;
			}
			WeatherManager.Instance.M_SetForceWeather(forceWeather, value);
		}

		// Token: 0x04000DD1 RID: 3537
		private int seed = -1;

		// Token: 0x04000DD2 RID: 3538
		[SerializeField]
		private Storm storm = new Storm();

		// Token: 0x04000DD3 RID: 3539
		[SerializeField]
		private Precipitation precipitation = new Precipitation();

		// Token: 0x04000DD4 RID: 3540
		private const string SaveKey = "WeatherManagerData";

		// Token: 0x04000DD5 RID: 3541
		private Weather _cachedWeather;

		// Token: 0x04000DD6 RID: 3542
		private TimeSpan _cachedDayAndTime;

		// Token: 0x04000DD7 RID: 3543
		private bool _weatherDirty;

		// Token: 0x02000531 RID: 1329
		[Serializable]
		private struct SaveData
		{
			// Token: 0x060027BA RID: 10170 RVA: 0x000917D6 File Offset: 0x0008F9D6
			public SaveData(WeatherManager weatherManager)
			{
				this = default(WeatherManager.SaveData);
				this.seed = weatherManager.seed;
				this.valid = true;
			}

			// Token: 0x060027BB RID: 10171 RVA: 0x000917F2 File Offset: 0x0008F9F2
			internal void Setup(WeatherManager weatherManager)
			{
				weatherManager.seed = this.seed;
			}

			// Token: 0x04001E77 RID: 7799
			public bool valid;

			// Token: 0x04001E78 RID: 7800
			public int seed;
		}
	}
}
