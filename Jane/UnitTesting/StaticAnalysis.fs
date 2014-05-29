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

    [<Test>]
    member x.``Не реализован член интерфейса 1`` ()=
        x.MainBadTest
            "interface A {int r();} class B implements A {}"
            "In class \"B\" not implement member \"r\" of interface \"A\"."

    [<Test>]
    member x.``Не реализован член интерфейса 2`` ()=
        x.MainBadTest
            "interface A {int r();} class B implements A {int r = 5;}"
            "In class \"B\" not implement member \"r\" of interface \"A\"."

    [<Test>]
    member x.``Не реализован член интерфейса 3`` ()=
        x.MainBadTest
            "interface A {int r();} class B implements A {int r(int a) {}}"
            "In class \"B\" not implement member \"r\" of interface \"A\"."

    [<Test>]
    member x.``Не реализован член интерфейса 4`` ()=
        x.MainBadTest
            "interface A {int r();} class B implements A {void r() {}}"
            "In class \"B\" not implement member \"r\" of interface \"A\"."

    [<Test>]
    member x.``Некорректное переопределение 1`` ()=
        x.MainBadTest
            "class A {int r(float t) {}} class B extends A {void r() {}}"
            "Duplicate class member: \"r\"."

    [<Test>]
    member x.``Некорректное переопределение 2`` ()=
        x.MainBadTest
            "class A {int r(float t) {}} class B extends A {void r(float y) {}}"
            "Duplicate class member: \"r\"."

    [<Test>]
    member x.``Некорректное переопределение 3`` ()=
        x.MainBadTest
            "class A {int r(float t) {}} class B extends A {int r(char y) {}}"
            "Duplicate class member: \"r\"."

    [<Test>]
    member x.``Некорректное переопределение 4`` ()=
        x.MainBadTest
            "class A {int r(float t) {}} class B extends A {int r(float y, float z) {}}"
            "Duplicate class member: \"r\"."

    [<Test>]
    member x.``Объект несуществующего типа 1`` ()=
        x.MainBadTest
            "class A {assdfsdf r(float t) {}}"
            "Type with name \"assdfsdf\" not exists."

    [<Test>]
    member x.``Объект несуществующего типа 2`` ()=
        x.MainBadTest
            "class A {assdfsdf r = 1;}"
            "Type with name \"assdfsdf\" not exists."

    [<Test>]
    member x.``Объект несуществующего типа 3`` ()=
        x.MainBadTest
            "class A {int r(bool a) {}}"
            "Type with name \"bool\" not exists."

    [<Test>]
    member x.``Объект несуществующего типа 4`` ()=
        x.MainBadTest
            "class A {int r(int i, bool a) {}}"
            "Type with name \"bool\" not exists."

    [<Test>]
    member x.``Имя конструктора не совпадает с именем класса`` () =
        x.MainBadTest
            "class A {R() {}}"
            "Incorrect identifier of constructor: \"R\"."

    [<Test>]
    member x.``Ожидается тип значения`` () =
        x.MainBadTest
            "class A {int a = null;}"
            "Expected value type."

    [<Test>]
    member x.``Тип поля не совпaдает с типом выражения 1`` () =
        x.MainBadTest
            "class A {int a = '1';}"
            "Expected types: \"int\", \"float\"."

    [<Test>]
    member x.``Тип поля не совпaдает с типом выражения 2`` () =
        x.MainBadTest
            "class A {string a = \"123\" + 1;}"
            "Expected types: \"string\"."

    [<Test>]
    member x.``Тип поля не совпaдает с типом выражения 3`` () =
        x.MainBadTest
            "class A {boolean a = 2;}"
            "Expected types: \"boolean\"."

    [<Test>]
    member x.``Тип поля не совпaдает с типом выражения 4`` () =
        x.MainBadTest
            "class A {int a = 1 + 1 + 1 + 1 + '1' + 1 + 1 + 1 + 1;}"
            "Expected types: \"int\", \"float\"."

    [<Test>]
    member x.``Ожидается один из типов`` () =
        x.MainBadTest
            "interface A {}
             interface B extends A {}
             class C implements B { C x = 2;}"
            "Expected types: \"C\", \"B\", \"A\"."

    [<Test>]
    member x.``Переменная не существует 1`` () =
        x.MainBadTest
            "class C { int a = 55 + b;}"
            "Variable with name \"b\" not exists."

    [<Test>]
    member x.``Переменная не существует 2`` () =
        x.MainBadTest
            "class C { int a = a;}"
            "Variable with name \"a\" not exists."

