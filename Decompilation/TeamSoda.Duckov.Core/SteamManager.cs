using System;
using System.Text;
using AOT;
using Duckov;
using Duckov.Achievements;
using Steamworks;
using UnityEngine;

// Token: 0x02000134 RID: 308
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x0002A86B File Offset: 0x00028A6B
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				Debug.Log("Creating steam manager");
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x0002A899 File Offset: 0x00028A99
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0002A8A5 File Offset: 0x00028AA5
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0002A8AD File Offset: 0x00028AAD
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0002A8BC File Offset: 0x00028ABC
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)3167020U))
			{
				Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
		AchievementManager.OnAchievementUnlocked += this.OnAchievementUnlocked;
		AchievementManager.OnAchievementDataLoaded += this.OnAchievementDataLoaded;
		RichPresenceManager.OnInstanceChanged = (Action<RichPresenceManager>)Delegate.Combine(RichPresenceManager.OnInstanceChanged, new Action<RichPresenceManager>(this.OnRichPresenceChanged));
		PlatformInfo.GetIDFunc = new Func<string>(SteamManager.GetID);
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0002AA00 File Offset: 0x00028C00
	private static string GetID()
	{
		if (SteamManager.s_instance == null)
		{
			return null;
		}
		if (!SteamManager.Initialized)
		{
			return null;
		}
		return SteamUser.GetSteamID().ToString();
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0002AA38 File Offset: 0x00028C38
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
		AchievementManager.OnAchievementUnlocked -= this.OnAchievementUnlocked;
		AchievementManager.OnAchievementDataLoaded -= this.OnAchievementDataLoaded;
		RichPresenceManager.OnInstanceChanged = (Action<RichPresenceManager>)Delegate.Remove(RichPresenceManager.OnInstanceChanged, new Action<RichPresenceManager>(this.OnRichPresenceChanged));
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0002AAAC File Offset: 0x00028CAC
	private void OnRichPresenceChanged(RichPresenceManager manager)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (manager == null)
		{
			return;
		}
		string steamDisplay = manager.GetSteamDisplay();
		if (!SteamFriends.SetRichPresence("steam_display", steamDisplay))
		{
			Debug.LogError("Failed setting rich presence: level = " + steamDisplay);
		}
		if (!SteamFriends.SetRichPresence("level", manager.levelDisplayNameRaw))
		{
			Debug.LogError("Failed setting rich presence: level = " + manager.levelDisplayNameRaw);
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0002AB18 File Offset: 0x00028D18
	private void OnAchievementDataLoaded(AchievementManager manager)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (manager == null)
		{
			return;
		}
		bool flag = false;
		foreach (string text in manager.UnlockedAchievements)
		{
			bool flag2;
			if (SteamUserStats.GetAchievement(text, out flag2) && !flag2)
			{
				SteamUserStats.SetAchievement(text);
				flag = true;
			}
		}
		if (flag)
		{
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002AB98 File Offset: 0x00028D98
	private void OnAchievementUnlocked(string achievementKey)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetAchievement(achievementKey);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002ABB0 File Offset: 0x00028DB0
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0002ABFE File Offset: 0x00028DFE
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040008B7 RID: 2231
	public const bool SteamEnabled = true;

	// Token: 0x040008B8 RID: 2232
	public const int AppID_Int = 3167020;

	// Token: 0x040008B9 RID: 2233
	protected static bool s_EverInitialized;

	// Token: 0x040008BA RID: 2234
	protected static SteamManager s_instance;

	// Token: 0x040008BB RID: 2235
	protected bool m_bInitialized;

	// Token: 0x040008BC RID: 2236
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
