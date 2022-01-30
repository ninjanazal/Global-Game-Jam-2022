using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraViewEffects : MonoBehaviour
{
	/// <summary> Toggles the invert camera view output </summary>
	public bool InvertColors = false;


	[Header("Screen Space post-process")]
	/// <summary>
	/// Invert screen colors Material
	/// </summary>
	public Material InvertColor_Material = null;


	/// <summary>
	/// Unity graphics API onRenderImage funtion override
	/// </summary>
	void OnRenderImage(RenderTexture source, RenderTexture destination){
		if(InvertColor_Material != null && InvertColors){
			Graphics.Blit(source, destination, InvertColor_Material);
			return;
		}

		Graphics.Blit(source, destination);
	}
}
