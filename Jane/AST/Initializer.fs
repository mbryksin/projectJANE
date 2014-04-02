namespace AST

[<AbstractClass>]
type Initializer(pos : Position) =
    inherit Node(pos)

type ArrayInitializer(elements : Initializer list, pos : Position) =
    inherit Initializer(pos)
    member x.Elements = elements
