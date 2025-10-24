using System;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000235 RID: 565
	public class RichPresenceManager : MonoBehaviour
	{
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00043ED9 File Offset: 0x000420D9
		public bool isPlaying
		{
			get
			{
				return !this.isMainMenu;
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00043EE4 File Offset: 0x000420E4
		private void InvokeChangeEvent()
		{
			Action<RichPresenceManager> onInstanceChanged = RichPresenceManager.OnInstanceChanged;
			if (onInstanceChanged == null)
			{
				return;
			}
			onInstanceChanged(this);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00043EF8 File Offset: 0x000420F8
		private void Awake()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Combine(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Combine(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake += this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy += this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00043F68 File Offset: 0x00042168
		private void OnDestroy()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Remove(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Remove(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake -= this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy -= this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00043FD7 File Offset: 0x000421D7
		private void OnMainMenuAwake()
		{
			this.isMainMenu = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00043FE6 File Offset: 0x000421E6
		private void OnMainMenuDestroy()
		{
			this.isMainMenu = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00043FF5 File Offset: 0x000421F5
		private void OnMultiSceneCoreInstanceAwake(MultiSceneCore core)
		{
			this.levelDisplayNameRaw = core.DisplaynameRaw;
			this.isInLevel = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00044010 File Offset: 0x00042210
		private void OnMultiSceneCoreInstanceDestroy(MultiSceneCore core)
		{
			this.isInLevel = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004401F File Offset: 0x0004221F
		internal string GetSteamDisplay()
		{
			if (Application.isEditor)
			{
				return "#Status_UnityEditor";
			}
			if (!this.isMainMenu)
			{
				return "#Status_Playing";
			}
			return "#Status_MainMenu";
		}

		// Token: 0x04000D94 RID: 3476
		public bool isMainMenu = true;

		// Token: 0x04000D95 RID: 3477
		public bool isInLevel;

		// Token: 0x04000D96 RID: 3478
		public string levelDisplayNameRaw;

		// Token: 0x04000D97 RID: 3479
		public static Action<RichPresenceManager> OnInstanceChanged;
	}
}
