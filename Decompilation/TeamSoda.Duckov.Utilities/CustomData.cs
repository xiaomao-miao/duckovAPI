using System;
using System.Text;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class CustomData
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002160 File Offset: 0x00000360
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002168 File Offset: 0x00000368
		private byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
				Action<CustomData> onSetData = this.OnSetData;
				if (onSetData == null)
				{
					return;
				}
				onSetData(this);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002182 File Offset: 0x00000382
		[SerializeField]
		private string displayNameKey
		{
			get
			{
				return "Var_" + this.key;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000219D File Offset: 0x0000039D
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002194 File Offset: 0x00000394
		public bool Display
		{
			get
			{
				return this.display;
			}
			set
			{
				this.display = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021A5 File Offset: 0x000003A5
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021B2 File Offset: 0x000003B2
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000021BA File Offset: 0x000003BA
		public CustomDataType DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000E RID: 14 RVA: 0x000021C4 File Offset: 0x000003C4
		// (remove) Token: 0x0600000F RID: 15 RVA: 0x000021FC File Offset: 0x000003FC
		public event Action<CustomData> OnSetData;

		// Token: 0x06000010 RID: 16 RVA: 0x00002234 File Offset: 0x00000434
		public byte[] GetRawCopied()
		{
			byte[] array = new byte[this.Data.Length];
			this.Data.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000225D File Offset: 0x0000045D
		public void SetRaw(byte[] value)
		{
			this.Data = new byte[value.Length];
			value.CopyTo(this.Data, 0);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000227C File Offset: 0x0000047C
		public float GetFloat()
		{
			if (this.dataType != CustomDataType.Float)
			{
				Debug.Log(string.Format("Trying to get Float, but custom data {0} is {1}", this.key, this.dataType));
				return 0f;
			}
			float result;
			try
			{
				result = BitConverter.ToSingle(this.Data, 0);
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022E8 File Offset: 0x000004E8
		public void SetFloat(float value)
		{
			if (this.dataType != CustomDataType.Float)
			{
				Debug.LogWarning("Setting value in a different type! Allowed by CustomData.AllowChangeTypeWithSet");
				return;
			}
			this.Data = BitConverter.GetBytes(value);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000230C File Offset: 0x0000050C
		public int GetInt()
		{
			if (this.dataType != CustomDataType.Int)
			{
				Debug.Log(string.Format("Trying to get Int, but custom data {0} is {1}", this.key, this.dataType));
				return 0;
			}
			int result;
			try
			{
				result = BitConverter.ToInt32(this.Data, 0);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				result = 0;
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002380 File Offset: 0x00000580
		public void SetInt(int value)
		{
			if (this.dataType != CustomDataType.Int)
			{
				Debug.LogWarning("Setting value in a different type! Allowed by CustomData.AllowChangeTypeWithSet");
				return;
			}
			this.Data = BitConverter.GetBytes(value);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023A4 File Offset: 0x000005A4
		public bool GetBool()
		{
			if (this.dataType != CustomDataType.Bool)
			{
				Debug.Log(string.Format("Trying to get Bool, but custom data {0} is {1}", this.key, this.dataType));
				return false;
			}
			bool result;
			try
			{
				result = BitConverter.ToBoolean(this.Data, 0);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002418 File Offset: 0x00000618
		public void SetBool(bool value)
		{
			if (this.dataType != CustomDataType.Bool)
			{
				Debug.LogWarning("Setting value in a different type! Allowed by CustomData.AllowChangeTypeWithSet");
				return;
			}
			this.Data = BitConverter.GetBytes(value);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000243C File Offset: 0x0000063C
		public string GetString()
		{
			if (this.dataType != CustomDataType.String)
			{
				Debug.Log(string.Format("Trying to get String, but custom data {0} is {1}", this.key, this.dataType));
				return string.Empty;
			}
			string result;
			try
			{
				result = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				result = "*INVALID_VALUE*";
			}
			return result;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024C4 File Offset: 0x000006C4
		public void SetString(string value)
		{
			if (this.dataType != CustomDataType.String)
			{
				Debug.LogWarning("Setting value in a different type! Allowed by CustomData.AllowChangeTypeWithSet");
				return;
			}
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				this.Data = new byte[bytes.Length];
				bytes.CopyTo(this.Data, 0);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002538 File Offset: 0x00000738
		public string GetValueDisplayString(string format = "")
		{
			switch (this.dataType)
			{
			case CustomDataType.Raw:
				return "*BINARY DATA*";
			case CustomDataType.Float:
				return this.GetFloat().ToString(format);
			case CustomDataType.Int:
				return this.GetInt().ToString(format);
			case CustomDataType.Bool:
				if (this.GetBool())
				{
					return "+";
				}
				return "-";
			case CustomDataType.String:
				return this.GetString().ToPlainText();
			default:
				return "*INVALID*";
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025B3 File Offset: 0x000007B3
		public CustomData(string key, CustomDataType dataType, byte[] data)
		{
			this.key = key;
			this.dataType = dataType;
			this.Data = new byte[data.Length];
			data.CopyTo(this.Data, 0);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025F0 File Offset: 0x000007F0
		public CustomData(string key, float floatValue)
		{
			this.key = key;
			this.dataType = CustomDataType.Float;
			this.SetFloat(floatValue);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002619 File Offset: 0x00000819
		public CustomData(string key, int intValue)
		{
			this.key = key;
			this.dataType = CustomDataType.Int;
			this.SetInt(intValue);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002642 File Offset: 0x00000842
		public CustomData(string key, bool boolValue)
		{
			this.key = key;
			this.dataType = CustomDataType.Bool;
			this.SetBool(boolValue);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000266B File Offset: 0x0000086B
		public CustomData(string key, string stringValue)
		{
			this.key = key;
			this.dataType = CustomDataType.String;
			this.SetString(stringValue);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002694 File Offset: 0x00000894
		public CustomData()
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026A8 File Offset: 0x000008A8
		public CustomData(CustomData copyFrom)
		{
			this.key = copyFrom.key;
			this.dataType = copyFrom.dataType;
			this.Data = copyFrom.GetRawCopied();
		}

		// Token: 0x04000005 RID: 5
		private const bool ReturnDefaultIfTryingToGetOtherType = true;

		// Token: 0x04000006 RID: 6
		private const bool LogWarningWhenTryingToGetOtherType = true;

		// Token: 0x04000007 RID: 7
		private const bool AllowChangeTypeWithSet = false;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private string key;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private CustomDataType dataType;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private byte[] data = new byte[0];

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private bool display;
	}
}
