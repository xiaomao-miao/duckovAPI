using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class Action_Fishing : CharacterActionBase
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000556 RID: 1366 RVA: 0x00017D54 File Offset: 0x00015F54
	// (remove) Token: 0x06000557 RID: 1367 RVA: 0x00017D88 File Offset: 0x00015F88
	public static event Action<Action_Fishing, ICollection<Item>, Func<Item, bool>> OnPlayerStartSelectBait;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000558 RID: 1368 RVA: 0x00017DBC File Offset: 0x00015FBC
	// (remove) Token: 0x06000559 RID: 1369 RVA: 0x00017DF0 File Offset: 0x00015FF0
	public static event Action<Action_Fishing> OnPlayerStartFishing;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x0600055A RID: 1370 RVA: 0x00017E24 File Offset: 0x00016024
	// (remove) Token: 0x0600055B RID: 1371 RVA: 0x00017E58 File Offset: 0x00016058
	public static event Action<Action_Fishing, float, Func<float>> OnPlayerStartCatching;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x0600055C RID: 1372 RVA: 0x00017E8C File Offset: 0x0001608C
	// (remove) Token: 0x0600055D RID: 1373 RVA: 0x00017EC0 File Offset: 0x000160C0
	public static event Action<Action_Fishing, Item, Action<bool>> OnPlayerStopCatching;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x0600055E RID: 1374 RVA: 0x00017EF4 File Offset: 0x000160F4
	// (remove) Token: 0x0600055F RID: 1375 RVA: 0x00017F28 File Offset: 0x00016128
	public static event Action<Action_Fishing> OnPlayerStopFishing;

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x00017F5B File Offset: 0x0001615B
	public Action_Fishing.FishingStates FishingState
	{
		get
		{
			return this.fishingState;
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00017F63 File Offset: 0x00016163
	private void Awake()
	{
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00017F76 File Offset: 0x00016176
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00017F79 File Offset: 0x00016179
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Fishing;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00017F7C File Offset: 0x0001617C
	protected override bool OnStart()
	{
		if (!this.characterController)
		{
			return false;
		}
		this.fishingCamera.gameObject.SetActive(true);
		this.fishingRod = this.characterController.CurrentHoldItemAgent.GetComponent<FishingRod>();
		bool result = this.fishingRod != null;
		this.currentTask = this.Fishing();
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		InputManager.OnInteractButtonDown = (Action)Delegate.Combine(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		UIInputManager.OnCancel += this.UIOnCancle;
		return result;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00018039 File Offset: 0x00016239
	private void OnCatchButton()
	{
		if (this.fishingState != Action_Fishing.FishingStates.catching)
		{
			return;
		}
		this.catchInput = true;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0001804C File Offset: 0x0001624C
	private void UIOnCancle(UIInputEventData data)
	{
		data.Use();
		this.Quit();
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001805C File Offset: 0x0001625C
	protected override void OnStop()
	{
		base.OnStop();
		this.fishingState = Action_Fishing.FishingStates.notStarted;
		Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
		if (onPlayerStopFishing != null)
		{
			onPlayerStopFishing(this);
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000180C9 File Offset: 0x000162C9
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000180CC File Offset: 0x000162CC
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000180CF File Offset: 0x000162CF
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x000180D2 File Offset: 0x000162D2
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x000180D5 File Offset: 0x000162D5
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000180D8 File Offset: 0x000162D8
	private int NewToken()
	{
		this.fishingTaskToken++;
		this.fishingTaskToken %= 1000;
		return this.fishingTaskToken;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00018100 File Offset: 0x00016300
	private UniTask Fishing()
	{
		Action_Fishing.<Fishing>d__48 <Fishing>d__;
		<Fishing>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Fishing>d__.<>4__this = this;
		<Fishing>d__.<>1__state = -1;
		<Fishing>d__.<>t__builder.Start<Action_Fishing.<Fishing>d__48>(ref <Fishing>d__);
		return <Fishing>d__.<>t__builder.Task;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00018144 File Offset: 0x00016344
	private UniTask SingleFishingLoop(Func<bool> IsTaskValid)
	{
		Action_Fishing.<SingleFishingLoop>d__49 <SingleFishingLoop>d__;
		<SingleFishingLoop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SingleFishingLoop>d__.<>4__this = this;
		<SingleFishingLoop>d__.IsTaskValid = IsTaskValid;
		<SingleFishingLoop>d__.<>1__state = -1;
		<SingleFishingLoop>d__.<>t__builder.Start<Action_Fishing.<SingleFishingLoop>d__49>(ref <SingleFishingLoop>d__);
		return <SingleFishingLoop>d__.<>t__builder.Task;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001818F File Offset: 0x0001638F
	private void ResultConfirm(bool _continueFishing)
	{
		this.resultConfirmed = true;
		this.continueFishing = _continueFishing;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x000181A0 File Offset: 0x000163A0
	private UniTask<bool> Catching(Func<bool> IsTaskValid)
	{
		Action_Fishing.<Catching>d__51 <Catching>d__;
		<Catching>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Catching>d__.<>4__this = this;
		<Catching>d__.IsTaskValid = IsTaskValid;
		<Catching>d__.<>1__state = -1;
		<Catching>d__.<>t__builder.Start<Action_Fishing.<Catching>d__51>(ref <Catching>d__);
		return <Catching>d__.<>t__builder.Task;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000181EC File Offset: 0x000163EC
	private UniTask<bool> WaitForSelectBait()
	{
		Action_Fishing.<WaitForSelectBait>d__52 <WaitForSelectBait>d__;
		<WaitForSelectBait>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<WaitForSelectBait>d__.<>4__this = this;
		<WaitForSelectBait>d__.<>1__state = -1;
		<WaitForSelectBait>d__.<>t__builder.Start<Action_Fishing.<WaitForSelectBait>d__52>(ref <WaitForSelectBait>d__);
		return <WaitForSelectBait>d__.<>t__builder.Task;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00018230 File Offset: 0x00016430
	public List<Item> GetAllBaits()
	{
		List<Item> list = new List<Item>();
		if (!this.characterController)
		{
			return list;
		}
		foreach (Item item in this.characterController.CharacterItem.Inventory)
		{
			if (item.Tags.Contains(GameplayDataSettings.Tags.Bait))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x000182B4 File Offset: 0x000164B4
	public void CatchButton()
	{
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000182B6 File Offset: 0x000164B6
	public void Quit()
	{
		Debug.Log("Quit");
		this.quit = true;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000182CC File Offset: 0x000164CC
	private bool SelectBaitAndStartFishing(Item _bait)
	{
		if (_bait == null)
		{
			Debug.Log("鱼饵选了个null, 退出");
			this.Quit();
			return false;
		}
		if (!_bait.Tags.Contains(GameplayDataSettings.Tags.Bait))
		{
			this.Quit();
			return false;
		}
		this.bait = _bait;
		return true;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001831C File Offset: 0x0001651C
	private void OnDestroy()
	{
		if (base.Running)
		{
			Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
			if (onPlayerStopFishing != null)
			{
				onPlayerStopFishing(this);
			}
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
	}

	// Token: 0x040004C9 RID: 1225
	[SerializeField]
	private CinemachineVirtualCamera fishingCamera;

	// Token: 0x040004CA RID: 1226
	private FishingRod fishingRod;

	// Token: 0x040004CB RID: 1227
	[SerializeField]
	private FishingPoint fishingPoint;

	// Token: 0x040004CC RID: 1228
	[SerializeField]
	private float introTime = 0.2f;

	// Token: 0x040004CD RID: 1229
	private float fishingWaitTime = 2f;

	// Token: 0x040004CE RID: 1230
	private float catchTime = 0.5f;

	// Token: 0x040004CF RID: 1231
	private Item bait;

	// Token: 0x040004D0 RID: 1232
	private Transform socket;

	// Token: 0x040004D1 RID: 1233
	[SerializeField]
	[ItemTypeID]
	private int testCatchItem;

	// Token: 0x040004D2 RID: 1234
	private Item catchedItem;

	// Token: 0x040004D3 RID: 1235
	private bool quit;

	// Token: 0x040004D4 RID: 1236
	private UniTask currentTask;

	// Token: 0x040004D5 RID: 1237
	private bool catchInput;

	// Token: 0x040004D6 RID: 1238
	private bool resultConfirmed;

	// Token: 0x040004D7 RID: 1239
	private bool continueFishing;

	// Token: 0x040004DD RID: 1245
	private Action_Fishing.FishingStates fishingState;

	// Token: 0x040004DE RID: 1246
	private int fishingTaskToken;

	// Token: 0x0200044C RID: 1100
	public enum FishingStates
	{
		// Token: 0x04001AB1 RID: 6833
		notStarted,
		// Token: 0x04001AB2 RID: 6834
		intro,
		// Token: 0x04001AB3 RID: 6835
		selectingBait,
		// Token: 0x04001AB4 RID: 6836
		fishing,
		// Token: 0x04001AB5 RID: 6837
		catching,
		// Token: 0x04001AB6 RID: 6838
		over
	}
}
