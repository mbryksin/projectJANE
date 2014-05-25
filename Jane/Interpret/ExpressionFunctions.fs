module ExpressionFunctions

open AST

let compareOperation (operand1 : Val, operand2: Val, op : System.IComparable -> System.IComparable -> bool) = 
    match operand1, operand2 with
    | Bool log1, Bool log2     -> Bool((op) log1 log2)
    | Int num1, Int num2       -> Bool((op) num1 num2)
    | Float num1, Float num2   -> Bool((op) num1 num2)
    | Char char1, Char char2   -> Bool((op) char1 char2)
    | Str str1, Str str2       -> Bool((op) str1 str2)
    | Null, Null               -> Bool(true)
    | _                        -> Bool(false)

let logicalOperation (operand1 : Val, operand2: Val, op : bool -> bool -> bool) = 
    match operand1, operand2 with
    | Bool log1, Bool log2     -> Bool((op) log1 log2)
    | _                        -> Empty

let addition firstOpetandVal secondOpetandVal = 
    match firstOpetandVal, secondOpetandVal with
    | Int intnum1, Int intnum2         -> Int(intnum1 + intnum2)
    | Float floatnum1, Float floatnum2 -> Float(floatnum1 + floatnum2)
    | Str str1, Str str2               -> Str (str1 + str2)
    | _                                -> Empty

let substraction firstOpetandVal secondOpetandVal = 
    match firstOpetandVal, secondOpetandVal with
    | Int intnum1, Int intnum2         -> Int(intnum1 - intnum2)
    | Float floatnum1, Float floatnum2 -> Float(floatnum1 - floatnum2)
    | _                                -> Empty

let multyplication firstOpetandVal secondOpetandVal = 
    match firstOpetandVal, secondOpetandVal with
    | Int intnum1, Int intnum2         -> Int(intnum1 * intnum2)
    | Float floatnum1, Float floatnum2 -> Float(floatnum1 * floatnum2)
    | _                                -> Empty

let division firstOpetandVal secondOpetandVal position = 
    match firstOpetandVal, secondOpetandVal with
    | Int intnum1, Int intnum2 when intnum2 <> 0L -> Int(intnum1 / intnum2)
    | Float floatnum1, Float floatnum2 when floatnum2 <> 0.0 -> Float(floatnum1 / floatnum2)
    | _                                -> Err ("Division by zero", position)

let modul firstOpetandVal secondOpetandVal = 
    match firstOpetandVal, secondOpetandVal with
    | Int intnum1, Int intnum2         -> Int(intnum1 % intnum2)
    | Float floatnum1, Float floatnum2 -> Float(floatnum1 % floatnum2)
    | _                                -> Empty

let notLogical operandVal =
    match operandVal with
    | Bool log -> Bool (not log)
    | _        -> Empty // error

let minus operandVal =
    match operandVal with
    | Int   number -> Int(- number)
    | Float number -> Float(-number)
    | _        -> Empty // error 