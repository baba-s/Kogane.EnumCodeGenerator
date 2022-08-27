namespace Kogane
{
    /// <summary>
    /// 列挙型のコードを生成する時のオプションを管理するクラス
    /// </summary>
    public sealed class EnumCodeGeneratorOptions
    {
        /// <summary>
        /// 列挙型の要素の情報を管理するクラス
        /// </summary>
        public sealed class Value
        {
            /// <summary>
            /// 要素の名前
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 要素のコメント
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// ハッシュ値を代入する場合 true
            /// </summary>
            public bool UseHashCode { get; set; }
        }

        /// <summary>
        /// コードのテンプレート
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 名前空間の名前
        /// </summary>
        public string NamespaceName { get; set; }

        /// <summary>
        /// 列挙型の名前
        /// </summary>
        public string EnumName { get; set; }

        /// <summary>
        /// 列挙型のコメント
        /// </summary>
        public string EnumComment { get; set; }

        /// <summary>
        /// 列挙型の拡張メソッドを管理するクラスの名前
        /// </summary>
        public string EnumExtensionName { get; set; }

        /// <summary>
        /// 列挙型の拡張メソッドを管理するクラスのコメント
        /// </summary>
        public string EnumExtensionComment { get; set; }

        /// <summary>
        /// IEqualityComparer を実装するクラスの名前
        /// </summary>
        public string ComparerName { get; set; }

        /// <summary>
        /// IEqualityComparer を実装するクラスのコメント
        /// </summary>
        public string ComparerComment { get; set; }

        /// <summary>
        /// 列挙型の要素の情報を管理する配列
        /// </summary>
        public Value[] Values { get; set; }
    }
}