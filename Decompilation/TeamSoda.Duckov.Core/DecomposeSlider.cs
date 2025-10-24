using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000201 RID: 513
public class DecomposeSlider : MonoBehaviour
{
	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06000F02 RID: 3842 RVA: 0x0003B79C File Offset: 0x0003999C
	// (remove) Token: 0x06000F03 RID: 3843 RVA: 0x0003B7D4 File Offset: 0x000399D4
	public event Action<float> OnValueChangedEvent;

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0003B809 File Offset: 0x00039A09
	// (set) Token: 0x06000F05 RID: 3845 RVA: 0x0003B81B File Offset: 0x00039A1B
	public int Value
	{
		get
		{
			return Mathf.RoundToInt(this.slider.value);
		}
		set
		{
			this.slider.value = (float)value;
			this.valueText.text = value.ToString();
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0003B83C File Offset: 0x00039A3C
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0003B85A File Offset: 0x00039A5A
	private void OnDestroy()
	{
		this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0003B878 File Offset: 0x00039A78
	private void OnValueChanged(float value)
	{
		this.OnValueChangedEvent(value);
		this.valueText.text = value.ToString();
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0003B898 File Offset: 0x00039A98
	public void SetMinMax(int min, int max)
	{
		this.slider.minValue = (float)min;
		this.slider.maxValue = (float)max;
		this.minText.text = min.ToString();
		this.maxText.text = max.ToString();
	}

	// Token: 0x04000C50 RID: 3152
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C51 RID: 3153
	public TextMeshProUGUI minText;

	// Token: 0x04000C52 RID: 3154
	public TextMeshProUGUI maxText;

	// Token: 0x04000C53 RID: 3155
	public TextMeshProUGUI valueText;
}
