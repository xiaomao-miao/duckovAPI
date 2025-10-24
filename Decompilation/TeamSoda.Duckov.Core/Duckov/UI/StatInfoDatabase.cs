using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000396 RID: 918
	[CreateAssetMenu(menuName = "Duckov/Stat Info Database")]
	public class StatInfoDatabase : ScriptableObject
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600204E RID: 8270 RVA: 0x00070C70 File Offset: 0x0006EE70
		public static StatInfoDatabase Instance
		{
			get
			{
				return GameplayDataSettings.StatInfo;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x00070C77 File Offset: 0x0006EE77
		private static Dictionary<string, StatInfoDatabase.Entry> Dic
		{
			get
			{
				return StatInfoDatabase.Instance._dic;
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00070C84 File Offset: 0x0006EE84
		public static StatInfoDatabase.Entry Get(string statName)
		{
			if (!(StatInfoDatabase.Instance == null))
			{
				if (StatInfoDatabase.Dic == null)
				{
					StatInfoDatabase.RebuildDic();
				}
				StatInfoDatabase.Entry result;
				if (StatInfoDatabase.Dic.TryGetValue(statName, out result))
				{
					return result;
				}
			}
			return new StatInfoDatabase.Entry
			{
				statName = statName,
				polarity = Polarity.Neutral,
				displayFormat = "0.##"
			};
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00070CE0 File Offset: 0x0006EEE0
		public static Polarity GetPolarity(string statName)
		{
			return StatInfoDatabase.Get(statName).polarity;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00070CF0 File Offset: 0x0006EEF0
		[ContextMenu("Rebuild Dic")]
		private static void RebuildDic()
		{
			if (StatInfoDatabase.Instance == null)
			{
				return;
			}
			StatInfoDatabase.Instance._dic = new Dictionary<string, StatInfoDatabase.Entry>();
			foreach (StatInfoDatabase.Entry entry in StatInfoDatabase.Instance.entries)
			{
				if (StatInfoDatabase.Instance._dic.ContainsKey(entry.statName))
				{
					Debug.LogError("Stat Info 中有重复的 key: " + entry.statName);
				}
				else
				{
					StatInfoDatabase.Instance._dic[entry.statName] = entry;
				}
			}
		}

		// Token: 0x04001604 RID: 5636
		[SerializeField]
		private StatInfoDatabase.Entry[] entries = new StatInfoDatabase.Entry[0];

		// Token: 0x04001605 RID: 5637
		private Dictionary<string, StatInfoDatabase.Entry> _dic;

		// Token: 0x0200061A RID: 1562
		[Serializable]
		public struct Entry
		{
			// Token: 0x1700078E RID: 1934
			// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0009C0AA File Offset: 0x0009A2AA
			public string DisplayFormat
			{
				get
				{
					if (string.IsNullOrEmpty(this.displayFormat))
					{
						return "0.##";
					}
					return this.displayFormat;
				}
			}

			// Token: 0x040021A3 RID: 8611
			public string statName;

			// Token: 0x040021A4 RID: 8612
			public Polarity polarity;

			// Token: 0x040021A5 RID: 8613
			public string displayFormat;
		}
	}
}
