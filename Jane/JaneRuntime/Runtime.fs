namespace JaneRuntime

open System
open AST
        
type Runtime = 
    static member private classNotFound (classname : string) = printfn "No class %s found in runtime library" classname
    static member private methodNotFound (classname : string) (methodname : string) = printfn "No method %s found in %s class" methodname classname
    static member private fieldNotFound (classname : string) (fieldname : string) = printfn "No field %s found in %s class" fieldname classname
    static member private typeMismatch (classname : string) (methodname : string) = printfn "Type mismatch in %s.%s" classname methodname 

    static member getStaticField(classname : string, fieldname : string) : Val =
        match classname with
        | "Math" -> 
            match fieldname with
            | "PI" -> Float(JaneMath.PI)
            | "E" -> Float(JaneMath.E)
            | _ -> Runtime.fieldNotFound classname fieldname; Empty
        | _ -> Runtime.classNotFound classname; Empty

    static member callStaticMethod(classname : string, methodname : string, args : Val list) : Val =
        match classname with
        | "Console" -> 
            match methodname with
            | "writeLine" -> 
                Str(JaneConsole.writeLine args.Head)
            | _ -> Runtime.methodNotFound classname methodname; Empty
        | "Math" -> 
            match methodname with
            | "abs" -> 
                match args.Head with
                    | Int i -> Int (JaneMath.abs i)
                    | Float f -> Float (JaneMath.abs f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "acos" ->
                match args.Head with
                    | Float f -> Float (JaneMath.acos f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "asin" ->
                match args.Head with
                    | Float f -> Float (JaneMath.asin f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "atan" ->
                match args.Head with
                    | Float f -> Float (JaneMath.atan f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "cbrt" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cbrt f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "ceil" ->
                match args.Head with
                    | Float f -> Float (JaneMath.ceil f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "cos" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cos f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "cosh" ->
                match args.Head with
                    | Float f -> Float (JaneMath.cosh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "floor" ->
                match args.Head with
                    | Float f -> Float (JaneMath.floor f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "ln" ->
                match args.Head with
                    | Float f -> Float (JaneMath.ln f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "log10" ->
                match args.Head with
                    | Float f -> Float (JaneMath.log10 f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "max" -> 
                match (args.Head, args.Tail.Head) with
                    | (Int a, Int b) -> Int (JaneMath.max(a, b))
                    | (Float a, Float b) -> Float (JaneMath.max(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "min"-> 
                match (args.Head, args.Tail.Head) with
                    | (Int a, Int b) -> Int (JaneMath.min(a, b))
                    | (Float a, Float b) -> Float (JaneMath.min(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "pow" -> 
                match (args.Head, args.Tail.Head) with
                    | (Float a, Float b) -> Float (JaneMath.pow(a, b))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sin" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sin f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sinh" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sinh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "sqrt" ->
                match args.Head with
                    | Float f -> Float (JaneMath.sqrt f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "tan" ->
                match args.Head with
                    | Float f -> Float (JaneMath.tan f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "tanh" ->
                match args.Head with
                    | Float f -> Float (JaneMath.tanh f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toDegrees" ->
                match args.Head with
                    | Float f -> Float (JaneMath.toDegrees f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toRadians" ->
                match args.Head with
                    | Float f -> Float (JaneMath.toRadians f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | _ -> Runtime.methodNotFound classname methodname; Empty
        | "String" ->
            match methodname with

            | "charAt" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Int i) -> Char (JaneString.charAt(s, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "compareTo" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Bool (JaneString.compareTo(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "compareToIgnoreCase" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Bool (JaneString.compareToIgnoreCase(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "concat" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Str (JaneString.concat(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "copyValueOf" ->
                let valToCharArray (a : Val array) =
                    let mapping (v : Val) : char =
                        match v with
                        | Char c -> c
                        | _ -> (char)0
                    Array.map mapping a
                match args.Head with
                    | Array a -> Str (JaneString.copyValueOf (valToCharArray a))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "endsWith" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Bool (JaneString.endsWith(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "indexOfChar" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Char c) -> Int (JaneString.indexOfChar(s, c))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "indexOfCharFromIndex" ->
                match (args.Head, args.Tail.Head, args.Tail.Tail.Head) with
                    | (Str s, Char c, Int i) -> Int (JaneString.indexOfCharFromIndex(s, c, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "indexOfString" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Int (JaneString.indexOfString(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "indexOfStringFromIndex" ->
                match (args.Head, args.Tail.Head, args.Tail.Tail.Head) with
                    | (Str s, Str s1, Int i) -> Int (JaneString.indexOfStringFromIndex(s, s1, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "lastIndexOfChar" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Char c) -> Int (JaneString.lastIndexOfChar(s, c))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "lastIndexOfCharFromIndex" ->
                match (args.Head, args.Tail.Head, args.Tail.Tail.Head) with
                    | (Str s, Char c, Int i) -> Int (JaneString.lastIndexOfCharFromIndex(s, c, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "lastIndexOfString" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Int (JaneString.lastIndexOfString(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "lastIndexOfStringFromIndex" ->
                match (args.Head, args.Tail.Head, args.Tail.Tail.Head) with
                    | (Str s, Str s1, Int i) -> Int (JaneString.lastIndexOfStringFromIndex(s, s1, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "length" ->
                match args.Head with
                    | Str s -> Int (JaneString.length s)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "startsWith" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Str s1) -> Bool (JaneString.startsWith(s, s1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "substringFromIndex" ->
                match (args.Head, args.Tail.Head) with
                    | (Str s, Int i) -> Str (JaneString.substringFromIndex(s, i))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "substringFromInterval" ->
                match (args.Head, args.Tail.Head, args.Tail.Tail.Head) with
                    | (Str s, Int i, Int i1) -> Str (JaneString.substringFromInterval(s, i, i1))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toCharArray" ->
                match args.Head with
                    | Str s -> Array (Array.map (fun c -> Char c) (JaneString.toCharArray s))
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toLowerCase" ->
                match args.Head with
                    | Str s -> Str (JaneString.toLowerCase s)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "toUpperCase" ->
                match args.Head with
                    | Str s -> Str (JaneString.toUpperCase s)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "trim" ->
                match args.Head with
                    | Str s -> Str (JaneString.trim s)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "valueOfInt" ->
                match args.Head with
                    | Int i -> Str (JaneString.valueOfInt i)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "valueOfChar" ->
                match args.Head with
                    | Char c -> Str (JaneString.valueOfChar c)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "valueOfFloat" ->
                match args.Head with
                    | Float f -> Str (JaneString.valueOfFloat f)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | "valueOfBoolean" ->
                match args.Head with
                    | Bool b -> Str (JaneString.valueOfBoolean b)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | _ -> Runtime.methodNotFound classname methodname; Empty
        | "Array" ->
            match methodname with
            | "length" -> 
                match args.Head with
                    | Array a -> Int ((int64)a.Length)
                    | _ -> Runtime.typeMismatch classname methodname; Empty
            | _ -> Runtime.methodNotFound classname methodname; Empty

        | _ -> Runtime.classNotFound classname; Empty
        
