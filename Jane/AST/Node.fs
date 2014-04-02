namespace AST

type Position(startLine : int, startSymbol : int, endLine : int, endSymbol : int) =
    member x.StartLine   = startLine
    member x.StartSymbol = startSymbol
    member x.EndLine     = endLine
    member x.EndSymbol   = endSymbol
    

[<AbstractClass>]
type Node(pos : Position) =
    member x.Position    = pos
    member x.StartLine   = pos.StartLine
    member x.StartSymbol = pos.StartSymbol
    member x.EndLine     = pos.EndLine
    member x.EndSymbol   = pos.EndSymbol