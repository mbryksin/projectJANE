namespace AST

[<AbstractClass>]
type Suffix(pos : Position) =
    inherit Node(pos)
    
type Arguments(arguments : Expression list, pos : Position) =
    inherit Suffix(pos)
    member x.Arguments = arguments

type ArrayElement(index : Expression, pos : Position) =
    inherit Suffix(pos)
    member x.Index = index