using System;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000323 RID: 803
	public class StatisticsManager : MonoBehaviour
	{
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06001AC4 RID: 6852 RVA: 0x00060CC4 File Offset: 0x0005EEC4
		// (remove) Token: 0x06001AC5 RID: 6853 RVA: 0x00060CF8 File Offset: 0x0005EEF8
		public static event Action<string, long, long> OnStatisticsChanged;

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00060D2B File Offset: 0x0005EF2B
		private static string GetSaveKey(string statisticsKey)
		{
			return "Statistics/" + statisticsKey;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00060D38 File Offset: 0x0005EF38
		private static long Get(string key)
		{
			StatisticsManager.GetSaveKey(key);
			if (!SavesSystem.KeyExisits(key))
			{
				return 0L;
			}
			return SavesSystem.Load<long>(key);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00060D54 File Offset: 0x0005EF54
		private static void Set(string key, long value)
		{
			long arg = StatisticsManager.Get(key);
			StatisticsManager.GetSaveKey(key);
			SavesSystem.Save<long>(key, value);
			Action<string, long, long> onStatisticsChanged = StatisticsManager.OnStatisticsChanged;
			if (onStatisticsChanged == null)
			{
				return;
			}
			onStatisticsChanged(key, arg, value);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00060D88 File Offset: 0x0005EF88
		public static void Add(string key, long value = 1L)
		{
			long num = StatisticsManager.Get(key);
			checked
			{
				try
				{
					num += value;
				}
				catch (OverflowException exception)
				{
					Debug.LogException(exception);
					Debug.Log("Failed changing statistics of " + key + ". Overflow detected.");
					return;
				}
				StatisticsManager.Set(key, num);
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00060DD8 File Offset: 0x0005EFD8
		private void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00060DE0 File Offset: 0x0005EFE0
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00060DE8 File Offset: 0x0005EFE8
		private void RegisterEvents()
		{
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x00060DEA File Offset: 0x0005EFEA
		private void UnregisterEvents()
		{
		}
	}
}
