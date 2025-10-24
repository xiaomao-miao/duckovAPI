using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class FollowCharacterHUD : MonoBehaviour
{
	// Token: 0x06000644 RID: 1604 RVA: 0x0001C2B5 File Offset: 0x0001A4B5
	private void Awake()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Combine(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0001C2D7 File Offset: 0x0001A4D7
	private void OnDestroy()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Remove(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0001C2FC File Offset: 0x0001A4FC
	private void UpdatePos(GameCamera gameCamera, CharacterMainControl target)
	{
		Camera renderCamera = gameCamera.renderCamera;
		Vector3 vector = target.transform.position + this.offset;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = renderCamera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
		if (target.gameObject.activeInHierarchy != base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(target.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x040005FC RID: 1532
	public float maxDistance = 2f;

	// Token: 0x040005FD RID: 1533
	public float smoothTime;

	// Token: 0x040005FE RID: 1534
	private Vector3 worldPos;

	// Token: 0x040005FF RID: 1535
	private Vector3 velocityTemp;

	// Token: 0x04000600 RID: 1536
	public Vector3 offset;
}
