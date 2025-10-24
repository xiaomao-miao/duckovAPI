using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000279 RID: 633
	public class MiniMapDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x0004A364 File Offset: 0x00048564
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.SceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0004A388 File Offset: 0x00048588
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0004A390 File Offset: 0x00048590
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0004A3B7 File Offset: 0x000485B7
		// (set) Token: 0x06001412 RID: 5138 RVA: 0x0004A3BF File Offset: 0x000485BF
		public MiniMapDisplay Master { get; private set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0004A3C8 File Offset: 0x000485C8
		public bool Hide
		{
			get
			{
				return this.target != null && this.target.Hide;
			}
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004A3DF File Offset: 0x000485DF
		private void Awake()
		{
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004A3F2 File Offset: 0x000485F2
		private void OnDestroy()
		{
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004A405 File Offset: 0x00048605
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Mapping entries";
			Debug.Log("Mapping entries", this);
			this.RefreshGraphics();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004A422 File Offset: 0x00048622
		public bool NoSignal()
		{
			return this.target != null && this.target.NoSignal;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0004A43C File Offset: 0x0004863C
		internal void Setup(MiniMapDisplay master, IMiniMapEntry cur, bool showGraphics = true)
		{
			this.Master = master;
			this.target = cur;
			if (cur.Sprite != null)
			{
				this.image.sprite = cur.Sprite;
				this.rectTransform.sizeDelta = Vector2.one * (float)cur.Sprite.texture.width * cur.PixelSize;
				this.showGraphics = showGraphics;
			}
			else
			{
				this.showGraphics = false;
			}
			if (cur.Hide)
			{
				this.showGraphics = false;
			}
			this.rectTransform.anchoredPosition = cur.Offset;
			this.sceneID = cur.SceneID;
			this.isCombined = false;
			this.RefreshGraphics();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0004A4F0 File Offset: 0x000486F0
		internal void SetupCombined(MiniMapDisplay master, IMiniMapDataProvider dataProvider)
		{
			this.target = null;
			this.Master = master;
			if (dataProvider == null)
			{
				return;
			}
			if (dataProvider.CombinedSprite == null)
			{
				return;
			}
			this.image.sprite = dataProvider.CombinedSprite;
			this.rectTransform.sizeDelta = Vector2.one * (float)dataProvider.CombinedSprite.texture.width * dataProvider.PixelSize;
			this.rectTransform.anchoredPosition = dataProvider.CombinedCenter;
			this.sceneID = "";
			this.image.enabled = true;
			this.showGraphics = true;
			this.isCombined = true;
			this.RefreshGraphics();
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0004A5A4 File Offset: 0x000487A4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Right)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.sceneID))
			{
				return;
			}
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(base.transform as RectTransform, eventData.position, null, out vector);
			Vector3 worldPos;
			if (!this.Master.TryConvertToWorldPosition(eventData.position, out worldPos))
			{
				return;
			}
			MiniMapView.RequestMarkPOI(worldPos);
			eventData.Use();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0004A60C File Offset: 0x0004880C
		private void RefreshGraphics()
		{
			bool flag = this.ShouldShow();
			if (flag)
			{
				this.image.color = Color.white;
			}
			else
			{
				this.image.color = Color.clear;
			}
			this.image.enabled = flag;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0004A651 File Offset: 0x00048851
		public bool ShouldShow()
		{
			if (!this.showGraphics)
			{
				return false;
			}
			if (this.isCombined)
			{
				return this.showGraphics;
			}
			return MultiSceneCore.ActiveSubSceneID == this.SceneID;
		}

		// Token: 0x04000EB3 RID: 3763
		[SerializeField]
		private Image image;

		// Token: 0x04000EB4 RID: 3764
		private string sceneID;

		// Token: 0x04000EB5 RID: 3765
		private RectTransform _rectTransform;

		// Token: 0x04000EB7 RID: 3767
		private bool showGraphics;

		// Token: 0x04000EB8 RID: 3768
		private bool isCombined;

		// Token: 0x04000EB9 RID: 3769
		private IMiniMapEntry target;
	}
}
