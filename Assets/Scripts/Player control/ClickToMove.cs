// ClickToMove.cs
using System.Collections;
using UnityEngine;


[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour 
{
    [SerializeField] ParticleSystem onClickEffect;
//	[SerializeField] float delay = 0.5f;
	[SerializeField] int layerMask;
	[SerializeField] string layerMaskName;

	RaycastHit hitInfo = new RaycastHit();
	UnityEngine.AI.NavMeshAgent agent;

	public delegate void MouseClick();
	public static event MouseClick MouseClickEvent;

	void Start () 
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		layerMask = 9;
	}
	public void MovePlayer() 
	{
		if (Input.GetMouseButtonDown(0)) // || Input.GetMouseButton(0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, Mathf.Infinity, layerMask))
			{
//				print(hitInfo.transform.gameObject.name);

				agent.destination = hitInfo.point;

				if (MouseClickEvent != null)
					MouseClickEvent();

				OnClickEffect(hitInfo.transform.gameObject);
            }
		}
	}


	void OnClickEffect(GameObject clickTarget)
    {
			ParticleSystem clickEffect;
			clickEffect = Instantiate(onClickEffect, hitInfo.point, Quaternion.identity);
			Destroy(clickEffect.gameObject, 1f);
			//		yield return new WaitForSeconds(delay);
	}
}
