using System;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace CameraSystems
{
	// Token: 0x02000212 RID: 530
	public class CameraPropertiesControl : MonoBehaviour
	{
		// Token: 0x06000FD7 RID: 4055 RVA: 0x0003E150 File Offset: 0x0003C350
		private void Awake()
		{
			this.vCam = base.GetComponent<CinemachineVirtualCamera>();
			this.volumeSettings = base.GetComponent<CinemachineVolumeSettings>();
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0003E16C File Offset: 0x0003C36C
		private unsafe void Update()
		{
			float num = *Gamepad.current.dpad.x.value;
			if (*Gamepad.current.dpad.y.value != 0f)
			{
				float num2 = -(*Gamepad.current.dpad.y.value);
				if (*Gamepad.current.rightShoulder.value > 0f)
				{
					num2 *= 10f;
				}
				this.vCam.m_Lens.FieldOfView = Mathf.Clamp(this.vCam.m_Lens.FieldOfView + num2 * 5f * Time.deltaTime, 8f, 100f);
			}
		}

		// Token: 0x04000CB6 RID: 3254
		private CinemachineVirtualCamera vCam;

		// Token: 0x04000CB7 RID: 3255
		private CinemachineVolumeSettings volumeSettings;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		private VolumeProfile volumeProfile;
	}
}
