using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class MainMenu : MonoBehaviour
{
	// Token: 0x06000AE5 RID: 2789 RVA: 0x0002EBEC File Offset: 0x0002CDEC
	private void Awake()
	{
		Action onMainMenuAwake = MainMenu.OnMainMenuAwake;
		if (onMainMenuAwake == null)
		{
			return;
		}
		onMainMenuAwake();
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0002EBFD File Offset: 0x0002CDFD
	private void OnDestroy()
	{
		Action onMainMenuDestroy = MainMenu.OnMainMenuDestroy;
		if (onMainMenuDestroy == null)
		{
			return;
		}
		onMainMenuDestroy();
	}

	// Token: 0x04000961 RID: 2401
	public static Action OnMainMenuAwake;

	// Token: 0x04000962 RID: 2402
	public static Action OnMainMenuDestroy;
}
