namespace AST

type BinaryOperator = OR               // ||
                    | AND              // &&
                    | EQUAL            // == 
                    | NOT_EQUAL        // !=
                    | GREATER          // >
                    | GERATER_OR_EQUAL // >=
                    | LESS             // <
                    | LESS_OR_EQUAL    // <=
                    | ADDITION         // +
                    | SUBSTRACTION     // -
                    | MULTIPLICATION   // *
                    | DIVISION         // /
                    | MODULUS          // %
                    | MEMBER_CALL      // .

type UnaryOperator  = NOT              // !
                    | PLUS             // +
                    | MINUS            // -
                    