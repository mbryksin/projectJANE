namespace AST 

type Program(programMembers : ProgramMember list, pos : Position) =
    inherit Node(pos)
    member x.ProgramMembers = programMembers
