using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D5 RID: 981
	public abstract class FadeElement : MonoBehaviour
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x0007C98E File Offset: 0x0007AB8E
		public UniTask ActiveTask
		{
			get
			{
				return this.activeTask;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x0007C996 File Offset: 0x0007AB96
		protected int ActiveTaskToken
		{
			get
			{
				return this.activeTaskToken;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x0007C99E File Offset: 0x0007AB9E
		protected bool ManageGameObjectActive
		{
			get
			{
				return this.manageGameObjectActive;
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0007C9A6 File Offset: 0x0007ABA6
		private void CacheNewTaskToken()
		{
			this.activeTaskToken = UnityEngine.Random.Range(1, int.MaxValue);
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x0007C9B9 File Offset: 0x0007ABB9
		// (set) Token: 0x060023B1 RID: 9137 RVA: 0x0007C9C1 File Offset: 0x0007ABC1
		public bool IsFading { get; private set; }

		// Token: 0x060023B2 RID: 9138 RVA: 0x0007C9CC File Offset: 0x0007ABCC
		public UniTask Show(float delay = 0f)
		{
			FadeElement.<Show>d__18 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.delay = delay;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<FadeElement.<Show>d__18>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0007CA18 File Offset: 0x0007AC18
		public UniTask Hide()
		{
			FadeElement.<Hide>d__19 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<FadeElement.<Hide>d__19>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0007CA5C File Offset: 0x0007AC5C
		private UniTask WrapShowTask(int token, float delay = 0f)
		{
			FadeElement.<WrapShowTask>d__20 <WrapShowTask>d__;
			<WrapShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapShowTask>d__.<>4__this = this;
			<WrapShowTask>d__.token = token;
			<WrapShowTask>d__.delay = delay;
			<WrapShowTask>d__.<>1__state = -1;
			<WrapShowTask>d__.<>t__builder.Start<FadeElement.<WrapShowTask>d__20>(ref <WrapShowTask>d__);
			return <WrapShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0007CAB0 File Offset: 0x0007ACB0
		private UniTask WrapHideTask(int token, float delay = 0f)
		{
			FadeElement.<WrapHideTask>d__21 <WrapHideTask>d__;
			<WrapHideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapHideTask>d__.<>4__this = this;
			<WrapHideTask>d__.token = token;
			<WrapHideTask>d__.delay = delay;
			<WrapHideTask>d__.<>1__state = -1;
			<WrapHideTask>d__.<>t__builder.Start<FadeElement.<WrapHideTask>d__21>(ref <WrapHideTask>d__);
			return <WrapHideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023B6 RID: 9142
		protected abstract UniTask ShowTask(int token);

		// Token: 0x060023B7 RID: 9143
		protected abstract UniTask HideTask(int token);

		// Token: 0x060023B8 RID: 9144
		protected abstract void OnSkipHide();

		// Token: 0x060023B9 RID: 9145
		protected abstract void OnSkipShow();

		// Token: 0x060023BA RID: 9146 RVA: 0x0007CB03 File Offset: 0x0007AD03
		public void SkipHide()
		{
			this.activeTaskToken = 0;
			this.OnSkipHide();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0007CB26 File Offset: 0x0007AD26
		internal void SkipShow()
		{
			this.activeTaskToken = 0;
			this.OnSkipShow();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x04001834 RID: 6196
		protected UniTask activeTask;

		// Token: 0x04001835 RID: 6197
		private int activeTaskToken;

		// Token: 0x04001836 RID: 6198
		[SerializeField]
		private bool manageGameObjectActive;

		// Token: 0x04001837 RID: 6199
		[SerializeField]
		private float delay;

		// Token: 0x04001838 RID: 6200
		[SerializeField]
		private string sfx_Show;

		// Token: 0x04001839 RID: 6201
		[SerializeField]
		private string sfx_Hide;

		// Token: 0x0400183B RID: 6203
		private bool isShown;
	}
}
