using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// D.ブース規定
    /// 07:テクスチャ圧縮設定ルール
    /// テクスチャ圧縮（テクスチャのinspectorでUse Crunch Compressionにチェックを入れてApply）されていることをチェック
    /// </summary>
    public class TextureCompressionRule : BaseRule
    {
        public new string ruleName = "D07:テクスチャ圧縮設定 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public TextureCompressionRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();

            string[] guids1 = AssetDatabase.FindAssets("t:texture2D", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            IEnumerable<string> dictinctGuids = guids1.Distinct();
            string assetPath;
            List<string> nonCrunchPath = new List<string>();
            List<string> highQualityPath = new List<string>();
            foreach (string guid in dictinctGuids)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (tex != null)
                {
                    switch (tex.format)
                    {
                        case TextureFormat.DXT1Crunched://Crunch Compression設定のあるRGBテクスチャ
                        case TextureFormat.DXT5Crunched://Crunch Compression設定のあるRGBAテクスチャ
                        case TextureFormat.BC6H: //HDR画像の場合
                        case TextureFormat.Alpha8: //Texture typeがSingle channelの場合
                        case TextureFormat.RGB565: //RGB 16bit
                        case TextureFormat.RGB24: //RGB 24bit
                        case TextureFormat.ARGB4444: //ARGB 16bit
                        case TextureFormat.RGBA32: //RGBA 32bit
                        case TextureFormat.RGBAHalf: //RGBA Half
                        case TextureFormat.BC4: //R Compressed BC4
                        case TextureFormat.BC5: //RG Compressed BC5
                            break;
                        case TextureFormat.BC7: //High Quality設定の場合
                            highQualityPath.Add(assetPath);
                            break;
                        case TextureFormat.DXT1://Crunch Compression設定のないRGBテクスチャ
                        case TextureFormat.DXT5://Crunch Compression設定のないRGBAテクスチャ
                            nonCrunchPath.Add(assetPath);
                            break;
                        default:
                            nonCrunchPath.Add(assetPath);
                            AddResultLog("不明なテクスチャフォーマット" + tex.name + " " + tex.format);
                            break;
                    }
                }
            }

            if (nonCrunchPath.Count > 0)
            {
                AddResultLog("`Use Crunch Compression`が設定されていないテクスチャ(強い推奨)：" + nonCrunchPath.Count);
                foreach (string path in nonCrunchPath.ToArray())
                {
                    AddResultLog(" " + path);
                }
            }
            if (highQualityPath.Count > 0)
            {
                //ログには出力するが検証はOKとする
                AddResultLog("以下の画像の`Compression`がHigh Qualityに設定されています。Normalで十分な場合は変更を検討してください。");
                foreach (string path in highQualityPath.ToArray())
                {
                    AddResultLog(" " + path);
                }
            }

            return SetResult(Result.SUCCESS);
        }
    }
}