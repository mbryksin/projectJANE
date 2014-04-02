namespace AST 

type Program(programMembers : ProgramMember list) =
    member x.ProgramMembers = programMembers
    //interpret the main method in the class that contains it
    //mainclass sholud be in head of ProgramMembers, mainMethod sholud be in head of Members.
    member x.Interpret() =
        let classWithMain = x.ProgramMembers.Head :?> Class
        let mainMethod = classWithMain.Members.Head :?> ClassVoidMethod
        mainMethod.Interpret()
