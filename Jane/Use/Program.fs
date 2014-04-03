open LanguageParser
open AST


printfn "%s" <| (ParseProgram "((((new A(new B(a1,a4,6,9.0,t[5])))))).q.a[i].r(1)").ToString()
