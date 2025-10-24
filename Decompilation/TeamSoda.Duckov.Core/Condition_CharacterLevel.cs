using System;
using Duckov;
using Duckov.Quests;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class Condition_CharacterLevel : Condition
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000966 RID: 2406 RVA: 0x000292B0 File Offset: 0x000274B0
	[LocalizationKey("Default")]
	private string DisplayTextFormatKey
	{
		get
		{
			switch (this.relation)
			{
			case Condition_CharacterLevel.Relation.LessThan:
				return "Condition_CharacterLevel_LessThan";
			case Condition_CharacterLevel.Relation.Equals:
				return "Condition_CharacterLevel_Equals";
			case Condition_CharacterLevel.Relation.GreaterThan:
				return "Condition_CharacterLevel_GreaterThan";
			}
			return "";
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000967 RID: 2407 RVA: 0x000292F5 File Offset: 0x000274F5
	private string DisplayTextFormat
	{
		get
		{
			return this.DisplayTextFormatKey.ToPlainText();
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000968 RID: 2408 RVA: 0x00029302 File Offset: 0x00027502
	public override string DisplayText
	{
		get
		{
			return this.DisplayTextFormat.Format(new
			{
				this.level
			});
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0002931C File Offset: 0x0002751C
	public override bool Evaluate()
	{
		int num = EXPManager.Level;
		switch (this.relation)
		{
		case Condition_CharacterLevel.Relation.LessThan:
			return num <= this.level;
		case Condition_CharacterLevel.Relation.Equals:
			return num == this.level;
		case Condition_CharacterLevel.Relation.GreaterThan:
			return num >= this.level;
		}
		return false;
	}

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private Condition_CharacterLevel.Relation relation;

	// Token: 0x04000850 RID: 2128
	[SerializeField]
	private int level;

	// Token: 0x02000496 RID: 1174
	private enum Relation
	{
		// Token: 0x04001BDA RID: 7130
		LessThan = 1,
		// Token: 0x04001BDB RID: 7131
		Equals,
		// Token: 0x04001BDC RID: 7132
		GreaterThan = 4
	}
}
