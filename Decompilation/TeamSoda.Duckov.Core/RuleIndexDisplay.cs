using System;
using Duckov.Rules;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class RuleIndexDisplay : MonoBehaviour
{
	// Token: 0x06000AAA RID: 2730 RVA: 0x0002E50E File Offset: 0x0002C70E
	private void Awake()
	{
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0002E521 File Offset: 0x0002C721
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0002E534 File Offset: 0x0002C734
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.Refresh();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0002E53C File Offset: 0x0002C73C
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0002E544 File Offset: 0x0002C744
	private void Refresh()
	{
		this.text.text = string.Format("Rule_{0}", GameRulesManager.SelectedRuleIndex).ToPlainText();
	}

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	private TextMeshProUGUI text;
}
