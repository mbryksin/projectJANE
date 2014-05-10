namespace JaneRuntime

open AST
open System

type writeLineStatement(pos: Position) =
    inherit Statement(pos)
    override x.ToString() = "writeLineRuntimeCall"

    override x.Interpret() = Console.WriteLine("")
type Runtime() = 

    static member getRuntimeLibrary : ProgramMember list = 
        let dummyPosition = new Position(0, 0, 0, 0)
        //class Console
        let ConsoleClass : AST.Class = 
            //static void writeLine(String str);
            let writeLineMethod = 
                new ClassVoidMethod(true,
                    new ID("writeLine", dummyPosition),
                    [new FormalParameter(new StringType(1, dummyPosition), new ID("str", dummyPosition), dummyPosition)],
                    new Block([new writeLineStatement(dummyPosition)], dummyPosition),
                    dummyPosition)
            new Class(new ID("Console", dummyPosition), None, [], None, [writeLineMethod], dummyPosition)
        [ConsoleClass]
        
