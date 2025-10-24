using System;
using Saves;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class GameClock : MonoBehaviour
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0003663B File Offset: 0x0003483B
	// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00036642 File Offset: 0x00034842
	public static GameClock Instance { get; private set; }

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06000D09 RID: 3337 RVA: 0x0003664C File Offset: 0x0003484C
	// (remove) Token: 0x06000D0A RID: 3338 RVA: 0x00036680 File Offset: 0x00034880
	public static event Action OnGameClockStep;

	// Token: 0x06000D0B RID: 3339 RVA: 0x000366B3 File Offset: 0x000348B3
	private void Awake()
	{
		if (GameClock.Instance != null)
		{
			Debug.LogError("检测到多个Game Clock");
			return;
		}
		GameClock.Instance = this;
		SavesSystem.OnCollectSaveData += this.Save;
		this.Load();
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x000366EA File Offset: 0x000348EA
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000366FD File Offset: 0x000348FD
	private static string SaveKey
	{
		get
		{
			return "GameClock";
		}
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00036704 File Offset: 0x00034904
	private void Save()
	{
		SavesSystem.Save<GameClock.SaveData>(GameClock.SaveKey, new GameClock.SaveData
		{
			days = this.days,
			secondsOfDay = this.secondsOfDay,
			realTimePlayedTicks = this.RealTimePlayed.Ticks
		});
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00036754 File Offset: 0x00034954
	private void Load()
	{
		GameClock.SaveData saveData = SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey);
		this.days = saveData.days;
		this.secondsOfDay = saveData.secondsOfDay;
		this.realTimePlayed = saveData.RealTimePlayed;
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x000367A0 File Offset: 0x000349A0
	public static TimeSpan GetRealTimePlayedOfSaveSlot(int saveSlot)
	{
		return SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey, saveSlot).RealTimePlayed;
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000367C0 File Offset: 0x000349C0
	private TimeSpan RealTimePlayed
	{
		get
		{
			return this.realTimePlayed;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000D12 RID: 3346 RVA: 0x000367C8 File Offset: 0x000349C8
	private static double SecondsOfDay
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0.0;
			}
			return GameClock.Instance.secondsOfDay;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000367EC File Offset: 0x000349EC
	[TimeSpan]
	private long _TimeOfDayTicks
	{
		get
		{
			return GameClock.TimeOfDay.Ticks;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00036806 File Offset: 0x00034A06
	public static TimeSpan TimeOfDay
	{
		get
		{
			return TimeSpan.FromSeconds(GameClock.SecondsOfDay);
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00036812 File Offset: 0x00034A12
	public static long Day
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0L;
			}
			return GameClock.Instance.days;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0003682E File Offset: 0x00034A2E
	public static TimeSpan Now
	{
		get
		{
			return GameClock.TimeOfDay + TimeSpan.FromDays((double)GameClock.Day);
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00036848 File Offset: 0x00034A48
	public static int Hour
	{
		get
		{
			return GameClock.TimeOfDay.Hours;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00036864 File Offset: 0x00034A64
	public static int Minut
	{
		get
		{
			return GameClock.TimeOfDay.Minutes;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00036880 File Offset: 0x00034A80
	public static int Seconds
	{
		get
		{
			return GameClock.TimeOfDay.Seconds;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0003689C File Offset: 0x00034A9C
	public static int Milliseconds
	{
		get
		{
			return GameClock.TimeOfDay.Milliseconds;
		}
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x000368B6 File Offset: 0x00034AB6
	private void Update()
	{
		this.StepTime(Time.deltaTime * this.clockTimeScale);
		this.realTimePlayed += TimeSpan.FromSeconds((double)Time.unscaledDeltaTime);
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x000368E8 File Offset: 0x00034AE8
	private void StepTime(float deltaTime)
	{
		this.secondsOfDay += (double)deltaTime;
		while (this.secondsOfDay > 86300.0)
		{
			this.days += 1L;
			this.secondsOfDay -= 86300.0;
		}
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0003694C File Offset: 0x00034B4C
	public void StepTimeTil(TimeSpan time)
	{
		if (time.Days > 0)
		{
			time = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
		}
		TimeSpan timeSpan;
		if (time > GameClock.TimeOfDay)
		{
			timeSpan = time - GameClock.TimeOfDay;
		}
		else
		{
			timeSpan = time + TimeSpan.FromDays(1.0) - GameClock.TimeOfDay;
		}
		this.StepTime((float)timeSpan.TotalSeconds);
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x000369C7 File Offset: 0x00034BC7
	internal static void Step(float seconds)
	{
		if (GameClock.Instance == null)
		{
			return;
		}
		GameClock.Instance.StepTime(seconds);
	}

	// Token: 0x04000B43 RID: 2883
	public float clockTimeScale = 60f;

	// Token: 0x04000B44 RID: 2884
	private long days;

	// Token: 0x04000B45 RID: 2885
	private double secondsOfDay;

	// Token: 0x04000B46 RID: 2886
	private TimeSpan realTimePlayed;

	// Token: 0x04000B47 RID: 2887
	private const double SecondsPerDay = 86300.0;

	// Token: 0x020004CE RID: 1230
	[Serializable]
	private struct SaveData
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x0008D790 File Offset: 0x0008B990
		public TimeSpan RealTimePlayed
		{
			get
			{
				return TimeSpan.FromTicks(this.realTimePlayedTicks);
			}
		}

		// Token: 0x04001CD3 RID: 7379
		public long days;

		// Token: 0x04001CD4 RID: 7380
		public double secondsOfDay;

		// Token: 0x04001CD5 RID: 7381
		public long realTimePlayedTicks;
	}
}
