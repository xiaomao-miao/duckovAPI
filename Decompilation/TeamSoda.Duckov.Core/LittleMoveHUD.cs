using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class LittleMoveHUD : MonoBehaviour
{
	// Token: 0x06000648 RID: 1608 RVA: 0x0001C3DC File Offset: 0x0001A5DC
	private void LateUpdate()
	{
		if (!this.character)
		{
			if (LevelManager.Instance)
			{
				this.character = LevelManager.Instance.MainCharacter;
			}
			if (!this.character)
			{
				return;
			}
		}
		if (!this.camera)
		{
			this.camera = Camera.main;
			if (!this.camera)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + this.offset;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = this.camera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
	}

	// Token: 0x04000601 RID: 1537
	private Camera camera;

	// Token: 0x04000602 RID: 1538
	private CharacterMainControl character;

	// Token: 0x04000603 RID: 1539
	public float maxDistance = 2f;

	// Token: 0x04000604 RID: 1540
	public float smoothTime;

	// Token: 0x04000605 RID: 1541
	private Vector3 worldPos;

	// Token: 0x04000606 RID: 1542
	private Vector3 velocityTemp;

	// Token: 0x04000607 RID: 1543
	public Vector3 offset;
}
