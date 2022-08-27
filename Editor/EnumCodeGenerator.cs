using System.IO;
using System.Text;

namespace Kogane
{
    /// <summary>
    /// 列挙型のコードを生成するクラス
    /// </summary>
    public static class EnumCodeGenerator
    {
        //================================================================================
        // 定数
        //================================================================================
        public const string TAG_NAMESPACE_NAME         = "#NAMESPACE_NAME#";
        public const string TAG_ENUM_NAME              = "#ENUM_NAME#";
        public const string TAG_ENUM_COMMENT           = "#ENUM_COMMENT#";
        public const string TAG_ENUM_EXTENSION_NAME    = "#ENUM_EXTENSION_NAME#";
        public const string TAG_ENUM_EXTENSION_COMMENT = "#ENUM_EXTENSION_COMMENT#";
        public const string TAG_COMPARER_NAME          = "#COMPARER_NAME#";
        public const string TAG_COMPARER_COMMENT       = "#COMPARER_COMMENT#";
        public const string TAG_VALUES                 = "#VALUES#";
        public const string TAG_LENGTH                 = "#LENGTH#";
        public const string TAG_GET_VALUES_CONTENTS    = "#GET_VALUES_CONTENTS#";
        public const string TAG_TO_NAME_CONTENTS       = "#TO_NAME_CONTENTS#";
        public const string TAG_FROM_NAME_CONTENTS     = "#FROM_NAME_CONTENTS#";
        public const string TAG_TO_COMMENT_CONTENTS    = "#TO_COMMENT_CONTENTS#";

        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// 指定されたパスに列挙型のコードを書き込みます
        /// </summary>
        public static void Write( string path, EnumCodeGeneratorOptions options )
        {
            var code = Generate( options );
            Write( path, code );
        }

        /// <summary>
        /// 指定されたパスに指定された文字列を書き込みます
        /// </summary>
        public static void Write( string path, string code )
        {
            var dir = Path.GetDirectoryName( path );

            if ( string.IsNullOrWhiteSpace( dir ) ) return;

            if ( !Directory.Exists( dir ) )
            {
                Directory.CreateDirectory( dir );
            }

            File.WriteAllText( path, code, Encoding.UTF8 );
        }

        /// <summary>
        /// 列挙型のコードを表現する文字列を生成して返します
        /// </summary>
        public static string Generate( EnumCodeGeneratorOptions options )
        {
            var template             = options.Template;
            var namespaceName        = options.NamespaceName;
            var enumName             = options.EnumName;
            var enumComment          = options.EnumComment;
            var enumExtensionName    = options.EnumExtensionName;
            var enumExtensionComment = options.EnumExtensionComment;
            var comparerName         = options.ComparerName;
            var comparerComment      = options.ComparerComment;
            var length               = options.Values.Length;
            var values               = GetValuesText( options );
            var getValuesContents    = GetGetValuesContents( options );
            var toNameContents       = GetToNameContents( options );
            var fromNameContents     = GetFromNameContents( options );
            var toCommentContents    = GetToCommentContents( options );
            var output               = template;

            output = output.Replace( TAG_NAMESPACE_NAME, namespaceName );
            output = output.Replace( TAG_ENUM_NAME, enumName );
            output = output.Replace( TAG_ENUM_COMMENT, enumComment );
            output = output.Replace( TAG_ENUM_EXTENSION_NAME, enumExtensionName );
            output = output.Replace( TAG_ENUM_EXTENSION_COMMENT, enumExtensionComment );
            output = output.Replace( TAG_COMPARER_NAME, comparerName );
            output = output.Replace( TAG_COMPARER_COMMENT, comparerComment );
            output = output.Replace( TAG_VALUES, values );
            output = output.Replace( TAG_LENGTH, length.ToString() );
            output = output.Replace( TAG_GET_VALUES_CONTENTS, getValuesContents );
            output = output.Replace( TAG_TO_NAME_CONTENTS, toNameContents );
            output = output.Replace( TAG_FROM_NAME_CONTENTS, fromNameContents );
            output = output.Replace( TAG_TO_COMMENT_CONTENTS, toCommentContents );

            return output;
        }

        /// <summary>
        /// 列挙型の要素を定義するコードを生成して返します
        /// </summary>
        private static string GetValuesText( EnumCodeGeneratorOptions options )
        {
            var builder = new StringBuilder();

            foreach ( var value in options.Values )
            {
                var comment     = value.Comment;
                var name        = value.Name;
                var useHashCode = value.UseHashCode;

                // コメントが複数行の場合も考慮
                builder.Append( "\t\t" ).AppendLine( "///<summary>" );
                foreach ( var n in comment.Split( '\n' ) )
                {
                    builder.Append( "\t\t" ).AppendFormat( "///<para>{0}</para>", n ).AppendLine();
                }

                builder.Append( "\t\t" ).AppendLine( "///</summary>" );

                if ( useHashCode )
                {
                    builder.Append( "\t\t" ).AppendFormat( "{0} = {1},", name, name.GetHashCode() ).AppendLine();
                }
                else
                {
                    builder.Append( "\t\t" ).AppendFormat( "{0},", name ).AppendLine();
                }
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// 列挙型の一覧を返す関数の中身のコードを生成して返します
        /// </summary>
        private static string GetGetValuesContents( EnumCodeGeneratorOptions options )
        {
            var builder = new StringBuilder();

            var enumName = options.EnumName;

            foreach ( var value in options.Values )
            {
                var name = value.Name;

                builder.Append( "\t\t\t" ).AppendFormat( @"yield return {0}.{1};", enumName, name ).AppendLine();
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// 列挙型を文字列に変換して返す switch 文の中身のコードを生成して返します
        /// </summary>
        private static string GetToNameContents( EnumCodeGeneratorOptions options )
        {
            var builder = new StringBuilder();

            var enumName = options.EnumName;

            foreach ( var value in options.Values )
            {
                var name = value.Name;

                builder.Append( "\t\t\t\t" ).AppendFormat( @"case {0}.{1}: return ""{1}"";", enumName, name ).AppendLine();
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// 文字列を列挙型に変換して返す switch 文の中身のコードを生成して返します
        /// </summary>
        private static string GetFromNameContents( EnumCodeGeneratorOptions options )
        {
            var builder = new StringBuilder();

            var enumName = options.EnumName;

            foreach ( var value in options.Values )
            {
                var name = value.Name;

                builder.Append( "\t\t\t\t" ).AppendFormat( @"case ""{1}"": return {0}.{1};", enumName, name ).AppendLine();
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// 列挙型をコメントに変換して返す switch 文の中身のコードを生成して返します
        /// </summary>
        private static string GetToCommentContents( EnumCodeGeneratorOptions options )
        {
            var builder = new StringBuilder();

            var enumName = options.EnumName;

            foreach ( var value in options.Values )
            {
                var name    = value.Name;
                var comment = value.Comment;

                builder.Append( "\t\t\t\t" ).AppendFormat( @"case {0}.{1}: return @""{2}"";", enumName, name, comment ).AppendLine();
            }

            return builder.ToString().TrimEnd();
        }
    }
}