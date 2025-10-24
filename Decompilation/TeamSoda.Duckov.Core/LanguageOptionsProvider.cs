using System;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class LanguageOptionsProvider : OptionsProviderBase
{
	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00037B07 File Offset: 0x00035D07
	public override string Key
	{
		get
		{
			return "Language";
		}
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00037B0E File Offset: 0x00035D0E
	public override string GetCurrentOption()
	{
		return LocalizationManager.CurrentLanguageDisplayName;
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x00037B18 File Offset: 0x00035D18
	public override string[] GetOptions()
	{
		LocalizationDatabase instance = LocalizationDatabase.Instance;
		if (instance == null)
		{
			return new string[]
			{
				"?"
			};
		}
		string[] languageDisplayNameList = instance.GetLanguageDisplayNameList();
		this.cache = languageDisplayNameList;
		return languageDisplayNameList;
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x00037B52 File Offset: 0x00035D52
	public override void Set(int index)
	{
		if (this.cache == null)
		{
			this.GetOptions();
		}
		if (index < 0 || index >= this.cache.Length)
		{
			Debug.LogError("语言越界");
			return;
		}
		LocalizationManager.SetLanguage(index);
	}

	// Token: 0x04000B87 RID: 2951
	private string[] cache;
}
