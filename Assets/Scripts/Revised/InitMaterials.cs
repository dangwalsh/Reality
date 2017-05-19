#if!UNITY_EDITOR
using UnityEngine;
using Reality.ObjReader;

public static class InitMaterials {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static Texture2D CreateTexture2D(string path, string directory) {
        var texture = new Texture2D(4, 4, TextureFormat.DXT5, true);
        var bytes = System.IO.File.ReadAllBytes(
            directory + System.IO.Path.DirectorySeparatorChar + path);
        var result = texture.LoadImage(bytes);
        if (!result)
            throw new System.Exception("the texture map did not load");

        return texture;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="material"></param>
    public static void CreateTextureMaps(int index, ref Material material, string directory) {
        var mapKd = Facade.GetPathOfMapOfObject("Diffuse", index);
        if (mapKd != null && mapKd != "") {
            try {
                var texture = CreateTexture2D(mapKd, directory);
                var texScale = Facade.GetScaleOfMapOfObject("Diffuse", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", scale);
            }
            catch (System.IO.FileNotFoundException) {
#if UNITY_EDITOR
                throw;
#endif
            }
        }

        var mapBump = Facade.GetPathOfMapOfObject("Bump", index);
        if (mapBump != null && mapBump != "") {
            try {
                var texture = CreateTexture2D(mapBump, directory);
                var texScale = Facade.GetScaleOfMapOfObject("Bump", index);
                var scale = new Vector2(1 / texScale[0], 1 / texScale[1]);
                material.SetTexture("_BumpMap", texture);
                material.SetTextureScale("_BumpMap", scale);
            }
            catch (System.IO.FileNotFoundException) {
#if UNITY_EDITOR
                throw;
#endif
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="shaderType"></param>
    /// <returns></returns>
    public static Material CreateMaterial(int index, string shaderType) {
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="material"></param>
    /// <param name="blendMode"></param>
    private static void ChangeBlendMode(Material material, BlendMode blendMode) {
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