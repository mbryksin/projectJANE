namespace AST 

type Program(programMembers : ProgramMember list, pos : Position) =
    inherit Node(pos)
    member x.ProgramMembers = programMembers

    override x.ToString() = programMembers
                            |> List.map string
                            |> String.concat "\n\n" 
