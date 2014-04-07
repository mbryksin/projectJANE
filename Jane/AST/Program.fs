namespace AST 

type Program(programMembers : ProgramMember list, nameMainClass : string, pos : Position) =
    inherit Node(pos)
    member x.ProgramMembers = programMembers
    member x.NameMainClass  = nameMainClass

    override x.ToString() = programMembers
                            |> List.map string
                            |> String.concat "\n\n" 
