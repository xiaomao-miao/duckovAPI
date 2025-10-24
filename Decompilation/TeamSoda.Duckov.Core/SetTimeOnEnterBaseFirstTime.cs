using System;
using Saves;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class SetTimeOnEnterBaseFirstTime : MonoBehaviour
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x00019BA8 File Offset: 0x00017DA8
	private void Start()
	{
		if (SavesSystem.Load<bool>("FirstTimeToBaseTimeSetted"))
		{
			return;
		}
		SavesSystem.Save<bool>("FirstTimeToBaseTimeSetted", true);
		TimeSpan time = new TimeSpan(this.setTimeTo, 0, 0);
		GameClock.Instance.StepTimeTil(time);
	}

	// Token: 0x04000543 RID: 1347
	public int setTimeTo;
}
