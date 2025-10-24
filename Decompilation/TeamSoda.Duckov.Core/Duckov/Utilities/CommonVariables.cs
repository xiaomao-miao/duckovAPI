using System;
using Saves;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x020003F6 RID: 1014
	public class CommonVariables : MonoBehaviour
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x0007F023 File Offset: 0x0007D223
		public CustomDataCollection Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x0007F02C File Offset: 0x0007D22C
		private void Awake()
		{
			if (CommonVariables.instance == null)
			{
				CommonVariables.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个Common Variables");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x0007F07A File Offset: 0x0007D27A
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0007F09E File Offset: 0x0007D29E
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0007F0A6 File Offset: 0x0007D2A6
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0007F0AE File Offset: 0x0007D2AE
		private void Start()
		{
			this.Load();
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0007F0B6 File Offset: 0x0007D2B6
		private void Save()
		{
			SavesSystem.Save<CustomDataCollection>("CommonVariables", "Data", this.data);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x0007F0CD File Offset: 0x0007D2CD
		private void Load()
		{
			this.data = SavesSystem.Load<CustomDataCollection>("CommonVariables", "Data");
			if (this.data == null)
			{
				this.data = new CustomDataCollection();
			}
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0007F0F7 File Offset: 0x0007D2F7
		public static void SetFloat(string key, float value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetFloat(key, value, true);
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x0007F117 File Offset: 0x0007D317
		public static void SetInt(string key, int value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetInt(key, value, true);
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x0007F137 File Offset: 0x0007D337
		public static void SetBool(string key, bool value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetBool(key, value, true);
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x0007F157 File Offset: 0x0007D357
		public static void SetString(string key, string value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetString(key, value, true);
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x0007F177 File Offset: 0x0007D377
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0007F198 File Offset: 0x0007D398
		public static int GetInt(string key, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0007F1B9 File Offset: 0x0007D3B9
		public static bool GetBool(string key, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x0007F1DA File Offset: 0x0007D3DA
		public static string GetString(string key, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0007F1FB File Offset: 0x0007D3FB
		public static float GetFloat(int hash, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0007F21C File Offset: 0x0007D41C
		public static int GetInt(int hash, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0007F23D File Offset: 0x0007D43D
		public static bool GetBool(int hash, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0007F25E File Offset: 0x0007D45E
		public static string GetString(int hash, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x040018F0 RID: 6384
		private static CommonVariables instance;

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		private CustomDataCollection data;

		// Token: 0x040018F2 RID: 6386
		private const string saves_prefix = "CommonVariables";

		// Token: 0x040018F3 RID: 6387
		private const string saves_key = "Data";
	}
}
