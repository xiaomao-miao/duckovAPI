using System;
using System.Collections.Generic;
using System.Linq;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x0200025F RID: 607
	public class OptionsUIEntry_Dropdown : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x00046BC0 File Offset: 0x00044DC0
		private string optionKey
		{
			get
			{
				if (this.provider == null)
				{
					return "";
				}
				return this.provider.Key;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x00046BE1 File Offset: 0x00044DE1
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x00046BF3 File Offset: 0x00044DF3
		[LocalizationKey("Options")]
		public string LabelKey
		{
			get
			{
				return "Options_" + this.optionKey;
			}
			set
			{
			}
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00046BF5 File Offset: 0x00044DF5
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.SetupDropdown();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00046C00 File Offset: 0x00044E00
		private void SetupDropdown()
		{
			if (!this.provider)
			{
				return;
			}
			List<string> list = this.provider.GetOptions().ToList<string>();
			string currentOption = this.provider.GetCurrentOption();
			int num = list.IndexOf(currentOption);
			if (num < 0)
			{
				list.Insert(0, currentOption);
				num = 0;
			}
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(list.ToList<string>());
			this.dropdown.SetValueWithoutNotify(num);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00046C78 File Offset: 0x00044E78
		private void Awake()
		{
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnDropdownValueChanged));
			this.label.text = this.LabelKey.ToPlainText();
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00046CC8 File Offset: 0x00044EC8
		private void Start()
		{
			this.SetupDropdown();
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00046CD0 File Offset: 0x00044ED0
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00046CE3 File Offset: 0x00044EE3
		private void OnSetLanguage(SystemLanguage language)
		{
			this.SetupDropdown();
			this.label.text = this.LabelKey.ToPlainText();
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00046D04 File Offset: 0x00044F04
		private void OnDropdownValueChanged(int index)
		{
			if (!this.provider)
			{
				return;
			}
			int num = this.provider.GetOptions().ToList<string>().IndexOf(this.dropdown.options[index].text);
			if (num >= 0)
			{
				this.provider.Set(num);
			}
			this.SetupDropdown();
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00046D61 File Offset: 0x00044F61
		private void OnValidate()
		{
			if (this.label)
			{
				this.label.text = this.LabelKey.ToPlainText();
			}
		}

		// Token: 0x04000E2B RID: 3627
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E2C RID: 3628
		[SerializeField]
		private OptionsProviderBase provider;

		// Token: 0x04000E2D RID: 3629
		[SerializeField]
		private TMP_Dropdown dropdown;
	}
}
