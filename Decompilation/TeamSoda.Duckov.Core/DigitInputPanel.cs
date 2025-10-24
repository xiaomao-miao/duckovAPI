using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019D RID: 413
public class DigitInputPanel : MonoBehaviour
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06000C30 RID: 3120 RVA: 0x00033750 File Offset: 0x00031950
	// (remove) Token: 0x06000C31 RID: 3121 RVA: 0x00033788 File Offset: 0x00031988
	public event Action<string> onInputFieldValueChanged;

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000337C0 File Offset: 0x000319C0
	public long Value
	{
		get
		{
			string text = this.inputField.text;
			if (string.IsNullOrEmpty(text))
			{
				return 0L;
			}
			long result;
			if (!long.TryParse(text, out result))
			{
				return 0L;
			}
			return result;
		}
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x000337F4 File Offset: 0x000319F4
	private void Awake()
	{
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnInputFieldValueChanged));
		for (int i = 0; i < this.numKeys.Length; i++)
		{
			int v = i;
			this.numKeys[i].onClick.AddListener(delegate()
			{
				this.OnNumKeyClicked((long)v);
			});
		}
		this.clearButton.onClick.AddListener(new UnityAction(this.OnClearButtonClicked));
		this.backspaceButton.onClick.AddListener(new UnityAction(this.OnBackspaceButtonClicked));
		this.maximumButton.onClick.AddListener(new UnityAction(this.Max));
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x000338B8 File Offset: 0x00031AB8
	private void OnBackspaceButtonClicked()
	{
		if (string.IsNullOrEmpty(this.inputField.text))
		{
			return;
		}
		this.inputField.text = this.inputField.text.Substring(0, this.inputField.text.Length - 1);
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00033906 File Offset: 0x00031B06
	private void OnClearButtonClicked()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00033918 File Offset: 0x00031B18
	private void OnNumKeyClicked(long v)
	{
		this.inputField.text = string.Format("{0}{1}", this.inputField.text, v);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00033940 File Offset: 0x00031B40
	private void OnInputFieldValueChanged(string value)
	{
		long num;
		if (long.TryParse(value, out num) && num == 0L)
		{
			this.inputField.SetTextWithoutNotify(string.Empty);
		}
		Action<string> action = this.onInputFieldValueChanged;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0003397B File Offset: 0x00031B7B
	public void Setup(long value, Func<long> maxFunc = null)
	{
		this.maxFunction = maxFunc;
		this.inputField.text = string.Format("{0}", value);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x000339A0 File Offset: 0x00031BA0
	public void Max()
	{
		if (this.maxFunction == null)
		{
			return;
		}
		long num = this.maxFunction();
		this.inputField.text = string.Format("{0}", num);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x000339DD File Offset: 0x00031BDD
	internal void Clear()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private TMP_InputField inputField;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private Button clearButton;

	// Token: 0x04000A95 RID: 2709
	[SerializeField]
	private Button backspaceButton;

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private Button maximumButton;

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	private Button[] numKeys;

	// Token: 0x04000A98 RID: 2712
	public Func<long> maxFunction;
}
