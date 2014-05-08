namespace UnitTesting

open FsUnit
open NUnit.Framework
open AST
open SA.Program
open LanguageParser

[<TestFixture>]
type TestingStaticAnalysis() =
    
    let existsError (program : Program) (errorMessage : string) =
        List.exists (fun (a : Error) -> a.ErrorMessage = errorMessage) program.Errors

    member x.MainTest textOfProgram textOfError result =
        let program = ParseProgram textOfProgram
        program.NameMainClass <- "MainClass"
        SA_Program program
        existsError program textOfError |> should be result
        
    member x.MainBadTest textOfProgram textOfError = 
        x.MainTest textOfProgram textOfError True

    member x.MainGoodTest textOfProgram textOfError = 
        x.MainTest textOfProgram textOfError False
    
    [<Test>]
    member x. ``Отсутствует главный класс`` ()=
        x.MainBadTest 
            "interface a {}" 
            "Main class \"MainClass\" is not found."

    [<Test>]
    member x. ``Отсутствует main в главном классе`` ()=
        x.MainBadTest 
            "class MainClass {}" 
            "Method \"main\" is not found in the main class \"MainClass\"."

    [<Test>]
    member x. ``main не static-метод`` ()=
        x.MainBadTest 
            "class MainClass {void main() {}}" 
            "Method \"main\" must be static."

    [<Test>]
    member x. ``main содержит аргументы`` ()=
        x.MainBadTest 
            "class MainClass {static void main(int a, string b) {}}" 
            "Method \"main\" must not contain arguments."

    [<Test>]
    member x. ``main не void-метод`` ()=
        x.MainBadTest 
            "class MainClass {static int main() {}}" 
            "Method \"main\" must be void method."

    [<Test>]
    member x. `` main - правильный`` ()=
        x.MainGoodTest 
            "class MainClass {static void main() {}}" 
            "Method \"main\" must be void method."

    [<Test>]
    member x. ``Название типа - ключевое слово`` ()=
        x.MainBadTest 
            "class boolean {}" 
            "Incorrect identifier: \"boolean\"."

    [<Test>]
    member x.``Дублирование названий классов`` ()=
        x.MainBadTest
            "interface Name {} class Name {}"
            "Duplicate class: \"Name\"."

    [<Test>]
    member x.``Дублирование названий наследуемых интерфейсов`` ()=
        x.MainBadTest
            "interface Name {} interface A extends Name, C, Name {}"
            "Duplicate extend interface: \"Name\"."

    [<Test>]
    member x.``Дублирование названий членов интерфейса`` ()=
        x.MainBadTest
            "interface A {int Name(); static void Name(int b);}"
            "Duplicate interface member: \"Name\"."

    [<Test>]
    member x.``Дублирование названий членов класса`` ()=
        x.MainBadTest
            "class A {int Name = 1; static string Name(boolean r) {}}"
            "Duplicate class member: \"Name\"."

    [<Test>]
    member x.``Отсутствует наследуемый интерфейс`` ()=
        x.MainBadTest
            "interface A extends Name {}"
            "Interface with name \"Name\" not exists."

