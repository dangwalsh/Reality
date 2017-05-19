using System;
using System.IO;
using System.Linq;
using UnityEngine;

#if !UNITY_EDITOR
using Reality.ObjReader;
#endif

public class InitMesh : MonoBehaviour {
#if !UNITY_EDITOR
    const int MAXVERTS = 63000;
    const int MAXATTEMPTS = 1;

    /// <summary>
    /// removes "File=" from a string.
    /// </summary>
    /// <returns>the path</returns>
    private string GetPath() {

        var args = UnityEngine.WSA.Application.arguments;
        var path = args.Replace("File=", "");
        return path;
    }

    private void Awake() {
        var path = GetPath();
        var directory = Path.GetDirectoryName(path);
        var zipTask = Facade.UnzipFileAsync(path);
        var objPath = zipTask.Result;
        int count = Facade.ImportObjects(objPath);
        float[] bounds = Facade.GetBounds();
        float size = bounds.Max();
        Vector3[] verts = null;
        Vector3[] norms = null;
        Vector2[] uvs = null;

        ConvertVertices(ref verts);
        ConvertNormals(ref norms);
        ConvertUVs(ref uvs);

        CreateObjects(count, verts, norms, uvs, size, this.gameObject, directory);
    }

    static void CreateObjects(int count, Vector3[] verts, Vector3[] norms, Vector2[] uvs, float size, GameObject model, string directory) {

        for (int obj = 0; obj < count; ++obj) {
            var vertIndex = Facade.GetVertexIndexOfObject(obj);
            var uvIndex = Facade.GetUVIndexOfObject(obj);
            var normIndex = Facade.GetNormalIndexOfObject(obj);
            var divs = Math.Ceiling((double)vertIndex.Length / MAXVERTS);
            var name = Facade.GetNameOfObject(obj);
            var parent = GameObject.Find(name);

            if (parent == null) {
                parent = new GameObject(name + " Parent");
                parent.transform.parent = model.transform;

                var material = InitMaterials.CreateMaterial(obj, "Standard");

                InitMaterials.CreateTextureMaps(obj, ref material, directory);

                parent.AddComponent<MeshRenderer>();
                parent.GetComponent<MeshRenderer>().material = material;
            }

            for (int sector = 0; sector < divs; ++sector) {
                var child = new GameObject(name);

                child.AddComponent<MeshRenderer>();
                child.GetComponent<MeshRenderer>().material =
                    parent.GetComponent<MeshRenderer>().material;
                child.AddComponent<MeshFilter>();

                var mesh = child.GetComponent<MeshFilter>().mesh;

                mesh.vertices = GetVerts(verts, vertIndex, sector);
                mesh.uv = GetUVs(uvs, uvIndex, sector);
                mesh.normals = GetNorms(norms, normIndex, sector);
                // TODO: remove LINQ expressions for efficiency
                mesh.triangles = Enumerable
                    .Range(0, mesh.vertices.Length).ToArray();
                if (mesh.normals.Length == 0) mesh.RecalculateNormals();

                var t = TangentBasis.GetMeshTangets(mesh, 1.0f);
                mesh.tangents = t;
                child.AddComponent<MeshCollider>();
                child.transform.parent = parent.transform;
            }
        }
    }

    static Vector3[] GetNorms(Vector3[] norms, int[] index, int iter) {
        var start = MAXVERTS * iter;
        var sector = index.Length - start;
        var end = (sector < MAXVERTS) ? sector + start : MAXVERTS + start;

        var normals = new Vector3[end];
        for (int i = start; i < end; ++i)
            normals[i] = norms[index[i]];
        return normals;
    }

    static Vector2[] GetUVs(Vector2[] uvs, int[] index, int iter) {
        var start = MAXVERTS * iter;
        var sector = index.Length - start;
        var end = (sector < MAXVERTS) ? sector + start : MAXVERTS + start;

        var coords = new Vector2[end];
        for (int i = start; i < end; ++i)
            coords[i] = uvs[index[i]];
        return coords;
    }

    static Vector3[] GetVerts(Vector3[] verts, int[] index, int iter) {
        var start = MAXVERTS * iter;
        var sector = index.Length - start;
        var end = (sector < MAXVERTS) ? sector + start : MAXVERTS + start;

        var vertices = new Vector3[end];
        for (int i = start; i < end; ++i)
            vertices[i] = verts[index[i]];
        return vertices;
    }

    static void ConvertVertices(ref Vector3[] verts) {
        var objVerts = Facade.GetVertices();
        if (objVerts == null) return;
        verts = new Vector3[objVerts.Length];

        for (int i = 0; i < objVerts.Length; ++i) {
            verts[i] = new Vector3(
                objVerts[i][0],
                objVerts[i][1],
                objVerts[i][2]
            );
        }
    }

    static void ConvertNormals(ref Vector3[] norms) {
        var objNorms = Facade.GetNormals();
        if (objNorms == null) return;
        norms = new Vector3[objNorms.Length];

        for (int i = 0; i < objNorms.Length; ++i) {
            norms[i] = new Vector3(
                objNorms[i][0],
                objNorms[i][1],
                objNorms[i][2]
            );
        }
    }

    static void ConvertUVs(ref Vector2[] uvs) {
        var objUVs = Facade.GetUVs();
        if (objUVs == null) return;
        uvs = new Vector2[objUVs.Length];

        for (int i = 0; i < objUVs.Length; ++i) {
            uvs[i] = new Vector2(
                objUVs[i][0],
                objUVs[i][1]
            );
        }
    } 
#endif
}
