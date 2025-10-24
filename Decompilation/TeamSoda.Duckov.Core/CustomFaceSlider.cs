using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000036 RID: 54
public class CustomFaceSlider : MonoBehaviour
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600012C RID: 300 RVA: 0x00005BE3 File Offset: 0x00003DE3
	public float Value
	{
		get
		{
			return this.slider.value;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00005BF0 File Offset: 0x00003DF0
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEditField));
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00005C2A File Offset: 0x00003E2A
	private void Start()
	{
		this.RefreshFieldText();
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00005C34 File Offset: 0x00003E34
	private void OnEndEditField(string str)
	{
		float num;
		if (float.TryParse(str, out num))
		{
			num = Mathf.Clamp(num, this.slider.minValue, this.slider.maxValue);
			this.slider.SetValueWithoutNotify(num);
			this.master.SetDirty();
		}
		this.RefreshFieldText();
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00005C85 File Offset: 0x00003E85
	private void OnDestroy()
	{
		this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnSliderValueChanged));
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00005CA3 File Offset: 0x00003EA3
	public void Init(float minValue, float maxValue, CustomFaceUI _master, string nameKey)
	{
		this.master = _master;
		this.SetMinMaxValue(minValue, maxValue);
		this.SetNameKey(nameKey);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00005CBC File Offset: 0x00003EBC
	private void OnSliderValueChanged(float _value)
	{
		this.valueField.SetTextWithoutNotify(_value.ToString(this.valueFormat));
		this.master.SetDirty();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00005CE1 File Offset: 0x00003EE1
	public void SetNameKey(string _nameKey)
	{
		this.nameText.Key = _nameKey;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00005CEF File Offset: 0x00003EEF
	public void SetMinMaxValue(float min, float max)
	{
		this.slider.minValue = min;
		this.slider.maxValue = max;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00005D09 File Offset: 0x00003F09
	public void SetValue(float value)
	{
		this.slider.value = value;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00005D18 File Offset: 0x00003F18
	private void RefreshFieldText()
	{
		this.valueField.SetTextWithoutNotify(this.slider.value.ToString(this.valueFormat));
	}

	// Token: 0x040000B7 RID: 183
	[SerializeField]
	private Slider slider;

	// Token: 0x040000B8 RID: 184
	[SerializeField]
	private TMP_InputField valueField;

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private string valueFormat = "0.##";

	// Token: 0x040000BA RID: 186
	[SerializeField]
	private TextLocalizor nameText;

	// Token: 0x040000BB RID: 187
	private CustomFaceUI master;
}
