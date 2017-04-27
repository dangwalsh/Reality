#if !UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using Reality.ObjReader;
using System.Linq;
using System.Collections;

public class Geometry {
    const int MAXVERTS = 63000;//65536;
    static string directory = "";

    public static void Initialize(string path, GameObject model) {
        directory = GetDirectoryName(path);
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

        CreateObjects(count, verts, norms, uvs, size, model);
    }

    static void CreateObjects(int count, Vector3[] verts, Vector3[] norms, Vector2[] uvs, float size, GameObject model) {

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

                var material = CreateMaterial(obj, "Standard");

                CreateTextureMaps(obj, ref material);

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
                mesh.triangles = Enumerable
                    .Range(0, mesh.vertices.Length).ToArray();

                if (mesh.normals.Length == 0) mesh.RecalculateNormals();

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

    static void CreateTextureMaps(int index, ref Material material) {
        var mapKd = Facade.GetPathOfMapOfObject("Diffuse", index);
        if (mapKd != null && mapKd != "") {
            try {
                var texture = CreateTexture2D(mapKd);
                var texScale = Facade.GetScaleOfMapOfObject("Diffuse", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", scale);
            }
            catch (FileNotFoundException) {
#if UNITY_EDITOR
                throw;
#endif
            }
        }

        var mapBump = Facade.GetPathOfMapOfObject("Bump", index);
        if (mapBump != null && mapBump != "") {
            try {
                var texture = CreateTexture2D(mapBump);
                var texScale = Facade.GetScaleOfMapOfObject("Bump", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_BumpMap", texture);
                material.SetTextureScale("_BumpMap", scale);
            }
            catch (FileNotFoundException) {
#if UNITY_EDITOR
                throw;
#endif
            }
        }
    }

    static Material CreateMaterial(int index, string shaderType) {
        var shader = Shader.Find(shaderType);
        var material = new Material(shader);
        var color = Facade.GetColorOfChannelOfObject("Diffuse", index);
        var name = Facade.GetNameOfMaterialOfObject(index);
        material.name = name;
        material.color = new Color(color[0], color[1], color[2], color[3]);

        if (material.color.a < 1.0f)
            ChangeBlendMode(material, BlendMode.Transparent);

        return material;
    }

    static Texture2D CreateTexture2D(string path) {
        var texture = new Texture2D(4, 4, TextureFormat.DXT5, true);
        var bytes = File.ReadAllBytes(
            directory + Path.DirectorySeparatorChar + path);
        var result = texture.LoadImage(bytes);
        if (!result)
            throw new Exception("the texture map did not load");

        return texture;
    }

    static string GetDirectoryName(string path) {
        return Path.GetDirectoryName(path);
    }

    static void ChangeBlendMode(Material material, BlendMode blendMode) {
        switch (blendMode) {
            case BlendMode.Opaque:
                material.SetInt("_SrcBlend",
                   (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend",
                   (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetInt("_SrcBlend",
                   (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend",
                   (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                material.SetInt("_SrcBlend",
                   (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend",
                   (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                material.SetInt("_SrcBlend",
                   (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend",
                   (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
#endif