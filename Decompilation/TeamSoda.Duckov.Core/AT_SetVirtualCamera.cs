using System;
using Cinemachine;
using NodeCanvas.Framework;

// Token: 0x020001B3 RID: 435
public class AT_SetVirtualCamera : ActionTask
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00035AC6 File Offset: 0x00033CC6
	protected override string info
	{
		get
		{
			return "Set camera :" + ((this.target.value == null) ? "Empty" : this.target.value.name);
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00035AFC File Offset: 0x00033CFC
	protected override void OnExecute()
	{
		base.OnExecute();
		if (AT_SetVirtualCamera.cachedVCam != null)
		{
			AT_SetVirtualCamera.cachedVCam.gameObject.SetActive(false);
		}
		if (this.target.value != null)
		{
			this.target.value.gameObject.SetActive(true);
			AT_SetVirtualCamera.cachedVCam = this.target.value;
		}
		else
		{
			AT_SetVirtualCamera.cachedVCam = null;
		}
		base.EndAction();
	}

	// Token: 0x04000B1B RID: 2843
	private static CinemachineVirtualCamera cachedVCam;

	// Token: 0x04000B1C RID: 2844
	public BBParameter<CinemachineVirtualCamera> target;
}
