using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AimPostProcessing : MonoBehaviour {
    Material material;

	// Use this for initialization
	void Awake()
    {
        material = new Material(Shader.Find("Hidden/ShaderQ2"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
