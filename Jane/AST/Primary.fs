namespace AST

[<AbstractClass>]
type Primary(pos) =
    inherit Expression(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type Constructor(typeName : ID, arguments : Arguments, pos : Position) =
    inherit Primary(pos)
    member x.Name      = typeName
    member x.Arguments = arguments

    override x.ToString() = sprintf "new %A%A" typeName arguments

    override x.Interpret(context : Variable list) = Empty // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Identifier(name : ID) =
    inherit Primary(name.Position)
    member x.Name = name

    override x.ToString() = name.Value
 
    override x.Interpret(context : Variable list) = 
        let currVar = List.find (fun (var: Variable) -> var.Name = x.Name.Value) context
        currVar.Val



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Member(name : ID, suffix : Suffix, pos : Position) =
    inherit Primary(pos)
    member x.Name   = name
    member x.Suffix = suffix

    override x.ToString() = sprintf "%A%A" name suffix

    override x.Interpret(context : Variable list) =
        let currentVariable = List.find (fun (v: Variable) -> v.Name = x.Name.Value) context
        let ValOfIndex = 
            match x.Suffix with
            | :? ArrayElement  as arrayElem -> arrayElem.Index.Interpret(context)
            | _ -> Empty

        let IntValOfIndex = 
            match ValOfIndex with
            | Int num -> (int) num

        match currentVariable.Val with
        | Array array -> array.[IntValOfIndex]
        | _           -> Empty


