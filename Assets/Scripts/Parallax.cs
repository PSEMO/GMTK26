using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] List<GameObject> backgrounds;
    [SerializeField] private Vector3[] parallaxScales;
    [SerializeField] private float smoothing = 1f;
    
    private Vector3 previousCamPos;

    private void Start()
    {
        previousCamPos = transform.position;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (backgrounds[i] == null) continue;

            float parallaxX = (previousCamPos.x - transform.position.x) * parallaxScales[i].x;
            float parallaxY = (previousCamPos.y - transform.position.y) * parallaxScales[i].y;

            Vector3 targetPos = new Vector3(
                backgrounds[i].transform.position.x + parallaxX,
                backgrounds[i].transform.position.y + parallaxY,
                backgrounds[i].transform.position.z
            );

            backgrounds[i].transform.position = Vector3.Lerp(
                backgrounds[i].transform.position,
                targetPos,
                smoothing * Time.deltaTime
            );
        }

        previousCamPos = transform.position;
    }
}