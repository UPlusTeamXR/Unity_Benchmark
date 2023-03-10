using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ModeChange : MonoBehaviour
{
    public void ScenesCtrl()
    {
        Debug.Log("ScenesCtrl");
        switch (this.gameObject.name)
        {
            case "CityMode":
                SceneManager.LoadScene("SampleScene");
                Debug.Log("click CityMode");
                break;

            case "BoxMode":
                SceneManager.LoadScene("BoxScene");
                Debug.Log("click BoxMode");
                break;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ModeChange");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
