using System;
using Duckov;
using SodaCraft.Localizations;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000163 RID: 355
public class AddToWishListButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000ACD RID: 2765 RVA: 0x0002E977 File Offset: 0x0002CB77
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			AddToWishListButton.ShowPage();
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x0002E988 File Offset: 0x0002CB88
	public static void ShowPage()
	{
		if (SteamManager.Initialized)
		{
			SteamFriends.ActivateGameOverlayToStore(new AppId_t(3167020U), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			return;
		}
		if (GameMetaData.Instance.Platform == Platform.Steam)
		{
			Application.OpenURL("https://store.steampowered.com/app/3167020/");
			return;
		}
		if (LocalizationManager.CurrentLanguage == SystemLanguage.ChineseSimplified)
		{
			Application.OpenURL("https://game.bilibili.com/duckov/");
			return;
		}
		Application.OpenURL("https://www.duckov.com");
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0002E9E3 File Offset: 0x0002CBE3
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000955 RID: 2389
	private const string url = "https://store.steampowered.com/app/3167020/";

	// Token: 0x04000956 RID: 2390
	private const string CNUrl = "https://game.bilibili.com/duckov/";

	// Token: 0x04000957 RID: 2391
	private const string ENUrl = "https://www.duckov.com";

	// Token: 0x04000958 RID: 2392
	private const uint appid = 3167020U;
}
