namespace UnitTesting

open FsUnit
open NUnit.Framework
open AST
open SA
open LanguageParser

[<TestFixture>]
type TestingStaticAnalysis() =
    
    let existsError (program : Program) (errorMessage : string) =
        List.exists (fun (a : Error) -> a.ErrorMessage = errorMessage) program.Errors

    member x.MainTest textOfProgram textOfError result =
        let program = ParseProgram textOfProgram
        program.NameMainClass <- "MainClass"
        StaticAnalysis.Analyze program
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
    member x. ``Название интерфейса - ключевое слово`` ()=
        x.MainBadTest 
            "interface string {}" 
            "Incorrect identifier of interface: \"string\"."
    [<Test>]
    member x. ``Название класса - ключевое слово`` ()=
        x.MainBadTest 
            "class boolean {}" 
            "Incorrect identifier of class: \"boolean\"."

    [<Test>]
    member x. ``Название метода интерфейса - ключевое слово`` ()=
        x.MainBadTest 
            "interface A {void int();}" 
            "Incorrect identifier of interface member: \"int\"."

    [<Test>]
    member x. ``Название поля класса - ключевое слово`` ()=
        x.MainBadTest 
            "class A {char int = '5';}" 
            "Incorrect identifier of class member: \"int\"."

    [<Test>]
    member x.``Дублирование названий классов`` ()=
        x.MainBadTest
            "interface Name {} class Name {}"
            "Duplicate class: \"Name\"."

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
            "Extend interface with name \"Name\" not exists."

    [<Test>]
    member x.``Отсутствует наследуемый класс`` ()=
        x.MainBadTest
            "class A extends Name {}"
            "Extend class with name \"Name\" not exists."

    [<Test>]
    member x.``Отсутствует реализуемый интерфейс`` ()=
        x.MainBadTest
            "class A implements Name {}"
            "Implement interface with name \"Name\" not exists."
