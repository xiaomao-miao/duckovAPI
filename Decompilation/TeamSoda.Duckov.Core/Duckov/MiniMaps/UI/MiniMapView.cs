using System;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200027A RID: 634
	public class MiniMapView : View
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0004A689 File Offset: 0x00048889
		public static MiniMapView Instance
		{
			get
			{
				return View.GetViewInstance<MiniMapView>();
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0004A6A8 File Offset: 0x000488A8
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x0004A690 File Offset: 0x00048890
		private float Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				value = Mathf.Clamp01(value);
				this._zoom = value;
				this.OnSetZoom(value);
			}
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0004A6B0 File Offset: 0x000488B0
		private void OnSetZoom(float scale)
		{
			this.RefreshZoom();
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0004A6B8 File Offset: 0x000488B8
		private void RefreshZoom()
		{
			if (this.display == null)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			Transform transform = this.display.transform;
			Vector3 vector = rectTransform.localToWorldMatrix.MultiplyPoint(rectTransform.rect.center);
			Vector3 point = transform.worldToLocalMatrix.MultiplyPoint(vector);
			this.display.transform.localScale = Vector3.one * Mathf.Lerp(this.zoomMin, this.zoomMax, this.zoomCurve.Evaluate(this.Zoom));
			Vector3 b = transform.localToWorldMatrix.MultiplyPoint(point) - vector;
			this.display.transform.position -= b;
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004A7A0 File Offset: 0x000489A0
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.display.AutoSetup();
			MultiSceneCore instance = MultiSceneCore.Instance;
			SceneInfoEntry sceneInfoEntry = (instance != null) ? instance.SceneInfo : null;
			if (sceneInfoEntry != null)
			{
				this.mapNameText.text = sceneInfoEntry.DisplayName;
				this.mapInfoText.text = sceneInfoEntry.Description;
			}
			else
			{
				this.mapNameText.text = "";
				this.mapInfoText.text = "";
			}
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
			this.RefreshZoom();
			this.CeneterPlayer();
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004A83F File Offset: 0x00048A3F
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0004A852 File Offset: 0x00048A52
		protected override void Awake()
		{
			base.Awake();
			this.zoomSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnZoomSliderValueChanged));
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0004A876 File Offset: 0x00048A76
		private void FixedUpdate()
		{
			this.RefreshNoSignalIndicator();
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004A87E File Offset: 0x00048A7E
		private void RefreshNoSignalIndicator()
		{
			this.noSignalIndicator.SetActive(this.display.NoSignal());
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0004A896 File Offset: 0x00048A96
		private void OnZoomSliderValueChanged(float value)
		{
			this.Zoom = value;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004A89F File Offset: 0x00048A9F
		public static void Show()
		{
			if (MiniMapView.Instance == null)
			{
				return;
			}
			if (MiniMapSettings.Instance == null)
			{
				return;
			}
			MiniMapView.Instance.Open(null);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004A8C8 File Offset: 0x00048AC8
		public void CeneterPlayer()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			Vector3 minimapPos;
			if (!this.display.TryConvertWorldToMinimap(main.transform.position, SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex), out minimapPos))
			{
				return;
			}
			this.display.Center(minimapPos);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004A91E File Offset: 0x00048B1E
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = default(Vector3);
			return !(MiniMapView.Instance == null) && MiniMapView.Instance.display.TryConvertWorldToMinimap(worldPosition, sceneID, out result);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004A948 File Offset: 0x00048B48
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = default(Vector3);
			if (MiniMapView.Instance == null)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex);
			return MiniMapView.TryConvertWorldToMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004A986 File Offset: 0x00048B86
		internal void OnScroll(PointerEventData eventData)
		{
			this.Zoom += eventData.scrollDelta.y * this.scrollSensitivity;
			eventData.Use();
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004A9AD File Offset: 0x00048BAD
		internal static void RequestMarkPOI(Vector3 worldPos)
		{
			MapMarkerManager.Request(worldPos);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0004A9B5 File Offset: 0x00048BB5
		public void LoadData(PackedMapData mapData)
		{
			if (mapData == null)
			{
				return;
			}
			this.display.Setup(mapData);
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0004A9CD File Offset: 0x00048BCD
		public void LoadCurrent()
		{
			this.display.AutoSetup();
		}

		// Token: 0x04000EBA RID: 3770
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000EBB RID: 3771
		[SerializeField]
		private MiniMapDisplay display;

		// Token: 0x04000EBC RID: 3772
		[SerializeField]
		private TextMeshProUGUI mapNameText;

		// Token: 0x04000EBD RID: 3773
		[SerializeField]
		private TextMeshProUGUI mapInfoText;

		// Token: 0x04000EBE RID: 3774
		[SerializeField]
		private Slider zoomSlider;

		// Token: 0x04000EBF RID: 3775
		[SerializeField]
		private float zoomMin = 5f;

		// Token: 0x04000EC0 RID: 3776
		[SerializeField]
		private float zoomMax = 20f;

		// Token: 0x04000EC1 RID: 3777
		[SerializeField]
		[HideInInspector]
		private float _zoom = 5f;

		// Token: 0x04000EC2 RID: 3778
		[SerializeField]
		[Range(0f, 0.01f)]
		private float scrollSensitivity = 0.01f;

		// Token: 0x04000EC3 RID: 3779
		[SerializeField]
		private SimplePointOfInterest markPoiTemplate;

		// Token: 0x04000EC4 RID: 3780
		[SerializeField]
		private AnimationCurve zoomCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000EC5 RID: 3781
		[SerializeField]
		private GameObject noSignalIndicator;
	}
}
