using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
 
public class ImagePostProcress : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		TextureImporter importer = assetImporter as TextureImporter;
		string name = importer.assetPath.ToLower();
		string extension = name.Substring(name.LastIndexOf(".")).ToLower();

		var size = GetWidthAndHeight(importer);
		if (name.Contains("full")) return;

		if (extension == ".png") {
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.filterMode = FilterMode.Point;

			if (size.x == 16 || size.y == 16) { 
				importer.spritePixelsPerUnit = 16;
			}
			if (size.x != 16 || size.y != 16) {
				importer.spriteImportMode = SpriteImportMode.Multiple;
			}
		}
	}

	void OnPostprocessTexture(Texture2D texture) {
		TextureImporter importer = assetImporter as TextureImporter;
		string name = importer.assetPath.ToLower();
		string assetName = name.Substring(
			name.LastIndexOf("/") + 1, 
			name.LastIndexOf(".") - 1 - name.LastIndexOf("/"));

		int spriteSize = 16;
        int colCount = texture.width / spriteSize;
        int rowCount = texture.height / spriteSize;
 
        List<SpriteMetaData> metas = new List<SpriteMetaData>();
 
        for (int r = 0; r < rowCount; ++r) {
            for (int c = 0; c < colCount; ++c) {
                SpriteMetaData meta = new SpriteMetaData();
                meta.rect = new Rect(c * spriteSize, r * spriteSize, spriteSize, spriteSize);
                meta.name = assetName + "_" + c + "-" + r;
                metas.Add(meta);
            }
        }
 
        importer.spritesheet = metas.ToArray();
		AssetDatabase.Refresh(); 
	}
 
	private Vector2 GetWidthAndHeight(TextureImporter importer) {
		if (importer == null) return Vector2.zero;
		
		object[] args = new object[2] { 0, 0 };
		MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
		mi.Invoke(importer, args);
		return new Vector2(
			(int) args[0],
			(int) args[1]);
	}
}