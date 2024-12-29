using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentPage3 : MonoBehaviour
{
    private void Start()
    {
        List<ReadJSONAttribute> currentAttributes = ReadJSON.imgAtbArray.FindAll(attr => attr.button == "");
    }
}
