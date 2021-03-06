﻿namespace AST

open System

[<AbstractClass>]
type Expression(pos : Position) =
    inherit Initializer(pos)


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type InstanceOf(expression : Expression, controlType : AST.Type, pos : Position) =
    inherit Expression(pos)
    member x.Expression  = expression
    member x.ControlType = controlType

    override x.ToString() = sprintf "(%A instanceOf %A)" expression controlType

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BinaryOperation(firstOperand : Expression, operator : BinaryOperator, secondOperand : Expression, pos : Position) =
    inherit Expression(pos)
    member x.FirstOperand  = firstOperand
    member x.Operator      = operator
    member x.SecondOperand = secondOperand

    override x.ToString() = sprintf "(%A %s %A)" firstOperand (operator.ToString()) secondOperand


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type UnaryOperation(operator : UnaryOperator, operand : Expression, pos : Position) =
    inherit Expression(pos)
    member x.Operator = operator
    member x.Operand  = operand

    override x.ToString() = sprintf "(%s %A)" (operator.ToString()) operand
