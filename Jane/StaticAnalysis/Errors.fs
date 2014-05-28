module internal SA.Errors

open System

open AST

// Дополнение для класса Error из AST    
type Error with

    static member DuplicateNode (typeNode : string) (name : ID) = 
        new Error(sprintf "Duplicate %s: \"%A\"." typeNode name, name.Position)

    static member MainClassNotFound (p : Program) =
        new Error(sprintf "Main class \"%s\" is not found." p.NameMainClass, p.Position)

    static member MainNotFound (c : Class) =
        new Error(sprintf "Method \"main\" is not found in the main class \"%A\"." c.Name, c.Position)

    static member MainIsNotStatic (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must be static.", cn.Position)

    static member MainContaintsArgs (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must not contain arguments.", cn.Position)

    static member MainIsNotVoid (cn : ClassMethod) =
        new Error(sprintf "Method \"main\" must be void method.", cn.Position)

    static member IncorrectName (name : ID) (typeObject : string) =
        new Error(sprintf "Incorrect identifier of %s: \"%A\"." typeObject name, name.Position)

    static member ObjectIsNotExist (name : ID) (typeObject : string) =
        new Error (sprintf "%s with name \"%A\" not exists." typeObject name, name.Position)

    static member InterfaceMemberIsNotImplemented (im : InterfaceMember) (i : Interface) (c : Class) =
        new Error (sprintf "In class \"%A\" not implement member \"%A\" of interface \"%A\"." c.Name im.Name i.Name, c.Position)