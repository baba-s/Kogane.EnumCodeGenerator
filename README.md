# Uni Enum Code Generator

* 列挙型のコードを生成するエディタ拡張

## 使用例

### JobType 列挙型のコードを生成するエディタ拡張

```cs
using UniEnumCodeGenerator;
using UnityEditor;

public class Example
{
    [MenuItem( "Tools/Generate" )]
    private static void Generate()
    {
        var template = @"using System.Collections.Generic;

namespace #NAMESPACE_NAME#
{
    /// <summary>
    /// #ENUM_COMMENT#
    /// </summary>
    public enum #ENUM_NAME#
    {
#VALUES#
    }
    
    /// <summary>
    /// #COMPARER_COMMENT#
    /// </summary>
    public sealed class #COMPARER_NAME# : IEqualityComparer<#ENUM_NAME#>
    {
        public bool Equals( #ENUM_NAME# x, #ENUM_NAME# y )
        {
            return x == y;
        }
        
        public int GetHashCode( #ENUM_NAME# obj )
        {
            return ( int )obj;
        }
    }
    
    /// <summary>
    /// #ENUM_EXTENSION_COMMENT#
    /// </summary>
    public static partial class #ENUM_EXTENSION_NAME#
    {
        public const int LENGTH = #LENGTH#;
        
        public static IEnumerable<#ENUM_NAME#> GetValues()
        {
#GET_VALUES_CONTENTS#
        }
        
        public static string ToName( this #ENUM_NAME# self )
        {
            switch ( self )
            {
#TO_NAME_CONTENTS#
            }
            return string.Empty;
        }
        
        public static #ENUM_NAME# FromName( this string self )
        {
            switch ( self )
            {
#FROM_NAME_CONTENTS#
            }
            return default;
        }
        
        public static string ToComment( this #ENUM_NAME# self )
        {
            switch ( self )
            {
#TO_COMMENT_CONTENTS#
            }
            return string.Empty;
        }
    }
}
";
        var values = new[]
        {
            new EnumCodeGeneratorOptions.Value { Name = "NONE", Comment      = "無効" },
            new EnumCodeGeneratorOptions.Value { Name = "SOLDIER", Comment   = "王国兵士", UseHashCode = true },
            new EnumCodeGeneratorOptions.Value { Name = "SORCERER", Comment  = "魔法使い", UseHashCode = true },
            new EnumCodeGeneratorOptions.Value { Name = "HUNTER", Comment    = "狩人", UseHashCode   = true },
            new EnumCodeGeneratorOptions.Value { Name = "MERCENARY", Comment = "傭兵", UseHashCode   = true },
        };

        var options = new EnumCodeGeneratorOptions
        {
            Template             = template,
            NamespaceName        = "MyProject",
            EnumName             = "JobType",
            EnumComment          = "ジョブの種類",
            EnumExtensionName    = "JobTypeExt",
            EnumExtensionComment = "JobType 型の拡張メソッドを管理するクラス",
            ComparerName         = "JobTypeComparer",
            ComparerComment      = "JobType の比較をサポートするクラス",
            Values               = values,
        };

        var path = "Assets/JobType.cs";
        var code = EnumCodeGenerator.Generate( options );

        code = code
                .Replace( "\t", "    " )
                .Replace( "\r\n", "#NEW_LINE#" )
                .Replace( "\r", "#NEW_LINE#" )
                .Replace( "\n", "#NEW_LINE#" )
                .Replace( "#NEW_LINE#", "\r\n" )
            ;

        EnumCodeGenerator.Write( path, code );
        AssetDatabase.Refresh();
    }
}
```

### 生成された JobType 列挙型のコード

```cs
using System.Collections.Generic;

namespace MyProject
{
    /// <summary>
    /// ジョブの種類
    /// </summary>
    public enum JobType
    {
        ///<summary>
        ///<para>無効</para>
        ///</summary>
        NONE,
        ///<summary>
        ///<para>王国兵士</para>
        ///</summary>
        SOLDIER = 743680696,
        ///<summary>
        ///<para>魔法使い</para>
        ///</summary>
        SORCERER = 801230193,
        ///<summary>
        ///<para>狩人</para>
        ///</summary>
        HUNTER = 703509748,
        ///<summary>
        ///<para>傭兵</para>
        ///</summary>
        MERCENARY = -2029123294,
    }
    
    /// <summary>
    /// JobType の比較をサポートするクラス
    /// </summary>
    public sealed class JobTypeComparer : IEqualityComparer<JobType>
    {
        public bool Equals( JobType x, JobType y )
        {
            return x == y;
        }
        
        public int GetHashCode( JobType obj )
        {
            return ( int )obj;
        }
    }
    
    /// <summary>
    /// JobType 型の拡張メソッドを管理するクラス
    /// </summary>
    public static partial class JobTypeExt
    {
        public const int LENGTH = 5;
        
        public static IEnumerable<JobType> GetValues()
        {
            yield return JobType.NONE;
            yield return JobType.SOLDIER;
            yield return JobType.SORCERER;
            yield return JobType.HUNTER;
            yield return JobType.MERCENARY;
        }
        
        public static string ToName( this JobType self )
        {
            switch ( self )
            {
                case JobType.NONE: return "NONE";
                case JobType.SOLDIER: return "SOLDIER";
                case JobType.SORCERER: return "SORCERER";
                case JobType.HUNTER: return "HUNTER";
                case JobType.MERCENARY: return "MERCENARY";
            }
            return string.Empty;
        }
        
        public static JobType FromName( this string self )
        {
            switch ( self )
            {
                case "NONE": return JobType.NONE;
                case "SOLDIER": return JobType.SOLDIER;
                case "SORCERER": return JobType.SORCERER;
                case "HUNTER": return JobType.HUNTER;
                case "MERCENARY": return JobType.MERCENARY;
            }
            return default;
        }
        
        public static string ToComment( this JobType self )
        {
            switch ( self )
            {
                case JobType.NONE: return @"無効";
                case JobType.SOLDIER: return @"王国兵士";
                case JobType.SORCERER: return @"魔法使い";
                case JobType.HUNTER: return @"狩人";
                case JobType.MERCENARY: return @"傭兵";
            }
            return string.Empty;
        }
    }
}
```