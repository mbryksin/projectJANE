module StaticAnalysis

open AST
open Errors

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////// ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ //////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Добавляет по списку из имён узлов и их позиций ошибки дублирования узлов в program
let private DuplicateNode (program : Program) (listNodes : (string * Position) list) (kindNode : string) =
    
    // В отсортированном списке ищет повторы и добавляет ошибки
    let rec searchDuplicate (listNodes : (string * Position) list) (prev : string) =
        match listNodes with
            | (name, p) :: lns when name = prev -> program.AddError <| Error.DuplicateNode kindNode name p
                                                   searchDuplicate lns name
            | (name, _) :: lns                  -> searchDuplicate lns name
            | []                                -> ()

    // Сортирует список по именам
    let sortedListNodes = List.sortBy fst listNodes

    // Проверяет сотсортированный список на пустоту и начинает поиск повторов
    match sortedListNodes with
    | (name, _) :: lns -> searchDuplicate lns name
    | []               -> ()

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Формирует ошибки корректности идентификатора
let private IncorrectNameErrors (program : Program) (name : string) (p : Position) =
    
    // Список запрещённых идентификаторов
    let listIncorrectNames = ["class"; "interface"; "extends"; "implements"; "static"; "final"; "if"; "else"; 
                              "while"; "for"; "break"; "continue"; "return"; "super"; "this"; "null"; "new";
                              "byte"; "short"; "int"; "long"; "float"; "double"; "char"; "string"; "boolean";
                              "false"; "true"; "instanceOf"; "void"]

    // Проверка на корректность
    let incorrectName = List.tryFind ((=) name) listIncorrectNames

    // Добавление ошибки
    match incorrectName with
    | Some str -> program.AddError <| Error.IncorrectName str p
    | None     -> ()

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////// АНАЛИЗАТОРЫ //////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_ProgramClass (program : Program) (c : Class) = () // Заглушка

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_ProgramInterface (program : Program) (i : Interface) = 

    // Ошибки дублирования названий членов интерфейса
    DuplicateNode program (List.map (fun (im : InterfaceMember) -> im.Name.Value, im.Position) i.Members) "interface member" 
    
    // Ошибки дублирования названий наследуемых интерфейсов
    DuplicateNode program (List.map (fun (id : ID) -> id.Value, id.Position) i.AncestorsNames) "extends interface"

    // Ошибки корректности названий наследуемых интерфесов
    List.iter (fun (id : ID) -> IncorrectNameErrors program id.Value id.Position) i.AncestorsNames

    // Если интерфейс наследует сам себя - ошибка
    let cicle = List.tryFind (fun (id : ID) -> id.Value = i.Name.Value) i.AncestorsNames in
        if cicle.IsSome then program.AddError <| Error.CycleInheritIinterface cicle.Value

    // Список ссылок на наследуемые интерфейсы + добавление ошибки, если такой интерфей не существует
    let ancestors =         
        // Поиск наследуемого интерфейса + добавление ошибки
        let searchInterface (id : ID) = 
            let interf = List.tryFind (fun (i : Interface) -> i.Name.Value = id.Value) program.Interfaces
            if interf.IsNone then program.AddError <| Error.ObjectIsNotExist id.Value id.Position "Interface"
            interf
        // Поиск всех наследуемых интерфейсов
        List.choose searchInterface i.AncestorsNames 

    // Добавляет ссылки на наследуемые иниерфейсы в interface      
    i.Ancestors <- ancestors

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_ProgramMember (program : Program) (pm : ProgramMember) =
    
    // Корректное ли название
    IncorrectNameErrors program pm.Name.Value pm.Position

    // Поиск ошибкок в нижних уровнях
    match pm with
    | :? Class     as c -> SA_ProgramClass     program c
    | :? Interface as i -> SA_ProgramInterface program i
    | _                 -> ()

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_Program(program : Program) = 
    
    // Ищет mainClass
    let mainClass = List.tryFind (fun (c : Class) -> c.Name.Value = program.NameMainClass) program.Classes

    // Добавляет ссылку на mainClass в program
    program.MainClass <- mainClass

    // Если mainClass найден, то ищет в нём main
    let mainMethod =
        match mainClass with
        | Some mc -> List.tryFind (fun (cm : ClassMethod) -> cm.Name.Value = "main") mc.Methods
        | None    -> None

    // Добавляет ссылку на метод main
    program.MainMethod <- mainMethod

    // Ошибки о дублировании названий классов
    DuplicateNode program (List.map (fun (pm : ProgramMember) -> pm.Name.Value, pm.Position) program.ProgramMembers) "class"   

    // Ошибки, связанные с main
    match mainClass with
    | None    -> program.AddError <| Error.MainClassNotFound program // Отсутствует main класс
    | Some mc -> match mainMethod with       
                    | None    -> program.AddError <| Error.MainNotFound mc // В main классе отсутствует метод main
                    | Some mm -> 
                        // main не void метод
                        if not (mm :? ClassVoidMethod) then program.AddError <| Error.MainIsNotVoid mm
                        // main не статический метод
                        if not mm.IsStatic then program.AddError <| Error.MainIsNotStatic mm
                        // main содержит параметры
                        if mm.Parameters.Length > 0 then program.AddError <| Error.MainContaintsArgs mm

    // Поиск ошибок в нижних уровнях
    List.iter (SA_ProgramMember program) program.ProgramMembers

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
