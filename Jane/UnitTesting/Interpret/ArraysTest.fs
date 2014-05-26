module ArrayTest


open FsUnit
open NUnit.Framework
open SA
open LanguageParser
open Interpret
open SupportFunction

[<TestFixture>]
type TestingArrays() =
    
    member x.getResult (textProgram : string)=
        let program = ParseProgram textProgram
        program.NameMainClass <- "mainClass"  
        StaticAnalysis.Analyze program
        interpretProgram program 
    [<Test>]
    member x. ``Interpret: ArrayCreate`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int[] array = {1,2,3};
                    Console.writeLine(array);
                }
            }"
        getResult(programText) = "{1 2 3 }"  |> should be True

    [<Test>]
    member x. ``Interpret: Array Index Call`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int[] array = {1,2,3};
                    Console.writeLine(array[0]);
                }
            }"
        getResult(programText) = "1"  |> should be True

    [<Test>]
    member x. ``Interpret: Array Double Index`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int[] array = {{1,2,3}, {4,5,6}};
                    Console.writeLine(array[0]);
                }
            }"
        getResult(programText) = "{1 2 3 }"  |> should be True

    [<Test>]
    member x. ``Interpret: Array, Expression in index`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    int j = 1;
                    int[] array = {{1,2,3}, {4,5,6}};
                    Console.writeLine(array[6-5*1 -j]);
                }
            }"
        getResult(programText) = "{1 2 3 }"  |> should be True

    [<Test>]
    member x. ``Interpret: String, get index`` ()=
        let programText ="
            class mainClass 
            {
                static void main() 
                {
                    string s = \"lalka\";
                    Console.writeLine(s[1]);
                }
            }"
        getResult(programText) = "a"  |> should be True