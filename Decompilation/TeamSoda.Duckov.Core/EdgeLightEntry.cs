using System;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class EdgeLightEntry : MonoBehaviour
{
	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06000DB8 RID: 3512 RVA: 0x0003835C File Offset: 0x0003655C
	// (remove) Token: 0x06000DB9 RID: 3513 RVA: 0x00038390 File Offset: 0x00036590
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000DBA RID: 3514 RVA: 0x000383C3 File Offset: 0x000365C3
	public static void SetEnabled(bool enabled)
	{
		EdgeLightEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = EdgeLightEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x000383DA File Offset: 0x000365DA
	private void Awake()
	{
		EdgeLightEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x000383FD File Offset: 0x000365FD
	private void OnDestroy()
	{
		EdgeLightEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x00038410 File Offset: 0x00036610
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(EdgeLightEntry.settingEnabled);
	}

	// Token: 0x04000B97 RID: 2967
	private static bool settingEnabled = true;
}
