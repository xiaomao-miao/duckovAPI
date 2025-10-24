using System;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200027B RID: 635
	public class PointOfInterestEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0004AA3A File Offset: 0x00048C3A
		public MonoBehaviour Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0004AA44 File Offset: 0x00048C44
		internal void Setup(MiniMapDisplay master, MonoBehaviour target, MiniMapDisplayEntry minimapEntry)
		{
			this.rectTransform = (base.transform as RectTransform);
			this.master = master;
			this.target = target;
			this.minimapEntry = minimapEntry;
			this.pointOfInterest = null;
			this.icon.sprite = this.defaultIcon;
			this.icon.color = this.defaultColor;
			this.areaDisplay.color = this.defaultColor;
			Color color = this.defaultColor;
			color.a *= 0.1f;
			this.areaFill.color = color;
			this.caption = target.name;
			this.icon.gameObject.SetActive(true);
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest == null)
			{
				return;
			}
			this.pointOfInterest = pointOfInterest;
			this.icon.gameObject.SetActive(!this.pointOfInterest.HideIcon);
			this.icon.sprite = ((pointOfInterest.Icon != null) ? pointOfInterest.Icon : this.defaultIcon);
			this.icon.color = pointOfInterest.Color;
			if (this.shadow)
			{
				this.shadow.Color = pointOfInterest.ShadowColor;
				this.shadow.OffsetDistance = pointOfInterest.ShadowDistance;
			}
			string value = this.pointOfInterest.DisplayName;
			this.caption = pointOfInterest.DisplayName;
			if (string.IsNullOrEmpty(value))
			{
				this.displayName.gameObject.SetActive(false);
			}
			else
			{
				this.displayName.gameObject.SetActive(true);
				this.displayName.text = this.pointOfInterest.DisplayName;
			}
			if (pointOfInterest.IsArea)
			{
				this.areaDisplay.gameObject.SetActive(true);
				this.rectTransform.sizeDelta = this.pointOfInterest.AreaRadius * Vector2.one * 2f;
				this.areaDisplay.color = pointOfInterest.Color;
				color = pointOfInterest.Color;
				color.a *= 0.1f;
				this.areaFill.color = color;
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
			}
			else
			{
				this.icon.enabled = true;
				this.areaDisplay.gameObject.SetActive(false);
			}
			this.RefreshPosition();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0004ACAC File Offset: 0x00048EAC
		private void RefreshPosition()
		{
			this.cachedWorldPosition = this.target.transform.position;
			Vector3 centerOfObjectScene = MiniMapCenter.GetCenterOfObjectScene(this.target);
			Vector3 vector = this.target.transform.position - centerOfObjectScene;
			Vector3 point = new Vector2(vector.x, vector.z);
			Vector3 position = this.minimapEntry.transform.localToWorldMatrix.MultiplyPoint(point);
			base.transform.position = position;
			this.UpdateScale();
			this.UpdateRotation();
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0004AD3C File Offset: 0x00048F3C
		private float ParentLocalScale
		{
			get
			{
				return base.transform.parent.localScale.x;
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004AD53 File Offset: 0x00048F53
		private void Update()
		{
			this.UpdateScale();
			this.UpdatePosition();
			this.UpdateRotation();
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004AD68 File Offset: 0x00048F68
		private void UpdateScale()
		{
			float d = (this.pointOfInterest != null) ? this.pointOfInterest.ScaleFactor : 1f;
			this.iconContainer.localScale = Vector3.one * d / this.ParentLocalScale;
			if (this.pointOfInterest != null && this.pointOfInterest.IsArea)
			{
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
				this.areaDisplay.FalloffDistance = 1f / this.ParentLocalScale;
			}
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004ADF5 File Offset: 0x00048FF5
		private void UpdatePosition()
		{
			if (this.cachedWorldPosition != this.target.transform.position)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004AE1A File Offset: 0x0004901A
		private void UpdateRotation()
		{
			base.transform.rotation = Quaternion.identity;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004AE2C File Offset: 0x0004902C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointOfInterest.NotifyClicked(eventData);
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				if (MiniMapCenter.GetSceneID(this.target) == null)
				{
					return;
				}
				CharacterMainControl.Main.SetPosition(this.target.transform.position);
			}
		}

		// Token: 0x04000EC6 RID: 3782
		private RectTransform rectTransform;

		// Token: 0x04000EC7 RID: 3783
		private MiniMapDisplay master;

		// Token: 0x04000EC8 RID: 3784
		private MonoBehaviour target;

		// Token: 0x04000EC9 RID: 3785
		private IPointOfInterest pointOfInterest;

		// Token: 0x04000ECA RID: 3786
		private MiniMapDisplayEntry minimapEntry;

		// Token: 0x04000ECB RID: 3787
		[SerializeField]
		private Transform iconContainer;

		// Token: 0x04000ECC RID: 3788
		[SerializeField]
		private Sprite defaultIcon;

		// Token: 0x04000ECD RID: 3789
		[SerializeField]
		private Color defaultColor = Color.white;

		// Token: 0x04000ECE RID: 3790
		[SerializeField]
		private Image icon;

		// Token: 0x04000ECF RID: 3791
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000ED0 RID: 3792
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04000ED1 RID: 3793
		[SerializeField]
		private ProceduralImage areaDisplay;

		// Token: 0x04000ED2 RID: 3794
		[SerializeField]
		private Image areaFill;

		// Token: 0x04000ED3 RID: 3795
		[SerializeField]
		private float areaLineThickness = 1f;

		// Token: 0x04000ED4 RID: 3796
		[SerializeField]
		private string caption;

		// Token: 0x04000ED5 RID: 3797
		private Vector3 cachedWorldPosition = Vector3.zero;
	}
}
