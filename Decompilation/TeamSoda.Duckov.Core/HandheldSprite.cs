using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class HandheldSprite : MonoBehaviour
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x00030BCB File Offset: 0x0002EDCB
	private void Start()
	{
		if (this.agent.Item)
		{
			this.spriteRenderer.sprite = this.agent.Item.Icon;
		}
	}

	// Token: 0x040009CE RID: 2510
	public DuckovItemAgent agent;

	// Token: 0x040009CF RID: 2511
	public SpriteRenderer spriteRenderer;
}
