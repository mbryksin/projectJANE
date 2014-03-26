namespace AST 

type Program(programMembers : ProgramMember list) =
    member x.ProgramMembers = programMembers
