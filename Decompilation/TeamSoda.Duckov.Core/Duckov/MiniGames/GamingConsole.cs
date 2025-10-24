using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using ItemStatsSystem.Items;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x0200027F RID: 639
	public class GamingConsole : InteractableBase
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0004B457 File Offset: 0x00049657
		public MiniGame SelectedGame
		{
			get
			{
				if (this.CatridgeGameID == null)
				{
					return null;
				}
				return this.possibleGames.Find((MiniGame e) => e != null && e.ID == this.CatridgeGameID);
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0004B47A File Offset: 0x0004967A
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0004B482 File Offset: 0x00049682
		public Slot MonitorSlot
		{
			get
			{
				return this.mainItem.Slots["Monitor"];
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x0004B499 File Offset: 0x00049699
		public Slot ConsoleSlot
		{
			get
			{
				return this.mainItem.Slots["Console"];
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0004B4B0 File Offset: 0x000496B0
		public bool controllerConnected
		{
			get
			{
				if (this.mainItem == null)
				{
					return false;
				}
				if (this.ConsoleSlot == null)
				{
					return false;
				}
				Item content = this.ConsoleSlot.Content;
				if (content == null)
				{
					return false;
				}
				Slot slot = content.Slots["FcController"];
				return slot != null && slot.Content != null;
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06001451 RID: 5201 RVA: 0x0004B510 File Offset: 0x00049710
		// (remove) Token: 0x06001452 RID: 5202 RVA: 0x0004B548 File Offset: 0x00049748
		public event Action<GamingConsole> onContentChanged;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06001453 RID: 5203 RVA: 0x0004B580 File Offset: 0x00049780
		// (remove) Token: 0x06001454 RID: 5204 RVA: 0x0004B5B8 File Offset: 0x000497B8
		public event Action<GamingConsole> OnAfterAnimateIn;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001455 RID: 5205 RVA: 0x0004B5F0 File Offset: 0x000497F0
		// (remove) Token: 0x06001456 RID: 5206 RVA: 0x0004B628 File Offset: 0x00049828
		public event Action<GamingConsole> OnBeforeAnimateOut;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06001457 RID: 5207 RVA: 0x0004B660 File Offset: 0x00049860
		// (remove) Token: 0x06001458 RID: 5208 RVA: 0x0004B694 File Offset: 0x00049894
		public static event Action<bool> OnGamingConsoleInteractChanged;

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0004B6C7 File Offset: 0x000498C7
		public Item Monitor
		{
			get
			{
				if (this.MonitorSlot == null)
				{
					return null;
				}
				return this.MonitorSlot.Content;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0004B6DE File Offset: 0x000498DE
		public Item Console
		{
			get
			{
				if (this.ConsoleSlot == null)
				{
					return null;
				}
				return this.ConsoleSlot.Content;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0004B6F8 File Offset: 0x000498F8
		public Item Cartridge
		{
			get
			{
				if (this.Console == null)
				{
					return null;
				}
				if (!this.Console.Slots)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				Slot slot = this.Console.Slots["Cartridge"];
				if (slot == null)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				return slot.Content;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0004B77E File Offset: 0x0004997E
		public string CatridgeGameID
		{
			get
			{
				if (this.Cartridge == null)
				{
					return null;
				}
				return this.Cartridge.Constants.GetString("GameID", null);
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004B7A8 File Offset: 0x000499A8
		private UniTask Load()
		{
			GamingConsole.<Load>d__50 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<GamingConsole.<Load>d__50>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004B7EC File Offset: 0x000499EC
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			GamingConsole.SaveData saveData = new GamingConsole.SaveData();
			if (this.Console != null)
			{
				saveData.consoleData = ItemTreeData.FromItem(this.Console);
			}
			if (this.Monitor != null)
			{
				saveData.monitorData = ItemTreeData.FromItem(this.Monitor);
			}
			SavesSystem.Save<GamingConsole.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004B85C File Offset: 0x00049A5C
		protected override void Awake()
		{
			base.Awake();
			UIInputManager.OnCancel += this.OnUICancel;
			SavesSystem.OnCollectSaveData += this.Save;
			this.inputHandler.enabled = false;
			this.mainItem.onItemTreeChanged += this.OnContentChanged;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004B8B4 File Offset: 0x00049AB4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			UIInputManager.OnCancel -= this.OnUICancel;
			SavesSystem.OnCollectSaveData -= this.Save;
			this.isBeingDestroyed = true;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004B901 File Offset: 0x00049B01
		private void OnDisable()
		{
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged == null)
			{
				return;
			}
			onGamingConsoleInteractChanged(false);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004B913 File Offset: 0x00049B13
		protected override void Start()
		{
			base.Start();
			this.Load().Forget();
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004B926 File Offset: 0x00049B26
		private void OnContentChanged(Item item)
		{
			Action<GamingConsole> action = this.onContentChanged;
			if (action != null)
			{
				action(this);
			}
			this.RefreshGame();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0004B940 File Offset: 0x00049B40
		private void OnUICancel(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (base.Interacting)
			{
				base.StopInteract();
				data.Use();
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0004B960 File Offset: 0x00049B60
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(this);
			}
			if (this.Console == null || this.Monitor == null || this.Cartridge == null)
			{
				NotificationText.Push(this.incompleteNotificationText.ToPlainText());
				base.StopInteract();
				return;
			}
			if (this.SelectedGame == null)
			{
				NotificationText.Push(this.noGameNotificationText.ToPlainText());
				base.StopInteract();
				return;
			}
			this.RefreshGame();
			this.inputHandler.enabled = this.controllerConnected;
			this.AnimateCameraIn().Forget();
			HUDManager.RegisterHideToken(this);
			CharacterMainControl.Main.SetPosition(this.teleportToPositionWhenBegin.position);
			GamingConsoleHUD.Show();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0004BA34 File Offset: 0x00049C34
		private UniTask AnimateCameraIn()
		{
			GamingConsole.<AnimateCameraIn>d__61 <AnimateCameraIn>d__;
			<AnimateCameraIn>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraIn>d__.<>4__this = this;
			<AnimateCameraIn>d__.<>1__state = -1;
			<AnimateCameraIn>d__.<>t__builder.Start<GamingConsole.<AnimateCameraIn>d__61>(ref <AnimateCameraIn>d__);
			return <AnimateCameraIn>d__.<>t__builder.Task;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0004BA78 File Offset: 0x00049C78
		private UniTask AnimateCameraOut()
		{
			GamingConsole.<AnimateCameraOut>d__62 <AnimateCameraOut>d__;
			<AnimateCameraOut>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraOut>d__.<>4__this = this;
			<AnimateCameraOut>d__.<>1__state = -1;
			<AnimateCameraOut>d__.<>t__builder.Start<GamingConsole.<AnimateCameraOut>d__62>(ref <AnimateCameraOut>d__);
			return <AnimateCameraOut>d__.<>t__builder.Task;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004BABB File Offset: 0x00049CBB
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			this.inputHandler.enabled = false;
			this.AnimateCameraOut().Forget();
			HUDManager.UnregisterHideToken(this);
			GamingConsoleHUD.Hide();
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004BAF8 File Offset: 0x00049CF8
		private void RefreshGame()
		{
			if (this.game == null)
			{
				this.CreateGame(this.SelectedGame);
				return;
			}
			if (this.SelectedGame == null || this.SelectedGame.ID != this.game.ID)
			{
				this.CreateGame(this.SelectedGame);
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004BB58 File Offset: 0x00049D58
		private void CreateGame(MiniGame prefab)
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.game != null)
			{
				UnityEngine.Object.Destroy(this.game.gameObject);
			}
			if (prefab == null)
			{
				return;
			}
			this.game = UnityEngine.Object.Instantiate<MiniGame>(prefab);
			this.game.transform.SetParent(base.transform, true);
			this.game.SetRenderTexture(this.rt);
			this.game.SetConsole(this);
			this.inputHandler.SetGame(this.game);
		}

		// Token: 0x04000EE5 RID: 3813
		[SerializeField]
		private List<MiniGame> possibleGames;

		// Token: 0x04000EE6 RID: 3814
		[SerializeField]
		private RenderTexture rt;

		// Token: 0x04000EE7 RID: 3815
		[SerializeField]
		private MiniGameInputHandler inputHandler;

		// Token: 0x04000EE8 RID: 3816
		[SerializeField]
		private CinemachineVirtualCamera virtualCamera;

		// Token: 0x04000EE9 RID: 3817
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000EEA RID: 3818
		[SerializeField]
		private Transform vcamEndPosition;

		// Token: 0x04000EEB RID: 3819
		[SerializeField]
		private Transform vcamLookTarget;

		// Token: 0x04000EEC RID: 3820
		[SerializeField]
		private AnimationCurve posCurve;

		// Token: 0x04000EED RID: 3821
		[SerializeField]
		private AnimationCurve rotCurve;

		// Token: 0x04000EEE RID: 3822
		[SerializeField]
		private AnimationCurve fovCurve;

		// Token: 0x04000EEF RID: 3823
		[SerializeField]
		private float activeFov = 45f;

		// Token: 0x04000EF0 RID: 3824
		[SerializeField]
		private Transform teleportToPositionWhenBegin;

		// Token: 0x04000EF1 RID: 3825
		[SerializeField]
		private Item mainItem;

		// Token: 0x04000EF2 RID: 3826
		[SerializeField]
		[LocalizationKey("Default")]
		private string incompleteNotificationText = "GamingConsole_Incomplete";

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		[LocalizationKey("Default")]
		private string noGameNotificationText = "GamingConsole_NoGame";

		// Token: 0x04000EF4 RID: 3828
		private MiniGame game;

		// Token: 0x04000EF9 RID: 3833
		private string SaveKey = "GamingConsoleData";

		// Token: 0x04000EFA RID: 3834
		private bool loading;

		// Token: 0x04000EFB RID: 3835
		private bool loaded;

		// Token: 0x04000EFC RID: 3836
		private bool isBeingDestroyed;

		// Token: 0x04000EFD RID: 3837
		private int animateToken;

		// Token: 0x02000551 RID: 1361
		[Serializable]
		private class SaveData
		{
			// Token: 0x04001EC5 RID: 7877
			public ItemTreeData monitorData;

			// Token: 0x04001EC6 RID: 7878
			public ItemTreeData consoleData;
		}
	}
}
