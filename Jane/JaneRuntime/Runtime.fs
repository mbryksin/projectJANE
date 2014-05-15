namespace JaneRuntime

open System
open AST

type JaneConsole =
    static member writeLine(str : string) =
        Console.WriteLine(str)
        
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
type Runtime = 
    static member private classNotFound (classname : string) = printfn "No class %s found in runtime library" classname
    static member private methodNotFound (classname : string) (methodname : string) = printfn "No method %s found in %s class" methodname classname
    static member private fieldNotFound (classname : string) (fieldname : string) = printfn "No field %s found in %s class" fieldname classname
    static member private typeMismatch (classname : string) (methodname : string) = printfn "Type mismatch in %s.%s" classname methodname 

    static member callStaticVoidMethod(classname : string, methodname : string, args : Val list) : unit = 
        match classname with
        | "Console" -> 
            match methodname with
            | "writeLine" -> 
                match args.Head with
                    | Str s -> JaneConsole.writeLine s
                    | _ -> Runtime.typeMismatch classname methodname
            | _ -> Runtime.methodNotFound classname methodname
        | _ -> Runtime.classNotFound classname

    static member getStaticField(classname : string, fieldname : string) : Val =
        match classname with
        | "Math" -> 
            match fieldname with
            | "PI_F" | "PI_D" -> Float(JaneMath.PI)
            | "E_F" | "E_D" -> Float(JaneMath.E)
            | _ -> Runtime.fieldNotFound classname fieldname; Empty
        | _ -> Runtime.classNotFound classname; Empty

    static member callStaticMethod(classname : string, methodname : string, args : Val list) : Val =
        match classname with
        | "Math" -> 
            match methodname with
            | "absb" | "abss" | "absi" | "absl" -> 
                match args.Head with
                    | Int i -> Int (JaneMath.abs i)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "absf" | "absd" -> 
                match args.Head with
                    | Float f -> Float (JaneMath.abs f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty

            | "acosf" | "acosd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.acos f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "asinf" | "asind" ->
                match args.Head with
                    | Float f -> Float (JaneMath.asin f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "atanf" | "atand" ->
                match args.Head with
                    | Float f -> Float (JaneMath.atan f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "cbrtf" | "cbrtd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cbrt f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "ceilf" | "ceild" ->
                match args.Head with
                    | Float f -> Float (JaneMath.ceil f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "cosf" | "cosd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cos f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "coshf" | "coshd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cosh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "floorf" | "floord" ->
                match args.Head with
                    | Float f -> Float (JaneMath.floor f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "lnf" | "lnd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.ln f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "log10f" | "log10d" ->
                match args.Head with
                    | Float f -> Float (JaneMath.log10 f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "maxb" | "maxs" | "maxi" | "maxl" -> 
                match (args.Head, args.Tail.Head) with
                    | (Int a, Int b) -> Int (JaneMath.max(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "maxf" | "maxd" -> 
                match (args.Head, args.Tail.Head) with
                    | (Float a, Float b) -> Float (JaneMath.max(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "minb" | "mins" | "mini" | "minl" -> 
                match (args.Head, args.Tail.Head) with
                    | (Int a, Int b) -> Int (JaneMath.min(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "minf" | "mind" -> 
                match (args.Head, args.Tail.Head) with
                    | (Float a, Float b) -> Float (JaneMath.min(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "powf" | "powd" -> 
                match (args.Head, args.Tail.Head) with
                    | (Float a, Float b) -> Float (JaneMath.pow(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sinf" | "sind" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sin f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sinhf" | "sinhd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sinh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sqrtf" | "sqrtd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sqrt f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "tanf" | "tand" ->
                match args.Head with
                    | Float f -> Float (JaneMath.tan f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "tanhf" | "tanhd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.tanh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toDegreesf" | "toDegreesd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.toDegrees f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toRadiansf" | "toRadiansd" ->
                match args.Head with
                    | Float f -> Float (JaneMath.toRadians f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | _ -> Runtime.methodNotFound classname methodname; Empty
        | _ -> Runtime.classNotFound classname; Empty
        
