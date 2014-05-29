namespace JaneRuntime

open System
open AST

type JaneString =
    static member charAt(str : string, index : int64) : char = 
        str.Chars((int)index)
    static member compareTo(str : string, str1 : string) : bool = 
        (str.CompareTo str1) = 0
    static member compareToIgnoreCase(str : string, str1 : string) : bool = 
        String.Equals(str, str1, StringComparison.CurrentCultureIgnoreCase)
    static member concat(str : string, str1 : string) : string = 
        str + str1
    static member copyValueOf(data : char[]) : string = 
        new string(data)
    static member endsWith(str : string, suffix : string) : bool = 
        str.EndsWith suffix
    //static member getBytes(str : string) :  byte[] = 
    
    static member indexOfChar(str : string, ch : char) : int64 = 
        (int64)(str.IndexOf ch)
    static member indexOfCharFromIndex(str : string, ch : char, fromIndex : int64) : int64 = 
        (int64)(str.IndexOf(ch, (int)fromIndex))
    static member indexOfString(str : string, str1 : string) : int64 = 
        (int64)(str.IndexOf str1)
    static member indexOfStringFromIndex(str : string, str1 : string, fromIndex : int64) : int64 = 
        (int64)(str.IndexOf(str1, (int)fromIndex))
    static member lastIndexOfChar(str : string, ch : char) : int64 = 
        (int64)(str.LastIndexOf ch)
    static member lastIndexOfCharFromIndex(str : string, ch : char, fromIndex : int64) : int64 = 
        (int64)(str.LastIndexOf(ch, (int)fromIndex))
    static member lastIndexOfString(str : string, str1 : string) : int64 = 
        (int64)(str.LastIndexOf str1)
    static member lastIndexOfStringFromIndex(str : string, str1 : string, fromIndex : int64) : int64 = 
        (int64)(str.LastIndexOf(str1, (int)fromIndex))
    static member length(str : string) : int64 = 
        (int64)(str.Length)
    static member startsWith(str : string, prefix : string) : bool = 
        str.StartsWith prefix
    static member substringFromIndex(str : string, beginIndex : int64) : string = 
        str.Substring((int)beginIndex)
    static member substringFromInterval(str : string, beginIndex : int64, endIndex : int64) : string = 
        str.Substring((int)beginIndex, (int)(endIndex - beginIndex))
    static member toCharArray(str : string) : char[] = 
        str.ToCharArray()
    static member toLowerCase(str : string) : string = 
        str.ToLower()
    static member toUpperCase(str : string) : string = 
        str.ToUpper()
    static member trim(str : string) : string = 
        str.Trim()
    static member valueOfInt(x : int64) : string = 
        x.ToString()
    static member valueOfChar(x : char) : string = 
        x.ToString()
    static member valueOfFloat(x : float) : string = 
        x.ToString()
    static member valueOfBoolean(x : bool) : string = 
        x.ToString()
    static member parseInt(s : string) : int64 =
        Int64.Parse s
    static member parseFloat(s : string) : float =
        (float)(Single.Parse s)