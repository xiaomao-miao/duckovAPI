using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MiniExcelLibs;
using MiniLocalizor;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class CSVFileLocalizor : ILocalizationProvider
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public string Path
	{
		get
		{
			return this.path;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public SystemLanguage Language
	{
		get
		{
			return this._language;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	public CSVFileLocalizor(string path)
	{
		this.path = path;
		if (!Enum.TryParse<SystemLanguage>(System.IO.Path.GetFileNameWithoutExtension(path), out this._language))
		{
			this._language = SystemLanguage.Unknown;
		}
		this.BuildDictionary();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000209C File Offset: 0x0000029C
	public CSVFileLocalizor(SystemLanguage language)
	{
		string text = System.IO.Path.Combine(Application.streamingAssetsPath, "Localization/" + language.ToString() + ".csv");
		this.path = text;
		if (!Enum.TryParse<SystemLanguage>(System.IO.Path.GetFileNameWithoutExtension(text), out this._language))
		{
			this._language = SystemLanguage.Unknown;
		}
		this.BuildDictionary();
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000210C File Offset: 0x0000030C
	public void BuildDictionary()
	{
		this.dic.Clear();
		if (!File.Exists(this.path))
		{
			Debug.LogWarning("本地化文件不存在 " + this.path + ", 将创建本地文件");
			File.Create(this.path);
		}
		try
		{
			using (FileStream fileStream = File.OpenRead(this.path))
			{
				foreach (DataEntry dataEntry in fileStream.Query(null, ExcelType.CSV, "A1", null))
				{
					if (dataEntry != null && !string.IsNullOrEmpty(dataEntry.key))
					{
						this.dic[dataEntry.key] = dataEntry;
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("读取文件时发生错误，请尝试关闭外部编辑软件（如Excel）再试。");
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002200 File Offset: 0x00000400
	public string Get(string key)
	{
		DataEntry entry = this.GetEntry(key);
		if (entry == null)
		{
			return null;
		}
		string value = entry.value;
		return CSVFileLocalizor.ConvertFromEscapes(value);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002229 File Offset: 0x00000429
	private static string ConvertFromEscapes(string origin)
	{
		if (string.IsNullOrEmpty(origin))
		{
			return origin;
		}
		return Regex.Unescape(origin);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000223B File Offset: 0x0000043B
	private static string ConvertToEscapes(string origin)
	{
		if (string.IsNullOrEmpty(origin))
		{
			return origin;
		}
		return Regex.Escape(origin);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002250 File Offset: 0x00000450
	public DataEntry GetEntry(string key)
	{
		DataEntry result;
		if (!this.dic.TryGetValue(key, out result))
		{
			return null;
		}
		return result;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002270 File Offset: 0x00000470
	public bool HasKey(string key)
	{
		return this.dic.ContainsKey(key);
	}

	// Token: 0x04000001 RID: 1
	private string path;

	// Token: 0x04000002 RID: 2
	private Dictionary<string, DataEntry> dic = new Dictionary<string, DataEntry>();

	// Token: 0x04000003 RID: 3
	private SystemLanguage _language;

	// Token: 0x04000004 RID: 4
	private const bool convertFromEscapes = true;
}
