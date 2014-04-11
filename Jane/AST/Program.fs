namespace AST

open System 

type Program(programMembers : ProgramMember list, nameMainClass : string, pos : Position) =
    inherit Node(pos)
    
    let classes    = List.fold (fun acc (m : ProgramMember) -> 
                                    try m :?> Class :: acc 
                                    with :? InvalidCastException -> acc
                               ) [] programMembers

    let interfaces = List.fold (fun acc (m : ProgramMember) -> 
                                    try m :?> Interface :: acc 
                                    with :? InvalidCastException -> acc
                               ) [] programMembers

    let mutable mainClass  : Class option       = None
    let mutable mainMethod : ClassMethod option = None
    let mutable errors     : Error list         = []

    member x.ProgramMembers = programMembers
    member x.NameMainClass  = nameMainClass
    member x.Classes        = classes
    member x.Interfaces     = interfaces
    
    member x.MainClass with get()         = mainClass
                       and  set(newClass) = mainClass <- newClass

    member x.MainMethod  with get()          = mainMethod
                         and  set(newMethod) = mainMethod <- newMethod

    member x.AddErrors newErrors = errors <- newErrors @ errors
    member x.AddError  newError  = errors <- newError :: errors

    override x.ToString() = programMembers
                            |> List.map string
                            |> String.concat "\n\n" 
