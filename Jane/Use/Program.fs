open LanguageParser
open AST

let a = ParseProgram "interface a {} class MyClass extends a {}"
printfn "%A" a