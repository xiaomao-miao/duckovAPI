using System;
using Duckov.Aquariums;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class DummyFish : MonoBehaviour, IAquariumContent
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00032C1A File Offset: 0x00030E1A
	private Vector3 TargetPosition
	{
		get
		{
			return this.target.position;
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00032C27 File Offset: 0x00030E27
	private void Awake()
	{
		this.rigidbody.useGravity = false;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00032C35 File Offset: 0x00030E35
	public void Setup(Aquarium master)
	{
		this.master = master;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00032C40 File Offset: 0x00030E40
	private void FixedUpdate()
	{
		Vector3 up = Vector3.up;
		Vector3 forward = base.transform.forward;
		Vector3 right = base.transform.right;
		Vector3 vector = this.TargetPosition - this.rigidbody.position;
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = Vector3.Cross(up, normalized);
		float b = Vector3.Dot(normalized, forward);
		float num = Mathf.Max(0f, b);
		this.swim = ((vector.magnitude > this.deadZone) ? 1f : (vector.magnitude / this.deadZone)) * num;
		Vector3 a = -(Vector3.Dot(vector2, this.rigidbody.velocity) * vector2);
		this.rigidbody.velocity += forward * this.swimForce * this.swim * Time.deltaTime + a * 0.5f;
		this.rigidbody.angularVelocity = Vector3.zero;
		Vector3 vector3 = vector;
		vector3.y = 0f;
		float num2 = Mathf.Clamp01(vector3.magnitude / this.deadZone - 0.5f);
		Vector3 normalized2 = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
		this._debug_projectedForward = normalized2;
		Vector3 vector4 = Vector3.Lerp(normalized2, normalized, num2);
		this._debug_idealRotForward = vector4;
		float num3 = Vector3.SignedAngle(forward, vector4, right);
		float num4 = Vector3.SignedAngle(forward, vector4, Vector3.up);
		float num5 = this.rotateForce * num3;
		float num6 = this.rotateForce * num4;
		this.rotVelocityX += num5 * Time.fixedDeltaTime;
		this.rotVelocityY += num6 * Time.fixedDeltaTime * num2;
		this.rotVelocityX *= 1f - this.rotationDamping;
		this.rotVelocityY *= 1f - this.rotationDamping;
		Vector3 eulerAngles = this.rigidbody.rotation.eulerAngles;
		eulerAngles.y += this.rotVelocityY * Time.deltaTime;
		eulerAngles.x += this.rotVelocityX * Time.deltaTime;
		if (eulerAngles.x < -179f)
		{
			eulerAngles.x += 360f;
		}
		if (eulerAngles.x > 179f)
		{
			eulerAngles.x -= 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -45f, 45f);
		eulerAngles.z = 0f;
		Quaternion rot = Quaternion.Euler(eulerAngles);
		this.rigidbody.MoveRotation(rot);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00032EF8 File Offset: 0x000310F8
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_idealRotForward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_projectedForward);
	}

	// Token: 0x04000A6C RID: 2668
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000A6D RID: 2669
	[SerializeField]
	private float rotateForce = 10f;

	// Token: 0x04000A6E RID: 2670
	[SerializeField]
	private float swimForce = 10f;

	// Token: 0x04000A6F RID: 2671
	[SerializeField]
	private float deadZone = 2f;

	// Token: 0x04000A70 RID: 2672
	[SerializeField]
	private float rotationDamping = 0.1f;

	// Token: 0x04000A71 RID: 2673
	[Header("Control")]
	[SerializeField]
	private Transform target;

	// Token: 0x04000A72 RID: 2674
	[Range(0f, 1f)]
	[SerializeField]
	private float swim;

	// Token: 0x04000A73 RID: 2675
	private float rotVelocityX;

	// Token: 0x04000A74 RID: 2676
	private float rotVelocityY;

	// Token: 0x04000A75 RID: 2677
	private Aquarium master;

	// Token: 0x04000A76 RID: 2678
	private Vector3 _debug_idealRotForward;

	// Token: 0x04000A77 RID: 2679
	private Vector3 _debug_projectedForward;
}
