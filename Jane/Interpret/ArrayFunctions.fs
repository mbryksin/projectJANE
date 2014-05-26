module ArrayFunctions

open AST

let getValueOfIndex (var : Variable) valOfIndex position = 
    let IntValOfIndex = 
        match valOfIndex with
        | Int num -> (int) num
        | _       -> -1 //Error index
    match var.Val with
    | Array array -> 
        match IntValOfIndex with
        | n when n >= 0 && n < array.Length -> array.[IntValOfIndex]
        | _                                 -> Err ("Index out of range", position)
    | Str str     -> 
        match IntValOfIndex with
        | n when n >= 0 && n < str.Length -> Char str.[IntValOfIndex]
        | _                                 -> Err ("Index out of range", position)
    | _           -> Empty