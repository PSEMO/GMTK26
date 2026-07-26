using System.Collections.Generic;
using PSEMO.Camera;
using Unity.Mathematics;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] CameraSO CamData;

    [SerializeField] List<GameObject> backgroundsLeft;
    [SerializeField] List<GameObject> backgroundsMiddle;
    [SerializeField] List<GameObject> backgroundsRight;
    [SerializeField] private Vector3[] parallaxScales;
    
    private Vector3 previousCamPos;
    private float[] widths;

    private void Awake()
    {
        previousCamPos = CamData.positionOffset;
        InitializeWidths();
    }

    private void InitializeWidths()
    {
        int count = backgroundsMiddle.Count;
        widths = new float[count];

        for (int i = 0; i < count; i++)
        {
            widths[i] = Mathf.Abs(backgroundsRight[i].transform.position.x - backgroundsMiddle[i].transform.position.x);
        }
    }

    private void LateUpdate()
    {
        int count = backgroundsMiddle.Count;

        for (int i = 0; i < count; i++)
        {
            float parallaxX = (previousCamPos.x - transform.position.x) * parallaxScales[i].x;
            float parallaxY = (previousCamPos.y - transform.position.y) * parallaxScales[i].y;

            MoveBackground(backgroundsLeft[i], parallaxX, parallaxY);
            MoveBackground(backgroundsMiddle[i], parallaxX, parallaxY);
            MoveBackground(backgroundsRight[i], parallaxX, parallaxY);

            if (backgroundsRight[i].transform.position.x <= transform.position.x)
            {
                MoveBackground(backgroundsLeft[i], widths[i], 0);
                MoveBackground(backgroundsMiddle[i], widths[i], 0);
                MoveBackground(backgroundsRight[i], widths[i], 0);
            }
            else if (backgroundsLeft[i].transform.position.x >= transform.position.x)
            {
                MoveBackground(backgroundsLeft[i], -widths[i], 0);
                MoveBackground(backgroundsMiddle[i], -widths[i], 0);
                MoveBackground(backgroundsRight[i], -widths[i], 0);
            }
        }

        previousCamPos = transform.position;
    }

    private void MoveBackground(GameObject bg, float parallaxX, float parallaxY)
    {
        bg.transform.position = new(
            bg.transform.position.x + parallaxX,
            bg.transform.position.y + parallaxY,
            bg.transform.position.z
        );
    }
}