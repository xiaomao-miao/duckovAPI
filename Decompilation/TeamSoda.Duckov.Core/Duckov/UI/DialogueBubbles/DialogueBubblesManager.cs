using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x020003E6 RID: 998
	public class DialogueBubblesManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0007DBAF File Offset: 0x0007BDAF
		// (set) Token: 0x0600241C RID: 9244 RVA: 0x0007DBB6 File Offset: 0x0007BDB6
		public static DialogueBubblesManager Instance { get; private set; }

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x0600241D RID: 9245 RVA: 0x0007DBC0 File Offset: 0x0007BDC0
		// (remove) Token: 0x0600241E RID: 9246 RVA: 0x0007DBF4 File Offset: 0x0007BDF4
		public static event Action<PointerEventData> onPointerClick;

		// Token: 0x0600241F RID: 9247 RVA: 0x0007DC27 File Offset: 0x0007BE27
		private void Awake()
		{
			if (DialogueBubblesManager.Instance == null)
			{
				DialogueBubblesManager.Instance = this;
			}
			this.prefab.gameObject.SetActive(false);
			this.raycastReceiver.enabled = false;
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x0007DC5C File Offset: 0x0007BE5C
		public static UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubblesManager.<Show>d__11 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.text = text;
			<Show>d__.target = target;
			<Show>d__.yOffset = yOffset;
			<Show>d__.needInteraction = needInteraction;
			<Show>d__.skippable = skippable;
			<Show>d__.speed = speed;
			<Show>d__.duration = duration;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<DialogueBubblesManager.<Show>d__11>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x0007DCD2 File Offset: 0x0007BED2
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<PointerEventData> action = DialogueBubblesManager.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x04001885 RID: 6277
		[SerializeField]
		private DialogueBubble prefab;

		// Token: 0x04001886 RID: 6278
		[SerializeField]
		private Graphic raycastReceiver;

		// Token: 0x04001887 RID: 6279
		private List<DialogueBubble> bubbles = new List<DialogueBubble>();
	}
}
