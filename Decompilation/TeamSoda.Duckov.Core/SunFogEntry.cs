using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class SunFogEntry : MonoBehaviour
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06000E14 RID: 3604 RVA: 0x000392A8 File Offset: 0x000374A8
	// (remove) Token: 0x06000E15 RID: 3605 RVA: 0x000392DC File Offset: 0x000374DC
	private static event Action OnSettingChangedEvent;

	// Token: 0x06000E16 RID: 3606 RVA: 0x0003930F File Offset: 0x0003750F
	public static void SetEnabled(bool enabled)
	{
		SunFogEntry.settingEnabled = enabled;
		Action onSettingChangedEvent = SunFogEntry.OnSettingChangedEvent;
		if (onSettingChangedEvent == null)
		{
			return;
		}
		onSettingChangedEvent();
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x00039326 File Offset: 0x00037526
	private void Awake()
	{
		SunFogEntry.OnSettingChangedEvent += this.OnSettingChanged;
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x00039349 File Offset: 0x00037549
	private void OnDestroy()
	{
		SunFogEntry.OnSettingChangedEvent -= this.OnSettingChanged;
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0003935C File Offset: 0x0003755C
	private void OnSettingChanged()
	{
		base.gameObject.SetActive(SunFogEntry.settingEnabled);
	}

	// Token: 0x04000BB9 RID: 3001
	private static bool settingEnabled = true;
}
