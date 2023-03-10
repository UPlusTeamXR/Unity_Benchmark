using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BoxCreator : MonoBehaviour
{
    [SerializeField]
    private Text fpsUI = null;
    private FpsChecker fpsChecker = null;

    [SerializeField]
    private Text BoxCountUI = null;
    private List<GameObject> createdObject = new List<GameObject>();


    Coroutine coroutine_CreateBox;
    Coroutine coroutine_makeChecker;

    public GameObject boxPrefab;
    private int boxCounter = 0;
    private bool makeStop = false;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Benchmark Scene Start");
        coroutine_CreateBox = StartCoroutine(createBox());
        coroutine_makeChecker = StartCoroutine(StartMakeChecker());
        fpsChecker = new FpsChecker();

    }

    // Update is called once per frame
    void Update()
    {
        if (fpsUI)
        {
            fpsUI.text = "FPS : " + fpsChecker.GetFps().ToString();
        }

        if (BoxCountUI)
        {
            BoxCountUI.text = "Box Count : " + boxCounter.ToString();
        }
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");

        if (coroutine_CreateBox != null)
        {
            StopCoroutine(coroutine_CreateBox);
            Debug.Log("StopCoroutine(coroutine_CreateBox);");
        }

        if (coroutine_makeChecker != null)
        {
            StopCoroutine(coroutine_makeChecker);
            Debug.Log("StopCoroutine(coroutine_makeChecker);");
        }

    }

    IEnumerator createBox()
    {
        while (true)
        {
            if (makeStop == true)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            Instantiate(boxPrefab);
            boxCounter++;
            //Debug.Log(boxCounter);


            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator StartMakeChecker()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (fpsChecker.GetFps() < 2)
            {
                makeStop = true;
            }
            else
            {
                makeStop = false;
            }
            yield return new WaitForSeconds(1f);

        }
    }
}
