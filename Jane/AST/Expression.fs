namespace AST

type Expression =
    inherit Initializer

type BinaryOperation(firstOperand : Expression, operator : BinaryOperator, secondOperand : Expression) =
    interface Expression
    member x.FirstOperand  = firstOperand
    member x.Operator      = operator
    member x.SecondOperand = secondOperand

type UnaryOperation(operator : UnaryOperator, operand : Expression) =
    interface Expression
    member x.Operator = operator
    member x.Operand  = operand

