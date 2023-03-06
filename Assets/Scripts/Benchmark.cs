using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Benchmark : MonoBehaviour
{
    [SerializeField]
    private Text fpsUI = null;

    [SerializeField]
    private Text cpuUsageUI = null;

    [SerializeField]
    private Vector3 boxInitPosition = new Vector3(5.0f, 10.0f, -10.0f);

    [SerializeField]
    private Vector3 avatarInitPosition = new Vector3(-2.0f, 5.2f, -7.0f);

    [SerializeField]
    private GameObject avatarPrefabs = null;

    [SerializeField]
    private GameObject boxPrefabs = null;

    private Vector3 initPosition = Vector3.zero;

    private List<GameObject> createdObject = new List<GameObject>();

    private bool autoFlag = false;

    private FpsChecker fpsChecker = null;

    private CpuUsage cpuUsage = null;

    // Start is called before the first frame update
    void Start()
    {
        if (avatarPrefabs == null || boxPrefabs == null)
        {
            Debug.LogError("Avatar is not set");
        }

        fpsChecker = new FpsChecker();

        cpuUsage = new CpuUsage();
        cpuUsage.UpdateProcessorCount();
        cpuUsage.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (fpsUI)
        {
            fpsUI.text = "FPS : " + fpsChecker.GetFps().ToString();
        }

        if (cpuUsageUI)
        {
            cpuUsageUI.text = "CPU : " + cpuUsage.GetCpuUsage().ToString("F1") + "%";
        }
    }

    private void OnDestroy()
    {
        cpuUsage.Stop();
    }

    IEnumerator CreateAutoPer(GameObject prefabs, float seconds)
    {
        GameObject root = new GameObject("Root");
        createdObject.Add(root);
        while (autoFlag)
        {
            GameObject one = Instantiate(prefabs);
            one.transform.position = initPosition;
            one.transform.parent = root.transform;
            createdObject.Add(one);
            yield return new WaitForSeconds(seconds);
        }
    }

    public void StartCreateBoxAutoPer(float seconds)
    {
        autoFlag = true;
        initPosition = boxInitPosition;
        StartCoroutine(CreateAutoPer(boxPrefabs, seconds));
    }

    public void StartCreateAvatarAutoPer(float seconds)
    {
        autoFlag = true;
        initPosition = avatarInitPosition;
        StartCoroutine(CreateAutoPer(avatarPrefabs, seconds));
    }

    public void StopCreateAuto()
    {
        autoFlag = false;
        initPosition = Vector3.zero;
        StopAllCoroutines();
        foreach (GameObject go in createdObject)
        {
            Destroy(go);
        }
    }
}
