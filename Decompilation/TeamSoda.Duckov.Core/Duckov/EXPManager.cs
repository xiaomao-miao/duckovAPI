using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000234 RID: 564
	public class EXPManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00043BBD File Offset: 0x00041DBD
		public static EXPManager Instance
		{
			get
			{
				return EXPManager.instance;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00043BC4 File Offset: 0x00041DC4
		private string LevelChangeNotificationFormat
		{
			get
			{
				return this.levelChangeNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00043BD1 File Offset: 0x00041DD1
		// (set) Token: 0x06001184 RID: 4484 RVA: 0x00043BF0 File Offset: 0x00041DF0
		public static long EXP
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.point;
			}
			private set
			{
				if (EXPManager.instance == null)
				{
					return;
				}
				int level = EXPManager.Level;
				EXPManager.instance.point = value;
				Action<long> action = EXPManager.onExpChanged;
				if (action != null)
				{
					action(value);
				}
				int level2 = EXPManager.Level;
				if (level != level2)
				{
					EXPManager.OnLevelChanged(level, level2);
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00043C3E File Offset: 0x00041E3E
		public static int Level
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0;
				}
				return EXPManager.instance.LevelFromExp(EXPManager.EXP);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00043C5E File Offset: 0x00041E5E
		public static long CachedExp
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.cachedExp;
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00043C7A File Offset: 0x00041E7A
		private static void OnLevelChanged(int oldLevel, int newLevel)
		{
			Action<int, int> action = EXPManager.onLevelChanged;
			if (action != null)
			{
				action(oldLevel, newLevel);
			}
			if (EXPManager.Instance == null)
			{
				return;
			}
			NotificationText.Push(EXPManager.Instance.LevelChangeNotificationFormat.Format(new
			{
				level = newLevel
			}));
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00043CB6 File Offset: 0x00041EB6
		public static bool AddExp(int amount)
		{
			if (EXPManager.instance == null)
			{
				return false;
			}
			EXPManager.EXP += (long)amount;
			return true;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00043CD5 File Offset: 0x00041ED5
		private void CacheExp()
		{
			this.cachedExp = this.point;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00043CE3 File Offset: 0x00041EE3
		public object GenerateSaveData()
		{
			return this.point;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00043CF0 File Offset: 0x00041EF0
		public void SetupSaveData(object data)
		{
			if (data is long)
			{
				long num = (long)data;
				this.point = num;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00043D13 File Offset: 0x00041F13
		private string realKey
		{
			get
			{
				return "EXP_Value";
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00043D1C File Offset: 0x00041F1C
		private void Load()
		{
			if (SavesSystem.KeyExisits(this.realKey))
			{
				long num = SavesSystem.Load<long>(this.realKey);
				this.SetupSaveData(num);
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00043D50 File Offset: 0x00041F50
		private void Save()
		{
			object obj = this.GenerateSaveData();
			SavesSystem.Save<long>(this.realKey, (long)obj);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00043D78 File Offset: 0x00041F78
		private void Awake()
		{
			if (EXPManager.instance == null)
			{
				EXPManager.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个ExpManager");
			}
			SavesSystem.OnSetFile += this.Load;
			SavesSystem.OnCollectSaveData += this.Save;
			LevelManager.OnLevelInitialized += this.CacheExp;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00043DD7 File Offset: 0x00041FD7
		private void Start()
		{
			this.Load();
			this.CacheExp();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00043DE5 File Offset: 0x00041FE5
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Load;
			SavesSystem.OnCollectSaveData -= this.Save;
			LevelManager.OnLevelInitialized -= this.CacheExp;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00043E1C File Offset: 0x0004201C
		public int LevelFromExp(long exp)
		{
			for (int i = 0; i < this.levelExpDefinition.Count; i++)
			{
				long num = this.levelExpDefinition[i];
				if (exp < num)
				{
					return i - 1;
				}
			}
			return this.levelExpDefinition.Count - 1;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00043E64 File Offset: 0x00042064
		[return: TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		public ValueTuple<long, long> GetLevelExpRange(int level)
		{
			int num = this.levelExpDefinition.Count - 1;
			if (level >= num)
			{
				List<long> list = this.levelExpDefinition;
				return new ValueTuple<long, long>(list[list.Count - 1], long.MaxValue);
			}
			long item = this.levelExpDefinition[level];
			long item2 = this.levelExpDefinition[level + 1];
			return new ValueTuple<long, long>(item, item2);
		}

		// Token: 0x04000D8B RID: 3467
		private static EXPManager instance;

		// Token: 0x04000D8C RID: 3468
		[SerializeField]
		private string levelChangeNotificationFormatKey = "UI_LevelChangeNotification";

		// Token: 0x04000D8D RID: 3469
		[SerializeField]
		private List<long> levelExpDefinition;

		// Token: 0x04000D8E RID: 3470
		[SerializeField]
		private long point;

		// Token: 0x04000D8F RID: 3471
		public static Action<long> onExpChanged;

		// Token: 0x04000D90 RID: 3472
		public static Action<int, int> onLevelChanged;

		// Token: 0x04000D91 RID: 3473
		private long cachedExp;

		// Token: 0x04000D92 RID: 3474
		private const string prefixKey = "EXP";

		// Token: 0x04000D93 RID: 3475
		private const string key = "Value";
	}
}
