# 404Finder

指定したドメインからページ一覧をクロールして404返されるURL一覧を取得します。  
リンク切れの確認を行うことを主目的とし、副次的にサイトに対する応答速度を取得することでためのパフォーマンスを測ります。  
また、サイトマップの構築や公開していないページの確認にも使用可能です。  


# 環境情報

|  機能  |  バージョン  |
| ---- | ---- |
|  C#  |  .NET依存  |
|  .NET  |  6.0  |
| Windows | 11 |
| Visual Studio | 2022 |
| SqlClient(https://github.com/dotnet/corefx) | 4.8.3 |
| [AngleSharp](https://anglesharp.github.io/) | 0.17.1 |
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



# 環境構築

## データベース

SQLServerとIDEであるSSMSをインストール

* https://www.microsoft.com/ja-jp/sql-server/sql-server-downloads
* https://docs.microsoft.com/ja-jp/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

```sql
CREATE DATABASE [404Finder];

USE [404Finder];

CREATE TABLE execute_log(
	id INT PRIMARY KEY,
	fqdn VARCHAR(300) NOT NULL,
	done_by VARCHAR(100) NOT NULL,
	done_at VARCHAR(100) NOT NULL,
	status BIT DEFAULT 0,
	rgdt DATETIME CURRENT_TIMESTAMP,
	updt DATETIME CURRENT_TIMESTAMP
);

CREATE TABLE result(
	id INT,
	path VARCHAR(300),
	ext VARCHAR(30) NULL,
	status_code CHAR(3) NULL,
	content_type VARCHAR(50) NULL,
	last_modified DATETIME NULL,
	file_size int NOT NULL,
	charset VARCHAR(50) NOT NULL,
	step INT NOT NULL,
	rgdt DATETIME DEFAULT CURRENT_TIMESTAMP,
	updt DATETIME DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(id, path)
);
```

