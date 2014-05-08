namespace AST

open System
open System.Collections.Generic

[<AbstractClass>]
type ProgramMember(name : ID, pos : Position) =
    inherit Node(pos)
    member x.Name = name 

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Interface(name : ID, ancestorNames : ID list, memberList : InterfaceMember list, pos : Position) =
    inherit ProgramMember(name, pos)

    let members          = new Dictionary<string, InterfaceMember>()
    let fields           = new Dictionary<string, InterfaceField>()
    let methods          = new Dictionary<string, InterfaceMethod>()
    let returnMethods    = new Dictionary<string, InterfaceReturnMethod>()
    let voidMethods      = new Dictionary<string, InterfaceVoidMethod>()
    let nearestAncestors = new Dictionary<string, Interface>()
    let allAncestors     = new Dictionary<string, Interface>()
    
    let mutable allancestorNames : ID list = []

    member x.MemberList       = memberList
    member x.AncestorNames    = ancestorNames
    member x.Members          = members  
    member x.Fields           = fields
    member x.Methods          = methods
    member x.ReturnMethods    = returnMethods
    member x.VoidMethods      = voidMethods
    member x.NearestAncestors = nearestAncestors
    member x.AllAncestors     = allAncestors

    member x.AllAncestorNames with get()     = allancestorNames
                               and set(list) = allancestorNames <- list

    override x.ToString() =
        let extendsStr   = if not ancestorNames.IsEmpty then " extends " else ""
        let ancestorsStr = ancestorNames |> List.map string |> String.concat ", "
        let membersStr   = memberList |> List.map string |> String.concat "\n\n"
        sprintf "interface %A%s%s {\n\n%s}\n" name extendsStr ancestorsStr membersStr

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Class(name : ID, ancestorName : ID option, implementInterfacesName : ID list, 
           classConstructor : ClassConstructor option, memberList : ClassMember list, pos : Position) =
    inherit ProgramMember(name, pos)
    
    let members       = new Dictionary<string, ClassMember>()
    let fields        = new Dictionary<string, ClassField>()
    let methods       = new Dictionary<string, ClassMethod>()
    let returnMethods = new Dictionary<string, ClassReturnMethod>()
    let voidMethods   = new Dictionary<string, ClassVoidMethod>()
                       
    let classConstructor = match classConstructor with
                           | Some cc -> cc
                           | None    -> let p = new Position(-1, -1, -1, -1)
                                        new ClassConstructor(name, [], new Block([], p), p)
    
    member x.MemberList              = memberList
    member x.AncestorName            = ancestorName
    member x.ImplementInterfacesName = implementInterfacesName
    member x.Members                 = members
    member x.Fields                  = fields
    member x.Methods                 = methods
    member x.ReturnMethods           = returnMethods
    member x.VoidMethods             = voidMethods
    member x.Constructor             = classConstructor

    override x.ToString() =
        let extendsStr    = if ancestorName.IsSome then sprintf " extends %A" ancestorName.Value else ""
        let implementsStr = if not implementInterfacesName.IsEmpty then " implements " else ""
        let interfacesStr = implementInterfacesName |> List.map string |>  String.concat ", "
        let membersStr    = memberList |> List.map string |> String.concat "\n"
        sprintf "class %A%s%s%s {\n\n%A\n%s\n}\n" name extendsStr implementsStr interfacesStr classConstructor membersStr
