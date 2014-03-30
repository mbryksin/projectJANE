namespace AST

type Initializer =
    interface
    end

type ArrayInitializer(elements : Initializer list) =
    interface Initializer
    member x.Elements = elements
