using System;
using System.Collections.Generic;
using System.IO;
using Duckov.Utilities;
using MiniExcelLibs;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000321 RID: 801
	[CreateAssetMenu]
	public class AchievementDatabase : ScriptableObject
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x0006049E File Offset: 0x0005E69E
		public static AchievementDatabase Instance
		{
			get
			{
				return GameplayDataSettings.AchievementDatabase;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x000604A5 File Offset: 0x0005E6A5
		private Dictionary<string, AchievementDatabase.Achievement> dic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDictionary();
				}
				return this._dic;
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000604BC File Offset: 0x0005E6BC
		private void RebuildDictionary()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, AchievementDatabase.Achievement>();
			}
			this._dic.Clear();
			if (this.achievementChart == null)
			{
				Debug.LogError("Achievement Chart is not assinged", this);
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(this.achievementChart.bytes))
			{
				foreach (AchievementDatabase.Achievement achievement in memoryStream.Query(null, ExcelType.UNKNOWN, "A1", null))
				{
					this._dic[achievement.id.Trim()] = achievement;
				}
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00060580 File Offset: 0x0005E780
		public static bool TryGetAchievementData(string id, out AchievementDatabase.Achievement achievement)
		{
			achievement = null;
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.dic.TryGetValue(id, out achievement);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000605A5 File Offset: 0x0005E7A5
		internal bool IsIDValid(string id)
		{
			return this.dic.ContainsKey(id);
		}

		// Token: 0x04001313 RID: 4883
		[SerializeField]
		private XlsxObject achievementChart;

		// Token: 0x04001314 RID: 4884
		private Dictionary<string, AchievementDatabase.Achievement> _dic;

		// Token: 0x020005C0 RID: 1472
		[Serializable]
		public class Achievement
		{
			// Token: 0x1700077C RID: 1916
			// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000978BA File Offset: 0x00095ABA
			// (set) Token: 0x060028F2 RID: 10482 RVA: 0x000978C2 File Offset: 0x00095AC2
			public string id { get; set; }

			// Token: 0x1700077D RID: 1917
			// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000978CB File Offset: 0x00095ACB
			// (set) Token: 0x060028F4 RID: 10484 RVA: 0x000978D3 File Offset: 0x00095AD3
			public string overrideDisplayNameKey { get; set; }

			// Token: 0x1700077E RID: 1918
			// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000978DC File Offset: 0x00095ADC
			// (set) Token: 0x060028F6 RID: 10486 RVA: 0x000978E4 File Offset: 0x00095AE4
			public string overrideDescriptionKey { get; set; }

			// Token: 0x1700077F RID: 1919
			// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000978ED File Offset: 0x00095AED
			// (set) Token: 0x060028F8 RID: 10488 RVA: 0x00097913 File Offset: 0x00095B13
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDisplayNameKey))
					{
						return this.overrideDisplayNameKey;
					}
					return "Achievement_" + this.id;
				}
				set
				{
				}
			}

			// Token: 0x17000780 RID: 1920
			// (get) Token: 0x060028F9 RID: 10489 RVA: 0x00097915 File Offset: 0x00095B15
			// (set) Token: 0x060028FA RID: 10490 RVA: 0x00097940 File Offset: 0x00095B40
			[LocalizationKey("Default")]
			public string DescriptionKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDescriptionKey))
					{
						return this.overrideDescriptionKey;
					}
					return "Achievement_" + this.id + "_Desc";
				}
				set
				{
				}
			}

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x060028FB RID: 10491 RVA: 0x00097942 File Offset: 0x00095B42
			public string DisplayName
			{
				get
				{
					return this.DisplayNameKey.ToPlainText();
				}
			}

			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x060028FC RID: 10492 RVA: 0x0009794F File Offset: 0x00095B4F
			public string Description
			{
				get
				{
					return this.DescriptionKey.ToPlainText();
				}
			}
		}
	}
}
