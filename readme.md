# 404Finder

指定したドメインからページ一覧をクロールして404返されるURL一覧を取得します。  
リンク切れの確認を行うことを主目的とし、副次的にサイトに対する応答速度を取得することでためのパフォーマンスを測ります。  
また、サイトマップの構築や公開していないページの確認にも使用可能です。  


# 環境情報

|  機能  |  バージョン  |
| ---- | ---- |
|  C#  |  .NET依存  |
|  .NET  |  6.0  |
| [AngleSharp](https://anglesharp.github.io/) | 0.17.1 |
| Windows | 11 |
| Visual Studio | 2022 |
| SqlClient(https://github.com/dotnet/corefx) | 4.8.3 |
| SQL Server | Microsoft SQL Server 2019 (RTM-GDR) (KB5014356) - 15.0.2095.3 (X64)   Apr 29 2022 18:00:13   Copyright (C) 2019 Microsoft Corporation  Express Edition (64-bit) on Windows 10 Home 10.0 <X64> (Build 22000: ) |
| SSMS | 18.11.1 |


# 処理フロー

ネットワークモデルによる幅優先探索からページ一覧を走査します。  
以下の流れで処理を実行します。

1. 各種設定の読み込み
2. URLの走査 (urlWalker)
3. トップページへのアクセス
4. HTMLの解析・リンクの取得
5. ページ以外で使用されているURLへリクエスト、ステータスコードの確認。
6. 他のリンクへアクセス
7. 「4」へ戻る


