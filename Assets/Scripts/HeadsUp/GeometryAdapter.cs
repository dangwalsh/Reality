
namespace HeadsUp
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.WSA;
#if !UNITY_EDITOR
    using Reality.ObjReader;
#endif
#if ENABLE_WINMD_SUPPORT
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.Storage.Pickers;
    using System.Threading.Tasks;
#endif

    public class GeometryAdapter : MonoBehaviour
    {
        const int MAXVERTS = 63000;
        string directory = "";
#if ENABLE_WINMD_SUPPORT
        FileOpenPicker openPicker;
#endif

#if ENABLE_WINMD_SUPPORT
        public async void OpenFileAsync()
        {
            openPicker = new FileOpenPicker();

            openPicker.FileTypeFilter.Add(".zip");

            var file = await openPicker.PickSingleFileAsync();

            byte[] fileBytes = null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            UnityEngine.WSA.Application.InvokeOnAppThread(new AppCallbackItem(() => { SaveToTemporaryFileAsync(file.Name, fileBytes); }), false);
        }
#endif

#if ENABLE_WINMD_SUPPORT
        public async void SaveToTemporaryFileAsync(string fileName, byte[] fileBytes)
        {
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, fileBytes);

            directory = ApplicationData.Current.TemporaryFolder.Path;

            var zipPath = Path.Combine(directory, file.Name);
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                archive.ExtractToDirectory(ApplicationData.Current.TemporaryFolder.Path, true);
            }

            var files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            var objFiles = new List<string>();

            foreach (var item in files)
            {
                if (Path.GetExtension(item.Path) != ".obj") continue;
                objFiles.Add(item.Path);
            }

            if (objFiles == null)
                throw new FileNotFoundException();

            foreach (string objPath in objFiles)
            {
                int count = Facade.ImportObjects(objPath);

                var verts = ConvertVertices();
                var uvs = ConvertUVs();

                CreateObjects(count, verts, uvs, gameObject);
            }

            var manager = gameObject.GetComponent<ModelManager>();
            manager.OrientModel();

            files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();

            foreach (var item in files)
                try
                {
                    await item.DeleteAsync();
                }
                catch (FileLoadException e)
                {
                    // recover after encountering an unreadable file
                }   
        }
#endif

        public void Initialize(string path, GameObject root)
        {

            directory = Path.GetDirectoryName(path);
#if !UNITY_EDITOR
            var zipTask = Facade.UnzipFileAsync(path);
            var objPaths = zipTask.Result;

            foreach (string objPath in objPaths)
            {
                int count = Facade.ImportObjects(objPath);

                var verts = ConvertVertices();
                var uvs = ConvertUVs();

                CreateObjects(count, verts, uvs, root);
            }
#endif
        }

        private void CreateObjects(int count, Vector3[] verts, Vector2[] uvs, GameObject root)
        {
            for (int obj = 0; obj < count; ++obj)
            {
#if !UNITY_EDITOR
                var vertIndex = Facade.GetVertexIndexOfObject(obj);
                var uvIndex = Facade.GetUVIndexOfObject(obj);
                //var normIndex = Facade.GetNormalIndexOfObject(obj);
                var divs = Math.Ceiling((double)vertIndex.Length / MAXVERTS);
                var name = Facade.GetNameOfObject(obj);

                var parent = GameObject.Find(name); 

                if (parent == null)
                {
                    parent = new GameObject(name + " Parent");
                    parent.transform.parent = root.transform;
                    parent.AddComponent<MeshRenderer>();
                    parent.GetComponent<MeshRenderer>().sharedMaterial = CreateMaterial(obj, "Standard");
                }

                for (int subdiv = 0; subdiv < divs; ++subdiv)
                {
                    var child = new GameObject(name);
                    child.AddComponent<MeshRenderer>();
                    child.GetComponent<MeshRenderer>().sharedMaterial = parent.GetComponent<MeshRenderer>().sharedMaterial;
                    child.AddComponent<MeshFilter>();

                    var mesh = child.GetComponent<MeshFilter>().mesh;
                    mesh.vertices = SplitArrays<Vector3>(verts, vertIndex, subdiv);
                    mesh.uv = SplitArrays<Vector2>(uvs, uvIndex, subdiv);
                    mesh.triangles = Enumerable.Range(0, mesh.vertices.Length).ToArray();

                    mesh.RecalculateNormals();
                    mesh.RecalculateTangents();
                    child.AddComponent<MeshCollider>();
                    child.transform.parent = parent.transform;
                }
#endif
            }
            
        }

        #region Material Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="shaderType"></param> 
        /// <returns></returns>
        private Material CreateMaterial(int index, string shaderType)
        {

            var shader = Shader.Find(shaderType);
            var material = new Material(shader);
#if !UNITY_EDITOR
            var color = Facade.GetColorOfChannelOfObject("Diffuse", index);
            var name = Facade.GetNameOfMaterialOfObject(index);

            material.name = name;
            material.color = new Color(color[0], color[1], color[2], color[3]);
#endif
            if (material.color.a < 1.0f)
                ChangeBlendMode(material, BlendMode.Transparent);
            CreateTextureMaps(index, material);
            return material;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="material"></param>
        private void CreateTextureMaps(int index, Material material)
        {
#if !UNITY_EDITOR
            var mapKd = Facade.GetPathOfMapOfObject("Diffuse", index);

            if (mapKd != null && mapKd != "")
            {
                try
                {
                    Texture2D texture = null;
                    texture = CreateTexture2D(mapKd);
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
                    Texture2D texture = null;
                    texture = CreateTexture2D(mapBump);
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
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Texture2D CreateTexture2D(string path)
        {

            var texture = new Texture2D(4, 4, TextureFormat.DXT5, true);
            var bytes = File.ReadAllBytes(directory + Path.DirectorySeparatorChar + path);
            var image = texture.LoadImage(bytes);
            if (!image)
                throw new Exception("the texture map did not load");

            return texture;
        }
        #endregion

        #region Geometry Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3[] ConvertVertices()
        {

            Vector3[] verts = null;
#if !UNITY_EDITOR
            var objVerts = Facade.GetVertices();
            if (objVerts == null) return null;
            verts = new Vector3[objVerts.Length];

            for (int i = 0; i < objVerts.Length; ++i)
            {
                verts[i] = new Vector3(
                    objVerts[i][0],
                    objVerts[i][1],
                    objVerts[i][2]
                );
            }
#endif
            return verts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3[] ConvertNormals()
        {

            Vector3[] norms = null;
#if !UNITY_EDITOR
            var objNorms = Facade.GetNormals();
            if (objNorms == null) return null;
            norms = new Vector3[objNorms.Length];

            for (int i = 0; i < objNorms.Length; ++i)
            {
                norms[i] = new Vector3(
                    objNorms[i][0],
                    objNorms[i][1],
                    objNorms[i][2]
                );
            }
#endif
            return norms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector2[] ConvertUVs()
        {

            Vector2[] uvs = null;
#if !UNITY_EDITOR
            var objUVs = Facade.GetUVs();
            if (objUVs == null) return null;
            uvs = new Vector2[objUVs.Length];

            for (int i = 0; i < objUVs.Length; ++i)
            {
                uvs[i] = new Vector2(
                    objUVs[i][0],
                    objUVs[i][1]
                );
            }
#endif
            return uvs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="verts"></param>
        /// <param name="index"></param>
        /// <param name="iter"></param>
        /// <returns></returns>
        private T[] SplitArrays<T>(T[] verts, int[] index, int iter)
        {

            var start = MAXVERTS * iter;
            var sector = index.Length - start;
            var end = (sector < MAXVERTS) ? sector + start : MAXVERTS + start;
            var vertices = new T[end];
            for (int i = start; i < end; ++i)
                vertices[i] = verts[index[i]];
            return vertices;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="blendMode"></param>
        private void ChangeBlendMode(Material material, BlendMode blendMode)
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
#if ENABLE_WINMD_SUPPORT
    /// <summary>
    /// Extension Class to ZipArchive
    /// </summary>
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Allows overwriting files while extracting to a directory
        /// </summary>
        /// <param name="archive">The file to be extracted</param>
        /// <param name="destinationDirectoryName">The location to extract</param>
        /// <param name="overwrite">Whether files should be overwritten</param>
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
#endif
}

