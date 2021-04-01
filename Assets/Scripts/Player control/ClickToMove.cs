﻿// ClickToMove.cs
using UnityEngine;


[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour 
{
    [SerializeField] ParticleSystem onClickEffect;
    RaycastHit hitInfo = new RaycastHit();
	UnityEngine.AI.NavMeshAgent agent;

	public delegate void MouseClick();
	public static event MouseClick MouseClickEvent;

	void Start () 
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}
	public void MovePlayer() 
	{
		if (Input.GetMouseButtonDown(0) /*|| Input.GetMouseButton(0)*/) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
			{
				MoveAgent();

				if (MouseClickEvent != null)
					MouseClickEvent();

//				OnClickEffect();
            }
		}
	}

	void MoveAgent()
    {
		agent.destination = hitInfo.point;
	}

	void OnClickEffect()
    {
		ParticleSystem clickEffect;
		clickEffect = Instantiate(onClickEffect, hitInfo.point, Quaternion.identity);
		Destroy(clickEffect.gameObject, 1f);
	}
}
