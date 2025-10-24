using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x02000260 RID: 608
	public class OptionsUIEntry_Slider : MonoBehaviour
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00046D8E File Offset: 0x00044F8E
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x00046DA0 File Offset: 0x00044FA0
		[LocalizationKey("Options")]
		private string labelKey
		{
			get
			{
				return "Options_" + this.key;
			}
			set
			{
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00046DA2 File Offset: 0x00044FA2
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00046DB5 File Offset: 0x00044FB5
		public float Value
		{
			get
			{
				return OptionsManager.Load<float>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<float>(this.key, value);
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00046DC4 File Offset: 0x00044FC4
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00046E20 File Offset: 0x00045020
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00046E33 File Offset: 0x00045033
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00046E3B File Offset: 0x0004503B
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = this.labelKey.ToPlainText();
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00046E60 File Offset: 0x00045060
		private void OnFieldEndEdit(string arg0)
		{
			float value;
			if (float.TryParse(arg0, out value))
			{
				value = Mathf.Clamp(value, this.slider.minValue, this.slider.maxValue);
				this.Value = value;
			}
			this.RefreshValues();
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00046EA1 File Offset: 0x000450A1
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00046EA9 File Offset: 0x000450A9
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00046EB8 File Offset: 0x000450B8
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00046EF5 File Offset: 0x000450F5
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x04000E2E RID: 3630
		[SerializeField]
		private string key;

		// Token: 0x04000E2F RID: 3631
		[Space]
		[SerializeField]
		private float defaultValue;

		// Token: 0x04000E30 RID: 3632
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E31 RID: 3633
		[SerializeField]
		private Slider slider;

		// Token: 0x04000E32 RID: 3634
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x04000E33 RID: 3635
		[SerializeField]
		private string valueFormat = "0";
	}
}
