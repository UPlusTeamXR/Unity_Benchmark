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
    private Vector3 initPosition = new Vector3(5.0f, 10.0f, -10.0f);

    [SerializeField]
    private GameObject avatarPrefabs = null;

    [SerializeField]
    private GameObject boxPrefabs = null;

    private List<GameObject> createdObject = new List<GameObject>();

    private bool autoFlag = false;

    private CpuUsage cpuUsage = null;

    // Start is called before the first frame update
    void Start()
    {
        if (avatarPrefabs == null || boxPrefabs == null)
        {
            Debug.LogError("Avatar is not set");
        }

        cpuUsage = new CpuUsage();
        cpuUsage.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (fpsUI)
        {
            fpsUI.text = CheckFPS();
        }

        if (cpuUsageUI)
        {
            cpuUsage.UpdateProcessorCount();
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
        StartCoroutine(CreateAutoPer(boxPrefabs, seconds));
    }

    public void StartCreateAvatarAutoPer(float seconds)
    {
        autoFlag = true;
        StartCoroutine(CreateAutoPer(avatarPrefabs, seconds));
    }

    public void StopCreateAuto()
    {
        autoFlag = false;
        StopAllCoroutines();
        foreach (GameObject go in createdObject)
        {
            Destroy(go);
        }
    }

    private string CheckFPS ()
    {
        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;
        return string.Format("FPS : {0:N1} ({1:N1}ms)", fps, ms);
    }
}
