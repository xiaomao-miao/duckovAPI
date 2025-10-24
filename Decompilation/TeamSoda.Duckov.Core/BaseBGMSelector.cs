using System;
using Duckov;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class BaseBGMSelector : MonoBehaviour
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008663 File Offset: 0x00006863
	// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000866A File Offset: 0x0000686A
	[LocalizationKey("Default")]
	private string BGMInfoFormatKey
	{
		get
		{
			return "BGMInfoFormat";
		}
		set
		{
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000866C File Offset: 0x0000686C
	private string BGMInfoFormat
	{
		get
		{
			return this.BGMInfoFormatKey.ToPlainText();
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00008679 File Offset: 0x00006879
	private void Awake()
	{
		SavesSystem.OnCollectSaveData += this.Save;
		this.Load(false);
		this.waitForStinger = true;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000869A File Offset: 0x0000689A
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000086AD File Offset: 0x000068AD
	private void Update()
	{
		if (this.waitForStinger && LevelManager.AfterInit && !AudioManager.IsStingerPlaying)
		{
			this.waitForStinger = false;
			this.Set(this.index, false, true);
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000086DA File Offset: 0x000068DA
	private void Load(bool play = false)
	{
		this.index = SavesSystem.Load<int>("BaseBGMSelector");
		this.Set(this.index, false, play);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x000086FA File Offset: 0x000068FA
	private void Save()
	{
		SavesSystem.Save<int>("BaseBGMSelector", this.index);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000870C File Offset: 0x0000690C
	public void Set(int index, bool showInfo = false, bool play = true)
	{
		this.waitForStinger = false;
		if (index < 0 || index >= this.entries.Length)
		{
			int num = index;
			index = Mathf.Clamp(index, 0, this.entries.Length - 1);
			Debug.LogError(string.Format("Index {0} Out Of Range,clampped to {1}", num, index));
		}
		BaseBGMSelector.Entry entry = this.entries[index];
		AudioManager.StopBGM();
		if (play)
		{
			AudioManager.PlayBGM(entry.switchName);
		}
		if (showInfo)
		{
			string text = this.BGMInfoFormat.Format(new
			{
				name = entry.musicName,
				author = entry.author,
				index = index + 1
			});
			this.proxy.Pop(text, 200f);
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000087B4 File Offset: 0x000069B4
	public void Set(string switchName)
	{
		int num = this.GetIndex(switchName);
		if (num < 0)
		{
			return;
		}
		this.Set(num, false, true);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000087D8 File Offset: 0x000069D8
	public int GetIndex(string switchName)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].switchName == switchName)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00008814 File Offset: 0x00006A14
	public void SetNext()
	{
		this.index++;
		if (this.index >= this.entries.Length)
		{
			this.index = 0;
		}
		this.Set(this.index, true, true);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00008849 File Offset: 0x00006A49
	public void SetPrevious()
	{
		this.index--;
		if (this.index < 0)
		{
			this.index = this.entries.Length - 1;
		}
		this.Set(this.index, true, true);
	}

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private string switchGroupName = "BGM";

	// Token: 0x04000167 RID: 359
	[SerializeField]
	private DialogueBubbleProxy proxy;

	// Token: 0x04000168 RID: 360
	public BaseBGMSelector.Entry[] entries;

	// Token: 0x04000169 RID: 361
	private int index;

	// Token: 0x0400016A RID: 362
	private const string savekey = "BaseBGMSelector";

	// Token: 0x0400016B RID: 363
	private bool waitForStinger;

	// Token: 0x0200042B RID: 1067
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001A03 RID: 6659
		public string switchName;

		// Token: 0x04001A04 RID: 6660
		public string musicName;

		// Token: 0x04001A05 RID: 6661
		public string author;
	}
}
