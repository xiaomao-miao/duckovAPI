using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003ED RID: 1005
	public class SaveSlotSelectionMenu : MonoBehaviour
	{
		// Token: 0x06002450 RID: 9296 RVA: 0x0007E4C9 File Offset: 0x0007C6C9
		private void OnEnable()
		{
			UIInputManager.OnCancel += this.OnCancel;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0007E4DC File Offset: 0x0007C6DC
		private void OnDisable()
		{
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0007E4EF File Offset: 0x0007C6EF
		private void OnCancel(UIInputEventData data)
		{
			data.Use();
			this.Finish();
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0007E500 File Offset: 0x0007C700
		internal UniTask Execute()
		{
			SaveSlotSelectionMenu.<Execute>d__6 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<SaveSlotSelectionMenu.<Execute>d__6>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0007E543 File Offset: 0x0007C743
		public void Finish()
		{
			this.finished = true;
		}

		// Token: 0x040018B2 RID: 6322
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018B3 RID: 6323
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x040018B4 RID: 6324
		internal bool finished;
	}
}
