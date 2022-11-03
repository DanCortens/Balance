using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public GameObject gseObj;
    private GameStateEngine gse;
    public GameObject camObj;
    private CameraCont cam;
    private bool playing;

    public class Cinematic
    {
        public Cinematic (Vector2[] p, float[] t)
        {
            path = p;
            timeAtPath = t;
        }
        public Vector2[] path;
        public float[] timeAtPath;
    }

    public Cinematic currCinematic;

    // Start is called before the first frame update
    void Start()
    {
        gse = gseObj.GetComponent<GameStateEngine>();
        cam = camObj.GetComponent<CameraCont>();
        playing = false;
        currCinematic = new Cinematic(new Vector2[] {
                                        new Vector2(0f,5f),
                                        new Vector2(5f,0f),
                                        new Vector2(2f,2f)}, 
                                      new float[] {2f,2f,2f});
    }

    // Update is called once per frame
    void Update()
    {
        if (gse.state == GameStateEngine.State.Cinematic)
        {
            if (currCinematic != null && playing == false)
                StartCoroutine(PlayCinematic());
        }
    }
    private IEnumerator PlayCinematic()
    {
        playing = true;
        for (int i = 0; i < currCinematic.path.Length; i++)
        {
            float t = 0f;
            cam.setTarget(currCinematic.path[i]);
            while (t < currCinematic.timeAtPath[i])
            {
                t += Time.deltaTime;
                yield return null;
            }
                
        }
        currCinematic = null;
        gse.state = GameStateEngine.State.Play;
    }
}
