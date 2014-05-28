module ArrayFunctions

open AST

let getValueOfIndex (var : Variable) listOfValIndexes position = 
    let intListOfValIndexes = 
        List.map (fun (v : Val) ->
            match v with
            | Int num -> (int) num
            | _       -> -1 //Error index
            ) listOfValIndexes
    match var.Val with
    | Array array -> 
        let rec getIndex (arr : Val array) indexList =
            match indexList with
            | currIndex :: tailIndexes -> 
                match currIndex with
                | n when n >= 0 && n < arr.Length -> 
                    match arr.[currIndex] with
                    | Array ar -> getIndex ar tailIndexes
                    | _ -> arr.[currIndex]                           
                | _ -> Err ("Index out of range", position)
            | [] -> Array arr
        getIndex array intListOfValIndexes

    | Str str     -> 
        match intListOfValIndexes.Head with
        | n when n >= 0 && n < str.Length -> Char str.[intListOfValIndexes.Head]
        | _                                 -> Err ("Index out of range", position)
    | _           -> Empty