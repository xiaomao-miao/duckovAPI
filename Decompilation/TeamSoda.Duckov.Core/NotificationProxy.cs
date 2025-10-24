using System;
using Duckov.UI;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class NotificationProxy : MonoBehaviour
{
	// Token: 0x0600063C RID: 1596 RVA: 0x0001C043 File Offset: 0x0001A243
	public void Notify()
	{
		NotificationText.Push(this.notification.ToPlainText());
	}

	// Token: 0x040005F2 RID: 1522
	[LocalizationKey("Default")]
	public string notification;
}
