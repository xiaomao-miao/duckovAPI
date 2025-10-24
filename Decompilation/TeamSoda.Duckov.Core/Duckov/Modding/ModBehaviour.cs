using System;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026A RID: 618
	public abstract class ModBehaviour : MonoBehaviour
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x0004761E File Offset: 0x0004581E
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x00047626 File Offset: 0x00045826
		public ModManager master { get; private set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x0004762F File Offset: 0x0004582F
		// (set) Token: 0x0600132F RID: 4911 RVA: 0x00047637 File Offset: 0x00045837
		public ModInfo info { get; private set; }

		// Token: 0x06001330 RID: 4912 RVA: 0x00047640 File Offset: 0x00045840
		public void Setup(ModManager master, ModInfo info)
		{
			this.master = master;
			this.info = info;
			this.OnAfterSetup();
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00047656 File Offset: 0x00045856
		public void NotifyBeforeDeactivate()
		{
			this.OnBeforeDeactivate();
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0004765E File Offset: 0x0004585E
		protected virtual void OnAfterSetup()
		{
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00047660 File Offset: 0x00045860
		protected virtual void OnBeforeDeactivate()
		{
		}
	}
}
