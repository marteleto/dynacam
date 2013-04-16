using UnityEngine;
using System.Collections;

public class DynamicCamera : MonoBehaviour
{
	public GameObject target;
	
	private float baseDistance = 15.0F;
	private float baseHeight = 7.5F;
	
	private Vector3 fieldOrientation = Vector3.forward;
	
	private float fieldOrientationOffset = 15.0F;
	
	private Vector3 targetMotionDirection = Vector3.zero;
	
	private float targetMotionOffset = 10.0F;
	
	private Vector3 lastTargetPosition = Vector3.zero;
	
	void Start ()
	{
		transform.position = getCameraPosition();
		transform.LookAt(getObjectivePoint());
	}

	void Update ()
	{
		transform.position = Vector3.Lerp(
			transform.position,
			getCameraPosition(),
			0.85F * Time.smoothDeltaTime);
		
		LerpLookAt(
			transform,
			target.transform,
			getObjectivePoint(),
			1.0F * Time.smoothDeltaTime);
	}
	
	Vector3 getCameraPosition ()
	{
		Vector3 cameraPosition = Vector3.zero;
		
		Vector3 targetPosition = target.transform.position;
		
		lastTargetPosition = transform.position;
		
		cameraPosition.x = targetPosition.x - (baseDistance * fieldOrientation.x);
		cameraPosition.z = targetPosition.z - (baseDistance * fieldOrientation.z);
		cameraPosition.y = targetPosition.y + baseHeight;
		
		return cameraPosition;
	}
	
	Vector3 getObjectivePoint ()
	{
		targetMotionDirection = target.transform.position - lastTargetPosition;
		return target.transform.position + (fieldOrientationOffset * fieldOrientation) + (targetMotionOffset * targetMotionDirection);
	}
	
	void LerpLookAt (Transform referenceTransform, Transform targetTransform, Vector3 objectivePosition, float damping)
	{
		Vector3 relativePos = objectivePosition - targetTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(referenceTransform.rotation, targetRotation, damping);
	}
}
