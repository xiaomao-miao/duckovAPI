using System;
using Cysharp.Threading.Tasks;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020001DF RID: 479
public class UIKeybindingEntry : MonoBehaviour
{
	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00039690 File Offset: 0x00037890
	// (set) Token: 0x06000E3B RID: 3643 RVA: 0x000396DF File Offset: 0x000378DF
	[LocalizationKey("UIText")]
	private string displayNameKey
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrideDisplayNameKey))
			{
				return this.overrideDisplayNameKey;
			}
			if (this.actionRef == null)
			{
				return "?";
			}
			return "Input_" + this.actionRef.action.name;
		}
		set
		{
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x000396E1 File Offset: 0x000378E1
	private void Awake()
	{
		this.rebindButton.onClick.AddListener(new UnityAction(this.OnButtonClick));
		this.Setup();
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00039716 File Offset: 0x00037916
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00039729 File Offset: 0x00037929
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.label.text = this.displayNameKey.ToPlainText();
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00039741 File Offset: 0x00037941
	private void OnButtonClick()
	{
		InputRebinder.RebindAsync(this.actionRef.action.name, this.index, this.excludes, true).Forget<bool>();
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0003976A File Offset: 0x0003796A
	private void OnValidate()
	{
		this.Setup();
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00039772 File Offset: 0x00037972
	private void Setup()
	{
		this.indicator.Setup(this.actionRef, this.index);
		this.label.text = this.displayNameKey.ToPlainText();
	}

	// Token: 0x04000BC2 RID: 3010
	[SerializeField]
	private InputActionReference actionRef;

	// Token: 0x04000BC3 RID: 3011
	[SerializeField]
	private int index;

	// Token: 0x04000BC4 RID: 3012
	[SerializeField]
	private string overrideDisplayNameKey;

	// Token: 0x04000BC5 RID: 3013
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/press",
		"<Mouse>/scroll"
	};

	// Token: 0x04000BC6 RID: 3014
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04000BC7 RID: 3015
	[SerializeField]
	private Button rebindButton;

	// Token: 0x04000BC8 RID: 3016
	[SerializeField]
	private InputIndicator indicator;
}
