using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026B RID: 619
	public class ModManager : MonoBehaviour
	{
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06001335 RID: 4917 RVA: 0x0004766C File Offset: 0x0004586C
		// (remove) Token: 0x06001336 RID: 4918 RVA: 0x000476A0 File Offset: 0x000458A0
		public static event Action OnReorder;

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x000476D3 File Offset: 0x000458D3
		// (set) Token: 0x06001338 RID: 4920 RVA: 0x000476E0 File Offset: 0x000458E0
		public static bool AllowActivatingMod
		{
			get
			{
				return SavesSystem.LoadGlobal<bool>("AllowLoadingMod", false);
			}
			set
			{
				SavesSystem.SaveGlobal<bool>("AllowLoadingMod", value);
				if (ModManager.Instance != null && value)
				{
					ModManager.Instance.ScanAndActivateMods();
				}
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00047707 File Offset: 0x00045907
		private void Awake()
		{
			if (this.modParent == null)
			{
				this.modParent = base.transform;
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00047723 File Offset: 0x00045923
		private void Start()
		{
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00047728 File Offset: 0x00045928
		public void ScanAndActivateMods()
		{
			if (!ModManager.AllowActivatingMod)
			{
				return;
			}
			ModManager.Rescan();
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (!this.activeMods.ContainsKey(modInfo.name))
				{
					bool flag = this.ShouldActivateMod(modInfo);
					Debug.Log(string.Format("ModActive_{0}: {1}", modInfo.name, flag));
					if (flag && this.ActivateMod(modInfo) == null)
					{
						this.SetShouldActivateMod(modInfo, false);
					}
				}
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000477D0 File Offset: 0x000459D0
		private static void SortModInfosByPriority()
		{
			ModManager.modInfos.Sort(delegate(ModInfo a, ModInfo b)
			{
				int modPriority = ModManager.GetModPriority(a.name);
				int modPriority2 = ModManager.GetModPriority(b.name);
				return modPriority - modPriority2;
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Sorted mods:");
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				stringBuilder.AppendLine(modInfo.name);
			}
			Debug.Log(stringBuilder);
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x0004786C File Offset: 0x00045A6C
		private static ES3Settings settings
		{
			get
			{
				if (ModManager._settings == null)
				{
					ModManager._settings = new ES3Settings(null, null)
					{
						location = ES3.Location.File,
						path = "Saves/Mods.ES3"
					};
				}
				return ModManager._settings;
			}
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00047898 File Offset: 0x00045A98
		private static void Save<T>(string key, T value)
		{
			try
			{
				ES3.Save<T>(key, value, ModManager.settings);
				ES3.CreateBackup(ModManager.settings);
			}
			catch (Exception exception)
			{
				Debug.LogError("Failed saving mod info.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x000478E0 File Offset: 0x00045AE0
		private static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			try
			{
				result = ES3.Load<T>(key, defaultValue, ModManager.settings);
			}
			catch (Exception exception)
			{
				Debug.LogError("Failed loading mod info.");
				ES3.RestoreBackup(ModManager.settings);
				Debug.LogException(exception);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0004792C File Offset: 0x00045B2C
		public static void SetModPriority(string name, int priority)
		{
			ModManager.Save<int>("priority_" + name, priority);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0004793F File Offset: 0x00045B3F
		public static int GetModPriority(string name)
		{
			return ModManager.Load<int>("priority_" + name, 0);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00047952 File Offset: 0x00045B52
		private void SetShouldActivateMod(ModInfo info, bool value)
		{
			SavesSystem.SaveGlobal<bool>("ModActive_" + info.name, value);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0004796A File Offset: 0x00045B6A
		private bool ShouldActivateMod(ModInfo info)
		{
			return SavesSystem.LoadGlobal<bool>("ModActive_" + info.name, false);
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00047982 File Offset: 0x00045B82
		private static string DefaultModFolderPath
		{
			get
			{
				return Path.Combine(Application.dataPath, "Mods");
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x00047993 File Offset: 0x00045B93
		public static ModManager Instance
		{
			get
			{
				return GameManager.ModManager;
			}
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001346 RID: 4934 RVA: 0x0004799C File Offset: 0x00045B9C
		// (remove) Token: 0x06001347 RID: 4935 RVA: 0x000479D0 File Offset: 0x00045BD0
		public static event Action<List<ModInfo>> OnScan;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001348 RID: 4936 RVA: 0x00047A04 File Offset: 0x00045C04
		// (remove) Token: 0x06001349 RID: 4937 RVA: 0x00047A38 File Offset: 0x00045C38
		public static event Action<ModInfo, ModBehaviour> OnModActivated;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x0600134A RID: 4938 RVA: 0x00047A6C File Offset: 0x00045C6C
		// (remove) Token: 0x0600134B RID: 4939 RVA: 0x00047AA0 File Offset: 0x00045CA0
		public static event Action<ModInfo, ModBehaviour> OnModWillBeDeactivated;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x0600134C RID: 4940 RVA: 0x00047AD4 File Offset: 0x00045CD4
		// (remove) Token: 0x0600134D RID: 4941 RVA: 0x00047B08 File Offset: 0x00045D08
		public static event Action OnModStatusChanged;

		// Token: 0x0600134E RID: 4942 RVA: 0x00047B3C File Offset: 0x00045D3C
		public static void Rescan()
		{
			ModManager.modInfos.Clear();
			if (Directory.Exists(ModManager.DefaultModFolderPath))
			{
				string[] directories = Directory.GetDirectories(ModManager.DefaultModFolderPath);
				for (int i = 0; i < directories.Length; i++)
				{
					ModInfo item;
					if (ModManager.TryProcessModFolder(directories[i], out item, false, 0UL))
					{
						ModManager.modInfos.Add(item);
					}
				}
			}
			Action<List<ModInfo>> onScan = ModManager.OnScan;
			if (onScan != null)
			{
				onScan(ModManager.modInfos);
			}
			ModManager.SortModInfosByPriority();
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00047BAC File Offset: 0x00045DAC
		private static void RegeneratePriorities()
		{
			for (int i = 0; i < ModManager.modInfos.Count; i++)
			{
				string name = ModManager.modInfos[i].name;
				if (!string.IsNullOrWhiteSpace(name))
				{
					ModManager.SetModPriority(name, i);
				}
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00047BF0 File Offset: 0x00045DF0
		public static bool Reorder(int fromIndex, int toIndex)
		{
			if (fromIndex == toIndex)
			{
				return false;
			}
			if (fromIndex < 0 || fromIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			if (toIndex < 0 || toIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			ModInfo item = ModManager.modInfos[fromIndex];
			ModManager.modInfos.RemoveAt(fromIndex);
			ModManager.modInfos.Insert(toIndex, item);
			ModManager.RegeneratePriorities();
			Action onReorder = ModManager.OnReorder;
			if (onReorder != null)
			{
				onReorder();
			}
			return true;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00047C64 File Offset: 0x00045E64
		public static bool TryProcessModFolder(string path, out ModInfo info, bool isSteamItem = false, ulong publishedFileId = 0UL)
		{
			info = default(ModInfo);
			info.path = path;
			string path2 = Path.Combine(path, "info.ini");
			if (!File.Exists(path2))
			{
				return false;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (StreamReader streamReader = File.OpenText(path2))
			{
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine().Trim();
					if (!string.IsNullOrWhiteSpace(text) && !text.StartsWith('['))
					{
						string[] array = text.Split('=', StringSplitOptions.None);
						if (array.Length == 2)
						{
							string key = array[0].Trim();
							string value = array[1].Trim();
							dictionary[key] = value;
						}
					}
				}
			}
			string text2;
			if (!dictionary.TryGetValue("name", out text2))
			{
				Debug.LogError("Failed to get name value in mod info.ini file. Aborting.\n" + path);
				return false;
			}
			string displayName;
			if (!dictionary.TryGetValue("displayName", out displayName))
			{
				displayName = text2;
				Debug.LogError("Failed to get displayName value in mod info.ini file.\n" + path);
			}
			string description;
			if (!dictionary.TryGetValue("description", out description))
			{
				description = "?";
				Debug.LogError("Failed to get description value in mod info.ini file.\n" + path);
			}
			ulong num = 0UL;
			string s;
			if (dictionary.TryGetValue("publishedFileId", out s) && !ulong.TryParse(s, out num))
			{
				Debug.LogError("Invalid publishedFileId");
			}
			if (!isSteamItem)
			{
				publishedFileId = num;
			}
			else if (publishedFileId != num)
			{
				Debug.LogError("PublishFileId not match.\npath:" + path);
			}
			info.name = text2;
			info.displayName = displayName;
			info.description = description;
			info.publishedFileId = publishedFileId;
			info.isSteamItem = isSteamItem;
			string dllPath = info.dllPath;
			info.dllFound = File.Exists(dllPath);
			if (!info.dllFound)
			{
				Debug.LogError("Dll for mod " + text2 + " not found.\nExpecting: " + dllPath);
			}
			string path3 = Path.Combine(path, "preview.png");
			if (File.Exists(path3))
			{
				using (FileStream fileStream = File.OpenRead(path3))
				{
					Texture2D texture2D = new Texture2D(256, 256);
					byte[] array2 = new byte[fileStream.Length];
					fileStream.Read(array2);
					if (texture2D.LoadImage(array2))
					{
						info.preview = texture2D;
					}
				}
			}
			return true;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00047EA4 File Offset: 0x000460A4
		public static bool IsModActive(ModInfo info, out ModBehaviour instance)
		{
			instance = null;
			return !(ModManager.Instance == null) && ModManager.Instance.activeMods.TryGetValue(info.name, out instance) && instance != null;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00047EDC File Offset: 0x000460DC
		public ModBehaviour GetActiveModBehaviour(ModInfo info)
		{
			ModBehaviour result;
			if (this.activeMods.TryGetValue(info.name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00047F04 File Offset: 0x00046104
		public void DeactivateMod(ModInfo info)
		{
			ModBehaviour activeModBehaviour = this.GetActiveModBehaviour(info);
			if (activeModBehaviour == null)
			{
				return;
			}
			try
			{
				activeModBehaviour.NotifyBeforeDeactivate();
				Action<ModInfo, ModBehaviour> onModWillBeDeactivated = ModManager.OnModWillBeDeactivated;
				if (onModWillBeDeactivated != null)
				{
					onModWillBeDeactivated(info, activeModBehaviour);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.activeMods.Remove(info.name);
			try
			{
				UnityEngine.Object.Destroy(activeModBehaviour.gameObject);
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			this.SetShouldActivateMod(info, false);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00047FA0 File Offset: 0x000461A0
		public ModBehaviour ActivateMod(ModInfo info)
		{
			if (!ModManager.AllowActivatingMod)
			{
				Debug.LogError("Activating mod not allowed! \nUser must first interact with the agreement UI in order to allow activating mods.");
				return null;
			}
			string dllPath = info.dllPath;
			string name = info.name;
			ModBehaviour context;
			if (ModManager.IsModActive(info, out context))
			{
				Debug.LogError("Mod " + info.name + " instance already exists! Abort. Path: " + info.path, context);
				return null;
			}
			Debug.Log("Loading mod dll at path: " + dllPath);
			Type type;
			try
			{
				type = Assembly.LoadFrom(dllPath).GetType(name + ".ModBehaviour");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				string arg = "Mod loading failed: " + name + "\n" + ex.Message;
				Action<string, string> onModLoadingFailed = ModManager.OnModLoadingFailed;
				if (onModLoadingFailed != null)
				{
					onModLoadingFailed(info.dllPath, arg);
				}
				return null;
			}
			if (type == null || !type.InheritsFrom<ModBehaviour>())
			{
				Debug.LogError("Cannot load mod.\nA type named " + name + ".Mod is expected, and it should inherit from Duckov.Modding.Mod.");
				return null;
			}
			GameObject gameObject = new GameObject(name);
			ModBehaviour modBehaviour;
			try
			{
				modBehaviour = (gameObject.AddComponent(type) as ModBehaviour);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			if (modBehaviour == null)
			{
				UnityEngine.Object.Destroy(gameObject);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			gameObject.transform.SetParent(base.transform);
			Debug.Log("Mod Loaded: " + info.name);
			modBehaviour.Setup(this, info);
			this.activeMods[info.name] = modBehaviour;
			try
			{
				Action<ModInfo, ModBehaviour> onModActivated = ModManager.OnModActivated;
				if (onModActivated != null)
				{
					onModActivated(info, modBehaviour);
				}
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			this.SetShouldActivateMod(info, true);
			return modBehaviour;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00048190 File Offset: 0x00046390
		internal static void WriteModInfoINI(ModInfo modInfo)
		{
			string path = Path.Combine(modInfo.path, "info.ini");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (FileStream fileStream = File.Create(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.WriteLine("name = " + modInfo.name);
				streamWriter.WriteLine("displayName = " + modInfo.displayName);
				streamWriter.WriteLine("description = " + modInfo.description);
				streamWriter.WriteLine("");
				streamWriter.WriteLine(string.Format("publishedFileId = {0}", modInfo.publishedFileId));
				streamWriter.Close();
			}
		}

		// Token: 0x04000E4F RID: 3663
		[SerializeField]
		private Transform modParent;

		// Token: 0x04000E51 RID: 3665
		public static Action<string, string> OnModLoadingFailed;

		// Token: 0x04000E52 RID: 3666
		private static ES3Settings _settings;

		// Token: 0x04000E53 RID: 3667
		public static List<ModInfo> modInfos = new List<ModInfo>();

		// Token: 0x04000E54 RID: 3668
		private Dictionary<string, ModBehaviour> activeMods = new Dictionary<string, ModBehaviour>();
	}
}
