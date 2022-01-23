using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L11PlayerCamera : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float lerpSpeed = 20;

    [SerializeField] private Vector2 cameraPositionXRange = new Vector2(0, 100);

    private bool dead = true;
    private Vector2 deadZonePlayerPosition;

    [SerializeField] private float deadZoneDistance = 1;

    private float currentLerpSpeed;

  
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    
    private void Update()
    {
       
        if (!dead)
        {
            currentLerpSpeed = Mathf.Lerp(currentLerpSpeed, lerpSpeed, 0.05f * Time.deltaTime);
            float targetX = transform.position.x;
            targetX = Mathf.Lerp(targetX, L11Player.Singleton.transform.position.x, currentLerpSpeed * Time.deltaTime);
            targetX = Mathf.Clamp(targetX, cameraPositionXRange.x, cameraPositionXRange.y);

            
           

            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
        else
        {
            float distance = Vector2.Distance(L11Player.Singleton.transform.position, deadZonePlayerPosition);
            if (distance >= deadZoneDistance)
            {
                dead = false;
            }
        }
    }

   
}
