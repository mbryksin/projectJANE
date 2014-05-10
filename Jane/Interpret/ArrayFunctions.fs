module ArrayFunctions

open AST

let getValueOfIndex (var : Variable) valOfIndex = 
    let IntValOfIndex = 
        match valOfIndex with
        | Int num -> (int) num
        | _       -> -1 //Error index
    match var.Val with
    | Array array -> array.[IntValOfIndex]
    | Str str     -> Char str.[IntValOfIndex]
    | _           -> Empty