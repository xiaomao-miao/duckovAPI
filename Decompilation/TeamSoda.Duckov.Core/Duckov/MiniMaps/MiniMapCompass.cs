using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000277 RID: 631
	public class MiniMapCompass : MonoBehaviour
	{
		// Token: 0x060013F3 RID: 5107 RVA: 0x00049A8C File Offset: 0x00047C8C
		private void SetupRotation()
		{
			Vector3 from = LevelManager.Instance.GameCamera.mainVCam.transform.up.ProjectOntoPlane(Vector3.up);
			Vector3 forward = Vector3.forward;
			float num = Vector3.SignedAngle(from, forward, Vector3.up);
			this.arrow.localRotation = Quaternion.Euler(0f, 0f, -num);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00049AEA File Offset: 0x00047CEA
		private void Update()
		{
			this.SetupRotation();
		}

		// Token: 0x04000EAA RID: 3754
		[SerializeField]
		private Transform arrow;
	}
}
