namespace AST

[<AbstractClass>]
type Suffix(pos : Position) =
    inherit Node(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
type Arguments(arguments : Expression list, pos : Position) =
    inherit Suffix(pos)
    member x.Arguments = arguments

    override x.ToString() = arguments
                            |> List.map string
                            |> String.concat ", "
                            |> sprintf "(%s)"

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ArrayElement(index : Expression, pos : Position) =
    inherit Suffix(pos)
    member x.Index = index

    override x.ToString() = sprintf "[%A]" index