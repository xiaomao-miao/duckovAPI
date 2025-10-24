using System;
using TMPro;
using UnityEngine;

namespace FX
{
	// Token: 0x0200020F RID: 527
	public class PopTextEntity : MonoBehaviour
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0003D94D File Offset: 0x0003BB4D
		private RectTransform spriteRendererRectTransform
		{
			get
			{
				if (this._spriteRendererRectTransform_cache == null)
				{
					this._spriteRendererRectTransform_cache = this.spriteRenderer.GetComponent<RectTransform>();
				}
				return this._spriteRendererRectTransform_cache;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0003D974 File Offset: 0x0003BB74
		private TextMeshPro tmp
		{
			get
			{
				return this._tmp;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0003D97C File Offset: 0x0003BB7C
		public TextMeshPro Tmp
		{
			get
			{
				return this.tmp;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0003D984 File Offset: 0x0003BB84
		public Color EndColor
		{
			get
			{
				return this.endColor;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0003D98C File Offset: 0x0003BB8C
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x0003D994 File Offset: 0x0003BB94
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.endColor = this.color;
				this.endColor.a = 0f;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0003D9B9 File Offset: 0x0003BBB9
		public float timeSinceSpawn
		{
			get
			{
				return Time.time - this.spawnTime;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0003D9C7 File Offset: 0x0003BBC7
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x0003D9D4 File Offset: 0x0003BBD4
		private string text
		{
			get
			{
				return this.tmp.text;
			}
			set
			{
				this.tmp.text = value;
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0003D9E4 File Offset: 0x0003BBE4
		public void SetupContent(string text, Sprite sprite = null)
		{
			this.text = text;
			if (sprite == null)
			{
				this.spriteRenderer.gameObject.SetActive(false);
				return;
			}
			this.spriteRenderer.gameObject.SetActive(true);
			this.spriteRenderer.sprite = sprite;
			this.spriteRenderer.transform.localScale = Vector3.one * (0.5f / (sprite.rect.width / sprite.pixelsPerUnit));
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0003DA65 File Offset: 0x0003BC65
		internal void SetColor(Color newColor)
		{
			this.Tmp.color = newColor;
			this.spriteRenderer.color = newColor;
		}

		// Token: 0x04000C9F RID: 3231
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000CA0 RID: 3232
		private RectTransform _spriteRendererRectTransform_cache;

		// Token: 0x04000CA1 RID: 3233
		[SerializeField]
		private TextMeshPro _tmp;

		// Token: 0x04000CA2 RID: 3234
		public Vector3 velocity;

		// Token: 0x04000CA3 RID: 3235
		public float size;

		// Token: 0x04000CA4 RID: 3236
		private Color color;

		// Token: 0x04000CA5 RID: 3237
		private Color endColor;

		// Token: 0x04000CA6 RID: 3238
		public float spawnTime;
	}
}
