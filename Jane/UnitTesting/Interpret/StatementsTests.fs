module StatementsTests


open FsUnit
open NUnit.Framework
open SA
open LanguageParser
open Interpret
open SupportFunction

[<TestFixture>]
type TestingStatements() =
    
    member x.getResult (textProgram : string)=
        let program = ParseProgram textProgram
        program.NameMainClass <- "mainClass"  
        StaticAnalysis.Analyze program
        interpretProgram program 
    [<Test>]
        member x. `` Interpret:Simple For`` ()=
            let programText ="
                class mainClass 
                {
                    static void main() 
                    {
                        for(int i = 1; i < 5; i = i + 1)
                        {
                            Console.writeLine(i);
                        }
                    }
                }"            
            getResult(programText) = "1234"  |> should be True

        [<Test>]
        member x. `` Interpret: Simple if`` ()=
            let programText ="
                class mainClass 
                {
                    static void main() 
                    {
                        int i = 1;
                        if (i > 0)
                        {
                            Console.writeLine(i);
                        }
                    }
                }"            
            getResult(programText) = "1"  |> should be True

        [<Test>]
        member x. ``Interpret: Simple While`` ()=
            let programText ="
                class mainClass 
                {
                    static void main() 
                    {
                        int i = 1;
                        while(i < 5)
                        {
                            Console.writeLine(i);
                            i = i + 1;
                        }
                    }
                }"            
            getResult(programText) = "1234"  |> should be True

     [<Test>]
    member x. ``Interpret: If in for`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    for(int i = 1; i < 5; i = i + 1)
                    {
                        if (i > 3)
                        {
                            Console.writeLine(i);
                        }
                    }
                }
            }"
        getResult(programText) = "4"  |> should be True

    [<Test>]
    member x. ``Interpret: If in while`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int i = 1;
                    while(i < 5)
                    {
                        if (i > 3)
                        {
                            Console.writeLine(i);
                        }
                        i = i + 1;
                    }
                }
            }"
        getResult(programText) = "4"  |> should be True

    [<Test>]
    member x. ``Interpret:Else in for`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int i = 1;
                    if (i >5)
                    {
                        Console.writeLine(\"false\");
                    }
                    else
                    {
                        Console.writeLine(\"true\");
                    }
                }
            }"
        getResult(programText) = "true"  |> should be True

    [<Test>]
    member x. ``Interpret: big loop`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    for (int i = 1; i < 3; i = i + 1)
                    {
                        for (int j = 1; j < 3; j = j + 1)
                        {
                            for (int k = 1; k < 3; k = k + 1)
                            {
                                Console.writeLine(i+j+k);
                            }
                        }
                    }
                }
            }"
        getResult(programText) = "34454556"  |> should be True

    [<Test>]
    member x. ``Interpret: Break from while`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int i = 1;
                    while(i < 5)
                    {                    
                        if (i > 3)
                        {
                            break;
                        }
                        Console.writeLine(i);
                        i = i + 1;
                    }
                }
            }"
        getResult(programText) = "123"  |> should be True

    [<Test>]
    member x. ``Interpret: Continue`` ()=
        let programText ="
            class mainClass 
            {
                 static void main() 
                 {
                      for (int i = 1; i < 5; i = i + 1)
                      {
                          if (i == 3) continue;
		                  Console.writeLine(i);            
                      }
                 }
            }"
        getResult(programText) = "124"  |> should be True