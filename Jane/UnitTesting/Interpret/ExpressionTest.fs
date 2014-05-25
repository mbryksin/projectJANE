module ExpressionTest

open FsUnit
open NUnit.Framework
open SA
open LanguageParser
open Interpret
open SupportFunction

[<TestFixture>]
type TestingExpression() =
    
    member x.getResult (textProgram : string)=
        let program = ParseProgram textProgram
        program.NameMainClass <- "mainClass"  
        StaticAnalysis.Analyze program
        interpretProgram program 
    [<Test>]
    member x. ``Interpret: Hard expression`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int i = 1 + 2*3 - 4 +(5*4*0);
                    Console.Writeline(i);
                }
            }"
        getResult(programText) = "3"  |> should be True

    [<Test>]
    member x. ``Interpret: Literals, bool`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    bool f = true;
                    Console.Writeline(f);
                }
            }"
        getResult(programText) = "True"  |> should be True

    [<Test>]
    member x. ``Interpret: Literals, float`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    bool f = 1.45;
                    Console.Writeline(f);
                }
            }"
        getResult(programText) = "1,45"  |> should be True

    [<Test>]
    member x. ``Interpret: Literals, null`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    bool f = null;
                    Console.Writeline(f);
                }
            }"
        getResult(programText) = "null"  |> should be True

    [<Test>]
    member x. ``Interpret: Literals, char`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    char f = 'g';
                    Console.Writeline(f);
                }
            }"
        getResult(programText) = "g"  |> should be True


