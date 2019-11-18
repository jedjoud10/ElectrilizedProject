using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Settings")]
    public bool isLowpoly;
    public GameObject water;
    public GameObject details;
    [Tooltip("The camera which has the LODTerrain script on it for lod optimization")]
    public LODTerrain CameraLOD;
    [Tooltip("The resolution of the terrain. Between 2 and 100. Dont go past 100 or it will bug out")]
    public int resolution = 2;
    [Tooltip("How many chunks in a world")]
    public int chunks = 2;
    public Vector3 chunksize;
    [Tooltip("Regenerate a new mesh with a random seed on start")]
    public bool generate_on_start;
    private Texture2D texture;
    public Mesh mesh;
    private Vector3[] vertices;
    private int[] tris;
    private Vector2[] uvs;
    private Vector3 dotproduct = Vector3.up * 10.0f;
    [Header("Biomes")]
    [Tooltip("The used biomes, used for changing map colors")]
    public Biome[] Biomes;
    public NoiseLayers[] NoiseTerrain;
    public float noisebiomescale;
    public float noisebiomeheight;
    public float noisebiomeoffset;
    [Tooltip("How much smoothing should be applied in between the biomes ?")]
    public float colorsmoothing;
    public Vector2 editoroffset;
    public float masterscale;
    [Header("Masks")]
    [Tooltip("Use Circle Mask ?")]
    public bool UseMask;
    [Tooltip("Show only debug mask ?")]
    public bool debugMask;
    [Tooltip("Multiplicator value of the mask")]
    public float maskmult;
    [Tooltip("Circle mask radius")]
    public float maskradius;
    [Tooltip("Fading circle curve")]
    public AnimationCurve fade;
    private Vector2 pos;
    private Vector2 noiseoffset;
    public GameObject[] chunksvar;

    [System.Serializable]
    public struct NoiseLayers
    {
        public float basescale;
        public float baseampl;
        public int octaves;
        public float lacuranity;//controlls scale
        public float persistance;//controlls amplitude
        public enum NoiseType
        {
            PerlinNoise, Voronoi
        }
        public NoiseType noise_type;
        public AnimationCurve curveremap;
        public bool usecurve;
        public Vector2 clamp;
        public Vector2 affect;//only affects the range from the last noise
        public bool useaffect;
    }
    [System.Serializable]
    public struct Biome
    {
        public string Name;
        [Tooltip("The threshold number for the noise map to select this biome")]
        public float Threshold;
        [Tooltip("The threshold number for the heat map to select this biome")]
        public float ThresholdTemp;
        [Tooltip("The threshold number for the humidity map to select this biome")]
        public float ThresholdHumi;
        [Header("Colors")]
        [Tooltip("Divison number of the color gradiant")]
        public float div;
        [Tooltip("Color gradiant that selects color on terrain slope")]
        public Gradient grad;
    }

    private void Start()
    {
        if (generate_on_start)
        {
            GenerateChunks();
        }
    }
    private void OnValidate()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }
        createmesh(editoroffset);
        maketexture(editoroffset);
        FlatShading();
        updatemesh();
    }
    private GameObject GenerateSingleChunk(Vector2Int id, GameObject parent)//Generates a single chunk and setup the chunk
    {
        mesh = new Mesh();
        int idint;
        GameObject chunk = new GameObject();
        chunk.AddComponent<TerrainChunk>();
        idint = createmesh(new Vector2(id.x * resolution, id.y * resolution));
        chunk.GetComponent<TerrainChunk>().SetupChunk(id, resolution, idint);
        maketexture(new Vector2(id.x * resolution, id.y * resolution));
        FlatShading();
        updatemesh();
        chunk.GetComponent<TerrainChunk>().SetChunk(mesh, texture, details, water);        
        chunk.transform.parent = parent.transform;
        return chunk;
    }
    public void GenerateChunks()//Generates all the chunk with the specified chunk number
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        noiseoffset = new Vector2(UnityEngine.Random.Range(-100000, 100000), UnityEngine.Random.Range(-100000, 100000));
        GameObject main = new GameObject();
        main.transform.position = Vector3.zero;
        main.name = "Terain";
        chunksvar = new GameObject[chunks * chunks];
        for (int x = 0, i = 0; x < chunks; x++)
        {
            for (int y = 0; y < chunks; y++)
            {
                chunksvar[i] = GenerateSingleChunk(new Vector2Int(x, y), main);
                i++;
            }
        }
        gameObject.SetActive(false);
        CameraLOD.gridsize = resolution;
        CameraLOD.chunks = chunksvar;
        //CameraLOD.offset = new Vector2(chunks, chunks)*10;
        main.transform.position = new Vector3((-resolution * chunks) / 2.0f, 0, (-resolution * chunks) / 2.0f);
        timer.Stop();
        UnityEngine.Debug.Log("Took " + timer.ElapsedMilliseconds / 1000.0f + " seconds");
        main.transform.localScale = chunksize;
    }
    private int createmesh(Vector2 terrainoffset)//Makes the mesh for the chunks/the terrain
    {
        float midvar = 0;
        int mid = Mathf.RoundToInt((resolution + 1) / 2.0f);
        pos = new Vector2(0 - (resolution / 2), resolution / 2);
        Vector2 position;
        float y = 0;
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        uvs = new Vector2[vertices.Length];
        for (int z = 0, i = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                position.x = x; position.y = z;
                y = GetNoiseValue(position + terrainoffset + noiseoffset);
                if (x ==  mid && z == mid)
                {
                    midvar = y;
                }
                if (UseMask && !debugMask)
                {
                    y *= island_mask(x + pos.x + terrainoffset.x, z - pos.y + terrainoffset.y);
                }
                if (debugMask)
                {
                    y = island_mask(x + pos.x + terrainoffset.x, z - pos.y + terrainoffset.y);
                }
                vertices[i] = new Vector3(x + terrainoffset.x, y, z + terrainoffset.y);
                uvs[i] = new Vector2(x / (float)resolution, z / (float)resolution);
                i++;
            }
        }
        int vert = 0;
        int tri = 0;
        tris = new int[resolution * resolution * 6];
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                tris[tri] = vert;
                tris[tri + 1] = vert + resolution + 1;
                tris[tri + 2] = vert + 1;
                tris[tri + 3] = vert + 1;
                tris[tri + 4] = vert + resolution + 1;
                tris[tri + 5] = vert + resolution + 2;
                vert++;
                tri += 6;
            }
            vert++;
        }
        return IDfromvalue(midvar);
    }
    private Color BiomeTextureBlend(Vector3 pos, float normal)//Blends between the biomes and gets the color from the slope terrain help i need to sleep its midnight i cant sleep help
    {
        Color currentoutput;
        Color nextoutput;
        float lerp;
        Color finaloutput = Color.black;
        Biome nextbiome;
        bool hadasingle = false;
        float select = pos.y;
        select += (Mathf.PerlinNoise(pos.x * noisebiomescale, pos.z * noisebiomescale) * noisebiomeheight) - noisebiomeoffset;
        for (int i = 0; i < Biomes.Length; i++)
        {
            currentoutput = Biomes[i].grad.Evaluate(normal / Biomes[i].div);
            if (i != Biomes.Length - 1)//Checks if we are not the last element and that we cant get a null biome
            {
                nextoutput = Biomes[i + 1].grad.Evaluate(normal / Biomes[i + 1].div);
                nextbiome = Biomes[i + 1];
            }
            else
            {
                nextoutput = currentoutput;
                nextbiome = Biomes[i];
            }
            if (select > Biomes[i].Threshold && !hadasingle)
            {
                lerp = Remap(select, Biomes[i].Threshold, nextbiome.Threshold, 0, 1);
                finaloutput = Color.Lerp(currentoutput, nextoutput, Mathf.Pow(lerp, colorsmoothing));
            }
        }
        return finaloutput;
    }
    private float GetNoiseValue(Vector2 pos)//Calculates the noise from each biome then blends it with the noise layer from the biome section
    {
        float output = 0;
        bool doesaffect = false;
        float affectvalue = 0;
        float finaloutput = 0;
        float amplitude;
        float scale;
        foreach (NoiseLayers noise in NoiseTerrain)
        {
            doesaffect = output < noise.affect.y && output > noise.affect.x;
            affectvalue = 1 - Mathf.Abs(Remap(output, noise.affect.x, noise.affect.y, 0, 1) - 0.5f);
            output = 0;
            for (int i = 0; i < noise.octaves; i++)
            {
                amplitude = Mathf.Pow(noise.persistance, i) * noise.baseampl;
                scale = Mathf.Pow(noise.lacuranity, i) * noise.basescale;
                if (noise.noise_type == NoiseLayers.NoiseType.PerlinNoise)
                {
                    output += Mathf.PerlinNoise(pos.x * scale, pos.y * scale) * amplitude;
                }
                else
                {
                    output += 1.0f - Mathf.Abs(Mathf.PerlinNoise(pos.x * scale, pos.y * scale) - 0.5f) * amplitude;
                }
            }
            if (noise.usecurve)
            {
                output = Mathf.Clamp(noise.curveremap.Evaluate(output), noise.clamp.x, noise.clamp.y);
            }
            else
            {
                output = Mathf.Clamp(output, noise.clamp.x, noise.clamp.y);
            }            
            if (noise.useaffect)
            {
                {

                }
                if (!doesaffect)
                {
                    output = 0;
                }
                else
                {
                    output *= affectvalue;
                }
            }
            finaloutput += output;
        }        
        return finaloutput;        
    }
    public static float Remap(float value, float from1, float to1, float from2, float to2)//https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    private void updatemesh()//Updates the mesh, puts new vertices and set materials
    {        
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", texture);
    }
    private int IDfromvalue(float middlevalue)
    {
        int id = 0;
        for (int i = 0; i < Biomes.Length; i++)
        {
            if (middlevalue > Biomes[i].Threshold)
            {
                id = i;
            }
        }
        return id;
    }
    private void maketexture(Vector2 offset)//Makes a texture and puts the terrain slope as color , you can change it with the gradiant
    {
        texture = new Texture2D(resolution, resolution);
        texture.filterMode = FilterMode.Point;
        updatemesh();
        Vector3[] pos = mesh.vertices ;
        Vector3[] normals = mesh.normals;
        float normal = 0;
        for (int z = 0, i = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                normal = Vector3.Dot(normals[i], dotproduct);
                texture.SetPixel(x, z, BiomeTextureBlend(pos[i], normal));
                //texture.SetPixel(x, z, Color.gray);
                i++;
            }
        }
        texture.Apply();
    }
    private float island_mask(float x, float z)//Makes an island based mask (circle mask)
    {
        Vector2 vec2 = new Vector2(x, z);
        return fade.Evaluate(vec2.magnitude + maskradius) * maskmult;
    }
    void FlatShading()//Code from https://www.youtube.com/watch?v=V1vL9yRA_eM He makes extremly good videos !
    {
        if (isLowpoly)
        {
            Vector3[] flatShadedVertices = new Vector3[tris.Length];
            Vector2[] flatShadedUvs = new Vector2[tris.Length];

            for (int i = 0; i < tris.Length; i++)
            {
                flatShadedVertices[i] = vertices[tris[i]];
                flatShadedUvs[i] = uvs[tris[i]];
                tris[i] = i;
            }

            vertices = flatShadedVertices;
            uvs = flatShadedUvs;
        }
    }
    //2019-10-05 : Finally finsihed programming the biome system at 12:14 AM, now imma go to sleep;
    //2019-10-06 : It is 12:32 and im still awake boiii
}


