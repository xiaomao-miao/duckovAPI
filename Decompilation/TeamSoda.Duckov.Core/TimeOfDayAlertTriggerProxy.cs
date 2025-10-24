using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class TimeOfDayAlertTriggerProxy : MonoBehaviour
{
	// Token: 0x06000669 RID: 1641 RVA: 0x0001CF7F File Offset: 0x0001B17F
	public void OnEnter()
	{
		TimeOfDayAlert.EnterAlertTrigger();
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0001CF86 File Offset: 0x0001B186
	public void OnLeave()
	{
		TimeOfDayAlert.LeaveAlertTrigger();
	}
}
