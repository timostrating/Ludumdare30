using UnityEngine;
using System.Collections;

public class EnemyTwee : MonoBehaviour {
	public Transform[] patrolPoints;
	private float moveSpeed = 25;
	private int currentPoint;
	
	// Use this for initialization
	void Start () {
		transform.position = patrolPoints[0].position;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position == patrolPoints[currentPoint].position)	{
			currentPoint++;
		}
		
		if (currentPoint >= patrolPoints.Length){
			currentPoint = 0;
		}
		
		transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
	}
}
