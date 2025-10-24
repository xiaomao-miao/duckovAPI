using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B8 RID: 952
	public class MapSelectionView : View
	{
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00078B9F File Offset: 0x00076D9F
		public static MapSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<MapSelectionView>();
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00078BA6 File Offset: 0x00076DA6
		protected override void Awake()
		{
			base.Awake();
			this.btnConfirm.onClick.AddListener(delegate()
			{
				this.confirmButtonClicked = true;
			});
			this.btnCancel.onClick.AddListener(delegate()
			{
				this.cancelButtonClicked = true;
			});
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00078BE6 File Offset: 0x00076DE6
		protected override void OnOpen()
		{
			base.OnOpen();
			this.confirmIndicatorFadeGroup.SkipHide();
			this.mainFadeGroup.Show();
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00078C04 File Offset: 0x00076E04
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00078C18 File Offset: 0x00076E18
		internal void NotifyEntryClicked(MapSelectionEntry mapSelectionEntry, PointerEventData eventData)
		{
			if (!mapSelectionEntry.Cost.Enough)
			{
				return;
			}
			AudioManager.Post(this.sfx_EntryClicked);
			string sceneID = mapSelectionEntry.SceneID;
			LevelManager.loadLevelBeaconIndex = mapSelectionEntry.BeaconIndex;
			this.loading = true;
			this.LoadTask(sceneID, mapSelectionEntry.Cost).Forget();
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00078C70 File Offset: 0x00076E70
		private UniTask LoadTask(string sceneID, Cost cost)
		{
			MapSelectionView.<LoadTask>d__18 <LoadTask>d__;
			<LoadTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadTask>d__.<>4__this = this;
			<LoadTask>d__.sceneID = sceneID;
			<LoadTask>d__.cost = cost;
			<LoadTask>d__.<>1__state = -1;
			<LoadTask>d__.<>t__builder.Start<MapSelectionView.<LoadTask>d__18>(ref <LoadTask>d__);
			return <LoadTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00078CC4 File Offset: 0x00076EC4
		private UniTask<bool> WaitForConfirm()
		{
			MapSelectionView.<WaitForConfirm>d__21 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<MapSelectionView.<WaitForConfirm>d__21>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00078D08 File Offset: 0x00076F08
		private void SetupSceneInfo(SceneInfoEntry info)
		{
			if (info == null)
			{
				return;
			}
			string displayName = info.DisplayName;
			this.destinationDisplayNameText.text = displayName;
			this.destinationDisplayNameText.color = Color.white;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00078D3C File Offset: 0x00076F3C
		internal override void TryQuit()
		{
			if (!this.loading)
			{
				base.Close();
			}
		}

		// Token: 0x0400176C RID: 5996
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x0400176D RID: 5997
		[SerializeField]
		private FadeGroup confirmIndicatorFadeGroup;

		// Token: 0x0400176E RID: 5998
		[SerializeField]
		private TextMeshProUGUI destinationDisplayNameText;

		// Token: 0x0400176F RID: 5999
		[SerializeField]
		private CostDisplay confirmCostDisplay;

		// Token: 0x04001770 RID: 6000
		private string sfx_EntryClicked = "UI/confirm";

		// Token: 0x04001771 RID: 6001
		private string sfx_ShowDestination = "UI/destination_show";

		// Token: 0x04001772 RID: 6002
		private string sfx_ConfirmDestination = "UI/destination_confirm";

		// Token: 0x04001773 RID: 6003
		[SerializeField]
		private ColorPunch confirmColorPunch;

		// Token: 0x04001774 RID: 6004
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x04001775 RID: 6005
		[SerializeField]
		private Button btnCancel;

		// Token: 0x04001776 RID: 6006
		[SerializeField]
		private SceneReference overrideLoadingScreen;

		// Token: 0x04001777 RID: 6007
		private bool loading;

		// Token: 0x04001778 RID: 6008
		private bool confirmButtonClicked;

		// Token: 0x04001779 RID: 6009
		private bool cancelButtonClicked;
	}
}
