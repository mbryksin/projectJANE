namespace AST

type Expression =
    inherit Initializer

type BinaryOperation(firstOperand : Expression, operator : BinaryOperator, secondOperand : Expression) = 
    member x.FirstOperand  = firstOperand
    member x.Operator      = operator
    member x.SecondOperand = secondOperand
    interface Expression with
        member x.Interpret() =
            match operator with
            | OR               -> new Val(firstOperand.Interpret().Bool.Value || secondOperand.Interpret().Bool.Value)
            | AND              -> new Val(firstOperand.Interpret().Bool.Value && secondOperand.Interpret().Bool.Value)
            | EQUAL            -> new Val(firstOperand.Interpret().Bool.Value =  secondOperand.Interpret().Bool.Value)
            | NOT_EQUAL        -> new Val(firstOperand.Interpret().Bool.Value <> secondOperand.Interpret().Bool.Value)
            | GREATER          -> new Val(firstOperand.Interpret().Int.Value >  secondOperand.Interpret().Int.Value)
            | GERATER_OR_EQUAL -> new Val(firstOperand.Interpret().Int.Value >= secondOperand.Interpret().Int.Value)
            | LESS             -> new Val(firstOperand.Interpret().Int.Value <  secondOperand.Interpret().Int.Value)
            | LESS_OR_EQUAL    -> new Val(firstOperand.Interpret().Int.Value <= secondOperand.Interpret().Int.Value)
            | ADDITION         -> new Val(firstOperand.Interpret().Int.Value +  secondOperand.Interpret().Int.Value)
            | SUBSTRACTION     -> new Val(firstOperand.Interpret().Int.Value -  secondOperand.Interpret().Int.Value)
            | MULTIPLICATION   -> new Val(firstOperand.Interpret().Int.Value *  secondOperand.Interpret().Int.Value)
            | DIVISION         -> new Val(firstOperand.Interpret().Int.Value /  secondOperand.Interpret().Int.Value)
            | MODULUS          -> new Val(firstOperand.Interpret().Int.Value %  secondOperand.Interpret().Int.Value)
            | MEMBER_CELL      -> new Val() //later

type UnaryOperation(operator : UnaryOperator, operand : Expression) =
    member x.Operator = operator
    member x.Operand  = operand
    interface Expression with
        member x.Interpret() =
            match operator with
            | NOT              -> Val(not <| operand.Interpret().Bool.Value)
            | PLUS             -> Val(+      operand.Interpret().Int.Value)
            | MINUS            -> Val(-      operand.Interpret().Int.Value)
        

