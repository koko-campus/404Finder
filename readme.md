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


# 処理フロー

ネットワークモデルによる幅優先探索からページ一覧を走査します。  



