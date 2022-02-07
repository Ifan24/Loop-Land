using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    private static System.Random rng = new System.Random();  
    
    /// <summary>
    /// A generic function that shuffle a list
    /// </summary>
    public static void Shuffle<T>(this IList<T> list) {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
    
    /// <summary>
    /// Draw a OverlapSphere at position with radius and get the Colliders with tag name
    /// </summary>
    /// <param name="position">The Vector3 value represents the center of sphere.</param>
    /// <param name="radius">The Vector3 value represents the radius of the sphere.</param>
    /// <param name="tag">A string of the tag name.</param>
    /// <returns>The Colliders.</returns>
    public static List<Collider> GetCollidersAtWithTag(Vector3 position, float radius, string tag) {
        var res = new List<Collider>();
        foreach(Collider collider in Physics.OverlapSphere(position, radius)) {
            if (collider.CompareTag(tag)) {
                res.Add(collider);
            }
        }
        return res;
    }
    
}
