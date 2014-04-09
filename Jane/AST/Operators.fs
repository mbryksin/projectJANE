namespace AST

type BinaryOperator = OR
                    | AND
                    | EQUAL
                    | NOT_EQUAL
                    | GREATER
                    | GREATER_OR_EQUAL
                    | LESS
                    | LESS_OR_EQUAL
                    | ADDITION
                    | SUBSTRACTION
                    | MULTIPLICATION  
                    | DIVISION
                    | MODULUS         
                    | MEMBER_CALL

                        override x.ToString() =
                            match x with
                            | OR               -> "||"
                            | AND              -> "&&"
                            | EQUAL            -> "=="
                            | NOT_EQUAL        -> "!="
                            | GREATER          -> ">"
                            | GREATER_OR_EQUAL -> ">="
                            | LESS             -> "<"
                            | LESS_OR_EQUAL    -> "<="
                            | ADDITION         -> "+"
                            | SUBSTRACTION     -> "-"
                            | MULTIPLICATION   -> "*"
                            | DIVISION         -> "/"
                            | MODULUS          -> "%"
                            | MEMBER_CALL      -> "."

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type UnaryOperator  = NOT
                    | MINUS

                        override x.ToString() =
                            match x with
                            | NOT   -> "!"
                            | MINUS -> "-"
                    