using System;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class SoulCube : MonoBehaviour
{
	// Token: 0x060005DB RID: 1499 RVA: 0x0001A100 File Offset: 0x00018300
	public void Init(SoulCollector collectorTarget)
	{
		this.target = collectorTarget;
		this.direction = UnityEngine.Random.insideUnitSphere + Vector3.up;
		this.direction.Normalize();
		this.spawnSpeed = UnityEngine.Random.Range(this.speedRange.x, this.speedRange.y);
		this.roatePart.transform.localRotation = Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 360f);
		this.rotateAxis = UnityEngine.Random.insideUnitSphere;
		this.rotateSpeed = UnityEngine.Random.Range(this.rotateSpeedRange.x, this.rotateSpeedRange.y);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001A1A8 File Offset: 0x000183A8
	private void Update()
	{
		this.roatePart.Rotate(this.rotateSpeed * this.rotateAxis * Time.deltaTime);
		if (this.target == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.stateTimer += Time.deltaTime;
		SoulCube.States states = this.currentState;
		if (states != SoulCube.States.spawn)
		{
			if (states != SoulCube.States.goToTarget)
			{
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.target.transform.position, this.toTargetSpeed * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, this.target.transform.position) < 0.3f)
			{
				this.AddCube();
			}
		}
		else
		{
			this.velocity = this.spawnSpeed * this.direction * this.spawnSpeedCurve.Evaluate(Mathf.Clamp01(this.stateTimer / this.spawnTime));
			base.transform.position += this.velocity * Time.deltaTime;
			if (this.stateTimer > this.spawnTime)
			{
				this.currentState = SoulCube.States.goToTarget;
				return;
			}
		}
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0001A2EF File Offset: 0x000184EF
	private void AddCube()
	{
		if (this.target)
		{
			this.target.AddCube();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000556 RID: 1366
	private SoulCube.States currentState;

	// Token: 0x04000557 RID: 1367
	private SoulCollector target;

	// Token: 0x04000558 RID: 1368
	private Vector3 direction;

	// Token: 0x04000559 RID: 1369
	private float stateTimer;

	// Token: 0x0400055A RID: 1370
	public Vector2 speedRange;

	// Token: 0x0400055B RID: 1371
	private float spawnSpeed;

	// Token: 0x0400055C RID: 1372
	public float spawnTime;

	// Token: 0x0400055D RID: 1373
	public float toTargetSpeed;

	// Token: 0x0400055E RID: 1374
	public AnimationCurve spawnSpeedCurve;

	// Token: 0x0400055F RID: 1375
	private Vector3 velocity;

	// Token: 0x04000560 RID: 1376
	public Transform roatePart;

	// Token: 0x04000561 RID: 1377
	public Vector2 rotateSpeedRange = new Vector2(300f, 1000f);

	// Token: 0x04000562 RID: 1378
	private float rotateSpeed;

	// Token: 0x04000563 RID: 1379
	private Vector3 rotateAxis;

	// Token: 0x0200045D RID: 1117
	private enum States
	{
		// Token: 0x04001B0A RID: 6922
		spawn,
		// Token: 0x04001B0B RID: 6923
		goToTarget
	}
}
