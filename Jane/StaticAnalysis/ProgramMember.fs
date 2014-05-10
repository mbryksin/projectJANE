module internal SA.ProgramMember

open AST
open SA.HelperFunctions
open SA.Interface
open SA.Class

let SA_ProgramMember (program : Program) (pm : ProgramMember) =
    
    // Корректное ли название
    IncorrectNameErrors program pm.Name.Value pm.Position

    // Поиск ошибкок в нижних уровнях
    match pm with
    | :? Class     as c -> SA_Class     program c
    | :? Interface as i -> SA_Interface program i
    | _                 -> ()