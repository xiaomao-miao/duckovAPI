using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class Accessory_Lazer : AccessoryBase
{
	// Token: 0x0600017A RID: 378 RVA: 0x00007318 File Offset: 0x00005518
	protected override void OnInit()
	{
		this.hitLayers = (GameplayDataSettings.Layers.damageReceiverLayerMask | GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask | GameplayDataSettings.Layers.fowBlockLayers | GameplayDataSettings.Layers.halfObsticleLayer);
		this.HideHitMarker();
		this.lineRenderer.enabled = false;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00007394 File Offset: 0x00005594
	private void Update()
	{
		if (this.character == null)
		{
			if (this.parentAgent == null || this.parentAgent.Holder == null)
			{
				return;
			}
			this.character = this.parentAgent.Holder;
		}
		bool flag = this.character.IsAiming();
		if (flag != this.lineRenderer.enabled)
		{
			this.lineRenderer.enabled = flag;
		}
		if (flag)
		{
			if (this.localPoints == null)
			{
				this.localPoints = new Vector3[2];
				this.lineRenderer.useWorldSpace = false;
				this.localPoints[0] = Vector3.zero;
			}
			Vector3 position = this.lineRenderer.transform.position;
			Vector3 currentAimPoint = this.character.GetCurrentAimPoint();
			Vector3 vector = Vector3.zero;
			if (Vector3.Distance(this.character.transform.position, currentAimPoint) > 2f && this.character.IsMainCharacter)
			{
				vector = this.lineRenderer.transform.position + (this.character.GetCurrentAimPoint() - this.lineRenderer.transform.position).normalized * this.character.GetAimRange();
			}
			else
			{
				vector = this.lineRenderer.transform.position + this.character.CurrentAimDirection.normalized * this.character.GetAimRange();
			}
			Vector3 vector2;
			if (this.CheckObsticle(position, vector, out vector2))
			{
				this.ShowHitMarker(vector2);
				vector = vector2;
			}
			else
			{
				this.HideHitMarker();
			}
			this.localPoints[1] = this.lineRenderer.transform.InverseTransformPoint(vector);
			this.lineRenderer.SetPositions(this.localPoints);
			return;
		}
		this.HideHitMarker();
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000756B File Offset: 0x0000576B
	private void ShowHitMarker(Vector3 point)
	{
		if (!this.hitMarker.activeSelf)
		{
			this.hitMarker.SetActive(true);
		}
		this.hitMarker.transform.position = point;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00007597 File Offset: 0x00005797
	private void HideHitMarker()
	{
		if (this.hitMarker.activeSelf)
		{
			this.hitMarker.SetActive(false);
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x000075B4 File Offset: 0x000057B4
	private bool CheckObsticle(Vector3 startPoint, Vector3 endPoint, out Vector3 hitPoint)
	{
		bool flag = this.character.HasNearByHalfObsticle();
		Vector3 vector = endPoint - startPoint;
		float magnitude = vector.magnitude;
		vector.Normalize();
		int num = Physics.SphereCastNonAlloc(startPoint, 0.15f, vector, this.raycastHits, magnitude, this.hitLayers, QueryTriggerInteraction.Ignore);
		if (num > 0)
		{
			Vector3 zero = Vector3.zero;
			float num2 = float.MaxValue;
			bool flag2 = false;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.raycastHits[i];
				DamageReceiver component = raycastHit.collider.GetComponent<DamageReceiver>();
				if (!flag || (!GameplayDataSettings.LayersData.IsLayerInLayerMask(raycastHit.collider.gameObject.layer, GameplayDataSettings.Layers.halfObsticleLayer) && (!component || !component.isHalfObsticle)))
				{
					if (component && component.health)
					{
						CharacterMainControl characterMainControl = component.health.TryGetCharacter();
						if (characterMainControl != null && characterMainControl.characterModel && characterMainControl.characterModel.invisable)
						{
							goto IL_157;
						}
					}
					if (!(raycastHit.collider.gameObject == this.character.mainDamageReceiver.gameObject) && raycastHit.distance != 0f)
					{
						flag2 = true;
						if (raycastHit.distance < num2)
						{
							Vector3 point = raycastHit.point;
							num2 = raycastHit.distance;
						}
					}
				}
				IL_157:;
			}
			if (flag2)
			{
				hitPoint = startPoint + vector * num2;
				return true;
			}
		}
		hitPoint = Vector3.zero;
		return false;
	}

	// Token: 0x0400010D RID: 269
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x0400010E RID: 270
	[SerializeField]
	private GameObject hitMarker;

	// Token: 0x0400010F RID: 271
	private CharacterMainControl character;

	// Token: 0x04000110 RID: 272
	private Vector3[] localPoints;

	// Token: 0x04000111 RID: 273
	private RaycastHit[] raycastHits = new RaycastHit[4];

	// Token: 0x04000112 RID: 274
	private LayerMask hitLayers;
}
