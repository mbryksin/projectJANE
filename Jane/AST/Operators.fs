namespace AST

type Equality       = | EQUAL            // == 
                      | NOT_EQUAL        // !=

type Comparison     = | GREATER          // >
                      | GERATER_OR_EQUAL // >=
                      | LESS             // <
                      | LESS_OR_EQUAL    // <=

type Addition       = | ADDITION         // +
                      | SUBSTRACTION     // -

type Multiplication = | MULTIPLICATION   // *
                      | DIVISION         // /
                      | MODULUS          // %

type UnarySign      = | UNARY_PLUS       // +
                      | UNARY_MINUS      // -