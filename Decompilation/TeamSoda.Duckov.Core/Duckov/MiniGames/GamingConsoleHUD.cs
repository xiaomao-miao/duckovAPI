using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000281 RID: 641
	public class GamingConsoleHUD : View
	{
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0004BD91 File Offset: 0x00049F91
		private static GamingConsoleHUD Instance
		{
			get
			{
				if (GamingConsoleHUD._instance_cache == null)
				{
					GamingConsoleHUD._instance_cache = View.GetViewInstance<GamingConsoleHUD>();
				}
				return GamingConsoleHUD._instance_cache;
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004BDAF File Offset: 0x00049FAF
		public static void Show()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalShow();
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004BDC9 File Offset: 0x00049FC9
		public static void Hide()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalHide();
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0004BDE3 File Offset: 0x00049FE3
		private void LocalShow()
		{
			this.contentFadeGroup.Show();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0004BDF0 File Offset: 0x00049FF0
		private void LocalHide()
		{
			this.contentFadeGroup.Hide();
		}

		// Token: 0x04000F02 RID: 3842
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000F03 RID: 3843
		private static GamingConsoleHUD _instance_cache;
	}
}
