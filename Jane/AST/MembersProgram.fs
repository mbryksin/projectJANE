namespace AST

[<AbstractClass>]
type ProgramMember(name : string, pos : Position) =
    inherit Node(pos)
    member x.Name = name 

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Interface(name : string, ancestors : string list, members : InterfaceMember list, pos : Position) =
    inherit ProgramMember(name, pos)
    
    let fields        = List.fold (fun acc (m : InterfaceMember) ->
                                       try m :?> InterfaceField :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] members

    let methods       = List.fold (fun acc (m : InterfaceMember) ->
                                       try m :?> InterfaceMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] members 
    
    let returnMethods = List.fold (fun acc (m : InterfaceMethod) ->
                                       try m :?> InterfaceReturnMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] methods 
 
    let voidMethods   = List.fold (fun acc (m : InterfaceMethod) ->
                                       try m :?> InterfaceVoidMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] methods 

    member x.Ancestors     = ancestors
    member x.Members       = members  
    member x.Fields        = fields
    member x.Methods       = methods
    member x.ReturnMethods = returnMethods
    member x.VoidMethods   = voidMethods

    override x.ToString() =
        let extendsStr   = if not ancestors.IsEmpty then " extends " else ""
        let ancestorsStr = String.concat ", " ancestors
        let membersStr   = members |> List.map string |> String.concat "\n\n"
        sprintf "interface %s%s%s {\n\n%s\n\n}\n" name extendsStr ancestorsStr membersStr

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Class(name : string, ancestor : string option, interfaces : string list, 
           classConstructor : ClassConstructor, members : ClassMember list, pos : Position) =
    inherit ProgramMember(name, pos)
    
    let fields        = List.fold (fun acc (m : ClassMember) ->
                                       try m :?> ClassField :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] members

    let methods       = List.fold (fun acc (m : ClassMember) ->
                                       try m :?> ClassMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] members 
    
    let returnMethods = List.fold (fun acc (m : ClassMethod) ->
                                       try m :?> ClassReturnMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] methods 
 
    let voidMethods   = List.fold (fun acc (m : ClassMethod) ->
                                       try m :?> ClassVoidMethod :: acc 
                                       with :? System.InvalidCastException -> acc
                                  ) [] methods 
    
    member x.Ancestor    = ancestor
    member x.Interfaces  = interfaces
    member x.Members     = members
    member x.Fields      = fields
    member x.Methods       = methods
    member x.ReturnMethods = returnMethods
    member x.VoidMethods   = voidMethods
    member x.Constructor = classConstructor

    override x.ToString() =
        let extendsStr    = if ancestor.IsSome then sprintf " extends %s" ancestor.Value else ""
        let implementsStr = if not interfaces.IsEmpty then " implements " else ""
        let interfacesStr = String.concat ", " interfaces
        let membersStr   = members |> List.map string |> String.concat "\n"
        sprintf "class %s%s%s%s {\n\n%A\n%s\n}\n" name extendsStr implementsStr interfacesStr classConstructor membersStr
