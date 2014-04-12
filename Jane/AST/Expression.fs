namespace AST

open System

[<AbstractClass>]
type Expression(pos : Position) =
    inherit Initializer(pos)


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type InstanceOf(expression : Expression, controlType : Type, pos : Position) =
    inherit Expression(pos)
    member x.Expression  = expression
    member x.ControlType = controlType

    override x.ToString() = sprintf "(%A instanceOf %A)" expression controlType

    override x.Interpret(context : Variable list) = Empty //later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BinaryOperation(firstOperand : Expression, operator : BinaryOperator, secondOperand : Expression, pos : Position) =
    inherit Expression(pos)
    member x.FirstOperand  = firstOperand
    member x.Operator      = operator
    member x.SecondOperand = secondOperand

    override x.ToString() = sprintf "(%A %s %A)" firstOperand (operator.ToString()) secondOperand

    override x.Interpret(context : Variable list) =

        let compare (operand1 : Val, operand2: Val, op : System.IComparable -> System.IComparable -> bool) = 
            match operand1, operand2 with
            | Bool log1, Bool log2     -> Bool((op) log1 log2)
            | Int num1, Int num2       -> Bool((op) num1 num2)
            | Float num1, Float num2   -> Bool((op) num1 num2)
            | Char char1, Char char2   -> Bool((op) char1 char2)
            | Str str1, Str str2 -> Bool((op) str1 str2)
            | _                        -> Empty

        let logical (operand1 : Val, operand2: Val, op : bool -> bool -> bool) = 
            match operand1, operand2 with
            | Bool log1, Bool log2     -> Bool((op) log1 log2)
            | _                        -> Empty

        let firstOpetandVal = firstOperand.Interpret(context)
        let secondOpetandVal = secondOperand.Interpret(context)

        match operator with
        | OR               -> logical (firstOpetandVal, secondOpetandVal, (||))
        | AND              -> logical (firstOpetandVal, secondOpetandVal, (&&))
        | EQUAL            -> compare (firstOpetandVal, secondOpetandVal, (=))
        | NOT_EQUAL        -> compare (firstOpetandVal, secondOpetandVal, (<>))
        | GREATER          -> compare (firstOpetandVal, secondOpetandVal, (>))
        | GREATER_OR_EQUAL -> compare (firstOpetandVal, secondOpetandVal, (>=))
        | LESS             -> compare (firstOpetandVal, secondOpetandVal, (<))
        | LESS_OR_EQUAL    -> compare (firstOpetandVal, secondOpetandVal, (<=))

        | ADDITION         -> match firstOpetandVal, secondOpetandVal with
                              | Int intnum1, Int intnum2         -> Int(intnum1 + intnum2)
                              | Float floatnum1, Float floatnum2 -> Float(floatnum1 + floatnum2)
                              | Str str1, Str str2               -> Str (str1 + str2)
                              | _                                -> Empty

        | SUBSTRACTION     -> match firstOpetandVal, secondOpetandVal with
                              | Int intnum1, Int intnum2         -> Int(intnum1 - intnum2)
                              | Float floatnum1, Float floatnum2 -> Float(floatnum1 - floatnum2)
                              | _                                -> Empty

        | MULTIPLICATION   -> match firstOpetandVal, secondOpetandVal with
                              | Int intnum1, Int intnum2         -> Int(intnum1 * intnum2)
                              | Float floatnum1, Float floatnum2 -> Float(floatnum1 * floatnum2)
                              | _                                -> Empty

        | DIVISION         -> match firstOpetandVal, secondOpetandVal with
                              | Int intnum1, Int intnum2         -> Int(intnum1 / intnum2)
                              | Float floatnum1, Float floatnum2 -> Float(floatnum1 / floatnum2)
                              | _                                -> Empty

        | MODULUS          -> match firstOpetandVal, secondOpetandVal with
                              | Int intnum1, Int intnum2         -> Int(intnum1 % intnum2)
                              | Float floatnum1, Float floatnum2 -> Float(floatnum1 % floatnum2)
                              | _                                -> Empty

        | MEMBER_CELL      -> Empty //later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type UnaryOperation(operator : UnaryOperator, operand : Expression, pos : Position) =
    inherit Expression(pos)
    member x.Operator = operator
    member x.Operand  = operand

    override x.ToString() = sprintf "(%s %A)" (operator.ToString()) operand

    override x.Interpret(context : Variable list) =
        let operandVal = operand.Interpret(context)
        match operator with
        | NOT              ->                              
                              match operandVal with
                              | Bool log -> Bool (not log)
                              | _        -> Empty // error

        | MINUS            -> match operandVal with
                              | Int   number -> Int(- number)
                              | Float number -> Float(-number)
                              | _        -> Empty // error 
