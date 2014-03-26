module Test

open AST

//class myClass
//{
//    myClass()
//    {
//    }
//
//    int main()
//    {
//        stdout.print("Hello World!");
//    }
//}


let myExpression   = new Expression
                         (new ExpressionOR
                              (new ExpressionAND
                                   (new ExpressionEQ
                                        (new ExpressionINST
                                             (new ExpressionCOMP
                                                  (new ExpressionADD
                                                       (new ExpressionMUL
                                                            (false, 
                                                             new ExpressionNOT
                                                                (None, 
                                                                 new StringLiteral("Hello World!")
                                                                )
                                                             ), None
                                                       ), []
                                                  ), None
                                             ), None
                                        ), None
                                   ), []
                              ), []
                         )

let myPrint        = new MemberCall(new Identifier("stdout"), [("print", Some(new Arguments([myExpression]) :> Suffix))])
let myMethod       = new ClassMethod(true, new Type("int", 0), "main", [], new Block([myPrint :> Statement]))
let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor([], new Block([]))
let myClass        = new Class("myClass", None, [], myConstructor, myClassMembers)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses)
