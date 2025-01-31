using System;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This is a procedural terrain generator that will create infinite sized terrains based
/// on Simplex Noise. (A slightly faster than perlin implementation of coherent noise).
/// It uses a target transform and generates terrain tiles around that transform, as
/// it moves more terrain tiles are added and removed depending on the settings.
/// @author Quick Fingers
/// </summary>
public class TerrainGenerator : MonoBehaviour
{

    // target to track;
    public Transform target;

    // the 10x10 vertex default plane in Unity
    public GameObject terrainPlane;

    // (buffer * 2 + 1)^2 will be total number of planes in the scene at any one time
    public int buffer;

    // noise detail scalar. Higher values make spikier noise, values lower than 0.1 give more rolling hills
    public float detailScale;

    // as the noise always returns a value from -1 to 1 create a scalar
    public float heightScale;

    // the planes scale (default unity plane is 10 units by 10 units, any more will make localScale of terrainPlane change)
    public float planeSize = 60f;

    // plane count is the amount of planes in a single row (buffer * 2 + 1)
    private int planeCount;

    // your current position
    private int tileX;
    private int tileZ;
    /*
        private int noiseTileX;
        private int noiseTileZ;
        */
    private float noiseDX = 0f, noiseDZ = 0f;

    // array of tiles currently on screen
    private Tile[,] terrainTiles;

    // Use this for initialization
    void Start()
    {
        planeCount = buffer * 2 + 1;
        tileX = Mathf.RoundToInt(target.position.x / planeSize);
        tileZ = Mathf.RoundToInt(target.position.z / planeSize);
        /*
        noiseTileX = tileX;
        noiseTileZ = tileZ;
        */

        Generate();
    }

    public void Generate(float detailScale, float heightScale)
    {
        this.detailScale = detailScale;
        this.heightScale = heightScale;
        Generate();
    }

    public void ReplaceTiles(float dx, float dz)
    {
        if (dx != 0f)
        {
            tileX = 0;
            noiseDX -= dx;

        }

        if (dz != 0f)
        {
            tileZ = 0;
            noiseDZ -= dz;
        }

        for (int x = 0; x < planeCount; x++)
        {
            for (int z = 0; z < planeCount; z++)
            {
                terrainTiles[x, z].tileX = tileX - buffer + x;
                terrainTiles[x, z].tileZ = tileZ - buffer + z;
            }
        }

        foreach (Tile t in terrainTiles)
        {
            Vector3 p = t.gameObject.transform.localPosition;
            p.x += dx;
            p.z += dz;

            t.gameObject.transform.localPosition = p;

        }

    }

    public void Generate()
    {
        if (terrainTiles != null)
        {
            foreach (Tile t in terrainTiles)
            {
                Destroy(t.gameObject);
            }
        }

        terrainTiles = new Tile[planeCount, planeCount];

        for (int x = 0; x < planeCount; x++)
        {
            for (int z = 0; z < planeCount; z++)
            {
                terrainTiles[x, z] = GenerateTile(tileX - buffer + x, tileZ - buffer + z);
            }
        }
        terrainPlane.SetActive(false);
    }

    // Given a world tile x and tile z this generates a Tile object.
    private Tile GenerateTile(int x, int z)
    {
        GameObject plane =
                    (GameObject)Instantiate(terrainPlane, new Vector3(x * planeSize, 0, z * planeSize), Quaternion.identity);
        plane.transform.localScale = new Vector3(planeSize * 0.1f, 1f, planeSize * 0.1f);
        plane.transform.parent = transform;
        plane.SetActive(true);
        // Get the planes vertices
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // alter vertex Y position depending on simplex noise)
        for (int v = 0; v < vertices.Length; v++)
        {
            // generate the height for current vertex
            Vector3 vertexPosition = plane.transform.position + vertices[v] * planeSize / 10f;
            float height = SimplexNoise.Noise((vertexPosition.x + noiseDX) * detailScale, (vertexPosition.z + noiseDZ) * detailScale);
            // scale it with the heightScale field
            vertices[v].y = height * heightScale;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Tile tile = new Tile();
        tile.gameObject = plane;
        tile.tileX = x;
        tile.tileZ = z;

        return tile;
    }


    // this function could possibly be optimized, but is a proof of concept.
    // by recording the changeX and changeZ (as -1 or 1) we can
    // remove the un needed cells and re generate the grid array with the
    // correct tiles.
    private void Cull(int changeX, int changeZ)
    {
        int i, j;
        Tile[] newTiles = new Tile[planeCount];
        Tile[,] newTerrainTiles = new Tile[planeCount, planeCount];

        // firstly remove the old tile gameObjects and null them from the array.
        // populate a temporary array with the newly made tiles.
        if (changeX != 0)
        {
            for (i = 0; i < planeCount; i++)
            {
                try
                {
                    Destroy(terrainTiles[buffer - buffer * changeX, i].gameObject);
                }
                catch (System.Exception)
                {

                }
                terrainTiles[buffer - buffer * changeX, i] = null;
                newTiles[i] = GenerateTile(tileX + buffer * changeX + changeX, tileZ - buffer + i);
            }
        }
        if (changeZ != 0)
        {
            for (i = 0; i < planeCount; i++)
            {
                try
                {
                    Destroy(terrainTiles[i, buffer - buffer * changeZ].gameObject);
                }
                catch (System.Exception)
                {

                }
                terrainTiles[i, buffer - buffer * changeZ] = null;
                newTiles[i] = GenerateTile(tileX - buffer + i, tileZ + buffer * changeZ + changeZ);
            }
        }

        // make a copy of the old terrainTiles to reference when creating the new tile map.
        Array.Copy(terrainTiles, newTerrainTiles, planeCount * planeCount);

        // go through the current tiles on screen (minus the ones we just deleted, and reapply there 
        // new position in the newTerrainTiles array.
        for (i = 0; i < planeCount; i++)
        {
            for (j = 0; j < planeCount; j++)
            {
                Tile t = terrainTiles[i, j];
                if (t != null) newTerrainTiles[-tileX - changeX + buffer + t.tileX, -tileZ - changeZ + buffer + t.tileZ] = t;
            }
        }

        // add the newly created tiles to this new array.
        for (i = 0; i < newTiles.Length; i++)
        {
            Tile t = newTiles[i];
            newTerrainTiles[-tileX - changeX + buffer + t.tileX, -tileZ - changeZ + buffer + t.tileZ] = t;
        }

        // set the current map to the new array.
        terrainTiles = newTerrainTiles;
    }



    // Update is called once per frame
    void Update()
    {
        int newTileX = Mathf.RoundToInt(target.position.x / planeSize);
        int newTileZ = Mathf.RoundToInt(target.position.z / planeSize);

        if (newTileX != tileX)
        {
            Cull(newTileX - tileX, 0);
            tileX = newTileX;
        }

        if (newTileZ != tileZ)
        {
            Cull(0, newTileZ - tileZ);
            tileZ = newTileZ;
        }

        if (newTileX != tileX)
        {
            Cull(newTileX - tileX, 0);
            tileX = newTileX;
        }

        if (newTileZ != tileZ)
        {
            Cull(0, newTileZ - tileZ);
            tileZ = newTileZ;
        }
    }


}

public class Tile
{
    public GameObject gameObject;
    public int tileX;
    public int tileZ;
}