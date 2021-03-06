using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SplineAnimatorCamera : MonoBehaviour
{
	public Spline spline;
	
	public float speed = 1f;
	public float targetOffSet = 0f;
	public WrapMode wrapMode = WrapMode.Clamp;
	
	public float passedTime = 0f;
    private void Start()
    {
        transform.position = spline.GetPositionOnSpline(WrapValue(passedTime, 0f, 1f, wrapMode)) + Vector3.up;
        transform.rotation = Quaternion.LookRotation(spline.GetPositionOnSpline(WrapValue(passedTime + targetOffSet, 0f, 1f, wrapMode)) - transform.position);

    }
    // Update is called once per frame
    void Update( ) 
	{
		passedTime += Time.deltaTime * speed;
		
		transform.position = spline.GetPositionOnSpline( WrapValue( passedTime, 0f, 1f, wrapMode ) ) + Vector3.up;
        if (transform.GetComponent<AtlantisManager>().FixRotation == false)
        { 
            if (transform.rotation!=Quaternion.LookRotation(spline.GetPositionOnSpline(WrapValue(passedTime + targetOffSet, 0f, 1f, wrapMode)) - transform.position))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation
                    (spline.GetPositionOnSpline(WrapValue(passedTime + targetOffSet, 0f, 1f, wrapMode)) - transform.position), 0.8f);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(spline.GetPositionOnSpline(WrapValue(passedTime + targetOffSet, 0f, 1f, wrapMode)) - transform.position);
            }    
        }
        else
        {
            Quaternion rotation = transform.rotation;
            transform.GetComponent<AtlantisManager>().fixRotation(rotation);
        }
        //transform.rotation = Quaternion.LookRotation(spline.GetPositionOnSpline(WrapValue(passedTime + targetOffSet, 0f, 1f, wrapMode)) - transform.position);

    }

    private float WrapValue( float v, float start, float end, WrapMode wMode )
	{
		switch( wMode )
		{
		case WrapMode.Clamp:
		case WrapMode.ClampForever:
			return Mathf.Clamp( v, start, end );
		case WrapMode.Default:
		case WrapMode.Loop:
			return Mathf.Repeat( v, end - start ) + start;
		case WrapMode.PingPong:
			return Mathf.PingPong( v, end - start ) + start;
		default:
			return v;
		}
	}
}
