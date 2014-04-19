module ParseLiteral
open System

let ParseInt = 
    Int64.Parse

let ParseBool = 
    Boolean.Parse

let ParseChar (str:string) = 
    str.Substring(1, str.Length - 2)
    |> Char.Parse

let ParseFloat (str:string) =
    str.Replace('.', ',')
    |> Double.Parse

let ParseString (str:string) = 
    str.Substring(1, str.Length - 2)