namespace AST

[<AbstractClass>]
type ProgramMember(name : string) =
    member x.Name = name 

type Interface(name : string, ancestors : string list, members : InterfaceMember list) =
    inherit ProgramMember(name)
    member x.Ancestors = ancestors
    member x.Members   = members

type Class(name : string, ancestor : string option, interfaces : string list, classConstructor : ClassConstructor, members : ClassMember list) =
    inherit ProgramMember(name)
    member x.Ancestor    = ancestor
    member x.Interfaces  = interfaces
    member x.Members     = members
    member x.Constructor = classConstructor
