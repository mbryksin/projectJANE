namespace AST

[<AbstractClass>]
type ProgramMember(name : string, pos : Position) =
    inherit Node(pos)
    member x.Name = name 

type Interface(name : string, ancestors : string list, members : InterfaceMember list, pos : Position) =
    inherit ProgramMember(name, pos)
    member x.Ancestors = ancestors
    member x.Members   = members

type Class(name : string, ancestor : string option, interfaces : string list, 
           classConstructor : ClassConstructor, members : ClassMember list, pos : Position) =
    inherit ProgramMember(name, pos)
    member x.Ancestor    = ancestor
    member x.Interfaces  = interfaces
    member x.Members     = members
    member x.Constructor = classConstructor
