using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Dreambound.Astar;
using Dreambound.Astar.Editor;
using Dreambound.Astar.Data;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GridFile gridFile = (GridFile)Resources.Load("[Navigation]/[Grid Files]/" + SceneManager.GetActiveScene() + "_NavGrid.dll");
        TerrainTypes test = Resources.Load<TerrainTypes>("TerrainTypes");


        Debug.Log(test == null);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
