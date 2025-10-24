using System;
using System.IO;
using Saves;
using UnityEngine;

namespace Duckov.Options
{
	// Token: 0x0200025D RID: 605
	public class OptionsManager : MonoBehaviour
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x0004689B File Offset: 0x00044A9B
		private static string Folder
		{
			get
			{
				return SavesSystem.SavesFolder;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x000468A2 File Offset: 0x00044AA2
		public static string FilePath
		{
			get
			{
				return Path.Combine(OptionsManager.Folder, "Options.ES3");
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060012C4 RID: 4804 RVA: 0x000468B4 File Offset: 0x00044AB4
		// (remove) Token: 0x060012C5 RID: 4805 RVA: 0x000468E8 File Offset: 0x00044AE8
		public static event Action<string> OnOptionsChanged;

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0004691B File Offset: 0x00044B1B
		private static ES3Settings SaveSettings
		{
			get
			{
				if (OptionsManager._saveSettings == null)
				{
					OptionsManager._saveSettings = new ES3Settings(true);
					OptionsManager._saveSettings.path = OptionsManager.FilePath;
					OptionsManager._saveSettings.location = ES3.Location.File;
				}
				return OptionsManager._saveSettings;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0004694E File Offset: 0x00044B4E
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x0004695F File Offset: 0x00044B5F
		public static float MouseSensitivity
		{
			get
			{
				return OptionsManager.Load<float>("MouseSensitivity", 10f);
			}
			set
			{
				OptionsManager.Save<float>("MouseSensitivity", value);
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0004696C File Offset: 0x00044B6C
		public static void Save<T>(string key, T obj)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			try
			{
				ES3.Save<T>(key, obj, OptionsManager.SaveSettings);
				Action<string> onOptionsChanged = OptionsManager.OnOptionsChanged;
				if (onOptionsChanged != null)
				{
					onOptionsChanged(key);
				}
				ES3.CreateBackup(OptionsManager.SaveSettings);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Error: Failed saving options: " + key);
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x000469D4 File Offset: 0x00044BD4
		public static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			if (string.IsNullOrEmpty(key))
			{
				result = default(T);
				return result;
			}
			try
			{
				if (ES3.KeyExists(key, OptionsManager.SaveSettings))
				{
					result = ES3.Load<T>(key, OptionsManager.SaveSettings);
				}
				else
				{
					ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
					result = defaultValue;
				}
			}
			catch
			{
				if (ES3.RestoreBackup(OptionsManager.SaveSettings))
				{
					try
					{
						if (ES3.KeyExists(key, OptionsManager.SaveSettings))
						{
							return ES3.Load<T>(key, OptionsManager.SaveSettings);
						}
						ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
						return defaultValue;
					}
					catch
					{
						Debug.LogError("[OPTIONS MANAGER] Failed restoring backup");
					}
				}
				ES3.DeleteFile(OptionsManager.SaveSettings);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x04000E26 RID: 3622
		public const string FileName = "Options.ES3";

		// Token: 0x04000E28 RID: 3624
		private static ES3Settings _saveSettings;
	}
}
