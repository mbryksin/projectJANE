namespace AST

type Suffix =
    interface
    end

type Arguments(arguments : Expression list) =
    interface Suffix
    member x.Arguments = arguments

type ArrayElement(index : Expression) =
    interface Suffix
    member x.Index = index