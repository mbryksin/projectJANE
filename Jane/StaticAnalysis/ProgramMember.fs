module internal SA.ProgramMember

open AST
open SA.HelperFunctions
open SA.Interface
open SA.Class

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_ProgramMember (p : Program) (pm : ProgramMember) =

    // Сбор данных на нижних уровнях + проверка корректности названий
    match pm with
    | :? Class     as c -> IncorrectName p c.Name "class"
                           GD_Class     p c
    | :? Interface as i -> IncorrectName p i.Name "interface"
                           GD_Interface p i
    | _                 -> ()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_ProgramMember (p : Program) (pm : ProgramMember) =
    
    // Поиск ошибкок в нижних уровнях
    match pm with
    | :? Class     as c -> SA_Class     p c
    | :? Interface as i -> SA_Interface p i
    | _                 -> ()