using UnityEngine;
using System.Collections;

public class DynamicCamera : MonoBehaviour
{
	public GameObject target;
	
	private float baseDistance = 15.0F;
	private float baseHeight = 7.5F;
	
	private Vector3 fieldOrientation = Vector3.forward;
	
	private float fieldOrientationOffset = 7.0F;
	
	private Vector3 targetMotionDirection = Vector3.zero;
	
	private float targetMotionOffset = 70.0F;
	
	private Vector3 lastTargetPosition = Vector3.zero;
	
	void Start ()
	{
		lastTargetPosition = target.transform.position;
		transform.position = getCameraPosition();
		transform.LookAt(getObjectivePoint());
	}

	void FixedUpdate ()
	{
		Vector3 camPos = getCameraPosition();
		Vector3 objectivePoint = getObjectivePoint();
		Vector3 rayComponent = (objectivePoint - transform.position).normalized * Vector3.Distance(camPos, objectivePoint);
		Debug.DrawRay(transform.position, rayComponent, Color.red);
		
		transform.position = Vector3.Lerp(
			transform.position,
			camPos,
			2.00F * Time.smoothDeltaTime);
		
		LerpLookAt(
			transform,
			target.transform,
			objectivePoint,
			0.60F * Time.smoothDeltaTime);
	}
	
	Vector3 getCameraPosition ()
	{
		Vector3 cameraPosition = Vector3.zero;
		Vector3 targetPosition = target.transform.position;
		cameraPosition.x = targetPosition.x - (baseDistance * fieldOrientation.x);
		cameraPosition.z = targetPosition.z - (baseDistance * fieldOrientation.z);
		cameraPosition.y = targetPosition.y + baseHeight;
		return cameraPosition;
	}
	
	Vector3 getObjectivePoint ()
	{
		targetMotionDirection = target.transform.position - lastTargetPosition;
		lastTargetPosition = target.transform.position;
		Vector3 fieldObjective = (fieldOrientationOffset * fieldOrientation);
		Vector3 motionObjective = (targetMotionOffset * targetMotionDirection);
		return target.transform.position + fieldObjective + motionObjective;
	}
	
	void LerpLookAt (Transform referenceTransform, Transform targetTransform, Vector3 objectivePosition, float damping)
	{
		Vector3 worldAxis = Vector3.up;
		Vector3 relativePos = objectivePosition - referenceTransform.position;
		if (referenceTransform.InverseTransformPoint(targetTransform.position).z < 0) worldAxis *= -1;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos,worldAxis);
        transform.rotation = Quaternion.Lerp(referenceTransform.rotation, targetRotation, damping);
	}
}
