using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManagerScript : MonoBehaviour
{
    public class Marker
    {
        public Vector2 position;
        public Quaternion rotation;
        public Marker(Vector2 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public List<Marker> markers = new List<Marker>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMarkers();
    }

    public void UpdateMarkers()
    {
        markers.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        markers.Clear();
        markers.Add(new Marker(transform.position, transform.rotation));

    }
}
