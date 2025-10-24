using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x0200023F RID: 575
	public class ItemUnlockNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00044651 File Offset: 0x00042851
		public string MainTextFormat
		{
			get
			{
				return this.mainTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x0004465E File Offset: 0x0004285E
		private string SubTextFormat
		{
			get
			{
				return this.subTextFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0004466B File Offset: 0x0004286B
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00044672 File Offset: 0x00042872
		public static ItemUnlockNotification Instance { get; private set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x0004467A File Offset: 0x0004287A
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x0004468A File Offset: 0x0004288A
		public static bool Showing
		{
			get
			{
				return !(ItemUnlockNotification.Instance == null) && ItemUnlockNotification.Instance.showing;
			}
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000446A5 File Offset: 0x000428A5
		private void Awake()
		{
			if (ItemUnlockNotification.Instance == null)
			{
				ItemUnlockNotification.Instance = this;
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000446BA File Offset: 0x000428BA
		private void Update()
		{
			if (!this.showing && ItemUnlockNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000446D7 File Offset: 0x000428D7
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x000446E8 File Offset: 0x000428E8
		private UniTask ShowTask()
		{
			ItemUnlockNotification.<ShowTask>d__26 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<ItemUnlockNotification.<ShowTask>d__26>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004472C File Offset: 0x0004292C
		private UniTask DisplayContent(int itemTypeID)
		{
			ItemUnlockNotification.<DisplayContent>d__27 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.itemTypeID = itemTypeID;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<ItemUnlockNotification.<DisplayContent>d__27>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00044778 File Offset: 0x00042978
		private void Setup(int itemTypeID)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			string displayName = metaData.DisplayName;
			Sprite icon = metaData.icon;
			this.image.sprite = icon;
			this.textMain.text = this.MainTextFormat.Format(new
			{
				itemDisplayName = displayName
			});
			this.textSub.text = this.SubTextFormat;
			DisplayQuality displayQuality = metaData.displayQuality;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(displayQuality).Apply(this.shadow);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000447F1 File Offset: 0x000429F1
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointerClicked = true;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000447FA File Offset: 0x000429FA
		public static void Push(int itemTypeID)
		{
			ItemUnlockNotification.pending.Add(itemTypeID);
		}

		// Token: 0x04000DB4 RID: 3508
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000DB5 RID: 3509
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000DB6 RID: 3510
		[SerializeField]
		private Image image;

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000DB8 RID: 3512
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000DB9 RID: 3513
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000DBA RID: 3514
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000DBB RID: 3515
		[SerializeField]
		[LocalizationKey("Default")]
		private string mainTextFormatKey = "UI_ItemUnlockNotification";

		// Token: 0x04000DBC RID: 3516
		[SerializeField]
		[LocalizationKey("Default")]
		private string subTextFormatKey = "UI_ItemUnlockNotification_Sub";

		// Token: 0x04000DBD RID: 3517
		private static List<int> pending = new List<int>();

		// Token: 0x04000DBF RID: 3519
		private UniTask showingTask;

		// Token: 0x04000DC0 RID: 3520
		private bool pointerClicked;
	}
}
