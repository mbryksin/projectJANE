module ServiceFunction
open AST

let isTrueCondition (cond : Val) =
        match cond with
        | Bool true -> true
        | Int n when n <> 0L -> true
        | _ -> false

//take name and Type of Variable from Parameters and Value from Arguments
let rec addArgsToMethodContent (arguments : Val list) (parameters : FormalParameter list) (body : Block) =
    match arguments, parameters with
    | a :: args, p :: ps -> 
        let arg = arguments.Head
        let param = parameters.Head
        
        let VarName = param.Name.Value
        let VarType = param.Type
        let VarVal  = arg
        body.Context <- Variable(VarName, VarType, VarVal) :: body.Context
        addArgsToMethodContent arguments.Tail parameters.Tail body
    | _ -> () 