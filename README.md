Vket booth validator 2019.3b
====
Vケットのブースが入稿ルールに沿っているかチェックする入稿支援ツール(非公式)です。

## 注意事項
**万一に備え、実行前に必ずUnityプロジェクトのバックアップを取ってください。**
- 全てのルールをチェックできるわけではありません。（「対応ルール」参照）
- ツールの検証結果のOK/NGが公式に入稿したデータでのOK/NGを保証するものではありません。
- このツールは非公式です。全ての場合においてバーチャルマーケット運営の判断が優先されます。

## インストール
[ダウンロードページ](https://github.com/Kozu-vr/VketBoothValidator/releases/) からVketBoothValidator.zipをダウンロードし、解凍して中にあるUnitypackageを検証したいブースのシーンがあるプロジェクトにインポートしてください。

## 使い方
- ブースの作成配置を行いUnitypackageにエクスポートする直前の状態にします。
- シーンを開いた状態でTools＞Vket Booth Validatorから起動します。
- Base Folderに提出するフォルダを指定してValidateボタンを押すだけです。
  - On/Offブースで提出を申請している人は「For On/Off　Booth」にチェックを入れてください。
- 検証でNGだったルールはログに`[!]`がついて出力されます。

- 対応していないルールのチェックは各自でお願いします。

## 対応ルール
検証に対応しているルール・非対応のルールは以下の通りです。
各ルール対応の詳細は[Vket booth validator 2019.3b入稿ルール対応表](https://docs.google.com/spreadsheets/d/1Ut1YIa41OGs0pjQotlkYXKF2H3I76xygBRHKfPb-3aI/edit?usp=sharing)を確認してください


基準は2019.1.20更新の「バーチャルマーケット２入稿ルール」です。
使用前に公式のルールが改訂されていないことを確認してください。
https://www.v-market.work/

### 検証対応ルール
- A.提出形式
  - Unity 2017.4.15f1で作成すること
  - Assetフォルダ直下に「サークル名_サークル主(全て半角)」の名称のフォルダを作成していること(Assets/aaa_bbb形式であることの確認)
  - 提出フォルダ内に、フォルダ名と同名のブースのPrefabを作成すること
- B.ファイル&フォルダ名
  - 全角禁止（2バイト文字を使用していないこと）
  - 終わりの「~」禁止
  - あまりにも長すぎるパス名禁止（Assets以下が180文字以下と勝手に想定してます）
  - 「.blend」形式のファイル禁止
- C.Scene内階層形式
  - 「サークル名_サークル主名」という名称のEmptyオブジェクトが全ての親
Occluder Static, Occludee Static, Dynamicの３つのEmptyオブジェクトを作り、すべてのオブジェクトはこのどれかの階層下に入れること（入っていないオブジェクトはログだけを出力してスルーします）
  - 'Occluder Static'以下のオブジェクト設定が'Occluder Static'に、'Occludee Static'以下のオブジェクト設定が'Occludee Static'に設定されている。'Dynamic'以下ではどちらも設定されていない
- D.ブース形式
  - ブース寸法は幅4m×奥行き3m×高さ5m（初期表示でアクティブなブース内オブジェクトのBoundsが(X,Y,Z)=(4,5,3)以内）
  - マテリアル数制限10個以内（ブース内に設置するアバターのサンプル等全てを含む）（初期状態でアクティブなものをカウント）
　- テクスチャ圧縮（Use Crounch Compression）を指定すること
- G.Component規定
  - 使用可能コンポーネントと使用条件のチェック
  - 初期表示されていないObjectには、ObjectSync設定不可
  - VRC_Pickupは最大3つまで
  - RigidbodyはIs Kinematic推奨。Collision DetectionはDiscrete
  - 各種JointはVRC_Object Syncとの併用は不可
  - Lightの設定Baked推奨Real Timeで使いたい場合は応相談
  - Animator/AnimationとVRC_PickupおよびVRC_ObjectSyncとAnimatorの併用は不可 「../」の使用不可

### 非対応ルール
下記はツールでは検証できません。
[ツールが検証できないルールのチェック方法](https://github.com/Kozu-vr/VketBoothValidator/wiki)を参考にチェックしてください。
- 提出用UnityPackageの作成は検証の対象外です。不要な依存ファイルを含めないよう注意してください。
- ペデスタルアバターに関する規定は検証対象外です。
- ワールドアップロード時のサイズが10MB以内かは検証しません。
- 名前が「CVS」のフォルダ、ファイル、「.」で始まるフォルダ、ファイルはチェックできません。
- SetPass callsとBatchesの値は検証しません。
- VRC_Triggerの設定値が使用可能なものかは検証対象外です。

## 動作環境
以下の環境でテストしています。
- Windows 10
- Unity 2017.4.15f1
- VRCSDK-2018.12.19.17.03

（今のところWindows以外のOSは動作保証外です）

## ライセンス
MITライセンスの下でリリースされています。

Copyright (c) 2019 Kozu
Released under the MIT license
https://opensource.org/licenses/mit-license.php

## 作者
Kozu（Twitter：@kozu_vr）
ツールに関しての問い合わせはTwitterへお願いします。

## Special thanks（開発・動作テスト協力）
- がとーしょこら(Twitter: @gatosyocora)
- 小熊猫レッキー(Twitter: @VR_Red_Panda)
- sunafukin(Twitter: @sunafukin_vrc)
- 円フツ
- Karasuma-Kuro(Twitter: ＠Karasu_ma_Kuro)
