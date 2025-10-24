using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Saves
{
	// Token: 0x02000226 RID: 550
	public class SavesSystem
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0003FDE0 File Offset: 0x0003DFE0
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x0003FE3F File Offset: 0x0003E03F
		public static int CurrentSlot
		{
			get
			{
				if (SavesSystem._currentSlot == null)
				{
					SavesSystem._currentSlot = new int?(PlayerPrefs.GetInt("CurrentSlot", 1));
					int? currentSlot = SavesSystem._currentSlot;
					int num = 1;
					if (currentSlot.GetValueOrDefault() < num & currentSlot != null)
					{
						SavesSystem._currentSlot = new int?(1);
					}
				}
				return SavesSystem._currentSlot.Value;
			}
			private set
			{
				SavesSystem._currentSlot = new int?(value);
				PlayerPrefs.SetInt("CurrentSlot", value);
				SavesSystem.CacheFile();
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0003FE5C File Offset: 0x0003E05C
		public static string CurrentFilePath
		{
			get
			{
				return SavesSystem.GetFilePath(SavesSystem.CurrentSlot);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0003FE68 File Offset: 0x0003E068
		public static bool IsSaving
		{
			get
			{
				return SavesSystem.saving;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0003FE6F File Offset: 0x0003E06F
		public static string SavesFolder
		{
			get
			{
				return "Saves";
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0003FE76 File Offset: 0x0003E076
		public static string GetFullPathToSavesFolder()
		{
			return Path.Combine(Application.persistentDataPath, SavesSystem.SavesFolder);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0003FE87 File Offset: 0x0003E087
		public static string GetFilePath(int slot)
		{
			return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GetSaveFileName(slot));
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0003FE99 File Offset: 0x0003E099
		public static string GetSaveFileName(int slot)
		{
			return string.Format("Save_{0}.sav", slot);
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06001084 RID: 4228 RVA: 0x0003FEAC File Offset: 0x0003E0AC
		// (remove) Token: 0x06001085 RID: 4229 RVA: 0x0003FEE0 File Offset: 0x0003E0E0
		public static event Action OnSetFile;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06001086 RID: 4230 RVA: 0x0003FF14 File Offset: 0x0003E114
		// (remove) Token: 0x06001087 RID: 4231 RVA: 0x0003FF48 File Offset: 0x0003E148
		public static event Action OnSaveDeleted;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06001088 RID: 4232 RVA: 0x0003FF7C File Offset: 0x0003E17C
		// (remove) Token: 0x06001089 RID: 4233 RVA: 0x0003FFB0 File Offset: 0x0003E1B0
		public static event Action OnCollectSaveData;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600108A RID: 4234 RVA: 0x0003FFE4 File Offset: 0x0003E1E4
		// (remove) Token: 0x0600108B RID: 4235 RVA: 0x00040018 File Offset: 0x0003E218
		public static event Action OnRestoreFailureDetected;

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x0004004B File Offset: 0x0003E24B
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00040052 File Offset: 0x0003E252
		public static bool RestoreFailureMarker { get; private set; }

		// Token: 0x0600108E RID: 4238 RVA: 0x0004005A File Offset: 0x0003E25A
		public static bool IsOldSave(int index)
		{
			return !SavesSystem.KeyExisits("CreatedWithVersion", index);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004006A File Offset: 0x0003E26A
		public static void SetFile(int index)
		{
			SavesSystem.cached = false;
			SavesSystem.CurrentSlot = index;
			Action onSetFile = SavesSystem.OnSetFile;
			if (onSetFile == null)
			{
				return;
			}
			onSetFile();
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00040087 File Offset: 0x0003E287
		public static SavesSystem.BackupInfo[] GetBackupList()
		{
			return SavesSystem.GetBackupList(SavesSystem.CurrentSlot);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00040093 File Offset: 0x0003E293
		public static SavesSystem.BackupInfo[] GetBackupList(int slot)
		{
			return SavesSystem.GetBackupList(SavesSystem.GetFilePath(slot), slot);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x000400A4 File Offset: 0x0003E2A4
		public static SavesSystem.BackupInfo[] GetBackupList(string mainPath, int slot = -1)
		{
			SavesSystem.BackupInfo[] array = new SavesSystem.BackupInfo[10];
			for (int i = 0; i < 10; i++)
			{
				try
				{
					string backupPathByIndex = SavesSystem.GetBackupPathByIndex(mainPath, i);
					ES3Settings es3Settings = new ES3Settings(backupPathByIndex, null);
					es3Settings.location = ES3.Location.File;
					bool flag = ES3.FileExists(backupPathByIndex, es3Settings);
					long num = 0L;
					if (flag && ES3.KeyExists("SaveTime", backupPathByIndex, es3Settings))
					{
						num = ES3.Load<long>("SaveTime", backupPathByIndex, es3Settings);
					}
					DateTime.FromBinary(num);
					SavesSystem.BackupInfo backupInfo = new SavesSystem.BackupInfo
					{
						slot = slot,
						index = i,
						path = backupPathByIndex,
						exists = flag,
						time_raw = num
					};
					array[i] = backupInfo;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					array[i] = default(SavesSystem.BackupInfo);
				}
			}
			return array;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00040180 File Offset: 0x0003E380
		private static int GetEmptyOrOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (!backupInfo.exists)
				{
					return backupInfo.index;
				}
				if (backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x000401E4 File Offset: 0x0003E3E4
		private static int GetOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00040240 File Offset: 0x0003E440
		private static int GetNewestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MinValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time > t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0004029B File Offset: 0x0003E49B
		private static string GetBackupPathByIndex(int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.CurrentSlot, index);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x000402A8 File Offset: 0x0003E4A8
		private static string GetBackupPathByIndex(int slot, int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.GetFilePath(slot), index);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000402B6 File Offset: 0x0003E4B6
		private static string GetBackupPathByIndex(string path, int index)
		{
			return string.Format("{0}.bac.{1:00}", path, index + 1);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x000402CC File Offset: 0x0003E4CC
		private static void CreateIndexedBackup(int index = -1)
		{
			SavesSystem.LastIndexedBackupTime = DateTime.UtcNow;
			try
			{
				if (index < 0)
				{
					index = SavesSystem.GetEmptyOrOldestBackupIndex();
				}
				string backupPathByIndex = SavesSystem.GetBackupPathByIndex(index);
				ES3.DeleteFile(backupPathByIndex, SavesSystem.settings);
				ES3.CopyFile(SavesSystem.CurrentFilePath, backupPathByIndex);
				ES3.StoreCachedFile(backupPathByIndex);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating indexed backup");
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00040334 File Offset: 0x0003E534
		private static void CreateBackup()
		{
			try
			{
				SavesSystem.CreateBackup(SavesSystem.CurrentFilePath);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating backup");
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00040370 File Offset: 0x0003E570
		private static void CreateBackup(string path)
		{
			try
			{
				string filePath = path + ".bac";
				ES3.DeleteFile(filePath, SavesSystem.settings);
				ES3.CreateBackup(path);
				ES3.StoreCachedFile(filePath);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating backup for path " + path);
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x000403C8 File Offset: 0x0003E5C8
		public static void UpgradeSaveFileAssemblyInfo(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("没有找到存档文件：" + path);
				return;
			}
			string text;
			using (StreamReader streamReader = File.OpenText(path))
			{
				text = streamReader.ReadToEnd();
				if (text.Contains("TeamSoda.Duckov.Core"))
				{
					streamReader.Close();
					return;
				}
				text = text.Replace("Assembly-CSharp", "TeamSoda.Duckov.Core");
				streamReader.Close();
			}
			File.Delete(path);
			using (FileStream fileStream = File.OpenWrite(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(text);
				streamWriter.Close();
				fileStream.Close();
			}
			Debug.Log("存档格式已更新：" + path);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00040494 File Offset: 0x0003E694
		public static void RestoreIndexedBackup(int slot, int index)
		{
			string backupPathByIndex = SavesSystem.GetBackupPathByIndex(slot, index);
			SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, backupPathByIndex));
			string filePath = SavesSystem.GetFilePath(slot);
			string text = filePath + ".bac";
			try
			{
				ES3.CacheFile(backupPathByIndex);
				ES3.DeleteFile(text, SavesSystem.settings);
				ES3.CopyFile(backupPathByIndex, text);
				ES3.DeleteFile(filePath, SavesSystem.settings);
				ES3.RestoreBackup(filePath, SavesSystem.settings);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onSetFile = SavesSystem.OnSetFile;
				if (onSetFile != null)
				{
					onSetFile();
				}
			}
			catch
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("文件损坏，且无法修复。");
				ES3.DeleteFile(filePath);
				File.Delete(filePath);
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00040570 File Offset: 0x0003E770
		private static bool RestoreBackup(string path)
		{
			bool flag = false;
			try
			{
				string text = path + ".bac";
				SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, text));
				ES3.CacheFile(text);
				ES3.DeleteFile(path, SavesSystem.settings);
				ES3.RestoreBackup(path, SavesSystem.settings);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				ES3.CacheFile(path);
				flag = true;
			}
			catch
			{
				Debug.Log("默认备份损坏。");
			}
			if (!flag)
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("恢复默认备份失败");
				ES3.DeleteFile(path);
				ES3.Save<bool>("Created", true, path);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
			return flag;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x00040630 File Offset: 0x0003E830
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00040657 File Offset: 0x0003E857
		private static DateTime LastSavedTime
		{
			get
			{
				if (SavesSystem._lastSavedTime > DateTime.UtcNow)
				{
					SavesSystem._lastSavedTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastSavedTime;
			}
			set
			{
				SavesSystem._lastSavedTime = value;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0004065F File Offset: 0x0003E85F
		private static TimeSpan TimeSinceLastSave
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastSavedTime;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00040670 File Offset: 0x0003E870
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00040697 File Offset: 0x0003E897
		private static DateTime LastIndexedBackupTime
		{
			get
			{
				if (SavesSystem._lastIndexedBackupTime > DateTime.UtcNow)
				{
					SavesSystem._lastIndexedBackupTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastIndexedBackupTime;
			}
			set
			{
				SavesSystem._lastIndexedBackupTime = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0004069F File Offset: 0x0003E89F
		private static TimeSpan TimeSinceLastIndexedBackup
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastIndexedBackupTime;
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x000406B0 File Offset: 0x0003E8B0
		public DateTime GetSaveTimeUTC(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			if (!SavesSystem.KeyExisits("SaveTime", slot))
			{
				return default(DateTime);
			}
			return DateTime.FromBinary(SavesSystem.Load<long>("SaveTime", slot));
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000406F0 File Offset: 0x0003E8F0
		public DateTime GetSaveTimeLocal(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			DateTime saveTimeUTC = this.GetSaveTimeUTC(slot);
			if (saveTimeUTC == default(DateTime))
			{
				return default(DateTime);
			}
			return saveTimeUTC.ToLocalTime();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00040734 File Offset: 0x0003E934
		public static void SaveFile(bool writeSaveTime = true)
		{
			TimeSpan timeSinceLastIndexedBackup = SavesSystem.TimeSinceLastIndexedBackup;
			SavesSystem.LastSavedTime = DateTime.UtcNow;
			if (writeSaveTime)
			{
				SavesSystem.Save<long>("SaveTime", DateTime.UtcNow.ToBinary());
			}
			SavesSystem.saving = true;
			SavesSystem.CreateBackup();
			if (timeSinceLastIndexedBackup > TimeSpan.FromMinutes(5.0))
			{
				SavesSystem.CreateIndexedBackup(-1);
			}
			SavesSystem.SetAsOldGame();
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			SavesSystem.saving = false;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000407A5 File Offset: 0x0003E9A5
		private static void CacheFile()
		{
			SavesSystem.CacheFile(SavesSystem.CurrentSlot);
			SavesSystem.cached = true;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000407B8 File Offset: 0x0003E9B8
		private static void CacheFile(int slot)
		{
			if (slot == SavesSystem.CurrentSlot && SavesSystem.cached)
			{
				return;
			}
			string filePath = SavesSystem.GetFilePath(slot);
			if (!SavesSystem.CacheFile(filePath))
			{
				Debug.Log("尝试恢复 indexed backups");
				List<SavesSystem.BackupInfo> list = (from e in SavesSystem.GetBackupList(filePath, slot)
				where e.exists
				select e).ToList<SavesSystem.BackupInfo>();
				list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
				{
					if (!(a.Time > b.Time))
					{
						return 1;
					}
					return -1;
				});
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						SavesSystem.BackupInfo backupInfo = list[i];
						try
						{
							Debug.Log(string.Format("Restoreing {0}.bac.{1} \t", slot, backupInfo.index) + backupInfo.Time.ToString("MM/dd HH:mm:ss"));
							SavesSystem.RestoreIndexedBackup(slot, backupInfo.index);
							break;
						}
						catch
						{
							Debug.LogError(string.Format("slot:{0} backup_index:{1} 恢复失败。", slot, backupInfo.index));
						}
					}
				}
			}
			if (!ES3.FileExists(filePath))
			{
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00040914 File Offset: 0x0003EB14
		private static bool CacheFile(string path)
		{
			bool result;
			try
			{
				ES3.CacheFile(path);
				result = true;
			}
			catch
			{
				result = SavesSystem.RestoreBackup(path);
			}
			return result;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00040948 File Offset: 0x0003EB48
		public static void Save<T>(string prefix, string key, T value)
		{
			SavesSystem.Save<T>(prefix + key, value);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00040957 File Offset: 0x0003EB57
		public static void Save<T>(string realKey, T value)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			if (string.IsNullOrWhiteSpace(SavesSystem.CurrentFilePath))
			{
				Debug.Log("Save failed " + realKey);
				return;
			}
			ES3.Save<T>(realKey, value, SavesSystem.CurrentFilePath);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0004098E File Offset: 0x0003EB8E
		public static T Load<T>(string prefix, string key)
		{
			return SavesSystem.Load<T>(prefix + key);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0004099C File Offset: 0x0003EB9C
		public static T Load<T>(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			string.IsNullOrWhiteSpace(realKey);
			if (ES3.KeyExists(realKey, SavesSystem.CurrentFilePath))
			{
				return ES3.Load<T>(realKey, SavesSystem.CurrentFilePath);
			}
			return default(T);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000409DE File Offset: 0x0003EBDE
		public static bool KeyExisits(string prefix, string key)
		{
			return ES3.KeyExists(prefix + key);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x000409EC File Offset: 0x0003EBEC
		public static bool KeyExisits(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			return ES3.KeyExists(realKey, SavesSystem.CurrentFilePath);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00040A08 File Offset: 0x0003EC08
		public static bool KeyExisits(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.KeyExisits(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			return ES3.KeyExists(realKey, filePath);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00040A38 File Offset: 0x0003EC38
		public static T Load<T>(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.Load<T>(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			if (ES3.KeyExists(realKey, filePath))
			{
				return ES3.Load<T>(realKey, filePath);
			}
			return default(T);
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00040A7B File Offset: 0x0003EC7B
		public static string GlobalSaveDataFilePath
		{
			get
			{
				return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GlobalSaveDataFileName);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00040A8C File Offset: 0x0003EC8C
		public static string GlobalSaveDataFileName
		{
			get
			{
				return "Global.json";
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00040A93 File Offset: 0x0003EC93
		public static void SaveGlobal<T>(string key, T value)
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			ES3.Save<T>(key, value, SavesSystem.GlobalSaveDataFilePath);
			SavesSystem.CreateBackup(SavesSystem.GlobalSaveDataFilePath);
			ES3.StoreCachedFile(SavesSystem.GlobalSaveDataFilePath);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00040ACD File Offset: 0x0003ECCD
		public static T LoadGlobal<T>(string key, T defaultValue = default(T))
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			if (ES3.KeyExists(key, SavesSystem.GlobalSaveDataFilePath))
			{
				return ES3.Load<T>(key, SavesSystem.GlobalSaveDataFilePath);
			}
			return defaultValue;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00040B01 File Offset: 0x0003ED01
		public static void CollectSaveData()
		{
			Action onCollectSaveData = SavesSystem.OnCollectSaveData;
			if (onCollectSaveData == null)
			{
				return;
			}
			onCollectSaveData();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00040B12 File Offset: 0x0003ED12
		public static bool IsOldGame()
		{
			return SavesSystem.Load<bool>("IsOldGame");
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00040B1E File Offset: 0x0003ED1E
		public static bool IsOldGame(int index)
		{
			return SavesSystem.Load<bool>("IsOldGame", index);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00040B2B File Offset: 0x0003ED2B
		private static void SetAsOldGame()
		{
			SavesSystem.Save<bool>("IsOldGame", true);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00040B38 File Offset: 0x0003ED38
		public static void DeleteCurrentSave()
		{
			ES3.CacheFile(SavesSystem.CurrentFilePath);
			ES3.DeleteFile(SavesSystem.CurrentFilePath);
			ES3.Save<bool>("Created", false, SavesSystem.CurrentFilePath);
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			Debug.Log(string.Format("已删除存档{0}", SavesSystem.CurrentSlot));
			Action onSaveDeleted = SavesSystem.OnSaveDeleted;
			if (onSaveDeleted == null)
			{
				return;
			}
			onSaveDeleted();
		}

		// Token: 0x04000D1A RID: 3354
		private static int? _currentSlot = null;

		// Token: 0x04000D1B RID: 3355
		private static bool saving;

		// Token: 0x04000D1C RID: 3356
		private static ES3Settings settings = ES3Settings.defaultSettings;

		// Token: 0x04000D1D RID: 3357
		private static bool cached;

		// Token: 0x04000D23 RID: 3363
		private const int BackupListCount = 10;

		// Token: 0x04000D24 RID: 3364
		private static DateTime _lastSavedTime = DateTime.MinValue;

		// Token: 0x04000D25 RID: 3365
		private static DateTime _lastIndexedBackupTime = DateTime.MinValue;

		// Token: 0x04000D26 RID: 3366
		private static bool globalCached;

		// Token: 0x04000D27 RID: 3367
		private static ES3Settings GlobalFileSetting = new ES3Settings(null, null)
		{
			location = ES3.Location.File
		};

		// Token: 0x0200050A RID: 1290
		public struct BackupInfo
		{
			// Token: 0x1700074B RID: 1867
			// (get) Token: 0x06002778 RID: 10104 RVA: 0x000903E6 File Offset: 0x0008E5E6
			public bool TimeValid
			{
				get
				{
					return this.time_raw > 0L;
				}
			}

			// Token: 0x1700074C RID: 1868
			// (get) Token: 0x06002779 RID: 10105 RVA: 0x000903F2 File Offset: 0x0008E5F2
			public DateTime Time
			{
				get
				{
					return DateTime.FromBinary(this.time_raw);
				}
			}

			// Token: 0x04001DCE RID: 7630
			public int slot;

			// Token: 0x04001DCF RID: 7631
			public int index;

			// Token: 0x04001DD0 RID: 7632
			public string path;

			// Token: 0x04001DD1 RID: 7633
			public bool exists;

			// Token: 0x04001DD2 RID: 7634
			public long time_raw;
		}
	}
}
