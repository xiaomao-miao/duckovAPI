using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class SetActiveByInputDevice : MonoBehaviour
{
	// Token: 0x06000B0E RID: 2830 RVA: 0x0002F09D File Offset: 0x0002D29D
	private void Awake()
	{
		this.OnInputDeviceChanged();
		InputManager.OnInputDeviceChanged += this.OnInputDeviceChanged;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0002F0B6 File Offset: 0x0002D2B6
	private void OnDestroy()
	{
		InputManager.OnInputDeviceChanged -= this.OnInputDeviceChanged;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0002F0C9 File Offset: 0x0002D2C9
	private void OnInputDeviceChanged()
	{
		if (InputManager.InputDevice == this.activeIfDeviceIs)
		{
			base.gameObject.SetActive(true);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400097A RID: 2426
	public InputManager.InputDevices activeIfDeviceIs;
}
