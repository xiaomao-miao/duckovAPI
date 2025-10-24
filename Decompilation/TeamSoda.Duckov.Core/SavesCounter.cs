using System;
using Saves;

// Token: 0x02000125 RID: 293
public class SavesCounter
{
	// Token: 0x06000994 RID: 2452 RVA: 0x000298E4 File Offset: 0x00027AE4
	public static int AddCount(string countKey)
	{
		int num = SavesSystem.Load<int>("Count/" + countKey);
		num++;
		SavesSystem.Save<int>("Count/" + countKey, num);
		return num;
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00029918 File Offset: 0x00027B18
	public static int GetCount(string countKey)
	{
		return SavesSystem.Load<int>("Count/" + countKey);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0002992C File Offset: 0x00027B2C
	public static int AddKillCount(string key)
	{
		int num = SavesCounter.AddCount("Kills/" + key);
		Action<string, int> onKillCountChanged = SavesCounter.OnKillCountChanged;
		if (onKillCountChanged != null)
		{
			onKillCountChanged(key, num);
		}
		return num;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0002995D File Offset: 0x00027B5D
	public static int GetKillCount(string key)
	{
		return SavesCounter.GetCount("Kills/" + key);
	}

	// Token: 0x0400086B RID: 2155
	public static Action<string, int> OnKillCountChanged;
}
