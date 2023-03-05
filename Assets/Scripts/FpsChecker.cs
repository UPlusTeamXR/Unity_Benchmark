using UnityEngine;

public class FpsChecker
{
    private float deltaTime = 0.0f;

    public FpsChecker()
    {

    }

    public int GetFps()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //Debug.Log(deltaTime);
        return (int)(1.0f / deltaTime);
    }
}
