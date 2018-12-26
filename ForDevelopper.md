ルール作成者向け
===

# 概要
１検証ルールが１クラスに対応しているので新しいクラスを作って独自に拡張できます。

# 作り方
Z99_TemplateRule.csを参考に実装してください。
- BaseRuleを継承した新しいクラスを作成する。
- ruleNameにルール名を設定する。
- Validateメソッドをオーバーライドして実装する。
  - 検証結果をSetResult(Result)で設定する。
    - 検証を通過した場合はResult.SUCESS
    - 異常が見つかった場合はResult.FAIL
  - ログを表示したい場合はAddResultLog(string)を使用して追加する。
  - return で検証結果を返す。

- 作成したルールを実行するためRuleLoaderクラスの以下のいずれかのメソッドにルールを追加する。
  - 必ず検証を実行する：AddCommonRules
  - On/Offブースの指定がない時に検証を実行する：AddStandardBoothRules
  - On/Offブースの指定がある時に検証を実行する：AddOnOffBoothRules

# 使用できるプロパティ
BaseRuleクラスのoptionプロパティは以下の情報を持ち、Validateメソッド内で使用できます。
- public bool forOnoffBooth //On/Offブースとして検証するかどうか
- public DefaultAsset baseFolder //提出するベースフォルダ
- public string sceneGuid //ブースのあるシーンファイルのGUID

# 注意事項
- オーバーライドしたValidateメソッドの先頭で必ずbase.Validate()を呼ぶこと。
