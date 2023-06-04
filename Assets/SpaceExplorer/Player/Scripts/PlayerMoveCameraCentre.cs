using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCameraCentre : MonoBehaviour
{

    public void MovePosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
