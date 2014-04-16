module TestWhile

open AST
open StaticAnalysis
open Errors

//class myClass {
//
//    myClass() {
//    }
//
//    static int main() {
//        int a = 1;
//        if (a < 2)
//        {
//          int b = 1;
//          while (b < 5)
//          {
//              a = a + 1;
//              b = b + 1;
//          }
//        }
//        a = a + 2;
//    }
//
//}

let p              = new Position(0, 0, 0, 0)

let myInt          = new IntType(0, p)

let one            = new IntegerLiteral(1L, p)
let five           = new IntegerLiteral(5L, p)
let two            = new IntegerLiteral(2L, p)

let myDecl         = new DeclarationStatement(myInt, new ID("a", p), one, p, None)
let myDeclB         = new DeclarationStatement(myInt, new ID("b", p), one, p, None)


let binOpPlus      = new BinaryOperation(new Identifier(new ID("a", p)), ADDITION, five, p)
let myAssign       = new AssignmentStatement(["a"], binOpPlus, p, None)

//while
let myCondition    = new BinaryOperation (new Identifier(new ID("b", p)), LESS, five, p)
let binOpPlusB      = new BinaryOperation(new Identifier(new ID("b", p)), ADDITION, one, p)
let myAssignB       = new AssignmentStatement(["b"], binOpPlusB, p, None)
let WhileBlock     = new Block([myAssign; myAssignB], p, [], None)
let myWhile        = new WhileStatement(myCondition, WhileBlock, p, None)

//if
let myConditionIF  = new BinaryOperation (new Identifier(new ID("a", p)), LESS, two, p)
let IfBlock        = new Block([myDeclB; myWhile], p, [], None)
let myIf           = new IfStatement(myConditionIF, IfBlock, None, p, None)

let myBlock        = new Block([myDecl; myIf ; myAssign], p, [], None)
let myMethod       = new ClassVoidMethod(true, new ID("main", p),[], myBlock, p)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor(new ID("myClass", p), [], new Block([], p, [], None), p)
let myClass        = new Class(new ID("myClass", p), None, [], Some myConstructor, myClassMembers, p)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses, "myClass", p)

let err, main = SA_Program myProg

printfn "%A" myProg
myProg.Interpret()