using System;
using System.IO;
using UnityEngine;
using Reality.ObjReader;
using System.Linq;

public static class Geometry
{
    const int MAXVERTS = 65536;
    static string directory = "";

    public static void Initialize(string path)
    {
        directory = GetDirectoryName(path);

        int count = Facade.ImportObjects(path);
        Vector3[] verts = null;
        Vector3[] norms = null;
        Vector2[] uvs = null;

        ConvertVertices(ref verts);
        ConvertNormals(ref norms);
        ConvertUVs(ref uvs);

        CreateObjects(count, verts, norms, uvs);
    }

    static void CreateObjects(int count, Vector3[] verts, Vector3[] norms, Vector2[] uvs)
    {
        for (int obj = 0; obj < count; ++obj)
        {
            var vertIndex = Facade.GetVertexIndexOfObject(obj);
            var uvIndex = Facade.GetUVIndexOfObject(obj);
            var normIndex = Facade.GetNormalIndexOfObject(obj);

            var divs = Math.Ceiling((double)vertIndex.Length / MAXVERTS);

            var name = Facade.GetNameOfObject(obj);
            var parentObject = GameObject.Find(name);

            if (parentObject == null)
            {
                parentObject = new GameObject(name + " Root");
                var material = CreateMaterial(obj, "Standard");
                CreateTextureMaps(obj, ref material);
                parentObject.AddComponent<MeshRenderer>();
                parentObject.GetComponent<MeshRenderer>().material = material;
            }

            for (int sector = 0; sector < divs; ++sector)
            {
                var gameObject = new GameObject(name);

                gameObject.AddComponent<MeshRenderer>();
                gameObject.GetComponent<MeshRenderer>().material =
                    parentObject.GetComponent<MeshRenderer>().material;
                gameObject.AddComponent<MeshFilter>();

                var mesh = gameObject.GetComponent<MeshFilter>().mesh;

                mesh.vertices = GetVerts(verts, vertIndex, sector);
                mesh.uv = GetUVs(uvs, uvIndex, sector);
                mesh.normals = GetNorms(norms, normIndex, sector);
                mesh.triangles = Enumerable
                    .Range(0, mesh.vertices.Length).ToArray();

                if (mesh.normals.Length == 0) mesh.RecalculateNormals();

                gameObject.transform.parent = parentObject.transform;
            }
        }
    }

    static Vector3[] GetNorms(Vector3[] norms, int[] index, int iter)
    {
        var sector = index.Length - MAXVERTS * iter;
        var end = (sector < MAXVERTS) ? sector + MAXVERTS * iter : MAXVERTS * (iter + 1);
        var start = MAXVERTS * iter;

        var normals = new Vector3[end];
        for (int i = start; i < end; ++i)
            normals[i] = norms[index[i]];
        return normals;
    }

    static Vector2[] GetUVs(Vector2[] uvs, int[] index, int iter)
    {
        var sector = index.Length - MAXVERTS * iter;
        var end = (sector < MAXVERTS) ? sector + MAXVERTS * iter : MAXVERTS * (iter + 1);
        var start = MAXVERTS * iter;

        var coords = new Vector2[end];
        for (int i = start; i < end; ++i)
            coords[i] = uvs[index[i]];
        return coords;
    }

    static Vector3[] GetVerts(Vector3[] verts, int[] index, int iter)
    {
        var sector = index.Length - MAXVERTS * iter;
        var end = (sector < MAXVERTS) ? sector + MAXVERTS * iter : MAXVERTS * (iter + 1);
        var start = MAXVERTS * iter;

        var vertices = new Vector3[end];
        for (int i = start; i < end; ++i)
            vertices[i] = verts[index[i]];
        return vertices;
    }

    // static Vector3[] AddNorms( Vector3[] meshNorms, Vector3[] objNorms, int[] index)
    // {
    //     var tempNorms = new Vector3[index.Length];

    //     for (int i = 0; i < index.Length; ++i)
    //         tempNorms[i] = objNorms[index[i]];

    //     var normals = new Vector3[meshNorms.Length + tempNorms.Length];
    //     meshNorms.CopyTo(normals, 0);
    //     tempNorms.CopyTo(normals, meshNorms.Length);

    //     return normals;
    // }

    // static Vector2[] AddUVs(Vector2[] meshUVs, Vector2[] objUVs, int[] index)
    // {
    //     var tempUVs = new Vector2[index.Length];

    //     for (int i = 0; i < index.Length; ++i)
    //         tempUVs[i] = objUVs[index[i]];

    //     var coords = new Vector2[meshUVs.Length + tempUVs.Length];
    //     meshUVs.CopyTo(coords, 0);
    //     tempUVs.CopyTo(coords, meshUVs.Length);

    //     return coords;
    // }

    // static Vector3[] AddVerts(Vector3[] meshVerts, Vector3[] objVerts, int[] index)
    // {
    //     var tempVerts = new Vector3[index.Length];

    //     for (int i = 0; i < index.Length; ++i)
    //         tempVerts[i] = objVerts[index[i]];

    //     var vertices = new Vector3[meshVerts.Length + tempVerts.Length];
    //     meshVerts.CopyTo(vertices, 0);
    //     tempVerts.CopyTo(vertices, meshVerts.Length);

    //     return vertices;
    // }

    static void ConvertVertices(ref Vector3[] verts)
    {
        var objVerts = Facade.GetVertices();
        if (objVerts == null) return;
        verts = new Vector3[objVerts.Length];

        for (int i = 0; i < objVerts.Length; ++i)
        {
            verts[i] = new Vector3(
                objVerts[i][0],
                objVerts[i][1],
                objVerts[i][2]
            );
        }
    }

    static void ConvertNormals(ref Vector3[] norms)
    {
        var objNorms = Facade.GetNormals();
        if (objNorms == null) return;
        norms = new Vector3[objNorms.Length];

        for (int i = 0; i < objNorms.Length; ++i)
        {
            norms[i] = new Vector3(
                objNorms[i][0],
                objNorms[i][1],
                objNorms[i][2]
            );
        }
    }

    static void ConvertUVs(ref Vector2[] uvs)
    {
        var objUVs = Facade.GetUVs();
        if (objUVs == null) return;
        uvs = new Vector2[objUVs.Length];

        for (int i = 0; i < objUVs.Length; ++i)
        {
            uvs[i] = new Vector2(
                objUVs[i][0],
                objUVs[i][1]
            );
        }
    }

    static void CreateTextureMaps(int index, ref Material material)
    {
        var mapKd = Facade.GetPathOfMapOfObject("Diffuse", index);
        if (mapKd != null && mapKd != "")
        {
            try
            {
                var texture = CreateTexture2D(mapKd);
                var texScale = Facade.GetScaleOfMapOfObject("Diffuse", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", scale);
            }
            catch (FileNotFoundException)
            {
#if UNITY_EDITOR
                throw;
#endif
            }
        }

        var mapBump = Facade.GetPathOfMapOfObject("Bump", index);
        if (mapBump != null && mapBump != "")
        {
            try
            {
                var texture = CreateTexture2D(mapBump);
                var texScale = Facade.GetScaleOfMapOfObject("Bump", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_BumpMap", texture);
                material.SetTextureScale("_BumpMap", scale);
            }
            catch (FileNotFoundException)
            {
#if UNITY_EDITOR
                throw;
#endif
            }
        }
    }

    static Material CreateMaterial(int index, string shaderType)
    {
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

    static Texture2D CreateTexture2D(string path)
    {
        var texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
        var bytes = File.ReadAllBytes(
            directory + Path.DirectorySeparatorChar + path);
        var result = texture.LoadImage(bytes);
        if (!result)
            throw new Exception("the texture map did not load");

        return texture;
    }

    static string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    static void ChangeBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
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