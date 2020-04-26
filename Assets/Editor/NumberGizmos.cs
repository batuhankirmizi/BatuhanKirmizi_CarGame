using UnityEngine;
using UnityEditor;

public class NumberGizmos : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position, GetNumber().ToString());
    }

    private int GetNumber()
    {
        if(name.StartsWith("Entrance Point"))
            for (int i = 0; i < GameObject.Find("Entrance Points").transform.childCount; i++)
                if (GameObject.Find("Entrance Points").transform.GetChild(i).name.Equals(name))
                    return i + 1;
        if(name.StartsWith("Target Point"))
            for (int i = 0; i < GameObject.Find("Target Points").transform.childCount; i++)
                if (GameObject.Find("Target Points").transform.GetChild(i).name.Equals(name))
                    return i + 1;
        return 0;
    }
}
