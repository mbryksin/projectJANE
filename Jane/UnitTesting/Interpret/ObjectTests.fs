namespace ObjectTest


open FsUnit
open NUnit.Framework
open SA.Program
open LanguageParser
open Interpret
open SupportFunction

[<TestFixture>]
type TestingObject() =
    
    member x.getResult (textProgram : string)=
        let program = ParseProgram textProgram
        program.NameMainClass <- "mainClass"  
        SA_Program program
        interpretProgram program 
    [<Test>]
    member x. ``Interpret: Create Object`` ()=
        let programText ="
            class Number 
            {
                Number(int x)
                {
                    val = x;
                }
                int val = 0;
            }

            class mainClass 
            {
                static void main() 
                {
                    Number num = new Number(5);
                    Console.Writeline(num.val);
                }
            }"
        getResult(programText) = "5"  |> should be True
    
    [<Test>]
    member x. ``Interpret: method call`` ()=
        let programText ="
            class Number 
            {
                Number(int x)
                {
                    val = x;
                }
                int val = 0;
                int getVal()
                {
                    return val;
                }
            }

            class mainClass 
            {
                static void main() 
                {
                    Number num = new Number(5);
                    Console.Writeline(num.getVal());
                }
            }"
        getResult(programText) = "5"  |> should be True

    [<Test>]
    member x. ``Interpret: static method call`` ()=
        let programText ="
            class Number 
            {
                Number(int x)
                {
                    val = x;
                }
                int val = 0;
                int getVal()
                {
                    return val;
                }
                static int Plus(Number n, Number k)
                {
                    return new Number(n.getVal() + k.getVal());
                }
            }

            class mainClass 
            {
                static void main() 
                {
                    Number num1 = new Number(5);
                    Number num2 = new Number(1);
                    Number Result = Number.Plus(num1, num2);
                    Console.Writeline(Result.getVal());
                }
            }"
        getResult(programText) = "6"  |> should be True

    [<Test>]
    member x. ``Interpret: static field call`` ()=
        let programText ="
            class Number 
            {
                Number(int x)
                {
                    val = x;
                }
                int val = 0;
                static int pi = 3.14;
                int getVal()
                {
                    return val;
                }
                static int Plus(Number n, Number k)
                {
                    return new Number(n.getVal() + k.getVal());
                }
            }

            class mainClass 
            {
                static void main() 
                {
                    Console.Writeline(Number.pi);
                }
            }"
        getResult(programText) = "3.14"  |> should be True

