namespace AST

type Initializer =
    interface
        abstract member Interpret: unit -> Val
    end

type ArrayInitializer(elements : Initializer list) =
    member x.Elements = elements
    interface Initializer with
        member x.Interpret()= new Val() // later

