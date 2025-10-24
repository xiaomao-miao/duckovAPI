using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x02000262 RID: 610
	public class RuleEntry_Float : MonoBehaviour
	{
		// Token: 0x060012F6 RID: 4854 RVA: 0x00046FF0 File Offset: 0x000451F0
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0004705A File Offset: 0x0004525A
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00047064 File Offset: 0x00045264
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00047094 File Offset: 0x00045294
		public void RefreshValue()
		{
			float value = this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(value);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000470B9 File Offset: 0x000452B9
		protected void SetValue(Ruleset ruleset, float value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000470CD File Offset: 0x000452CD
		protected float GetValue(Ruleset ruleset)
		{
			return (float)this.field.GetValue(ruleset);
		}

		// Token: 0x04000E38 RID: 3640
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000E39 RID: 3641
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000E3A RID: 3642
		private FieldInfo field;
	}
}
