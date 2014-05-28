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

type ArrayElement(indexes : Expression list, pos : Position) =
    inherit Suffix(pos)
    member x.Indexes = indexes
    
    override x.ToString() = List.fold (fun acc x -> acc + x.ToString()) "" indexes