using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("← 键按下！");
        }
    }
}
