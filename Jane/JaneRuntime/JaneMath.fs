namespace JaneRuntime

open System
open AST

type JaneMath =
    static member PI = Math.PI
    static member E = Math.E
    static member writeLine(str : string) =
        Console.WriteLine(str)
    static member abs(value : int64) : int64 =
        Math.Abs value
    static member abs(value : float) : float =
        Math.Abs value
    static member acos(value : float) : float =
        Math.Acos value
    static member asin(value : float) : float =
        Math.Asin value
    static member atan(value : float) : float =
        Math.Atan value
    static member cbrt(value : float) : float =
        Math.Pow(value, 1.0 / 3.0)
    static member ceil(value : float) : float =
        Math.Ceiling value
    static member cos(value : float) : float =
        Math.Cos value
    static member cosh(value : float) : float =
        Math.Cosh value
    static member floor(value : float) : float =
        Math.Floor value
    static member ln(value : float) : float =
        Math.Log value
    static member log10(value : float) : float =
        Math.Log10 value
    static member max(a : int64, b : int64) : int64 =
        Math.Max(a, b)
    static member max(a : float, b : float) : float =
        Math.Max(a, b)
    static member min(a : int64, b : int64) : int64 =
        Math.Min(a, b)
    static member min(a : float, b : float) : float =
        Math.Min(a, b)
    static member pow(a : float, b : float) : float =
        Math.Pow(a, b)
    static member sin(value : float) : float =
        Math.Sin value
    static member sinh(value : float) : float =
        Math.Sinh value
    static member sqrt(value : float) : float =
        Math.Sqrt value
    static member tan(value : float) : float =
        Math.Tan value
    static member tanh(value : float) : float =
        Math.Tanh value
    static member toDegrees(value : float) : float =
        value * 180.0 / Math.PI
    static member toRadians(value : float) : float =
        value * Math.PI / 180.0
