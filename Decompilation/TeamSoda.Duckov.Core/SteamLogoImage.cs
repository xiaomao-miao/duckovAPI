using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EA RID: 490
public class SteamLogoImage : MonoBehaviour
{
	// Token: 0x06000E73 RID: 3699 RVA: 0x0003A19F File Offset: 0x0003839F
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0003A1A8 File Offset: 0x000383A8
	private void Refresh()
	{
		if (!SteamManager.Initialized)
		{
			this.image.sprite = this.steamLogo;
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.image.sprite = this.steamChinaLogo;
			return;
		}
		this.image.sprite = this.steamLogo;
	}

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private Image image;

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private Sprite steamLogo;

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private Sprite steamChinaLogo;
}
