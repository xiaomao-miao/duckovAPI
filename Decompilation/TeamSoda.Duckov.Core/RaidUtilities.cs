using System;
using Duckov;
using Saves;
using UnityEngine;

// Token: 0x02000173 RID: 371
public static class RaidUtilities
{
	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0002FEB0 File Offset: 0x0002E0B0
	// (set) Token: 0x06000B4F RID: 2895 RVA: 0x0002FEFC File Offset: 0x0002E0FC
	public static RaidUtilities.RaidInfo CurrentRaid
	{
		get
		{
			RaidUtilities.RaidInfo raidInfo = SavesSystem.Load<RaidUtilities.RaidInfo>("RaidInfo");
			raidInfo.totalTime = Time.unscaledTime - raidInfo.raidBeginTime;
			raidInfo.expOnEnd = EXPManager.EXP;
			raidInfo.expGained = raidInfo.expOnEnd - raidInfo.expOnBegan;
			return raidInfo;
		}
		private set
		{
			SavesSystem.Save<RaidUtilities.RaidInfo>("RaidInfo", value);
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0002FF0C File Offset: 0x0002E10C
	public static void NewRaid()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		RaidUtilities.RaidInfo raidInfo = new RaidUtilities.RaidInfo
		{
			valid = true,
			ID = currentRaid.ID + 1U,
			dead = false,
			ended = false,
			raidBeginTime = Time.unscaledTime,
			raidEndTime = 0f,
			expOnBegan = EXPManager.EXP
		};
		RaidUtilities.CurrentRaid = raidInfo;
		Action<RaidUtilities.RaidInfo> onNewRaid = RaidUtilities.OnNewRaid;
		if (onNewRaid == null)
		{
			return;
		}
		onNewRaid(raidInfo);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0002FF8C File Offset: 0x0002E18C
	public static void NotifyDead()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.dead = true;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd != null)
		{
			onRaidEnd(currentRaid);
		}
		Action<RaidUtilities.RaidInfo> onRaidDead = RaidUtilities.OnRaidDead;
		if (onRaidDead == null)
		{
			return;
		}
		onRaidDead(currentRaid);
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00030018 File Offset: 0x0002E218
	public static void NotifyEnd()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd == null)
		{
			return;
		}
		onRaidEnd(currentRaid);
	}

	// Token: 0x0400099F RID: 2463
	public static Action<RaidUtilities.RaidInfo> OnNewRaid;

	// Token: 0x040009A0 RID: 2464
	public static Action<RaidUtilities.RaidInfo> OnRaidDead;

	// Token: 0x040009A1 RID: 2465
	public static Action<RaidUtilities.RaidInfo> OnRaidEnd;

	// Token: 0x040009A2 RID: 2466
	private const string SaveID = "RaidInfo";

	// Token: 0x020004B4 RID: 1204
	[Serializable]
	public struct RaidInfo
	{
		// Token: 0x04001C68 RID: 7272
		public bool valid;

		// Token: 0x04001C69 RID: 7273
		public uint ID;

		// Token: 0x04001C6A RID: 7274
		public bool dead;

		// Token: 0x04001C6B RID: 7275
		public bool ended;

		// Token: 0x04001C6C RID: 7276
		public float raidBeginTime;

		// Token: 0x04001C6D RID: 7277
		public float raidEndTime;

		// Token: 0x04001C6E RID: 7278
		public float totalTime;

		// Token: 0x04001C6F RID: 7279
		public long expOnBegan;

		// Token: 0x04001C70 RID: 7280
		public long expOnEnd;

		// Token: 0x04001C71 RID: 7281
		public long expGained;
	}
}
