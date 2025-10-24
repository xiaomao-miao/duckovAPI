using System;
using System.Reflection;
using Duckov.Rules;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class RuleEntry_Bool : OptionsProviderBase
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0003983D File Offset: 0x00037A3D
	public override string Key
	{
		get
		{
			return this.fieldName;
		}
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x00039845 File Offset: 0x00037A45
	private void Awake()
	{
		this.field = typeof(Ruleset).GetField(this.fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x00039864 File Offset: 0x00037A64
	public override string GetCurrentOption()
	{
		Ruleset obj = GameRulesManager.Current;
		if ((bool)this.field.GetValue(obj))
		{
			return "Options_On".ToPlainText();
		}
		return "Options_Off".ToPlainText();
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0003989F File Offset: 0x00037A9F
	public override string[] GetOptions()
	{
		return new string[]
		{
			"Options_Off".ToPlainText(),
			"Options_On".ToPlainText()
		};
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x000398C4 File Offset: 0x00037AC4
	public override void Set(int index)
	{
		bool flag = index > 0;
		Ruleset obj = GameRulesManager.Current;
		this.field.SetValue(obj, flag);
	}

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	private string fieldName;

	// Token: 0x04000BCD RID: 3021
	private FieldInfo field;
}
