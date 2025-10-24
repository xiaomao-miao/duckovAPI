using System;
using Steamworks;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class SteamStatsAPITest : MonoBehaviour
{
	// Token: 0x06000E6A RID: 3690 RVA: 0x0003A041 File Offset: 0x00038241
	private void Awake()
	{
		this.onStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatReceived));
		this.onStatsStoredCallback = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatStored));
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0003A071 File Offset: 0x00038271
	private void OnUserStatStored(UserStatsStored_t param)
	{
		Debug.Log("Stat Stored!");
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0003A080 File Offset: 0x00038280
	private void OnUserStatReceived(UserStatsReceived_t param)
	{
		string str = "Stat Fetched:";
		CSteamID steamIDUser = param.m_steamIDUser;
		Debug.Log(str + steamIDUser.ToString() + " " + param.m_nGameID.ToString());
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x0003A0C1 File Offset: 0x000382C1
	private void Start()
	{
		SteamUserStats.RequestGlobalStats(60);
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0003A0CC File Offset: 0x000382CC
	private void Test()
	{
		int num;
		Debug.Log(SteamUserStats.GetStat("game_finished", out num).ToString() + " " + num.ToString());
		bool flag = SteamUserStats.SetStat("game_finished", num + 1);
		Debug.Log(string.Format("Set: {0}", flag));
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0003A12C File Offset: 0x0003832C
	private void GetGlobalStat()
	{
		long num;
		if (SteamUserStats.GetGlobalStat("game_finished", out num))
		{
			Debug.Log(string.Format("game finished: {0}", num));
			return;
		}
		Debug.Log("Failed");
	}

	// Token: 0x04000BF0 RID: 3056
	private Callback<UserStatsReceived_t> onStatsReceivedCallback;

	// Token: 0x04000BF1 RID: 3057
	private Callback<UserStatsStored_t> onStatsStoredCallback;
}
