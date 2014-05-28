module internal SA.Expression

// SA ~ Static Analysis

open AST
open SA.Errors

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let private position   = new Position(0,0,0,0)
let private boolType   = new BooleanType(0, position) :> Type
let private intType    = new IntType(0, position)     :> Type
let private floatType  = new FloatType(0, position)   :> Type
let private charType   = new CharType(0, position)    :> Type
let private stringType = new StringType(0, position)  :> Type

let rec SA_Expression (p : Program) (expr : Expression) (expectedTypes : Type list) (context : Variable list) =
    
    let ErrorCreator () = p.AddError <| Error.ExpectedTypes expectedTypes expr
    let NextExpression expr expectedTypes = SA_Expression p expr expectedTypes context

    let isBadExpr f = List.forall f expectedTypes
    
    match expr with
    | :? CharLiteral     as l  -> let isBad = isBadExpr ((<>) charType)
                                  if isBad then ErrorCreator ()
    | :? StringLiteral   as l  -> let isBad = isBadExpr ((<>) stringType)
                                  if isBad then ErrorCreator ()
    | :? IntegerLiteral  as l  -> let isBad = isBadExpr ((<>) intType)
                                  if isBad then ErrorCreator ()
    | :? BooleanLiteral  as l  -> let isBad = isBadExpr ((<>) boolType)
                                  if isBad then ErrorCreator ()
    | :? FloatLiteral    as l  -> let isBad = isBadExpr ((<>) floatType)
                                  if isBad then ErrorCreator ()
    | :? NullLiteral     as l  -> let isBad = isBadExpr (fun (t : Type) -> t = charType || t = intType || t = boolType || t = floatType)
                                  if isBad then p.AddError <| Error.ExpectedValueType l
  
    | :? UnaryOperation  as uo -> 
        match uo.Operator with
        | NOT   -> let isBad = isBadExpr ((<>) boolType)
                   if isBad then ErrorCreator ()
                   else NextExpression uo.Operand [boolType]
        | MINUS -> let isBad = isBadExpr (fun (t : Type) -> t = intType || t = floatType)
                   if isBad then ErrorCreator ()
                   else NextExpression uo.Operand [intType; floatType] 
          
    | :? BinaryOperation as bo ->
        match bo.Operator with
        | ADDITION -> 
            let isBad1 = isBadExpr (fun (t : Type) -> t <> intType && t <> floatType)
            let isBad2 = isBadExpr ((<>) stringType)
            if isBad1 && isBad2 then ErrorCreator ()
            elif isBad2 then NextExpression bo.FirstOperand  [intType; floatType] 
                             NextExpression bo.SecondOperand [intType; floatType]
            else             NextExpression bo.FirstOperand  [stringType] 
                             NextExpression bo.SecondOperand [stringType]
        | op when op = SUBSTRACTION || op = MULTIPLICATION || op = DIVISION || op = MODULUS ->
            let isBad = isBadExpr (fun (t : Type) -> t <> intType && t <> floatType)
            if isBad then ErrorCreator ()
            else NextExpression bo.FirstOperand  [intType; floatType] 
                 NextExpression bo.SecondOperand [intType; floatType] 
        
        | op when op = EQUAL || op = NOT_EQUAL || op = GREATER || op = GREATER_OR_EQUAL || op = LESS || op = LESS_OR_EQUAL ->
           let isBad = isBadExpr ((<>) boolType)
           if isBad then ErrorCreator ()
           else NextExpression bo.FirstOperand  [intType; floatType]
                NextExpression bo.SecondOperand [intType; floatType]  
        
        | op when op = AND || op = OR ->
            let isBad = isBadExpr ((<>) boolType)
            if isBad then ErrorCreator ()
            else NextExpression bo.FirstOperand  [boolType]
                 NextExpression bo.SecondOperand [boolType]
        
        | MEMBER_CALL -> () // Заглушка
        |_ -> ()
         
    | :? Identifier      as i  -> let var = context |> List.tryFind(fun v -> v.Name = i.Name.Value)
                                  if var.IsNone then p.AddError <| Error.ObjectIsNotExist i.Name "Variable"
                                  
    | :? InstanceOf      as io -> () // Заглушка
    | :? Constructor     as c  -> () // Заглушка
    | :? Member          as m  -> () // Заглушка
    | _                        -> ()
