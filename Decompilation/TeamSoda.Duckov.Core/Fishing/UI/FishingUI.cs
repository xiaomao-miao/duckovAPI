using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing.UI
{
	// Token: 0x0200021A RID: 538
	public class FishingUI : View
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x0003E998 File Offset: 0x0003CB98
		protected override void Awake()
		{
			base.Awake();
			Action_Fishing.OnPlayerStartSelectBait += this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003E9D3 File Offset: 0x0003CBD3
		protected override void OnDestroy()
		{
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
			Action_Fishing.OnPlayerStartSelectBait -= this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			base.OnDestroy();
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003EA0E File Offset: 0x0003CC0E
		internal override void TryQuit()
		{
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003EA10 File Offset: 0x0003CC10
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			Debug.Log("Open Fishing Panel");
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003EA2D File Offset: 0x0003CC2D
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003EA40 File Offset: 0x0003CC40
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.baitSelectPanel.NotifyStop();
			this.confirmPanel.NotifyStop();
			base.Close();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003EA5E File Offset: 0x0003CC5E
		private void OnStartSelectBait(Action_Fishing fishing, ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			this.SelectBaitTask(availableBaits, baitSelectionResultCallback).Forget();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003EA70 File Offset: 0x0003CC70
		private UniTask SelectBaitTask(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			FishingUI.<SelectBaitTask>d__10 <SelectBaitTask>d__;
			<SelectBaitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectBaitTask>d__.<>4__this = this;
			<SelectBaitTask>d__.availableBaits = availableBaits;
			<SelectBaitTask>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<SelectBaitTask>d__.<>1__state = -1;
			<SelectBaitTask>d__.<>t__builder.Start<FishingUI.<SelectBaitTask>d__10>(ref <SelectBaitTask>d__);
			return <SelectBaitTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003EAC3 File Offset: 0x0003CCC3
		private void OnStopCatching(Action_Fishing fishing, Item catchedItem, Action<bool> confirmCallback)
		{
			this.ConfirmTask(catchedItem, confirmCallback).Forget();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003EAD4 File Offset: 0x0003CCD4
		private UniTask ConfirmTask(Item catchedItem, Action<bool> confirmCallback)
		{
			FishingUI.<ConfirmTask>d__12 <ConfirmTask>d__;
			<ConfirmTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ConfirmTask>d__.<>4__this = this;
			<ConfirmTask>d__.catchedItem = catchedItem;
			<ConfirmTask>d__.confirmCallback = confirmCallback;
			<ConfirmTask>d__.<>1__state = -1;
			<ConfirmTask>d__.<>t__builder.Start<FishingUI.<ConfirmTask>d__12>(ref <ConfirmTask>d__);
			return <ConfirmTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000CDA RID: 3290
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CDB RID: 3291
		[SerializeField]
		private BaitSelectPanel baitSelectPanel;

		// Token: 0x04000CDC RID: 3292
		[SerializeField]
		private ConfirmPanel confirmPanel;
	}
}
