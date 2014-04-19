open LanguageParser
open AST

let a = ParseProgram "interface a {} class MyClass extends a {}"
//printfn "%A" a

let textTest = @"
    interface ISummator { }
    
    class Incrementer extends ISummator 
    {
        Incrementer(int startValue)
        {
            x = startValue;    
        }

        int x = 0; 

    }
    
    class MainClass
    {
	    static void main(String[][] args)
	    {
            Incrementer i = new Incrementer(1);
	    }
    }
"

let text = @"
    interface ISummator { }
    
    class Incrementer extends ISummator 
    {
        int x = 0;

        Incrementer(int startValue)
        {
            x = startValue;    
        }

        int getValue()
        {
            return x;
        }

        void incr()
        {
            x = x + 1;
        }

    }
    
    class MainClass
    {

	    static void main(String[][] args)
	    {
			Incrementer i = new Incrementer(0);
            i.incr();
            i.getValue();
	    }

    }
"

let b = ParseProgram textTest
printfn "%A" b