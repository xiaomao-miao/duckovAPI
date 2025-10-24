using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000389 RID: 905
	public class NoteIndexView_Inspector : MonoBehaviour
	{
		// Token: 0x06001F7D RID: 8061 RVA: 0x0006E25C File Offset: 0x0006C45C
		private void Awake()
		{
			this.placeHolder.Show();
			this.content.SkipHide();
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0006E274 File Offset: 0x0006C474
		internal void Setup(Note value)
		{
			if (value == null)
			{
				this.placeHolder.Show();
				this.content.Hide();
				return;
			}
			this.note = value;
			this.SetupContent(this.note);
			this.placeHolder.Hide();
			this.content.Show();
			NoteIndex.SetNoteRead(value.key);
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0006E2D0 File Offset: 0x0006C4D0
		private void SetupContent(Note value)
		{
			this.textTitle.text = value.Title;
			this.textContent.text = value.Content;
			this.image.sprite = value.image;
			this.image.gameObject.SetActive(value.image == null);
		}

		// Token: 0x04001579 RID: 5497
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x0400157A RID: 5498
		[SerializeField]
		private FadeGroup content;

		// Token: 0x0400157B RID: 5499
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x0400157C RID: 5500
		[SerializeField]
		private TextMeshProUGUI textContent;

		// Token: 0x0400157D RID: 5501
		[SerializeField]
		private Image image;

		// Token: 0x0400157E RID: 5502
		private Note note;
	}
}
