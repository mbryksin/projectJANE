module Errors

open AST
    

type Error with

    static member DuplicateNode (kindNode : string) (nameNode : string) (positionNode : Position) = 
        new Error(sprintf "Duplicate %s: %s." kindNode nameNode, positionNode)

    static member MainClassNotFound (p : Program) =
        new Error(sprintf "Main class is \"%s\" not found." p.NameMainClass, p.Position)

    static member MainNotFound (c : Class) =
        new Error(sprintf "Method \"main\" is not found in the main class \"%s\"." c.Name.Value, c.Position)

    static member MainIsNotStatic (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must be static.", cn.Position)

    static member MainContaintsArgs (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must not contain arguments.", cn.Position)

    static member MainIsNotVoid (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must be void method.", cn.Position)

    static member IncorrectName (name : string) (position : Position) =
        new Error(sprintf "Incorrect identifier: %s." name, position)

    static member ObjectIsNotExist (name : string) (position : Position) (typeObject : string) =
        new Error (sprintf "%s with name %s not exists." typeObject name, position)

    static member CycleInheritIinterface (id : ID)=
        new Error(sprintf "Interface %s iherits in cycle." id.Value, id.Position)