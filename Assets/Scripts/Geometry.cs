using System;
using System.IO;
using UnityEngine;
using Reality.ObjReader;

public static class Geometry
{
    static string directory;
    static Vector3[] verts;
    static Vector3[] norms;
    static Vector2[] uvs;

    public static void CreateObjects(string path)
    {
        directory = GetDirectoryName(path);

        var objCount = CreateVertices(path);

        for (int i = 0; i < objCount; ++i)
        {
            var gameObject = new GameObject(Facade.GetNameOfObject(i));

            gameObject.AddComponent<MeshFilter>();
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            var triangles = Facade.GetVertexIndexOfObject(i);

            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = norms;
            mesh.triangles = triangles;

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
        material.color = new Color(color[0], color[1], color[2], color[3]);

        return material;
    }

    static int CreateVertices(string path)
    {
        var objCount = Facade.ImportObjects(path);
        var objVerts = Facade.GetVertices();
        var objUVs = Facade.GetUVs();
        var objNorms = Facade.GetNormals();

        verts = new Vector3[objVerts.Length];
        uvs = new Vector2[objUVs.Length];
        norms = new Vector3[objNorms.Length];

        for (int i = 0; i < objVerts.Length; ++i)
        {
            verts[i] = new Vector3(
                objVerts[i][0],
                objVerts[i][1],
                objVerts[i][2]);

            norms[i] = new Vector3(
                objNorms[i][0],
                objNorms[i][1],
                objNorms[i][2]);

            uvs[i] = new Vector2(
                objUVs[i][0],
                objUVs[i][1]);
        }

        return objCount;
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
}