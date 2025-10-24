using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x0200021B RID: 539
	public class DialogueUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0003EB30 File Offset: 0x0003CD30
		private PrefabPool<DialogueUIChoice> ChoicePool
		{
			get
			{
				if (this._choicePool == null)
				{
					this._choicePool = new PrefabPool<DialogueUIChoice>(this.choiceTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._choicePool;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0003EB69 File Offset: 0x0003CD69
		public static bool Active
		{
			get
			{
				return !(DialogueUI.instance == null) && DialogueUI.instance.mainFadeGroup.IsShown;
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06001017 RID: 4119 RVA: 0x0003EB8C File Offset: 0x0003CD8C
		// (remove) Token: 0x06001018 RID: 4120 RVA: 0x0003EBC0 File Offset: 0x0003CDC0
		public static event Action OnDialogueStatusChanged;

		// Token: 0x06001019 RID: 4121 RVA: 0x0003EBF3 File Offset: 0x0003CDF3
		private void Awake()
		{
			DialogueUI.instance = this;
			this.choiceTemplate.gameObject.SetActive(false);
			this.RegisterEvents();
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003EC12 File Offset: 0x0003CE12
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003EC1A File Offset: 0x0003CE1A
		private void Update()
		{
			this.RefreshActorPositionIndicator();
			if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				this.Confirm();
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0003EC40 File Offset: 0x0003CE40
		private void OnEnable()
		{
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003EC42 File Offset: 0x0003CE42
		private void OnDisable()
		{
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0003EC44 File Offset: 0x0003CE44
		private void RegisterEvents()
		{
			DialogueTree.OnDialogueStarted += this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused += this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished += this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest += this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += this.OnMultipleChoiceRequest;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0003ECA8 File Offset: 0x0003CEA8
		private void UnregisterEvents()
		{
			DialogueTree.OnDialogueStarted -= this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused -= this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished -= this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest -= this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= this.OnMultipleChoiceRequest;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0003ED0A File Offset: 0x0003CF0A
		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			this.DoMultipleChoice(info).Forget();
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003ED18 File Offset: 0x0003CF18
		private void OnSubtitlesRequest(SubtitlesRequestInfo info)
		{
			this.DoSubtitle(info).Forget();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003ED26 File Offset: 0x0003CF26
		public static void HideTextFadeGroup()
		{
			DialogueUI.instance.MHideTextFadeGroup();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003ED32 File Offset: 0x0003CF32
		private void MHideTextFadeGroup()
		{
			this.textAreaFadeGroup.Hide();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003ED3F File Offset: 0x0003CF3F
		private void OnDialogueFinished(DialogueTree tree)
		{
			this.textAreaFadeGroup.Hide();
			InputManager.ActiveInput(base.gameObject);
			this.mainFadeGroup.Hide();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003ED71 File Offset: 0x0003CF71
		private void OnDialoguePaused(DialogueTree tree)
		{
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003ED82 File Offset: 0x0003CF82
		private void OnDialogueStarted(DialogueTree tree)
		{
			InputManager.DisableInput(base.gameObject);
			this.mainFadeGroup.Show();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged != null)
			{
				onDialogueStatusChanged();
			}
			this.actorNameFadeGroup.SkipHide();
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0003EDB8 File Offset: 0x0003CFB8
		private UniTask DoSubtitle(SubtitlesRequestInfo info)
		{
			DialogueUI.<DoSubtitle>d__37 <DoSubtitle>d__;
			<DoSubtitle>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoSubtitle>d__.<>4__this = this;
			<DoSubtitle>d__.info = info;
			<DoSubtitle>d__.<>1__state = -1;
			<DoSubtitle>d__.<>t__builder.Start<DialogueUI.<DoSubtitle>d__37>(ref <DoSubtitle>d__);
			return <DoSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0003EE04 File Offset: 0x0003D004
		private void SetupActorInfo(IDialogueActor actor)
		{
			DuckovDialogueActor duckovDialogueActor = actor as DuckovDialogueActor;
			if (duckovDialogueActor == null)
			{
				this.actorNameFadeGroup.Hide();
				this.actorPortraitContainer.gameObject.SetActive(false);
				this.actorPositionIndicator.gameObject.SetActive(false);
				this.talkingActor = null;
				return;
			}
			this.talkingActor = duckovDialogueActor;
			Sprite portraitSprite = duckovDialogueActor.portraitSprite;
			string nameKey = duckovDialogueActor.NameKey;
			Transform transform = duckovDialogueActor.transform;
			this.actorNameText.text = nameKey.ToPlainText();
			this.actorNameFadeGroup.Show();
			this.actorPortraitContainer.SetActive(portraitSprite);
			this.actorPortraitDisplay.sprite = portraitSprite;
			if (this.talkingActor.transform != null)
			{
				this.actorPositionIndicator.gameObject.SetActive(true);
			}
			this.RefreshActorPositionIndicator();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0003EED0 File Offset: 0x0003D0D0
		private void RefreshActorPositionIndicator()
		{
			if (this.talkingActor == null)
			{
				this.actorPositionIndicator.gameObject.SetActive(false);
				return;
			}
			this.actorPositionIndicator.MatchWorldPosition(this.talkingActor.transform.position + this.talkingActor.Offset, default(Vector3));
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0003EF34 File Offset: 0x0003D134
		private UniTask DoMultipleChoice(MultipleChoiceRequestInfo info)
		{
			DialogueUI.<DoMultipleChoice>d__40 <DoMultipleChoice>d__;
			<DoMultipleChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoMultipleChoice>d__.<>4__this = this;
			<DoMultipleChoice>d__.info = info;
			<DoMultipleChoice>d__.<>1__state = -1;
			<DoMultipleChoice>d__.<>t__builder.Start<DialogueUI.<DoMultipleChoice>d__40>(ref <DoMultipleChoice>d__);
			return <DoMultipleChoice>d__.<>t__builder.Task;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0003EF80 File Offset: 0x0003D180
		private UniTask DisplayOptions(Dictionary<IStatement, int> options)
		{
			DialogueUI.<DisplayOptions>d__41 <DisplayOptions>d__;
			<DisplayOptions>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayOptions>d__.<>4__this = this;
			<DisplayOptions>d__.options = options;
			<DisplayOptions>d__.<>1__state = -1;
			<DisplayOptions>d__.<>t__builder.Start<DialogueUI.<DisplayOptions>d__41>(ref <DisplayOptions>d__);
			return <DisplayOptions>d__.<>t__builder.Task;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0003EFCB File Offset: 0x0003D1CB
		internal void NotifyChoiceConfirmed(DialogueUIChoice choice)
		{
			this.confirmedChoice = choice.Index;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0003EFDC File Offset: 0x0003D1DC
		private UniTask<int> WaitForChoice()
		{
			DialogueUI.<WaitForChoice>d__45 <WaitForChoice>d__;
			<WaitForChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<WaitForChoice>d__.<>4__this = this;
			<WaitForChoice>d__.<>1__state = -1;
			<WaitForChoice>d__.<>t__builder.Start<DialogueUI.<WaitForChoice>d__45>(ref <WaitForChoice>d__);
			return <WaitForChoice>d__.<>t__builder.Task;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0003F01F File Offset: 0x0003D21F
		public void Confirm()
		{
			this.confirmed = true;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0003F028 File Offset: 0x0003D228
		private UniTask WaitForConfirm()
		{
			DialogueUI.<WaitForConfirm>d__48 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<DialogueUI.<WaitForConfirm>d__48>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0003F06B File Offset: 0x0003D26B
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x04000CDD RID: 3293
		private static DialogueUI instance;

		// Token: 0x04000CDE RID: 3294
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000CDF RID: 3295
		[SerializeField]
		private FadeGroup textAreaFadeGroup;

		// Token: 0x04000CE0 RID: 3296
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000CE1 RID: 3297
		[SerializeField]
		private GameObject continueIndicator;

		// Token: 0x04000CE2 RID: 3298
		[SerializeField]
		private float speed = 10f;

		// Token: 0x04000CE3 RID: 3299
		[SerializeField]
		private RectTransform actorPositionIndicator;

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		private FadeGroup actorNameFadeGroup;

		// Token: 0x04000CE5 RID: 3301
		[SerializeField]
		private TextMeshProUGUI actorNameText;

		// Token: 0x04000CE6 RID: 3302
		[SerializeField]
		private GameObject actorPortraitContainer;

		// Token: 0x04000CE7 RID: 3303
		[SerializeField]
		private Image actorPortraitDisplay;

		// Token: 0x04000CE8 RID: 3304
		[SerializeField]
		private FadeGroup choiceListFadeGroup;

		// Token: 0x04000CE9 RID: 3305
		[SerializeField]
		private Menu choiceMenu;

		// Token: 0x04000CEA RID: 3306
		[SerializeField]
		private DialogueUIChoice choiceTemplate;

		// Token: 0x04000CEB RID: 3307
		private PrefabPool<DialogueUIChoice> _choicePool;

		// Token: 0x04000CEC RID: 3308
		private DuckovDialogueActor talkingActor;

		// Token: 0x04000CEE RID: 3310
		private int confirmedChoice;

		// Token: 0x04000CEF RID: 3311
		private bool waitingForChoice;

		// Token: 0x04000CF0 RID: 3312
		private bool confirmed;
	}
}
