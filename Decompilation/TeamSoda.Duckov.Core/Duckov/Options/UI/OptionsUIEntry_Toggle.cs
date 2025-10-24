using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x02000261 RID: 609
	public class OptionsUIEntry_Toggle : MonoBehaviour
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00046F10 File Offset: 0x00045110
		// (set) Token: 0x060012EE RID: 4846 RVA: 0x00046F22 File Offset: 0x00045122
		[LocalizationKey("Default")]
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

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00046F24 File Offset: 0x00045124
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x00046F37 File Offset: 0x00045137
		public bool Value
		{
			get
			{
				return OptionsManager.Load<bool>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<bool>(this.key, value);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00046F45 File Offset: 0x00045145
		private int SliderValue
		{
			get
			{
				if (!this.Value)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00046F54 File Offset: 0x00045154
		private void Awake()
		{
			this.toggle.wholeNumbers = true;
			this.toggle.minValue = 0f;
			this.toggle.maxValue = 1f;
			this.toggle.onValueChanged.AddListener(new UnityAction<float>(this.OnToggleValueChanged));
			this.label.text = this.labelKey.ToPlainText();
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00046FBF File Offset: 0x000451BF
		private void OnEnable()
		{
			this.toggle.SetValueWithoutNotify((float)this.SliderValue);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00046FD3 File Offset: 0x000451D3
		private void OnToggleValueChanged(float value)
		{
			this.Value = (value > 0f);
		}

		// Token: 0x04000E34 RID: 3636
		[SerializeField]
		private string key;

		// Token: 0x04000E35 RID: 3637
		[SerializeField]
		private bool defaultValue;

		// Token: 0x04000E36 RID: 3638
		[Space]
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E37 RID: 3639
		[SerializeField]
		private Slider toggle;
	}
}
