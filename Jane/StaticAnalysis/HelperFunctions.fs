module internal SA.HelperFunctions

open AST
open SA.Errors

// Добавляет по ID списку ошибки дублирования узлов в program
let DuplicateNode (program : Program) (listNodes : ID list) (kindNode : string) =
    
    // В отсортированном списке ищет повторы и добавляет ошибки
    let rec searchDuplicate (listNodes : ID list) (prev : ID) =
        match listNodes with
            | node :: lns when node.Value = prev.Value -> program.AddError <| Error.DuplicateNode kindNode node
                                                          searchDuplicate lns node
            | node :: lns                              -> searchDuplicate lns node
            | []                                       -> ()

    // Сортирует список по именам
    let sortedListNodes = List.sortBy (fun (node : ID) -> node.Value) listNodes

    // Проверяет сотсортированный список на пустоту и начинает поиск повторов
    match sortedListNodes with
    | node :: lns -> searchDuplicate lns node
    | []          -> ()

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Формирует ошибки корректности идентификатора
let IncorrectNameErrors (program : Program) (name : string) (p : Position) =
    
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
