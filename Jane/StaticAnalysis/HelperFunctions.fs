module internal SA.HelperFunctions

open AST
open SA.Errors

// Формирует ошибки корректности идентификатора
let IncorrectName (p : Program) (name : ID) (typeObject : string) =
    
    // Список запрещённых идентификаторов
    let listIncorrectNames = ["class"; "interface"; "extends"; "implements"; "static"; "final"; "if"; "else"; 
                              "while"; "for"; "break"; "continue"; "return"; "super"; "this"; "null"; "new";
                              "byte"; "short"; "int"; "long"; "float"; "double"; "char"; "string"; "boolean";
                              "false"; "true"; "instanceOf"; "void"]

    // Проверка на корректность
    let incorrectName = List.exists ((=) name.Value) listIncorrectNames

    // Добавление ошибки
    if incorrectName then p.AddError <| Error.IncorrectName name typeObject
