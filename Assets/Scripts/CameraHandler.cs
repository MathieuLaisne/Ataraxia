using UnityEngine;

namespace Exion.Handler
{
    public class CameraHandler : MonoBehaviour
    {
        private Camera cam;

        private float zoomSpeed = 6.5f;

        [SerializeField]
        private float zoomFactor = 1.0f;

        public int size;

        private float originalSize = 0f;

        // Start is called before the first frame update
        private void Start()
        {
            cam = GetComponent<Camera>();
            originalSize = cam.orthographicSize;
        }


        // Update is called once per frame
        private void Update()
        {
            zoomFactor -= Input.mouseScrollDelta.y * 0.1f;
            zoomFactor = Mathf.Clamp(zoomFactor, 0.1f, 1f);

            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < size * 0.5f)
            {
                transform.Translate(new Vector3(0.5f / zoomFactor * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -size * 0.5f)
            {
                transform.Translate(new Vector3(-0.5f / zoomFactor * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y < size * 0.5f)
            {
                transform.Translate(new Vector3(0, -0.5f / zoomFactor * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y > - size * 0.5f)
            {
                transform.Translate(new Vector3(0, 0.5f / zoomFactor * Time.deltaTime, 0));
            }

            float targetSize = originalSize * zoomFactor;
            if (targetSize != cam.orthographicSize)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,
        targetSize, Time.deltaTime * zoomSpeed);
            }
        }
    }
}