using System;
using System.Collections.Generic;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000259 RID: 601
	public class ModifyCharacterStatsBase : PerkBehaviour
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00046539 File Offset: 0x00044739
		private string DescriptionFormat
		{
			get
			{
				return "PerkBehaviour_ModifyCharacterStatsBase".ToPlainText();
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x00046548 File Offset: 0x00044748
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
				{
					if (entry != null && !string.IsNullOrEmpty(entry.key))
					{
						string statDisplayName = ("Stat_" + entry.key.Trim()).ToPlainText();
						bool flag = entry.value > 0f;
						float value = entry.value;
						string str = entry.percentage ? string.Format("{0}%", value * 100f) : value.ToString();
						string value2 = (flag ? "+" : "") + str;
						string value3 = this.DescriptionFormat.Format(new
						{
							statDisplayName = statDisplayName,
							value = value2
						});
						stringBuilder.AppendLine(value3);
					}
				}
				return stringBuilder.ToString().Trim();
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00046650 File Offset: 0x00044850
		protected override void OnUnlocked()
		{
			LevelManager instance = LevelManager.Instance;
			Item item;
			if (instance == null)
			{
				item = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
			}
			this.targetItem = item;
			if (this.targetItem == null)
			{
				return;
			}
			StatCollection stats = this.targetItem.Stats;
			if (stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
			{
				Stat stat = stats.GetStat(entry.key);
				if (stat == null)
				{
					break;
				}
				stat.BaseValue += entry.value;
				this.records.Add(new ModifyCharacterStatsBase.Record
				{
					stat = stat,
					value = entry.value
				});
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00046734 File Offset: 0x00044934
		protected override void OnLocked()
		{
			if (this.targetItem == null)
			{
				return;
			}
			if (this.targetItem.Stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Record record in this.records)
			{
				if (record.stat == null)
				{
					break;
				}
				record.stat.BaseValue -= record.value;
			}
		}

		// Token: 0x04000E21 RID: 3617
		[SerializeField]
		private List<ModifyCharacterStatsBase.Entry> entries = new List<ModifyCharacterStatsBase.Entry>();

		// Token: 0x04000E22 RID: 3618
		private Item targetItem;

		// Token: 0x04000E23 RID: 3619
		private List<ModifyCharacterStatsBase.Record> records = new List<ModifyCharacterStatsBase.Record>();

		// Token: 0x02000537 RID: 1335
		[Serializable]
		public class Entry
		{
			// Token: 0x17000755 RID: 1877
			// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000918EB File Offset: 0x0008FAEB
			private StringList AvaliableKeys
			{
				get
				{
					return StringLists.StatKeys;
				}
			}

			// Token: 0x04001E7F RID: 7807
			public string key;

			// Token: 0x04001E80 RID: 7808
			public float value;

			// Token: 0x04001E81 RID: 7809
			public bool percentage;
		}

		// Token: 0x02000538 RID: 1336
		private struct Record
		{
			// Token: 0x04001E82 RID: 7810
			public Stat stat;

			// Token: 0x04001E83 RID: 7811
			public float value;
		}
	}
}
