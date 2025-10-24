using System;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001FE RID: 510
public class UI_Bus_Slider : MonoBehaviour
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0003B570 File Offset: 0x00039770
	private AudioManager.Bus BusRef
	{
		get
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (this.busRef == null)
			{
				this.busRef = AudioManager.GetBus(this.busName);
				if (this.busRef == null)
				{
					Debug.LogError("Bus not found:" + this.busName);
				}
			}
			return this.busRef;
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0003B5C4 File Offset: 0x000397C4
	private void Initialize()
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.slider.SetValueWithoutNotify(this.BusRef.Volume);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
		this.initialized = true;
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0003B620 File Offset: 0x00039820
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0003B63E File Offset: 0x0003983E
	private void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0003B64E File Offset: 0x0003984E
	private void OnEnable()
	{
		this.Initialize();
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0003B658 File Offset: 0x00039858
	private void OnValueChanged(float value)
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.BusRef.Volume = value;
		this.BusRef.Mute = (value == 0f);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
	}

	// Token: 0x04000C49 RID: 3145
	private AudioManager.Bus busRef;

	// Token: 0x04000C4A RID: 3146
	[SerializeField]
	private string busName;

	// Token: 0x04000C4B RID: 3147
	[SerializeField]
	private TextMeshProUGUI volumeNumberText;

	// Token: 0x04000C4C RID: 3148
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C4D RID: 3149
	private bool initialized;
}
