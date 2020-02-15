using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class ImagePostProcress : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		TextureImporter importer = assetImporter as TextureImporter;
		string name = importer.assetPath.ToLower();
		string extension = name.Substring(name.LastIndexOf(".")).ToLower();
        
		if (extension == ".png") { 
			importer.spritePixelsPerUnit = 16;
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.filterMode = FilterMode.Point;
		}
	}
 
	private bool IsPixelArt(string path)
	{
		if (path.Contains("piskel"))
			return true;
 
		return false;
	}
}