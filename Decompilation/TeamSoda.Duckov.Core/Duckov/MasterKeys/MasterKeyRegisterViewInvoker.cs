using System;
using Duckov.MasterKeys.UI;

namespace Duckov.MasterKeys
{
	// Token: 0x020002DD RID: 733
	public class MasterKeyRegisterViewInvoker : InteractableBase
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x00055F9E File Offset: 0x0005419E
		protected override void Awake()
		{
			base.Awake();
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00055FAD File Offset: 0x000541AD
		protected override void OnInteractFinished()
		{
			MasterKeysRegisterView.Show();
		}
	}
}
