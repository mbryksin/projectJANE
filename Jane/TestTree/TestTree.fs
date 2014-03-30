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
//        string helloWorld = "Hello";
//        helloWorld = helloWorld + " World!"
//        stdout.print(helloWorld);
//    }
//}


let myTypeString   = new Type("string", 0)

let myHello        = new StringLiteral("Hello")
let myDecl         = new DeclarationStatement(myTypeString, "helloWorld", myHello)

let myWorld        = new StringLiteral(" World!")
let myMemberCall1  = new BinaryOperation(new Identifier("helloWorld"), ADDITION, myWorld)
let myAssign       = new AssignmentStatement(["helloWorld"], myMemberCall1)
                           
let myMember       = new Member("print", new Arguments([new Identifier("helloWorld")]))
let myMemberCall2  = new BinaryOperation(new Identifier("stdout"), MEMBER_CELL, myMember)
let myPrint        = new MemberCallStatement(myMemberCall2)

let myBlock        = new Block([myDecl; myAssign; myPrint])
let myMethod       = new ClassMethod(true, new Type("int", 0), "main", [], myBlock)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor([], new Block([]))
let myClass        = new Class("myClass", None, [], myConstructor, myClassMembers)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses)