using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeTest : MonoBehaviour
{
    public int numRows;
    public int numCols;

    public GameObject cubePrefab;
    public ComputeShader computeShader;

    private GameObject[,] cubes;
    private float[] curHeights;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = new Vector3(0, 50.0f, -(numCols / 2.0f) - 8.0f);

        cubes = new GameObject[numRows, numCols];
        curHeights = new float[numRows * numCols];
        GameObject cubesParent = new GameObject("Cubes");
        for (var row = 0; row < numRows; row++)
        {
            for (var col = 0; col < numCols; col++)
            {
                GameObject cube = Instantiate(cubePrefab);
                cube.transform.position = new Vector3(col - (numCols / 2), Random.Range(-50.0f, 50.0f), row - (numRows / 2));
                cube.transform.parent = cubesParent.transform;
                cube.transform.name = string.Format("({0}, {1})", row, col);
                cube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                cubes[row, col] = cube;
                curHeights[row * numCols + col] = cube.transform.position.y;
            }
        }
    }

    public int numIterations = 10;

    public void RecalculateGPU()
    {
        float trueStart = Time.realtimeSinceStartup;
        for (int i = 0; i < numIterations; i++)
        {
            RecalculateGPUIteration();
        }

        float start = Time.realtimeSinceStartup;
        for (var row = 0; row < numRows; row++)
        {
            for (var col = 0; col < numCols; col++)
            {
                var cube = cubes[row, col].transform;
                cube.position = new Vector3(cube.position.x, curHeights[row * numCols + col], cube.position.z);
            }
        }
        Debug.Log("Compute shader position update takes " + (Time.realtimeSinceStartup - start) + " seconds.");

        Debug.Log("Compute shader method takes " + (Time.realtimeSinceStartup - trueStart) + " seconds.");
    }

    public void RecalculateGPUIteration()
    {
        float start = Time.realtimeSinceStartup;
        int floatSize = sizeof(float);
        int totalSize = floatSize;
        ComputeBuffer inHeightsBuffer = new ComputeBuffer(curHeights.Length, totalSize);
        inHeightsBuffer.SetData(curHeights);
        computeShader.SetBuffer(0, "inHeights", inHeightsBuffer);

        ComputeBuffer outHeightsBuffer = new ComputeBuffer(curHeights.Length, totalSize);
        computeShader.SetBuffer(0, "outHeights", outHeightsBuffer);
        computeShader.SetInt("numRows", numRows);
        computeShader.SetInt("numCols", numCols);
        Debug.Log("Compute shader buffer creation takes " + (Time.realtimeSinceStartup - start) + " seconds.");

        start = Time.realtimeSinceStartup;
        computeShader.Dispatch(0, curHeights.Length / 32, 1, 1);
        Debug.Log("Compute shader dispatch takes " + (Time.realtimeSinceStartup - start) + " seconds.");

        start = Time.realtimeSinceStartup;
        outHeightsBuffer.GetData(curHeights);
        Debug.Log("Compute shader get data takes " + (Time.realtimeSinceStartup - start) + " seconds.");

        inHeightsBuffer.Dispose();
        outHeightsBuffer.Dispose();
    }

    public void RecalculateCPU()
    {
        float start = Time.realtimeSinceStartup;
        float trueStart = start;

        float[,] heights = new float[numRows, numCols];
        float[,] newHeights = new float[numRows, numCols];
        for (var row = 0; row < numRows; row++)
        {
            for (var col = 0; col < numCols; col++)
            {
                heights[row, col] = cubes[row, col].transform.position.y;
            }
        }
        Debug.Log("CPU method buffer creation takes " + (Time.realtimeSinceStartup - start) + " seconds.");
        start = Time.realtimeSinceStartup;
        for (var i = 0; i < numIterations; i++)
        {
            for (var row = 0; row < numRows; row++)
            {
                for (var col = 0; col < numCols; col++)
                {
                    var count = 1;
                    var total = cubes[row, col].transform.position.y;

                    if (row > 0 && col > 0)
                    {
                        // top left
                        total += cubes[row - 1, col - 1].transform.position.y;
                        count++;
                    }
                    if (row > 0)
                    {
                        // top
                        total += cubes[row - 1, col].transform.position.y;
                        count++;
                    }
                    if (row > 0 && col < numCols - 1)
                    {
                        // top right
                        total += cubes[row - 1, col + 1].transform.position.y;
                        count++;
                    }
                    if (col > 0)
                    {
                        // left
                        total += cubes[row, col - 1].transform.position.y;
                        count++;
                    }
                    if (col < numCols - 1)
                    {
                        // right
                        total += cubes[row, col + 1].transform.position.y;
                        count++;
                    }
                    if (row < numRows - 1 && col > 0)
                    {
                        // bottom left
                        total += cubes[row + 1, col - 1].transform.position.y;
                        count++;
                    }
                    if (row < numRows - 1)
                    {
                        // bottom
                        total += cubes[row + 1, col].transform.position.y;
                        count++;
                    }
                    if (row < numRows - 1 && col < numCols - 1)
                    {
                        // bottom right
                        total += cubes[row + 1, col + 1].transform.position.y;
                        count++;
                    }

                    newHeights[row, col] = total / count;
                }
            }

            // swap buffers
            float[,] temp = heights;
            heights = newHeights;
            newHeights = temp;
        }
        Debug.Log("CPU method calculations take " + (Time.realtimeSinceStartup - start) + " seconds.");
        start = Time.realtimeSinceStartup;
        for (var row = 0; row < numRows; row++)
        {
            for (var col = 0; col < numCols; col++)
            {
                var cubeTransform = cubes[row, col].transform;
                cubeTransform.position = new Vector3(cubeTransform.position.x, newHeights[row, col], cubeTransform.position.z);
            }
        }
        Debug.Log("CPU position updates take " + (Time.realtimeSinceStartup - start) + " seconds.");

        Debug.Log("CPU method takes " + (Time.realtimeSinceStartup - trueStart) + " seconds.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
