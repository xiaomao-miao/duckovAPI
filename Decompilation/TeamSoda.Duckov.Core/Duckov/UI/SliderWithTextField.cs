using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038A RID: 906
	public class SliderWithTextField : MonoBehaviour
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x0006E334 File Offset: 0x0006C534
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x0006E33C File Offset: 0x0006C53C
		[LocalizationKey("Default")]
		public string LabelKey
		{
			get
			{
				return this._labelKey;
			}
			set
			{
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x0006E33E File Offset: 0x0006C53E
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x0006E346 File Offset: 0x0006C546
		public float Value
		{
			get
			{
				return this.GetValue();
			}
			set
			{
				this.SetValue(value);
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0006E34F File Offset: 0x0006C54F
		public void SetValueWithoutNotify(float value)
		{
			this.value = value;
			this.RefreshValues();
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0006E35E File Offset: 0x0006C55E
		public void SetValue(float value)
		{
			this.SetValueWithoutNotify(value);
			Action<float> action = this.onValueChanged;
			if (action == null)
			{
				return;
			}
			action(value);
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0006E378 File Offset: 0x0006C578
		public float GetValue()
		{
			return this.value;
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0006E380 File Offset: 0x0006C580
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0006E3DC File Offset: 0x0006C5DC
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0006E3EF File Offset: 0x0006C5EF
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0006E3F7 File Offset: 0x0006C5F7
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = this.LabelKey.ToPlainText();
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0006E41C File Offset: 0x0006C61C
		private void OnFieldEndEdit(string arg0)
		{
			float num;
			if (float.TryParse(arg0, out num))
			{
				if (this.isPercentage)
				{
					num /= 100f;
				}
				num = Mathf.Clamp(num, this.slider.minValue, this.slider.maxValue);
				this.Value = num;
			}
			this.RefreshValues();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0006E46D File Offset: 0x0006C66D
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0006E475 File Offset: 0x0006C675
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0006E484 File Offset: 0x0006C684
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0006E4C1 File Offset: 0x0006C6C1
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x0400157F RID: 5503
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001580 RID: 5504
		[SerializeField]
		private Slider slider;

		// Token: 0x04001581 RID: 5505
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x04001582 RID: 5506
		[SerializeField]
		private string valueFormat = "0";

		// Token: 0x04001583 RID: 5507
		[SerializeField]
		private bool isPercentage;

		// Token: 0x04001584 RID: 5508
		[SerializeField]
		private string _labelKey = "?";

		// Token: 0x04001585 RID: 5509
		[SerializeField]
		private float value;

		// Token: 0x04001586 RID: 5510
		public Action<float> onValueChanged;
	}
}
