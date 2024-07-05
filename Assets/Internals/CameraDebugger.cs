using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MathDebbuger.Internals
{
    public struct VectorArrow
    {
        public VectorArrow(int subDivisions, float radius, float height)
        {
            this.subDivitions = subDivisions;
            if (this.subDivitions <= 3)
                this.subDivitions = 3;
            this.radius = radius;
            if (this.radius <= 0.01f)
                this.radius = 0.01f;
            this.height = height;
            if (this.height <= 0.001f)
                this.height = 0.001f;
        }

        public static VectorArrow DefaultValue { get { return new VectorArrow(50, 0.2f, 1.0f); } }

        public int subDivitions;
        public float radius;
        public float height;
    }

    namespace Internals
    {
        public class CameraInternals
        {
            public class VectorHandles : MonoBehaviour
            {
                public static VectorHandles instance;
                public struct AuxColor
                {

                    public AuxColor(Color c)
                    {
                        r = c.r;
                        g = c.g;
                        b = c.b;
                        a = c.a;
                        acum = 1.0f;
                    }

                    public void AddColor(Color c)
                    {
                        r += c.r;
                        g += c.g;
                        b += c.b;
                        a += c.a;
                        acum += 1.0f;
                    }

                    public Color GetColorAverage()
                    {
                        return new Color((r / acum), (g / acum), (b / acum), (a / acum));
                    }
                    public float r;
                    public float g;
                    public float b;
                    public float a;
                    private float acum;
                }
                private Dictionary<Vector3, GUIStyle> gameHandles = new Dictionary<Vector3, GUIStyle>();
                private Dictionary<Vector3, AuxColor> gameColors = new Dictionary<Vector3, AuxColor>();
                private Dictionary<Vector3, GUIStyle> sceneHandles = new Dictionary<Vector3, GUIStyle>();
                private Dictionary<Vector3, AuxColor> sceneColors = new Dictionary<Vector3, AuxColor>();

                private void Awake()
                {
                    instance = this;
                }
                private void ClearGameDictionary()
                {
                    gameHandles.Clear();
                    gameColors.Clear();
                }

                private void ClearSceneDictionary()
                {
                    sceneHandles.Clear();
                    sceneColors.Clear();
                }
                public void AddToGameDictionary(Vector3 position, GUIStyle drawStyle)
                {
                    if (gameHandles.ContainsKey(position))
                    {
                        gameColors[position].AddColor(drawStyle.normal.textColor);
                    }
                    else
                    {
                        gameHandles.Add(position, drawStyle);
                        gameColors.Add(position, new AuxColor(drawStyle.normal.textColor));
                    }
                }

                public void AddToSceneDictionary(Vector3 position, GUIStyle drawStyle)
                {
                    if (sceneHandles.ContainsKey(position))
                    {
                        sceneHandles[position].normal.textColor = Color.white;// *= drawStyle.normal.textColor;
                    }
                    else
                    {
                        sceneHandles.Add(position, drawStyle);
                    }
                }

                void OnPostRender()
                {
                    foreach (KeyValuePair<Vector3, GUIStyle> gameHandle in gameHandles)
                    {
                        string coordinatesText = "X = " + gameHandle.Key.x.ToString() + "\nY = " + gameHandle.Key.y.ToString() + "\nZ = " + gameHandle.Key.z.ToString();
                        GUIStyle style = gameHandle.Value;
                        style.normal.textColor = gameColors[gameHandle.Key].GetColorAverage();
                        Handles.Label(gameHandle.Key, coordinatesText, style);
                    }
                    ClearGameDictionary();
                }
                void OnDrawGizmos()
                {
                    foreach (KeyValuePair<Vector3, GUIStyle> sceneHandle in sceneHandles)
                    {
                        string coordinatesText = "X = " + sceneHandle.Key.x.ToString() + "\nY = " + sceneHandle.Key.y.ToString() + "\nZ = " + sceneHandle.Key.z.ToString();
                        Handles.Label(sceneHandle.Key, coordinatesText, sceneHandle.Value);
                    }
                    ClearSceneDictionary();
                }

            }

            public class CameraDebugger : MonoBehaviour
            {
                private Material vectorMaterial;
                private List<Vector3> vectorsPositions;
                private bool showInEditor;
                private Mesh coneMesh;
                private VectorArrow vectorArrow = VectorArrow.DefaultValue;
                private GUIStyle style = new GUIStyle();
                public void Init(List<Vector3> positions)
                {
                    vectorMaterial = new Material(Shader.Find("Unlit/Color"));
                    vectorsPositions = positions;
                    vectorMaterial.color = Color.white;
                    coneMesh = CreateCone(vectorArrow.subDivitions, vectorArrow.radius, vectorArrow.height);
                    style.normal.textColor = vectorMaterial.color;
                }

                public void Init(List<Vector3> positions, Color vectorColor)
                {
                    vectorMaterial = new Material(Shader.Find("Unlit/Color"));
                    vectorsPositions = positions;
                    vectorMaterial.color = vectorColor;
                    coneMesh = CreateCone(vectorArrow.subDivitions, vectorArrow.radius, vectorArrow.height);
                    style.normal.textColor = vectorMaterial.color;
                }

                public void SetVectorArrow(VectorArrow arrow)
                {
                    vectorArrow = arrow;
                    coneMesh = CreateCone(vectorArrow.subDivitions, vectorArrow.radius, vectorArrow.height);
                }

                public void UpdateVectors(List<Vector3> newPositions)
                {
                    vectorsPositions = newPositions;
                }
                public void UpdateColor(Color newColor)
                {
                    vectorMaterial.color = newColor;
                    style.normal.textColor = vectorMaterial.color;
                }
                public void EnableShowInEditor()
                {
                    showInEditor = true;
                }
                public void DisableShowInEditor()
                {
                    showInEditor = false;
                }
                public void Delete()
                {
                    Destroy(this);
                }
                public void SetFontSize(int size)
                {
                    style.fontSize = size;
                }

                void OnPostRender()
                {
                    DrawVector();
                   // for (int i = 0; i < vectorsPositions.Count; i++)
                   // {
                   //     VectorHandles.instance.AddToGameDictionary(vectorsPositions[i], style);
                   // }
                }
                void OnDrawGizmos()
                {
                    if (showInEditor)
                    {
                        DrawVector();
                     //   for (int i = 0; i < vectorsPositions.Count; i++)
                     //   {
                     //       VectorHandles.instance.AddToSceneDictionary(vectorsPositions[i], style);
                     //
                     //   }
                    }
                }

                private void Update()
                {
                    //for (int i = 0; i < vectorsPositions.Count; i++)
                    //{
                    //    VectorHandles.instance.AddToGameDictionary(vectorsPositions[i], style);
                    //    if (showInEditor)
                    //        VectorHandles.instance.AddToSceneDictionary(vectorsPositions[i], style);
                    //}
                }

                void DrawVector()
                {
                    for (int i = 0; i < vectorsPositions.Count - 1; i++)
                    {
                        GL.Begin(GL.LINES);
                        vectorMaterial.SetPass(0);
                        GL.Color(vectorMaterial.color);
                        GL.Vertex3(vectorsPositions[i].x, vectorsPositions[i].y, vectorsPositions[i].z);
                        GL.Vertex3(vectorsPositions[i + 1].x, vectorsPositions[i + 1].y, vectorsPositions[i + 1].z);
                        Quaternion arrowRotation = Quaternion.FromToRotation(Vector3.up, vectorsPositions[i + 1] - vectorsPositions[i]);
                        Graphics.DrawMeshNow(coneMesh, vectorsPositions[i + 1] - (arrowRotation * new Vector3(0.0f, vectorArrow.height, 0.0f)), arrowRotation);
                        GL.End();
                    }

                     for (int i = 0; i < vectorsPositions.Count; i++)
                     {
                         string coordinatesText = "X = " + vectorsPositions[i].x.ToString() + "\nY = " + vectorsPositions[i].y.ToString() + "\nZ = " + vectorsPositions[i].z.ToString();
                         Handles.Label(vectorsPositions[i], coordinatesText, style);
                     }
                }

                //from: https://gist.github.com/mattatz/aba0d06fa56ef65e45e2
                private Mesh CreateCone(int subdivisions, float radius, float height)
                {
                    Mesh mesh = new Mesh();

                    Vector3[] vertices = new Vector3[subdivisions + 2];
                    Vector2[] uv = new Vector2[vertices.Length];
                    int[] triangles = new int[(subdivisions * 2) * 3];

                    vertices[0] = Vector3.zero;
                    uv[0] = new Vector2(0.5f, 0f);
                    for (int i = 0, n = subdivisions - 1; i < subdivisions; i++)
                    {
                        float ratio = (float)i / n;
                        float r = ratio * (Mathf.PI * 2f);
                        float x = Mathf.Cos(r) * radius;
                        float z = Mathf.Sin(r) * radius;
                        vertices[i + 1] = new Vector3(x, 0f, z);

                        uv[i + 1] = new Vector2(ratio, 0f);
                    }
                    vertices[subdivisions + 1] = new Vector3(0f, height, 0f);
                    uv[subdivisions + 1] = new Vector2(0.5f, 1f);

                    // construct bottom

                    for (int i = 0, n = subdivisions - 1; i < n; i++)
                    {
                        int offset = i * 3;
                        triangles[offset] = 0;
                        triangles[offset + 1] = i + 1;
                        triangles[offset + 2] = i + 2;
                    }

                    // construct sides

                    int bottomOffset = subdivisions * 3;
                    for (int i = 0, n = subdivisions - 1; i < n; i++)
                    {
                        int offset = i * 3 + bottomOffset;
                        triangles[offset] = i + 1;
                        triangles[offset + 1] = subdivisions + 1;
                        triangles[offset + 2] = i + 2;
                    }

                    mesh.vertices = vertices;
                    mesh.uv = uv;
                    mesh.triangles = triangles;
                    mesh.RecalculateBounds();
                    mesh.RecalculateNormals();

                    return mesh;
                }

            }
        }
    }
}
