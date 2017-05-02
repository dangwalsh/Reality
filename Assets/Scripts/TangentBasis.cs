using System;
using System.Collections.Generic;
using UnityEngine;

public class TangentBasis : MonoBehaviour {

    /// <summary>
    /// Gets an array of tangent vectors for a mesh.
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
    public static Vector4[] GetMeshTangets(Mesh mesh, float hand) {

        var verts = mesh.vertices;
        var uvs = mesh.uv;
        var norms = mesh.normals;
        var length = mesh.triangles.Length;
        var tangents = new Vector4[length];

        for (int i = 0; i < length; i+=3) {
            var tangent = new Vector3();
            TangentBasis.Compute(verts[i],   uvs[i],
                                 verts[i+1], uvs[i+1],
                                 verts[i+2], uvs[i+2],
                                 ref tangent);
            tangents[i]   = AddHand(tangent, hand);
            tangents[i+1] = AddHand(tangent, hand);
            tangents[i+2] = AddHand(tangent, hand);
        }

        return tangents;
    }

    /// <summary>
    /// Adds the w component to the vector representing handedness of the tangent.
    /// </summary>
    /// <param name="tangent"></param>
    /// <param name="hand"></param>
    /// <returns></returns>
    static Vector4 AddHand(Vector3 tangent, float hand) {
        return new Vector4(tangent.x, tangent.y, tangent.z, hand);
    }

    /// <summary>
    /// Computes tangent, normal and basis for a triangle.
    /// </summary>
    /// <param name="P0">triangle vert 1</param>
    /// <param name="P1">triangle vert 2</param>
    /// <param name="P2">triangle vert 2</param>
    /// <param name="UV0">triangle uv 1</param>
    /// <param name="UV1">triagnle uv 2</param>
    /// <param name="UV2">triangle uv 3</param>
    /// <param name="normal">normal vector reference</param>
    /// <param name="tangent">tangent vector reference</param>
    /// <param name="binormal">binormal vector reference</param>
    static void Compute(Vector3 P0, Vector2 UV0,
                        Vector3 P1, Vector2 UV1,
                        Vector3 P2, Vector2 UV2,
                        ref Vector3 normal, 
                        ref Vector3 tangent, 
                        ref Vector3 binormal) {

        Vector3 e0 = P1 - P0;
        Vector3 e1 = P2 - P0;
        normal = Vector3.Cross(e0, e1);

        Vector3 P = P1 - P0;
        Vector3 Q = P2 - P0;

        float s1 = UV1.x - UV0.x;
        float t1 = UV1.y - UV0.y;
        float s2 = UV2.x - UV0.x;
        float t2 = UV2.y - UV0.y;

        float tmp = 0.0f;
        if (Mathf.Abs(s1 * t2 - s2 * t1) <= 0.0001f) {
            tmp = 1.0f;
        }
        else {
            tmp = 1.0f / (s1 * t2 - s2 * t1);
        }

        tangent.x = (t2 * P.x - t1 * Q.x);
        tangent.y = (t2 * P.y - t1 * Q.y);
        tangent.z = (t2 * P.z - t1 * Q.z);

        tangent = tangent * tmp;

        binormal.x = (s1 * Q.x - s2 * P.x);
        binormal.y = (s1 * Q.y - s2 * P.y);
        binormal.z = (s1 * Q.z - s2 * P.z);

        binormal = binormal * tmp;

        normal.Normalize();
        tangent.Normalize();
        binormal.Normalize();
    }

    /// <summary>
    /// Computes tangent and basis for a triangle.
    /// </summary>
    /// <param name="P0"></param>
    /// <param name="P1"></param>
    /// <param name="P2"></param>
    /// <param name="UV0"></param>
    /// <param name="UV1"></param>
    /// <param name="UV2"></param>
    /// <param name="tangent"></param>
    /// <param name="binormal"></param>
    static void Compute(Vector3 P0, Vector2 UV0,
                        Vector3 P1, Vector2 UV1,
                        Vector3 P2, Vector2 UV2,
                        ref Vector3 tangent, 
                        ref Vector3 binormal) {

        Vector3 P = P1 - P0;
        Vector3 Q = P2 - P0;

        float s1 = UV1.x - UV0.x;
        float t1 = UV1.y - UV0.y;
        float s2 = UV2.x - UV0.x;
        float t2 = UV2.y - UV0.y;

        float tmp = 0.0f;
        if (Mathf.Abs(s1 * t2 - s2 * t1) <= 0.0001f) {
            tmp = 1.0f;
        }
        else {
            tmp = 1.0f / (s1 * t2 - s2 * t1);
        }

        tangent.x = (t2 * P.x - t1 * Q.x);
        tangent.y = (t2 * P.y - t1 * Q.y);
        tangent.z = (t2 * P.z - t1 * Q.z);

        tangent = tangent * tmp;

        binormal.x = (s1 * Q.x - s2 * P.x);
        binormal.y = (s1 * Q.y - s2 * P.y);
        binormal.z = (s1 * Q.z - s2 * P.z);

        binormal = binormal * tmp;

        tangent.Normalize();
        binormal.Normalize();
    }

    /// <summary>
    /// Computes tangent for a triangle.
    /// </summary>
    /// <param name="P0"></param>
    /// <param name="P1"></param>
    /// <param name="P2"></param>
    /// <param name="UV0"></param>
    /// <param name="UV1"></param>
    /// <param name="UV2"></param>
    /// <param name="tangent"></param>
    static void Compute(Vector3 P0, Vector2 UV0, 
                        Vector3 P1, Vector2 UV1, 
                        Vector3 P2, Vector2 UV2,
                        ref Vector3 tangent) {

        Vector3 P = P1 - P0;
        Vector3 Q = P2 - P0;

        float s1 = UV1.x - UV0.x;
        float t1 = UV1.y - UV0.y;
        float s2 = UV2.x - UV0.x;
        float t2 = UV2.y - UV0.y;

        float tmp = 0.0f;
        if (Mathf.Abs(s1 * t2 - s2 * t1) <= 0.0001f) {
            tmp = 1.0f;
        }
        else {
            tmp = 1.0f / (s1 * t2 - s2 * t1);
        }

        tangent.x = (t2 * P.x - t1 * Q.x);
        tangent.y = (t2 * P.y - t1 * Q.y);
        tangent.z = (t2 * P.z - t1 * Q.z);

        tangent = tangent * tmp;

        tangent.Normalize();
    }
}
