using UdonSharp;
using UnityEngine;

public class TextObjectFollow : UdonSharpBehaviour
{

    public float yFactor = 0.5f;

    private void Update()
    {
        Quaternion parentRotation = transform.parent.rotation;
        Quaternion inverseRotation = Quaternion.Inverse(parentRotation);

        // Extract the Euler angles from both rotations
        Vector3 parentEuler = parentRotation.eulerAngles;
        Vector3 inverseEuler = inverseRotation.eulerAngles;

        // Interpolate the Y component based on the yFactor
        float newY = Mathf.Lerp(parentEuler.y, inverseEuler.y, yFactor);

        // Construct a new rotation using the interpolated Y and the original X and Z components
        transform.rotation = Quaternion.Euler(parentEuler.x, newY, parentEuler.z);
    }


}
