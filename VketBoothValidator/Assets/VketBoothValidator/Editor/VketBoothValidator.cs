using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;
/// <summary>
/// Vket booth validator
/// </summary>

namespace VketTools
{
    public enum Result
    {
        FAIL,
        SUCCESS,
        NOTRUN
    }

    public class VketBoothValidator : EditorWindow
    {
        //Valiables
        private string version = "2019.3 Beta";
        private string validationLog;
        private Vector2 scroll;
        private bool onoffBooth;
        private bool onlyErrorLog;
        private string sceneGuid;
        private DefaultAsset baseFolder;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        #region Unity GUI
        [MenuItem("Tools/Vket Booth Validator")]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = EditorWindow.GetWindow(typeof(VketBoothValidator));
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.titleContent = new GUIContent("Validator");
            editorWindow.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Vket booth validator Ver." + version);
            //Base folder setting
            DefaultAsset newFolder = (DefaultAsset)EditorGUILayout.ObjectField("Base Folder", baseFolder, typeof(DefaultAsset), true);
            var path = AssetDatabase.GetAssetPath(newFolder);
            if (AssetDatabase.IsValidFolder(path))
            {
                baseFolder = newFolder;
            }
            else
            {
                baseFolder = null;
            }
            //On/Off booth setting
            onoffBooth = EditorGUILayout.ToggleLeft("For On/Off Booth", onoffBooth);
            onlyErrorLog = EditorGUILayout.ToggleLeft("Error log only", onlyErrorLog);
            if (GUILayout.Button("Validate"))
            {
                OnValidate();
            };
            //Result log
            scroll = EditorGUILayout.BeginScrollView(scroll);
            validationLog = GUILayout.TextArea(validationLog, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Copy result"))
            {
                OnCopyResult();
            };
        }
        #endregion

        #region Actions
        void OnValidate()
        {
            validationLog = "";
            //Base folder
            OutLog(string.Format("Start vket booth validation. ({0})", version));
            if (baseFolder)
            {
                OutLog("Base folder:" + baseFolder.name);
            }
            else
            {
                OutLog("Select Base folder.");
                return;
            }
            //Booth scene
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { AssetDatabase.GetAssetPath(baseFolder) });
            foreach (string guid in sceneGuids)
            {
                OutLog("Scene file:" + AssetDatabase.GUIDToAssetPath(guid));
            }
            if (sceneGuids.Length == 0)
            {
                OutError("Scene file not found.");
                return;
            }
            else if (sceneGuids.Length > 1)
            {
                OutError(String.Format("{0} scene files found.", sceneGuids.Length));

                return;
            }
            else
            {
                sceneGuid = sceneGuids[0];
            }
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(sceneGuid));
            if (SceneManager.GetActiveScene() != scene)
            {
                OutError(String.Format("対象のシーンを開いてください"));
                return;
            }
            //On/OffBooth
            if (onoffBooth)
            {
                OutLog("For On/Off booth");
            }

            //Validation
            Options options = new Options(baseFolder, onoffBooth, sceneGuid);
            Utils.GetInstance().setOptons(options);
            BaseRule[] targetRules = RuleLoader.Load(options);
            int invalidRuleCount = 0;
            foreach (BaseRule rule in targetRules)
            {
                rule.Validate();
                OutLog(rule, onlyErrorLog);
                if (rule.GetResult() == Result.FAIL)
                {
                    invalidRuleCount++;
                }
            }
            OutLog(string.Format("---\n{0}件のルール違反が見つかりました。", invalidRuleCount));
            OutLog("Finish validation");
        }

        private void OnCopyResult()
        {
            EditorGUIUtility.systemCopyBuffer = validationLog;
        }
        #endregion

        #region Log
        private void OutLog(string txt)
        {
            Debug.Log(txt);
            validationLog += txt + System.Environment.NewLine;
        }

        private void OutLog(BaseRule rule, bool onlyErrorLog)
        {
            if (rule.GetResult() == Result.FAIL)
            {
                Debug.LogError(rule.ResultLog);
                validationLog += System.Environment.NewLine + "[!]" + rule.RuleName + System.Environment.NewLine;
                validationLog += rule.ResultLog + System.Environment.NewLine;
            }
            else
            {
                Debug.Log(rule.RuleName + ":" + rule.ResultLog);
                if (!onlyErrorLog)
                {
                    validationLog += rule.ResultLog + System.Environment.NewLine;
                }
            }

        }

        private void OutError(string txt)
        {
            Debug.LogError(txt);
            validationLog += "[!]" + txt + System.Environment.NewLine;
        }
        #endregion
    }

    /// <summary>
    /// 検証の実行オプション
    /// </summary>
    public class Options
    {
        //OnOffBoothとして検証する
        public bool forOnoffBooth = false;
        //提出するベースフォルダ
        public DefaultAsset baseFolder = null;
        //ブースのあるシーンファイルのGUID
        public string sceneGuid = null;
        public Options(DefaultAsset _baseFolder, bool _forOnoff, string _sceneGuid)
        {
            baseFolder = _baseFolder;
            forOnoffBooth = _forOnoff;
            sceneGuid = _sceneGuid;
        }
    }

    /// <summary>
    /// 検証ルールの基本クラス
    /// </summary>
    public class BaseRule
    {
        //検証設定
        public Options options;
        //検証ルール名
        public string ruleName = "Base Rule";
        public virtual string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        //検証ログ
        public string ResultLog { get; set; }
        //検証結果
        private Result _result;
        private bool logFlag = false;

        public BaseRule(Options _options)
        {
            options = _options;
            ResultLog = RuleName + ":Not run yet.";
            SetResult(Result.NOTRUN);
        }

        /// <summary>
        /// 検証を行い、結果を返す。
        /// </summary>
        public virtual Result Validate()
        {
            ResultLog = "";
            return _result;
        }

        public Result SetResult(Result re)
        {
            _result = re;
            return _result;
        }
        public Result GetResult()
        {
            return _result;
        }

        public void AddResultLog(string log)
        {
            if (!logFlag)
            {
                ResultLog = log;
                logFlag = true;
            }
            else
            {
                ResultLog += System.Environment.NewLine + log;
            }
        }
    }
}
