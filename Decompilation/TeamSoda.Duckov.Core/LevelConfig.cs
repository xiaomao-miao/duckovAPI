using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class LevelConfig : MonoBehaviour
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0002776A File Offset: 0x0002596A
	public static LevelConfig Instance
	{
		get
		{
			if (!LevelConfig.instance)
			{
				LevelConfig.SetInstance();
			}
			return LevelConfig.instance;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00027782 File Offset: 0x00025982
	public float LootBoxQualityLowPercent
	{
		get
		{
			return 1f - 1f / this.lootBoxHighQualityChanceMultiplier;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00027796 File Offset: 0x00025996
	public float LootboxItemCountMultiplier
	{
		get
		{
			return this.lootboxItemCountMultiplier;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0002779E File Offset: 0x0002599E
	public static bool IsBaseLevel
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isBaseLevel;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060008C8 RID: 2248 RVA: 0x000277B8 File Offset: 0x000259B8
	public static bool IsRaidMap
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isRaidMap;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x000277D2 File Offset: 0x000259D2
	public static int MinExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.minExitCount;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060008CA RID: 2250 RVA: 0x000277EC File Offset: 0x000259EC
	public static bool SpawnTomb
	{
		get
		{
			return !LevelConfig.Instance || LevelConfig.Instance.spawnTomb;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x00027806 File Offset: 0x00025A06
	public static int MaxExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.maxExitCount;
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00027820 File Offset: 0x00025A20
	private void Awake()
	{
		UnityEngine.Object.Instantiate<LevelManager>(GameplayDataSettings.Prefabs.LevelManagerPrefab).transform.SetParent(base.transform);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00027841 File Offset: 0x00025A41
	private static void SetInstance()
	{
		if (LevelConfig.instance)
		{
			return;
		}
		LevelConfig.instance = UnityEngine.Object.FindFirstObjectByType<LevelConfig>();
		LevelConfig.instance;
	}

	// Token: 0x040007F4 RID: 2036
	private static LevelConfig instance;

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private bool isBaseLevel;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	private bool isRaidMap = true;

	// Token: 0x040007F7 RID: 2039
	[SerializeField]
	private bool spawnTomb = true;

	// Token: 0x040007F8 RID: 2040
	[SerializeField]
	private int minExitCount;

	// Token: 0x040007F9 RID: 2041
	[SerializeField]
	private int maxExitCount;

	// Token: 0x040007FA RID: 2042
	public TimeOfDayConfig timeOfDayConfig;

	// Token: 0x040007FB RID: 2043
	[SerializeField]
	[Min(1f)]
	private float lootBoxHighQualityChanceMultiplier = 1f;

	// Token: 0x040007FC RID: 2044
	[SerializeField]
	[Range(0.1f, 10f)]
	private float lootboxItemCountMultiplier = 1f;
}
