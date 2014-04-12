namespace AST

[<AbstractClass>]
type Initializer(pos : Position) =
    inherit Node(pos)  
    abstract member Interpret: Variable list -> Val

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ArrayInitializer(elements : Initializer list, pos : Position) =
    inherit Initializer(pos)
    member x.Elements = elements
    
    override x.ToString() = elements
                            |> List.map string
                            |> String.concat ", "
                            |> sprintf "{ %s }"

    override x.Interpret(context : Variable list)= Empty // later
