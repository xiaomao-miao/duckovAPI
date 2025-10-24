using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.DialogueBubbles;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class DialogueBubbleProxy : MonoBehaviour
{
	// Token: 0x06000CBD RID: 3261 RVA: 0x0003559F File Offset: 0x0003379F
	public void Pop()
	{
		DialogueBubblesManager.Show(this.textKey.ToPlainText(), base.transform, this.yOffset, false, false, -1f, this.duration).Forget();
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x000355CF File Offset: 0x000337CF
	public void Pop(string text, float speed = -1f)
	{
		DialogueBubblesManager.Show(text, base.transform, this.yOffset, false, false, speed, 2f).Forget();
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x000355F0 File Offset: 0x000337F0
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.position + Vector3.up * this.yOffset, Vector3.one * 0.2f);
	}

	// Token: 0x04000B07 RID: 2823
	[LocalizationKey("Dialogues")]
	public string textKey;

	// Token: 0x04000B08 RID: 2824
	public float yOffset;

	// Token: 0x04000B09 RID: 2825
	public float duration = 2f;
}
