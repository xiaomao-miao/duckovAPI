using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x02000263 RID: 611
	public class RuleEntry_Int : MonoBehaviour
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x000470F4 File Offset: 0x000452F4
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0004715E File Offset: 0x0004535E
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00047168 File Offset: 0x00045368
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, (int)value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00047198 File Offset: 0x00045398
		public void RefreshValue()
		{
			float valueWithoutNotify = (float)this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000471BE File Offset: 0x000453BE
		protected void SetValue(Ruleset ruleset, int value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000471D2 File Offset: 0x000453D2
		protected int GetValue(Ruleset ruleset)
		{
			return (int)this.field.GetValue(ruleset);
		}

		// Token: 0x04000E3B RID: 3643
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000E3C RID: 3644
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000E3D RID: 3645
		private FieldInfo field;
	}
}
