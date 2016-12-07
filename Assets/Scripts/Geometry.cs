using System;
using System.IO;
using UnityEngine;
using Reality.ObjReader;

public static class Geometry
{
    static string directory;

    public static void Initialize(string path)
    {
        directory = GetDirectoryName(path);

        var count = Facade.ImportObjects(path);

        Vector3[] verts = null;
        Vector3[] norms = null;
        Vector2[] uvs = null;

        GetVertices(ref verts);
        GetNormals(ref norms);
        GetUVs(ref uvs);

        CreateObjects(count, ref verts, ref norms, ref uvs);
    }

    static void CreateObjects(
        int count,
        ref Vector3[] verts,
        ref Vector3[] norms,
        ref Vector2[] uvs
    )
    {
        for (int i = 0; i < count; ++i)
        {
            var gameObject = new GameObject(Facade.GetNameOfObject(i));

            gameObject.AddComponent<MeshFilter>();
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            var triangles = Facade.GetVertexIndexOfObject(i);

            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = norms;
            mesh.triangles = triangles;

            if (mesh.normals == null) mesh.RecalculateNormals();

            gameObject.AddComponent<MeshRenderer>();
            var renderer = gameObject.GetComponent<MeshRenderer>();
            var material = CreateMaterial(i, "Standard");
            CreateTextureMaps(i, ref material);
            renderer.material = material;
        }
    }

    static void CreateTextureMaps(int index, ref Material material)
    {
        var mapKd = Facade.GetPathOfMapOfObject("Diffuse", index);
        if (mapKd != null)
        {
            try
            {
                var texture = CreateTexture2D(mapKd);
                var texScale = Facade.GetScaleOfMapOfObject("Diffuse", index);
                var scale = new Vector2(texScale[0], texScale[1]);
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", scale);
            }
            catch (FileNotFoundException)
            {

            }
        }

        var mapBump = Facade.GetPathOfMapOfObject("Bump", index);
        if (mapBump != null)
        {
            try
            {
                var texture = CreateTexture2D(mapBump);
                var texScale = Facade.GetScaleOfMapOfObject("Bump", index);
                var scale = new Vector2(texScale[0], texScale[1]);
                material.SetTexture("_BumpMap", texture);
                material.SetTextureScale("_BumpTex", scale);
            }
            catch (FileNotFoundException)
            {

            }
        }
    }

    static Material CreateMaterial(int index, string shaderType)
    {
        var shader = Shader.Find(shaderType);
        var material = new Material(shader);
        var color = Facade.GetColorOfChannelOfObject("Diffuse", index);
        material.name = Facade.GetNameOfMaterialOfObject(index);
        material.color = new Color(color[0], color[1], color[2], color[3]);

        if (material.color.a < 1.0f)
            ChangeBlendMode(material, BlendMode.Transparent);

        return material;
    }

    static void GetVertices(ref Vector3[] verts)
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

    static void GetNormals(ref Vector3[] norms)
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

    static void GetUVs(ref Vector2[] uvs)
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